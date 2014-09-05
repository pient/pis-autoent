Ext.define('PIS.view.setup.org.BaseOrgFormWin', {
    extend: 'PIS.view.base.BaseFormWin',
    title: '组织权限',

    requires: [
        'PIS.model.sys.org.*'
    ],

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
        }, config);

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    }
});