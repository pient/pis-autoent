Ext.define('PIS.view.portal.page.Portal', {
    extend: 'PIS.Panel',
    alias: 'widget.pis-portalpage',
    alternateClassName: 'PIS.PortalPage',

    requires: [
    ],

    layoutItems: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            layout: 'column',
            margins: '35 5 5 0',
            border: false,
            autoScroll: true,
            height: "100%",
            defaults: {
                layout: 'anchor',
                defaults: {
                    anchor: '100%'
                }
            },
        }, config);

        config.items = me.layoutItems = config.layoutItems;

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);

        me.on('afterrender', function () {
            // 获取Layout
            me.loadPortlets();
        });
    },

    // Set columnWidth, and set first and last column classes to allow exact CSS targeting.
    beforeLayout: function () {
        return this.callParent(arguments);
    },

    loadPortlets: function () {
        var me = this;
    },

    statics: {
    }
});

