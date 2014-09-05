var PISSubPortal = null;

Ext.define('PIS.SubPortalViewport', {
    extend: 'Ext.Viewport',
    alias: 'widget.pis-subportalviewport',

    pageCode : '',
    pageData: {},

    menuPanel: null,
    stagePanel: null,
    mainPanel: null,

    constructor: function (config) {
        var me = this;

        PISSubPortal = me;

        config = Ext.apply({
            layout: 'border'
        }, config);

        me.pageData = config.pageData;

        var isMenuValid = me.pageData.Menu
            && me.pageData.Menu.Items
            && me.pageData.Menu.Items.length;

        me.menuPanel = Ext.create('PIS.SubPortalMenuPanel', {
            region: 'west',

            title: me.pageData.Title,
            data: me.pageData.Menu,

            collapsible: true,
            hidden: !isMenuValid,
            collapseDirection: 'left',
            split: true
        });

        me.stagePanel = Ext.create('PIS.StagePanel', {
            region: 'center',
            height: '100%',
            listeners: {
                afterrender: function () {
                    Ext.defer(function () {
                        var _pgCode = me.pageData.Code;
                        var _pgType = me.pageData.Type;
                        var _pgPath = me.pageData.Path;

                        if (_pgType === "Url") {
                            PISSubPortal.loadPage(_pgPath, true);
                        } else {
                            PISSubPortal.loadPage(_pgCode);
                        }
                    }, 100);
                }
            }
        });

        me.mainPanel = Ext.create("PIS.Panel", {
            region: 'center',
            layout: 'border',
            border: false,
            items: [me.menuPanel, me.stagePanel]
        });

        config.items = [me.mainPanel]

        this.callParent([config]);
    },

    initComponent: function () {
        this.callParent(arguments);
    },

    loadPage: function (path, isurl) {
        var _url;

        if (isurl === true) {
            _url = path;
        } else {
            _url = PISConfig.StagePagePath + "?code=" + path;
        }

        this.stagePanel.frameContent.dom.src = _url;
    },

    statics: {
    }
});

//----------------------PIS ExtJs 设置菜单 开始----------------------//

Ext.define('PIS.SubPortalMenuItem', {
    extend: 'Ext.data.Model',
    fields: [
            { name: 'Id', type: 'string' },
            { name: 'Code', type: 'string' },
            { name: 'Name', type: 'string' },
            { name: 'Type', type: 'string' },
            { name: 'ParentId', type: 'string' },
            { name: 'Path', type: 'string' },
            { name: 'PathLevel', type: 'string' },
            { name: 'leaf', type: 'string' },
            { name: 'SortIndex', type: 'string' },
            { name: 'Url', type: 'string' },
            { name: 'Icon', type: 'string' },
            { name: 'Description', type: 'string' },
            { name: 'Status', type: 'string' }
    ]
});

Ext.define('PIS.SubPortalMenuStore', {
    extend: 'PIS.data.TreeStore',
    alias: 'store.pis-subportalmenustore',

    autoLoad: true,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            model: 'PIS.SubPortalMenuItem',
            rootPathLevel: 1,
            expanded: false
        }, config);

        me.callParent([config]);
    }
});

Ext.define('PIS.SubPortalMenuPanel', {
    extend: 'PIS.TreePanel',
    alias: 'widget.pis-subportalmenupanel',
    headerPanel: null,
    menuPanel: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            hideHeaders: true,
            rootVisible: false,
            useArrows: true,
            border: false,
            noline: true,
            collapsible: false,
            split: false,
            width: 180,
            expanded: true,
            minSize: 150,
            maxSize: 250,
            margins: '0 0 0 0'
        }, config);

        var data = (!config.data ? null : me.adjustNodeData(config.data));

        config.store = Ext.create('PIS.SubPortalMenuStore', { root: data });

        config.columns = [
            { id: 'col_SortIndex', dataIndex: 'SortIndex', header: '排序号', hidden: true },
            { id: 'col_Name', dataIndex: 'Name', flex: 1, header: "名称", xtype: 'treecolumn', renderer: PIS.SubPortalMenuPanel.NameRenderer }
        ];

        me.callParent([config]);

        me.expandAll();

        me.on("cellclick", function (cmp, td, cellIdx, rec, tr, rowIdx, e, eOpts) {
            var code = rec.get('Code');

            PISSubPortal.loadPage(code);
        });

        me.on("select", function (cmp, rec, e, a) {
            var code = rec.get('Code');

            PISSubPortal.loadPage(code);
        });
    },

    adjustNodeData: function (data) {
        var me = this;

        if (Ext.isArray(data) && data.length > 0) {
            Ext.each(data, function (node) {
                node['Icon'] = me.getIconUrl(node['Icon']);
            });
        }

        return data;
    },

    getIconUrl: function (icon, baseurl) {
        var baseurl = baseurl || ICON_IMG_BASE;
        var iconurl = icon;

        if (iconurl) {
            if (iconurl.indexOf('/') < 0) {
                iconurl = baseurl + iconurl;
            }
        }

        return iconurl;
    },

    statics: {
        NameRenderer: function (val, meta, rec, rowIdx, colIdx, store, view) {
            var rtn = val || "";

            return rtn;
        }
    }
});

//----------------------PIS ExtJs 设置菜单 结束----------------------//