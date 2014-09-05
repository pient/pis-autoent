Ext.define('PIS.view.setup.mdl.ModuleFormWin', {
    extend: 'PIS.view.base.BaseFormWin',
    title: '系统模块',

    requires: [
        'PIS.model.sys.mdl.Module'
    ],

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
        }, config);

        me.form = Ext.widget({
            xtype: 'pis-frm',
            autolayout: false,
            items: [
                { xtype: 'pis-hidden', fieldLabel: 'Id', name: 'Id' },
                { xtype: 'pis-hidden', fieldLabel: 'ParentId', name: 'ParentId' },
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
                            }, {
                                xtype: 'pis-enumbox',
                                fieldLabel: '状态',
                                allowBlank: false,
                                name: 'Status',
                                anchor: '95%',
                                value: false,
                                enumdata: PIS.ModuleModel.StatusEnum
                            }]
                    }, {
                        xtype: 'container',
                        flex: 1,
                        layout: 'anchor',
                        items: [{
                            xtype: 'pis-enumbox',
                            enumdata: PIS.ModuleModel.TypeEnum,
                            value: 'Auto',
                            fieldLabel: '类型',
                            allowBlank: false,
                            name: 'Type',
                            anchor: '100%'
                        }, {
                            xtype: 'pis-text',
                            fieldLabel: '排序号',
                            name: 'SortIndex',
                            anchor: '100%',
                            value: 0
                        }, {
                            xtype: 'pis-text',
                            fieldLabel: '图标',
                            name: 'Icon',
                            anchor: '100%'
                        }]
                    }]
                }, {
                    xtype: 'pis-text',
                    fieldLabel: '路径',
                    name: 'MdlPath',
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