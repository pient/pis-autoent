
Ext.define('PIS.model.ptl.ModuleItem', {
    extend: 'Ext.data.Model',

    idProperty: 'Id',

    fields: [
            { name: 'Id', type: 'string' },
            { name: 'Code', type: 'string' },
            { name: 'Title', type: 'string' },
            { name: 'Type', type: 'string' },
            { name: 'Path', type: 'string' },
            { name: 'Icon', type: 'string' },
            { name: 'Status', type: 'string' },

            { name: 'ParentId', type: 'string' },
            { name: 'SortIndex', type: 'string' },
            { name: 'PathLevel', type: 'string' },
            { name: 'leaf', type: 'string' }
    ],

    statics: {
        MenuRootPathLevel : 1
    }
});