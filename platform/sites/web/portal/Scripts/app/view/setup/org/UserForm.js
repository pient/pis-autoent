Ext.define('PIS.view.setup.org.UserForm', {
    extend: 'PIS.FormPanel',
    alternateClassName: 'PIS.UserSetupForm',
    win: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            autolayout: false
        }, config);

        me.win = config.win;

        me.items = [{
            xtype: 'pis-hidden',
            fieldLabel: 'Id',
            name: 'Id'
        }, {
            xtype: 'pis-text',
            fieldLabel: '名称',
            allowBlank: false,
            name: 'Name',
            anchor: '100%',
            value: ''
        }, {
            xtype: 'pis-text',
            fieldLabel: '登录名',
            allowBlank: false,
            name: 'LoginName',
            anchor: '100%',
            value: ''
        }, {
            xtype: 'pis-text',
            fieldLabel: '编号',
            allowBlank: false,
            name: 'Code',
            anchor: '100%'
        }, {
            xtype: 'pis-text',
            fieldLabel: '电子邮箱',
            name: 'Email',
            anchor: '100%',
            value: ''
        }, {
            xtype: 'pis-enumbox',
            fieldLabel: '状态',
            allowBlank: false,
            name: 'Status',
            anchor: '100%',
            value: 'Enabled',
            enumdata: PIS.UserModel.StatusEnum
        }];

        me.buttons = [{
            xtype: 'pis-savebutton',
            handler: function () {
                me.win.onSaveClick();
            }
        }, {
            xtype: 'pis-cancelbutton',
            handler: me.win.doHide
        }];

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    }
});