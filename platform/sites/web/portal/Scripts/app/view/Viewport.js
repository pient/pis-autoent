
Ext.define('PIS.view.Viewport', {
    extend: 'Ext.Viewport',
    alias: 'widget.pis-portalviewport',
    alternateClassName: 'PIS.PortalViewport',

    PG_TIMER_INTERVAL: 600000,  // 600s

    pageData: {},

    funcPanel: null,
    menuPanel: null,
    stagePanel: null,
    mainPanel: null,
    footerPanel: null,

    pgTimerEnabled: false,
    pgTimer: null,

    layout: 'fit',

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            layout: 'border'
        }, config);

        me.pageData = config.pageData;

        me.menuPanel = Ext.create('PIS.view.portal.main.MenuPanel', {
            region: 'west',

            title: me.pageData.Title,
            data: me.pageData.Menu,

            collapsible: true,
            split: true,
            collapseDirection: 'left'
        });

        me.stagePanel = Ext.create('PIS.view.portal.main.StagePanel', {
            region: 'center',
            height: '100%',
            listeners: {
                afterrender: function () {
                    $('#app_footer').show();

                    Ext.defer(function () {
                        me.loadHome();
                        // me.loadWorkbench();
                        // me.loadSetting();
                    }, 100);
                }
            }
        });

        me.funcPanel = Ext.create("PIS.Panel", {
            applyTo: 'func_bar',
            margins: '0 0 0 0',
            bodyStyle: "background:transparent; margin: 5px;",
            width: 260,
            border: false,
            contentEl: 'func_content'
        });

        me.mainPanel = Ext.create("PIS.Panel", {
            region: 'center',
            layout: 'border',
            margins: '40 0 0 0',
            border: false,
            cls: 'empty',
            bodyStyle: 'background:#dce2e7;',
            items: [me.menuPanel, me.stagePanel]
        });

        me.footerPanel = Ext.create("PIS.Panel", {
            region: 'south',
            margins: '0 0 0 0',
            cls: 'empty',
            border: false,
            bodyStyle: 'background:#f1f1f1',
            contentEl: 'app_footer'
        });

        config.items = [me.mainPanel, me.footerPanel]

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        me.callParent(arguments);

        me.addEvents('pgTimeElapsed');
    },

    enablePgTimer: function () {
        var me = this;
        me.pgTimerEnabled = true;

        pgTimer = setTimeout(pgTimeElapsed, PG_TIMER_INTERVAL);

        function pgTimeElapsed() {
            if (me.pgTimerEnabled == true) {
                me.fireEvent('pgTimeElapsed', me);

                me.pgTimer = setTimeout(me.pgTimeElapsed, me.PG_TIMER_INTERVAL);
            }
        }
    },

    loadPage: function (code) {
        this.stagePanel.loadPage(code);
    },

    loadWorkbench: function () {
        this.menuPanel.selectPath("//M_FAV/M_FAV_WORK", "Code");
    },

    loadHome: function () {
        this.menuPanel.selectPath("//M_FAV/M_FAV_HOME", "Code");
    },

    refreshTaskMsg: function () {
        ExtAjaxRequest('msgtask', {}, {
            afterrequest: function (respData, opts) {
                $("#lbl_msgnew").html(respData.NewMsgCount);
                $("#lbl_tasknew").html(respData.NewTaskCount);

                $("#mitem_msgnew").html(respData.NewMsgCount);
                $("#mitem_tasknew").html(respData.NewTaskCount);
            }
        });
    },

    relogin: function () {
        Ext.defer(function () {
            location.href = '/Account/SignOut';
        }, 200);
    },

    exit: function () {
        if (!PISPortal.exitting) {
            Ext.defer(function () {
                location.href = '/Account/SignOut?action=exit';
            }, 200);

            PISPortal.exitting = true;

            window.close();
        }
    },

    reloadMdlOrgAuth: function (params) {
        var params = params || {};

        PISAjaxRequest({
            url: '/api/Portal/ReloadMdlOrgAuth',
            onsuccess: function (response, opts) {
                if (params.callback) {
                    params.callback.call();
                } else {
                    PISMsgBox.info("刷新模块成功！");
                }

                return true;
            }
        });
    },

    reloadRegistry: function (params) {
        var params = params || {};

        PISAjaxRequest({
            url: '/api/Portal/ReloadRegistry',
            onsuccess: function (response, opts) {
                if (params.callback) {
                    params.callback.call();
                } else {
                    PISMsgBox.info("加载注册信息成功！");
                }

                return true;
            }
        });
    },

    statics: {
        OnFuncClick: function (arg) {
            switch (arg) {
                case "msg":
                    onPgTimeElapsed();
                    PISOpenDialog("/portal/Modules/Msg/SysMessageMag.aspx?type=received", {}, { width: 780, height: 450, scrollbars: 'auto' });
                    break;
                case "task":
                    onPgTimeElapsed();
                    PISOpenDialog("/portal/Modules/Workflow/SysWfActionMag.aspx", {}, { width: 780, height: 450, scrollbars: 'auto' });
                    break;
                case "date":
                    PISOpenDialog("/portal/CommonPages/calendar/Calendar.htm?Year=" + tYear + "&Month=" + tMonth + "&Day=" + tDay, {}, { width: 850, height: 450, scrollbars: 'auto' });
                    break;
            }
        },

        OnStageLoad: function (frame) {
            // 加载路径
            if (!frame.contentWindow.PISPath) {
                PISPortal.stagePanel.loadTitle();
            }
        }
    }
});
