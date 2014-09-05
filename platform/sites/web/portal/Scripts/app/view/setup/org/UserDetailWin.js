
Ext.define('PIS.view.setup.org.UserDetailWin', {
    extend: 'Ext.window.Window',
    alias: ['widget.pis-userdetailwin'],
    alternateClassName: 'PIS.UserDetailWin',

    title: '用户详细信息',

    mainPanel: null,
    userPanel: null,

    record: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            closeAction: 'hide',
            resizable: true,
            layout: 'fit',
            border: false,

            width: 700,
            height: 550,

            modal: true,
            tools: [{ type: 'help' }]
        }, config);

        me.mainPanel = Ext.widget('tabpanel', {
            activeTab: 0,
            items: [{
                title: '用户信息',
                layout: 'fit',
                listeners: {
                    activate: function (tab) {
                        if (!me.userPanel) {
                            me.userPanel = Ext.create('PIS.view.setup.org.UserForm', {
                                win: me
                            });

                            tab.add(me.userPanel);
                        }

                        me.userPanel.loadRecord(me.record);
                    }
                }
            }, {
                title: '用户组',
                layout: 'fit',
                listeners: {
                    activate: function (tab) {
                        if (!me.groupPanel) {
                            me.groupPanel = Ext.create('PIS.view.setup.org.UserGroup', {
                                win: me
                            });

                            tab.add(me.groupPanel);
                        }
                    }
                }
            }, {
                title: '角色'
            }, {
                title: '权限'
            }]
        });

        me.items = me.mainPanel;

        this.callParent([config]);
    },

    load: function (owner, rec, action) {
        var me = this;

        if (rec) {
            me.setTitle("用户 " + rec.get("Name") + " 详细信息");
            me.record = rec;

            me.mainPanel.setActiveTab(0);
            me.userPanel.loadRecord(rec);
        }

        me.show();
    },

    reset: function () {
    },

    onSaveClick: function () {
        var me = this;
    },

    doHide: function () {
        var me = this;

        var _win = me.up("window");

        _win.reset();
        _win.hide();
    }
});