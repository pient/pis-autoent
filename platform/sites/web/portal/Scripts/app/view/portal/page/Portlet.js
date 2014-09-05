Ext.define('PIS.view.portal.page.Portlet', {
    extend: 'PIS.Panel',
    alias: 'widget.pis-portlet',
    alternateClassName: 'PIS.Portlet',

    requires: [
    ],

    layoutData: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
        }, config);

        me.layoutData = config.layoutData;

        config.items = config.layoutData;

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

    loadPortlets: function () {
        var me = this;
    },

    statics: {
    }
});

