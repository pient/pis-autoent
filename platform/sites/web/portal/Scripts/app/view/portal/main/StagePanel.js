

Ext.define('PIS.view.portal.main.StagePanel', {
    extend: 'PIS.StagePanel',
    alias: 'widget.pis-portalstagepanel',
    alternateClassName: 'PIS.PortalStagePanel',

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            border: false
        }, config);

        me.callParent([config]);
    },

    loadPage: function (pageRec) {
        var me = this;

        try{
            me.removeAll();

            var pagePath = pageRec.get("Path");
            var pageTitle = pageRec.get("Title");
            var pathType = pageRec.get("Type") || "Auto";

            if (pagePath) {
                if ('Auto' == pathType) {
                    if (pagePath.indexOf('/') >= 0 || pagePath.indexOf('\\') >= 0) {
                        pathType = 'Url'
                    } else {
                        pathType = 'View';
                    }
                }

                if ('View' === pathType) {
                    var v = 'PIS.' + pagePath;

                    var view = Ext.create(v, {
                        pageData: pageRec.data
                    });

                    me.add(view);

                } else if ('Url' === pathType) {
                    me.loadUrl(pagePath, pageTitle);
                }
            }
        } catch (ex) { }

        return;
    }
});