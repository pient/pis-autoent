
Ext.define('PIS.model.sys.org.User', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIS.UserModel',

    idProperty: 'Id',

    fields: [
        { name: 'Id', type: 'string' },
        { name: 'Code', type: 'string' },
        { name: 'LoginName', type: 'string' },
        { name: 'Name', type: 'string' },
        { name: 'Type', type: 'string' },
        { name: 'Status', type: 'string', defaultValue: 'Enabled' },
        { name: 'Email', type: 'string' },
        { name: 'Tag', type: 'string' }
    ],

    validations: [
        { type: 'length', field: 'Code', min: 2 },
        { type: 'length', field: 'Name', min: 2 },
        { type: 'format', field: 'Email', matcher: PISValidator.RegExps.EMail }
    ],

    statics: {
        StatusEnum: { Enabled: '有效', Diabled: '无效' }
    }
});