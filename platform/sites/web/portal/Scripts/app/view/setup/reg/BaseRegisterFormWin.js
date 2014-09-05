Ext.define('PIS.view.setup.reg.BaseRegisterFormWin', {
    extend: 'PIS.view.base.BaseFormWin',
    title: '注册信息',

    requires: [
        'PIS.model.sys.reg.Register'
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