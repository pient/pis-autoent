Ext.define('PIS.view.setup.reg.Registry', {
    extend: 'PIS.Page',
    alternateClassName: 'PIS.RegistrySetup',

    requires: [
        'PIS.model.sys.reg.Register',
        'PIS.view.setup.reg.RegisterFormWin'
    ],

    pageData: {},

    mainPanel: null,
    navPanel: null,
    stagePanel: null,
    itemContextMenu: null,
    
    defFormWinCls: 'PIS.view.setup.reg.RegisterFormWin',
    defFormWin: null,
    formWin: null,
    cachedFormWins: [],

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            layout: 'border'
        }, config);

        me.pageData = config.pageData;

        me.navDataStore = Ext.create('PIS.data.TreeStore', {
            root: null,
            model: 'PIS.model.sys.reg.Register',

            autoLoad: true,

            proxy: {
                noCache: false,
                type: 'ajax',
                url: '/api/RegistrySetup/GetNodes',
                extractResponseData: function (response) {
                    // 处理返回数据
                    return response
                }
            }
        });

        me.navPanel = Ext.create("PIS.TreePanel", {
            region: 'west',
            width: 240,
            split: true,
            border: false,
            store: me.navDataStore,
            columns: [
                { width: 10, menuDisabled: true },
                { dataIndex: 'Name', xtype: 'treecolumn', text: '名称', flex: 1 }
            ],
            listeners: {
                itemcontextmenu: function (v, rec, item, rowIdx, e) {
                    Ext.EventObject.preventDefault();

                    if (rec) {
                        me.miDelete.setDisabled(!(rec.childNodes.length == 0) || !rec.get("ParentId"));

                        me.miAddSibling.setDisabled((!rec.get('ParentId')));
                        me.miModify.setDisabled((!rec.get('ParentId')));
                    }

                    var xy = e.getXY();
                    me.itemContextMenu.showAt(xy);
                },
                select: function (cmp, rec, e, a) {
                    me.loadItem(rec);
                }
            }
        });

        me.stageDataStore = Ext.create('PIS.data.Store', {
            model: 'PIS.model.sys.reg.Register',
            proxy: {
                noCache: false,
                type: 'ajax',
                url: '/api/RegistrySetup/GetItems',
                extractResponseData: function (response) {
                    // 处理返回数据
                    return response
                }
            }
        });

        me.stagePanel = Ext.create("PIS.GridPanel", {
            region: 'center',
            border: 0,
            store: me.stageDataStore,
            selModel: Ext.create('PIS.selection.CheckboxModel', { mode: 'single' }),
            columns: [
                {
                    dataIndex: 'Id',
                    text: '操作',
                    align: 'center',
                    xtype: 'actioncolumn',
                    width: 70,
                    menuDisabled: true,
                    items: [
                        {
                            iconCls: 'pis-icon-view',
                            tooltip: '查看',
                            handler: function (grid, rowIndex, colIndex) {
                                var rec = grid.getStore().getAt(rowIndex);
                                me.loadFrmWin("view", rec);
                            }
                        },
                        {
                            iconCls: 'pis-icon-edit',
                            tooltip: '编辑',
                            handler: function (grid, rowIndex, colIndex) {
                                var rec = grid.getStore().getAt(rowIndex);
                                me.loadFrmWin("modifyitem", rec);
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
                { dataIndex: 'Name', text: '名称', flex: 1 },
                { dataIndex: 'Code', text: '编码', flex: 1 },
                { 
                    dataIndex: 'DisplayData',  text: '数据', flex: 2,
                    renderer: function (val, meta, rec) { return val || rec.get("Data"); }
                }, {
                    dataIndex: 'Status', text: '状态', width: 90, align: 'center', menuDisabled: true,
                    renderer: function (val) { return (PIS.RegisterModel.StatusEnum[val] || ''); }
                },
                { dataIndex: 'Description', text: '描述', flex: 1 }
            ]
        });

        me.mainPanel = Ext.create("PIS.Panel", {
            region: 'center',
            layout: 'border',
            border: 0,
            title: me.pageData.Title,
            tbar: ['-', 
                { xtype: 'pis-refreshbutton', text: '重新加载注册信息', handler: me.reloadRegistry },
                { xtype: 'pis-savebutton', text: '持久保存注册信息', handler: me.persistRegistry }],
            items: [me.navPanel, me.stagePanel]
        });

        me.items = me.mainPanel;

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);

        me.miAddItem = Ext.create('Ext.menu.Item', {
            text: '新建子项',
            handler: function () {
                me.addItem();
            }
        });

        me.miAddSibling = Ext.create('Ext.menu.Item', {
            text: '新建同级节点',
            handler: function () {
                me.addSiblingNode();
            }
        });

        me.miAddSub = Ext.create('Ext.menu.Item', {
            text: '新建子节点',
            handler: function () {
                me.addSubNode();
            }
        });

        me.miAdd = Ext.create('Ext.menu.Item', {
            text: '新建',
            menu: {
                items: [me.miAddItem, '-', me.miAddSibling, me.miAddSub]
            }
        });

        me.miDelete = Ext.create('Ext.menu.Item', {
            text: '删除',
            iconCls: 'pis-icon-delete',
            handler: function () {
                me.deleteNode();
            }
        });

        me.miModify = Ext.create('Ext.menu.Item', {
            text: '修改',
            handler: function () {
                me.modifyNode();
            }
        });

        me.itemContextMenu = Ext.create('Ext.menu.Menu', {
            items: [me.miAdd, '-', me.miDelete, me.miModify]
        });

        me.defFormWin = me.cachedFormWins[me.defFormWinCls] = Ext.create(me.defFormWinCls, {
            listeners: {
                save: me.onSave
            }
        });

        me.navDataStore.load();
    },

    loadFrmWin: function (action, rec, refrec) {
        var me = this;

        var edit_frm = "PIS." + (rec.get("EditPage") || "view.setup.reg.RegisterFormWin");
        
        if (!me.cachedFormWins[edit_frm]) {
            try{
                me.cachedFormWins[edit_frm] = Ext.create(edit_frm, {
                    listeners: {
                        save: me.onSave
                    }
                });
            } catch (ex) {
                edit_frm = me.defFormWinCls;
            }
        }

        me.formWin = me.cachedFormWins[edit_frm];

        me.formWin.load(me, action, rec);
    },

    loadItem: function (rec) {
        var me = this;
        var nid = rec.get("Id");

        me.stageDataStore.load({ params: { node: nid } });
    },

    addItem: function (rec) {
        var me = this;

        var _rec = rec || me.navPanel.getFirstSelection();

        var _new_rec = Ext.create('PIS.model.sys.reg.Register', {
            ParentId: _rec.get('Id'),
            RegDataType: _rec.get('RegDataType'),
            EditPage: _rec.get('EditPage'),
            Status: _rec.get('Status'),
            SortIndex: me.stageDataStore.getTotalCount() + 1
        });

        me.loadFrmWin("additem", _new_rec, _rec);
    },

    addSubNode: function (rec) {
        var me = this;

        var _rec = rec || me.navPanel.getFirstSelection();

        var _new_rec = Ext.create('PIS.model.sys.reg.Register', {
            ParentId: _rec.get('Id'),
            Code: _rec.get('Code'),
            RegDataType: _rec.get('RegDataType'),
            EditPage: _rec.get('EditPage'),
            Status: _rec.get('Status'),
            SortIndex: 1
        });

        me.loadFrmWin("addsub", _new_rec, _rec);
    },

    addSiblingNode: function (rec) {
        var me = this;

        var _rec = rec || me.navPanel.getFirstSelection();
        var _prec = _rec.parentNode;

        var _new_rec = Ext.create('PIS.model.sys.reg.Register', {
            ParentId: _rec.get('ParentId'),
            Code: (_prec ? _prec.get('Code') : ""),
            RegDataType: _rec.get('RegDataType'),
            EditPage: _rec.get('EditPage'),
            Status: _rec.get('Status'),
            SortIndex: (_rec.get('SortIndex') || 0) + 1
        });

        me.loadFrmWin("addsibling", _new_rec, _rec);
    },

    modifyNode: function (rec) {
        var me = this;

        var rec = me.navPanel.getFirstSelection();

        me.loadFrmWin("modify", rec);
    },

    onSave: function (frm, action, rec, sender) {
        var owner = frm.owner;

        if (action == 'additem' || action == 'modifyitem') {
            owner.onItemSave(frm, action, rec, owner.stageDataStore, sender);
        } else {
            owner.onNodeSave(frm, action, rec, owner.navDataStore, sender);
        }
    },

    onItemSave: function (frm, action, rec, store, sender) {
        var me = this;

        frm.submit({
            url: '/api/RegistrySetup/SaveRegister?nodeType=Item',
            onsuccess: function (response, opts) {
                store.reload();

                frm.close();
            }
        });
    },

    onNodeSave: function (frm, action, rec, store, sender) {
        var me = this;

        frm.submit({
            url: '/api/RegistrySetup/SaveRegister?nodeType=Node',
            onsuccess: function (response, opts) {
                // 要刷新的记录
                var _ref_rec = store.getNodeById(rec.internalId) || me.navPanel.getFirstSelection();

                if (_ref_rec && _ref_rec.parentNode) {
                    _ref_rec = _ref_rec.parentNode;
                }

                store.load({ node: _ref_rec || {} });

                frm.close();
            }
        });
    },

    deleteItem: function (rec) {
        var me = this;
        rec = rec || me.stagePanel.getFirstSelection();

        if (!rec) {
            PISMsgBox.alert("请先选择要删除的记录！");
            return;
        }

        PISMsgBox.confirm("确定执行删除操作？", function (val) {
            if ('yes' === val) {
                PISAjaxRequest({
                    method: 'DELETE',
                    url: '/api/RegistrySetup/DeleteRegister?node=' + rec.internalId,
                    onsuccess: function (response, opts) {
                        me.stageDataStore.reload();
                    }
                });
            }
        });
    },

    deleteNode: function (recs) {
        var me = this;
        recs = recs || me.navPanel.getSelections();

        if (!recs || recs.length <= 0) {
            PISMsgBox.alert("请先选择要删除的记录！");
            return;
        }

        for (var i = 0; i < recs.length; i++) {
            if (recs[i].childNodes.length > 0) {
                PISMsgBox.warn("存在非叶节点，请删除叶节点子节点再作删除操作！");
                return;
            }
        }

        PISMsgBox.confirm("确定执行删除操作？", function (val) {
            var ids = PIS.Util.getModelIds(recs);
            var pids = [];

            var rec = recs[0];

            if ('yes' === val) {
                PISAjaxRequest({
                    method: 'DELETE',
                    url: '/api/RegistrySetup/DeleteRegister?node=' + internalId,
                    params: { ids: ids },
                    onsuccess: function (response, opts) {
                        var _ref_rec = rec;
                        var _p_rec = rec.parentNode;
                        var _g_rec = null;

                        if (_p_rec) {
                            _ref_rec = _p_rec;
                            _g_rec = _p_rec.parentNode;

                            if (_g_rec) {
                                _ref_rec = _g_rec;
                            }
                        }

                        me.navDataStore.load({
                            node: _ref_rec,
                            callback: function () {
                                if (_p_rec) {
                                    me.navPanel.expandNode(_p_rec);
                                }
                            }
                        });
                    }
                });
            }
        });
    },

    reloadRegistry: function () {
        PISPortal.reloadRegistry();
    },

    persistRegistry: function () {
        PISAjaxRequest({
            url: '/api/RegistrySetup/PersistRegistry',
            onsuccess: function (response, opts) {
                PISMsgBox.info("持久保存成功！");
                return true;
            }
        });
    }
});