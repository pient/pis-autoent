Ext.define('PIS.view.setup.reg.PageLayoutFormWin', {
    extend: 'PIS.view.setup.reg.BaseRegisterFormWin',

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            height: 530,
            width: 800
        }, config);

        me.form = Ext.widget({
            xtype: 'pis-frm',
            autolayout: false,
            items: [
                { xtype: 'pis-hidden', fieldLabel: 'Id', name: 'Id' },
                { xtype: 'pis-hidden', fieldLabel: 'ParentId', name: 'ParentId' },
                { xtype: 'pis-hidden', fieldLabel: 'RegDataType', name: 'RegDataType' },
                {
                    xtype: 'container',
                    anchor: '100%',
                    layout: 'hbox',
                    items: [{
                        xtype: 'container',
                        flex: 1,
                        layout: 'anchor',
                        items: [
                            {
                                xtype: 'pis-text',
                                fieldLabel: '名称',
                                allowBlank: false,
                                name: 'Name',
                                anchor: '95%',
                                value: ''
                            }, {
                                xtype: 'pis-text',
                                fieldLabel: '编号',
                                allowBlank: false,
                                name: 'Code',
                                anchor: '95%'
                            }]
                    }, {
                        xtype: 'container',
                        flex: 1,
                        layout: 'anchor',
                        items: [{
                            xtype: 'pis-text',
                            fieldLabel: '排序号',
                            name: 'SortIndex',
                            anchor: '100%',
                            value: 0
                        }, {
                            xtype: 'pis-enumbox',
                            enumdata: PIS.RegisterModel.StatusEnum,
                            value: 'Enabled',
                            fieldLabel: '状态',
                            allowBlank: false,
                            name: 'Status',
                            anchor: '100%',
                            value: 0
                        }]
                    }]
                }, {
                    xtype: 'pis-text',
                    fieldLabel: '显示数据',
                    name: 'DisplayData',
                    anchor: '100%',
                    value: ''
                }, {
                    xtype: 'pis-textarea',
                    fieldLabel: '布局数据',
                    name: 'Data',
                    anchor: '100%',
                    height: 350,
                    value: ''
                }, {
                    xtype: 'pis-hidden',
                    fieldLabel: '编辑页面',
                    name: 'EditPage',
                    anchor: '100%',
                    value: ''
                }, {
                    xtype: 'pis-textarea',
                    fieldLabel: '描述',
                    name: 'Description',
                    anchor: '100%',
                    value: ''
                }],

            buttons: [{
                xtype: 'pis-savebutton',
                handler: me.onSaveClick
            }, {
                xtype: 'pis-cancelbutton',
                handler: me.doHide
            }]
        });

        me.items = me.form;

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    }
});