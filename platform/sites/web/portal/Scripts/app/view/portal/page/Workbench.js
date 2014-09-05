Ext.define('PIS.view.portal.page.Workbench', {
    extend: 'PIS.Page',
    alternateClassName: 'PIS.WorkbenchView',

    requires: [
    ],

    pageData: {},
    mainPanel: null,

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
    },

    statics: {
    }
});