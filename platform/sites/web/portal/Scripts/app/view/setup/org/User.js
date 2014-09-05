Ext.define('PIS.view.setup.org.User', {
    extend: 'PIS.Page',
    alternateClassName: 'PIS.UserSetup',

    requires: [
        'PIS.model.sys.org.User',
        'PIS.view.setup.org.UserFormWin'
    ],

    pageData: {},

    mainPanel: null,
    stagePanel: null,

    formWin: null,
    userDetailWin: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            layout: 'border'
        }, config);

        me.pageData = config.pageData;

        me.dataStore = Ext.create('PIS.PageGridStore', {
            model: 'PIS.model.sys.org.User',
            proxy: {
                url: '/api/OrgSetup/QueryUsers'
            }
        });

        me.stagePanel = Ext.create("PIS.PageGridPanel", {
            region: 'center',
            border: false,
            loadFrmWin: me.loadFrmWin,
            store: me.dataStore,
            tlitems: [{ bttype: 'add' }, '-', '->', 'schfield', 'cquery'],
            schpanel: {
                schitems: [
                    { fieldLabel: '编号', id: 'Code', schopts: { condition: { Type: 'Like', Field: 'Code' } } },
                    { fieldLabel: '姓名', id: 'Name', schopts: { condition: { Type: 'Like', Field: 'Name' } } }]
            },
            columns: [
                {
                    dataIndex: 'Id',
                    text: '操作',
                    align: 'center',
                    xtype: 'actioncolumn',
                    width: 90,
                    menuDisabled: true,
                    items: [
                        {
                            iconCls: 'pis-icon-view',
                            tooltip: '查看',
                            handler: function (grid, rowIndex, colIndex) {
                                var rec = grid.getStore().getAt(rowIndex);
                                me.viewItem(rec);
                            }
                        },
                        {
                            iconCls: 'pis-icon-group',
                            tooltip: '组织权限',
                            handler: function (grid, rowIndex, colIndex) {
                                var rec = grid.getStore().getAt(rowIndex);
                                me.loadUserDetail(rec);
                            }
                        },
                        {
                            iconCls: 'pis-icon-edit',
                            tooltip: '编辑',
                            handler: function (grid, rowIndex, colIndex) {
                                var rec = grid.getStore().getAt(rowIndex);
                                me.modifyItem(rec);
                            }
                        },
                        {
                            iconCls: 'pis-icon-delete',
                            tooltip: '删除 ',
                            handler: function (grid, rowIndex, colIndex) {
                                var rec = grid.getStore().getAt(rowIndex);

                                me.deleteItem(rec);
                            }
                        }
                    ]
                },
                { dataIndex: 'Name', text: '名称', schfield: true, flex: 1, menuDisabled: true },
                { dataIndex: 'LoginName', text: '登录名', schfield: true, flex: 1, menuDisabled: true },
                { dataIndex: 'Code', text: '编号', schfield: true, flex: 1, menuDisabled: true },
                { dataIndex: 'Email', text: '邮箱', schfield: true, flex: 2, menuDisabled: true },
                {
                    dataIndex: 'Status', text: '状态', width: 90, align: 'center', menuDisabled: true,
                    renderer: function (val) { return (PIS.UserModel.StatusEnum[val] || ''); }
                }
            ],
            listeners: {
                itemcontextmenu: function (v, rec, item, rowIdx, e) {
                    Ext.EventObject.preventDefault();

                    var xy = e.getXY();
                    me.itemContextMenu.showAt(xy);
                },
                select: function (cmp, rec, e, a) {
                    me.loadItem(rec);
                }
            }
        });

        me.mainPanel = Ext.create("PIS.Panel", {
            region: 'center',
            layout: 'border',
            border: 0,
            title: me.pageData.Title,
            tbar: ['-',
                { xtype: 'pis-refreshbutton', text: '重新加载组织信息', handler: me.reloadOrgUser }
            ],
            items: [me.stagePanel]
        });

        me.items = me.mainPanel;

        this.callParent([config]);

        me.dataStore.reload();
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);

        me.formWin = me.stagePanel.formWin = Ext.create('PIS.view.setup.org.UserFormWin', {
            listeners: {
                save: function (frm, action, rec, sender) { me.onSave(frm, action, rec, sender); }
            }
        });

        me.userDetailWin = Ext.create('PIS.view.setup.org.UserDetailWin', { });
    },

    loadFrmWin: function (action, rec, refrec) {
        var me = this;

        me.formWin.load(me, action, rec);
    },

    loadItem: function (rec) {
        var me = this;
        var nid = rec.get("Id");

    },

    viewItem: function (rec) {
        var me = this;

        rec = rec || me.stagePanel.getFirstSelection();

        me.loadFrmWin("view", rec);
    },

    addItem: function (rec) {
        var me = this;

        var _rec = rec || me.stagePanel.getFirstSelection();

        var _new_rec = Ext.create('PIS.model.sys.org.User', {
        });

        me.loadFrmWin("add", _new_rec, _rec);
    },

    loadUserDetail: function (rec) {
        var me = this;

        me.userDetailWin.load(me, rec);
    },

    modifyItem: function (rec) {
        var me = this;

        var rec = rec || me.stagePanel.getFirstSelection();

        me.loadFrmWin("modify", rec);
    },

    deleteItem: function (rec) {
        var me = this;
        rec = rec || me.stagePanel.getFirstSelection();

        if (!rec || rec.length <= 0) {
            PISMsgBox.alert("请先选择要删除的用户！");
            return;
        }

        PISMsgBox.confirm("确定执行删除操作？", function (val) {
            if ('yes' === val) {
                PISAjaxRequest({
                    method: 'DELETE',
                    url: '/api/OrgSetup/DeleteUser?node=' + rec.internalId,
                    onsuccess: function (response, opts) {
                        me.dataStore.reload();
                    }
                });
            }
        });
    },

    onSave: function (frm, action, rec, sender) {
        var me = this;
        var store = me.stagePanel.store

        frm.submit({
            url: '/api/OrgSetup/SaveUser',
            onsuccess: function (response, opts) {
                store.reload();

                frm.close();
            }
        });
    },

    reloadOrgUser: function () {
        PISPortal.reloadMdlOrgAuth();
    }
});
