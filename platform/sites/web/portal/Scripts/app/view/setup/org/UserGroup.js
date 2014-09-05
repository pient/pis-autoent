Ext.define('PIS.view.setup.org.UserGroup', {
    extend: 'PIS.TreePanel',
    alternateClassName: 'PIS.UserGroupSetup',

    win: null,
    record: null,

    constructor: function (config) {
        var me = this;

        me.win = config.win;

        me.dataStore = Ext.create('PIS.data.TreeStore', {
            root: null,
            model: 'PIS.model.sys.org.Group',
            proxy: {
                noCache: false,
                type: 'ajax',
                url: '/api/Portal/GetSysStructuredData?name=OrgGroup',
                extractResponseData: function (response) {
                    // 处理返回数据
                    return response
                }
            }
        });

        config = Ext.apply({
            autolayout: false,
            store: me.dataStore,
            buttons: [{
                xtype: 'pis-savebutton',
                handler: function () { me.win.onSaveClick }
            }, {
                xtype: 'pis-cancelbutton',
                handler: me.win.doHide
            }]
        }, config);

        me.columns = [
            { dataIndex: 'Name', xtype: 'treecolumn', text: '名称', flex: 1 },
            { dataIndex: 'Code', text: '编号', flex: 1 }
        ];

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);

        me.dataStore.load();
    },

    reset: function () {

    },

    load: function (rec) {
        var me = this;

        if (me.record != rec) {
            me.record = rec;
        }
    }
});