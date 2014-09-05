Ext.define('PIS.view.portal.page.Home', {
    extend: 'PIS.Page',
    alternateClassName: 'PIS.HomeView',

    requires: [
        'PIS.view.portal.page.Portal'
    ],

    pageData: {},
    layoutData: {},
    mainPanel: null,
    portalPanel: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            layout: 'border'
        }, config);

        me.pageData = config.pageData;

        me.mainPanel = Ext.create('PIS.Panel', {
            region: 'center',
            border: false,
            title: me.pageData.Title,
        });

        me.items = me.mainPanel;

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);

        me.on('afterrender', function () {
            // 获取Layout
            me.loadLayout();
        });
    },

    loadLayout: function () {
        var me = this;

        PISAjaxRequest({
            method: 'GET',
            url: '/api/Page/GetLayout?code=Portal',
            onsuccess: function (response, opts) {
                me.layoutData = Ext.decode(response.responseText);
                me.layoutData.Layout = Ext.decode(me.layoutData.Layout);

                me.renderPortal();

                return true;
            }
        });
    },

    renderPortal: function () {
        var me = this;

        me.mainPanel.removeAll();

        me.portalPanel = Ext.create('PIS.view.portal.page.Portal', {
            layoutItems: me.layoutData.Layout
        });

        me.mainPanel.add(me.portalPanel);
    },

    statics: {
    }
});

