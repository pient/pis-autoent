var PISPortal = null;
var pgOp = "";

Ext.application({
    name: 'PIS',
    enabled: true,
    paths: {
        'PIS': '/Scripts/app'
    },

    autoCreateViewport: false,

    init: function () {
        Ext.tip.QuickTipManager.init();

        Ext.Ajax.defaultHeaders = {
            'Accept': 'application/json'
        };

        PISAjaxRequest({
            method: 'GET',
            url: '/api/portal/GetPageData',
            onsuccess: function (response, opts) {
                var pageData = Ext.decode(response.responseText);

                PISPortal = Ext.create('PIS.view.Viewport', { pageData: pageData });
            },
            onfailure: function (response, opts) {
                alert('请求失败,错误代码：' + response.status)
            }
        });
    }
});
