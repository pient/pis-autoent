
Ext.define('PIS.model.sys.reg.Register', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIS.RegisterModel',

    idProperty: 'Id',

    fields: [
        { name: 'Id', type: 'string' },
        { name: 'ParentId', type: 'string' },
        { name: 'Code', type: 'string' },
        { name: 'Name', type: 'string' },
        { name: 'SortIndex', type: 'int' },
        { name: 'leaf', type: 'boolean', defaultValue: false },
        { name: 'Status', type: 'string', defaultValue: 'Enabled' },
        { name: 'EditPage', type: 'string' },
        { name: 'DisplayData', type: 'string' },
        { name: 'RegDataType', type: 'string', defaultValue: 'Default' },
        { name: 'Data', type: 'string' },
        { name: 'Tag', type: 'string' },
        { name: 'Description', type: 'string' }
    ],

    validations: [
        { type: 'length', field: 'Code', min: 2 },
        { type: 'length', field: 'Name', min: 2 }
    ],

    statics: {
        StatusEnum: { Enabled: '启用', Diabled: '停用' },
        RegDataTypeEnum: { Default: '默认', Enum: '枚举', Config: '配置' }
    }
});