
Ext.define('PIS.model.sys.mdl.Module', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIS.ModuleModel',

    idProperty: 'Id',

    fields: [
        { name: 'Id', type: 'string' },
        { name: 'ParentId', type: 'string' },
        { name: 'Code', type: 'string' },
        { name: 'Name', type: 'string' },
        { name: 'Path', type: 'string' },
        { name: 'SortIndex', type: 'int' },
        { name: 'leaf', type: 'boolean', defaultValue: false },
        { name: 'Type', type: 'string' },
        { name: 'MdlPath', type: 'string' },
        { name: 'Icon', type: 'string' },
        { name: 'Description', type: 'string' },
        { name: 'Status', type: 'string' }
    ],

    validations: [
        { type: 'length', field: 'Code', min: 2 },
        { type: 'length', field: 'Name', min: 2 }
    ],

    statics: {
        StatusEnum: { Enabled: '启用', Disabled: '停用' },
        TypeEnum: { Auto: '自动', Url: '网址', View: '视图' }
    }
});