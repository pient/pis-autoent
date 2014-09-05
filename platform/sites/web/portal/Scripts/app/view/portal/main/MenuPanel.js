Ext.define('PIS.view.portal.main.MenuPanel', {
    extend: 'PIS.TreePanel',
    alias: 'widget.pis-portalmenupanel',
    alternateClassName: 'PIS.PortalMenuPanel',

    requires: [
        'PIS.model.ptl.ModuleItem',
    ],

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

        var data = me.adjustNodeData(config.data || []);
        
        config.store = Ext.create('PIS.data.TreeStore', {
            model: 'PIS.model.ptl.ModuleItem',
            root: data,
            iconProperty: 'Icon',
            expanded: true
        });

        config.tbar = Ext.create("PIS.Panel", {
            border: false,
            height: 68,
            contentEl: 'app_menu_header',
            bodyStyle: "background:#f5f5f5; padding: 5px;"
        });

        config.columns = [
            { id: 'col_SortIndex', dataIndex: 'SortIndex', header: '排序号', hidden: true },
            { id: 'col_Title', dataIndex: 'Title', flex: 1, header: "名称", xtype: 'treecolumn', renderer: PIS.PortalMenuPanel.NameRenderer }
        ];

        me.callParent([config]);

        me.expandAll();

        me.on("cellclick", function (cmp, td, cellIdx, rec, tr, rowIdx, e, eOpts) {
            PISPortal.loadPage(rec);
        });

        me.on("select", function (cmp, rec, e, a) {
            PISPortal.loadPage(rec);
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

            //if (rec.get("PathLevel") == PIS.PortalMenuItem.MenuRootPathLevel) {
            //    rtn = '<span style="font-weight: bold; margin-top:10px; color:gray">' + val + '</span>';
            //}

            //if ('M_SYS_FAV_MSG'.equals(rec.get('Code'))) {
            //    rtn += '<span style="font-weight:bold; color: red; margin:2px;">(<span id="mitem_msgnew">0</span>)</span>'
            //}

            //if ('M_SYS_FAV_TASK'.equals(rec.get('Code'))) {
            //    rtn += '<span style="font-weight:bold; color: red; margin:2px;">(<span id="mitem_tasknew">0</span>)</span>'
            //}


            return rtn;
        }
    }
});