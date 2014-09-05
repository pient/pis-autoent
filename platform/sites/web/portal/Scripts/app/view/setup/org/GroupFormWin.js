Ext.define('PIS.view.setup.org.GroupFormWin', {
    extend: 'PIS.view.setup.org.BaseOrgFormWin',
    alternateClassName: 'PIS.GroupSetupFormWin',

    title: '用户组',

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            width: 200,
            height: 260,
        }, config);

        me.form = Ext.create('PIS.view.setup.org.GroupForm', { win: me });

        me.items = me.form;

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    }
});