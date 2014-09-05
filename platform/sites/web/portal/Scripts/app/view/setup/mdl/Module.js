Ext.define('PIS.view.setup.mdl.Module', {
    extend: 'PIS.Page',
    alternateClassName: 'PIS.ModuleSetup',

    requires: [
        'PIS.model.sys.mdl.Module',
        'PIS.view.setup.mdl.ModuleFormWin',
    ],

    pageData: {},

    mainPanel: null,
    itemContextMenu: null,
    formWin: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            layout: 'border'
        }, config);

        me.pageData = config.pageData;

        me.dataStore = Ext.create('PIS.data.TreeStore', {
            root: null,
            model: 'PIS.model.sys.mdl.Module',

            proxy: {
                noCache: false,
                type: 'ajax',
                url: '/api/Portal/GetSysStructuredData?name=SysModule&rootLevel=1',
                extractResponseData: function (response) {
                    // 处理返回数据
                    return response
                }
            }
        });

        me.mainPanel = Ext.create("PIS.TreePanel", {
            region: 'center',
            layout: 'border',
            // multiSelect: true,
            title: me.pageData.Title,
            border: false,
            store: me.dataStore,
            selModel: Ext.create('PIS.selection.CheckboxModel', { mode: 'single' }),
            tbar: ['-', { xtype: 'pis-refreshbutton', text: '重新加载系统模块', handler: me.reloadSysModule }],
            columns: [
                { dataIndex: 'SortIndex', text: '排序号', flex: .5, menuDisabled: true, align: 'center' },
                { dataIndex: 'Name', xtype: 'treecolumn', text: '名称', flex: 2 },
                { dataIndex: 'Code', text: '编号', flex: 1 },
                {
                    dataIndex: 'Type', text: '类型', flex: .5, align: 'center',
                    renderer: function (val) { return (PIS.ModuleModel.TypeEnum[val] || ''); }
                },
                { dataIndex: 'MdlPath', text: '路径', flex: 1.5 },
                {
                    dataIndex: 'Status', text: '是否有效', flex: .5, menuDisabled: true, align: 'center',
                    renderer: function (val) { return (PIS.ModuleModel.StatusEnum[val] || ''); }
                }
            ],
            listeners: {
                itemcontextmenu: function (v, rec, item, rowIdx, e) {
                    Ext.EventObject.preventDefault();

                    if (rec) {
                        me.miDelete.setDisabled(!(rec.childNodes.length == 0));
                        me.miAddSibling.setDisabled((!rec.get('ParentId')));
                    }

                    var xy = e.getXY();
                    me.itemContextMenu.showAt(xy);
                }
            }
        });

        me.items = me.mainPanel;

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);

        me.miDelete = Ext.create('Ext.menu.Item', {
            text: '删除',
            iconCls: 'pis-icon-delete',
            handler: function () {
                me.deleteItem();
            }
        });

        me.miAddSibling = Ext.create('Ext.menu.Item', {
            text: '新增同级模块',
            handler: function () {
                me.addSiblingItem();
            }
        });

        me.miAddSub = Ext.create('Ext.menu.Item', {
            text: '新增子模块',
            handler: function () {
                me.addSubItem();
            }
        });

        me.miModify = Ext.create('Ext.menu.Item', {
            text: '修改',
            handler: function () {
                me.modifyItem();
            }
        });

        me.itemContextMenu = Ext.create('Ext.menu.Menu', {
            items: [me.miDelete, '-', me.miAddSibling, me.miAddSub, me.miModify]
        });

        me.formWin = Ext.create('PIS.view.setup.mdl.ModuleFormWin', {
            listeners: {
                save: me.onItemSave
            }
        });
        
        me.dataStore.load();
    },

    loadFrmWin: function (action, rec, refrec) {
        var me = this;

        me.formWin.load(me, action, rec);
    },

    addSubItem: function (rec) {
        var me = this;

        var _rec = rec || me.mainPanel.getFirstSelection();

        var _new_rec = Ext.create('PIS.model.sys.mdl.Module', {
            ParentId: _rec.get('Id'),
            Code: _rec.get('Code'),
            Type: _rec.get('Type'),
            IsEnabled: _rec.get('IsEnabled'),
            SortIndex: 1
        });

        me.loadFrmWin("addsub", _new_rec, _rec);
    },

    addSiblingItem: function (rec) {
        var me = this;

        var _rec = rec || me.mainPanel.getFirstSelection();
        var _prec = _rec.parentNode;

        var _new_rec = Ext.create('PIS.model.sys.mdl.Module', {
            ParentId: _rec.get('ParentId'),
            Code: (_prec ? _prec.get('Code') : ""),
            Type: _rec.get('Type'),
            IsEnabled: _rec.get('IsEnabled'),
            Icon: _rec.get('Icon'),
            SortIndex: (_rec.get('SortIndex') || 0) + 1
        });

        me.loadFrmWin("addsibling", _new_rec, _rec);
    },

    modifyItem: function (rec) {
        var me = this;

        var rec = me.mainPanel.getFirstSelection();

        me.loadFrmWin("modify", rec);
    },

    onItemSave: function (win, action, rec, sender) {
        var me = this;

        var owner = win.owner,
            store = owner.dataStore;

        me.submit({
            url: '/api/ModuleSetup/SaveModule',
            onsuccess: function (response, opts) {
                // 要刷新的记录
                var _ref_rec = store.getNodeById(rec.internalId) || owner.mainPanel.getFirstSelection();

                if (_ref_rec && _ref_rec.parentNode) {
                    _ref_rec = _ref_rec.parentNode;
                }

                store.load({ node: _ref_rec || {} });

                me.close();
            }
        });
    },

    deleteItem: function (recs) {
        var me = this;
        recs = recs || me.mainPanel.getSelections();

        if (!recs || recs.length <= 0) {
            PISMsgBox.alert("请先选择要操作的记录！");
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
                    url: '/api/ModuleSetup/DeleteModules?node=' + rec.get("Id"),
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

                        me.dataStore.load({
                            node: _ref_rec,
                            callback: function () {
                                if (_p_rec) {
                                    me.mainPanel.expandNode(_p_rec);
                                }
                            }
                        });
                    }
                });
            }
        });
    },

    reloadSysModule: function () {
        PISPortal.reloadMdlOrgAuth();
    }
});