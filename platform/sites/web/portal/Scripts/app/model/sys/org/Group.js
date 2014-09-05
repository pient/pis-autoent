
Ext.define('PIS.model.sys.org.Group', {
    extend: 'Ext.data.Model',

    idProperty: 'Id',

    fields: [
        { name: 'Id', type: 'string' },
        { name: 'Code', type: 'string' },
        { name: 'Name', type: 'string' },
        { name: 'Type', type: 'string' },
        { name: 'Status', type: 'string', defaultValue: 'Enabled' },
        { name: 'Description', type: 'string' },
        { name: 'Tag', type: 'string' },
        { name: 'ParentId', type: 'string' },
        { name: 'Path', type: 'string' },
        { name: 'SortIndex', type: 'int' },
        { name: 'leaf', type: 'boolean', defaultValue: false }
    ],

    validations: [
        { type: 'length', field: 'Code', min: 2 },
        { type: 'length', field: 'Name', min: 2 }
    ]
});