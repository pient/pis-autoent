
//------------------------PIS ExtJs 表单面板 开始------------------------//

function ExtDummyPost(config) {
    if (!Ext.fly('frmDummy')) {
        var frm = document.createElement('form');
        frm.id = 'frmDummy';
        frm.name = config.name || 'frmDummy';
        frm.className = 'x-hidden';
        document.body.appendChild(frm);
    }

    config = Ext.apply({
        url: location.href,
        method: 'POST',
        form: Ext.fly('frmDummy')
    }, config || {});

    Ext.Ajax.request(config);
}

Ext.define('PIS.form.FormPanelMixin', {
    extend: 'Ext.form.FormPanel',
    alias: 'widget.pis-formpanelmi',

    getAutoLayoutItems: function (items, cfg) {
        var me = this;
        var autoitems = [];
        var colcount = cfg.columns || me.columns || 2;
        var autoitems = [];

        if (items && $.isArray(items)) {
            // 开始自动布局
            var tautoitem;  // autoitem为表单域的容器
            for (var i = 0, pi = 0; i < items.length; i++) {
                var titem = items[i];

                if ((titem.isfield == false)) {
                    autoitems.push(titem);
                    continue;
                }

                var tfitem = { xtype: 'container', layout: 'anchor', flex: titem.flex, items: [titem] };

                var tflexcount = 0;
                if (tautoitem) {
                    // 计算当前autoitem的flex 
                    tflexcount = me.calcAutoItemTotalFlex(tautoitem);
                }

                // 如果当前域段度过大，则自动换行(即构建新的autoitem)
                if (i == 0 || (tflexcount + titem.flex) > pi) {
                    tautoitem = { xtype: 'container', layout: 'hbox', items: [] };
                    tautoitem.anchor = tautoitem.anchor || '-20';
                    autoitems.push(tautoitem);
                    pi = colcount;
                }

                tautoitem.items.push(tfitem);
            }

            $.each(autoitems, function () {
                tautoitem = this;

                if (tautoitem.isfield == false) {
                    return true;
                }

                var tflexcount = me.calcAutoItemTotalFlex(tautoitem);
                if (tautoitem && tflexcount < colcount) {
                    tautoitem.items.push({ unstyled: true, flex: (colcount - tflexcount) });    // 添加panel以维持对齐状态
                }
            });
        } else {
            autoitems = items;
        }

        return autoitems;
    },

    // 计算一个自动生成的HBox Item flex和
    calcAutoItemTotalFlex: function (autoitem) {
        var tflexcount = 0;
        $.each(autoitem.items, function () {
            var tflex = this.flex;
            if (tflex !== 0) { tflex = tflex || 1 };
            tflexcount += tflex;
        });

        return tflexcount;
    }
});

Ext.define('PIS.FormWin', {
    extend: 'Ext.window.Window',
    alias: ['widget.pis-formwin', 'widget.pis-frmwin'],

    owner: null,
    form: null,
    action: null,

    constructor: function (config) {
        config = Ext.apply({
            closeAction: 'hide',
            resizable: true,
            layout: 'fit',

            width: 600,
            height: 350,
            minWidth: 400,
            minHeight: 200,

            modal: true,
            tools: [{ type: 'help' }]
        }, config);

        this.callParent([config]);
    },

    getForm: function () {
        return this.form;
    },

    getRecord: function () {
        return this.getForm().getRecord();
    },

    load: function (owner, action, rec) {
        var me = this;

        me.action = action;
        me.owner = owner;

        var _f = this.getForm();

        if (rec) {
            me.form.loadRecord(rec);
        } else {
            me.form.reset();
        }

        var is_view = ("view" === me.action);

        me.setReadonly(is_view)

        me.show();
    },

    loadRecord: function (rec, action) {
        return this.getForm().loadRecord(rec);
    },

    setReadonly: function (flag) {
        var me = this;

        me.form.setReadOnly(flag);
        var btns = me.query(".button");

        Ext.each(btns, function (btn) {
            btn.setVisible(!(flag && btn.pisexecutable));
        });
    },

    reset: function () {
        this.getForm().reset();
    },

    submit: function (params) {
        return this.getForm().submit(params);
    }
});

Ext.define('PIS.FormPanel', {
    extend: 'Ext.form.FormPanel',
    alias: ['widget.pis-form', 'widget.pis-frm'],
    mixins: ['PIS.form.FormPanelMixin'],

    constructor: function (config) {
        var me = this;

        var _fieldDefaults = Ext.apply({
            labelAlign: 'left',
            labelWidth: 80,
            msgTarget: 'side'
        }, config.fieldDefaults || {});

        config = Ext.apply({
            region: 'center',
            autoHeight: true,
            autoScroll: true,
            border: false,
            frame: false,
            url: '#',
            bodyPadding: 10,
            labelWidth: 80,
            autolayout: true
        }, config);

        config.fieldDefaults = _fieldDefaults;

        if (config.tbar) { config.tbar.formpanel = me; }

        // 自动布局
        if (config.autolayout) {
            var colcount = config.columns || 2;
            var autoitems = [];
            if (config.items && $.isArray(config.items)) {
                // 预处理
                $.each(config.items, function () {
                    if (this.hidden == true) { this.flex = 0; }
                    if (this.flex !== 0) { this.flex = (this.flex || 1); }
                    if (this.flex > colcount) { this.flex = colcount; } // flex不应当大于colcount
                    this.pishandler = this.pishandler || config.pishandler;
                    this.xtype = this.xtype || "pis-textfield";
                    this.labelWidth = this.labelWidth || config.labelWidth;
                    this.anchor = this.anchor || config.anchor || "-10";
                });

                config.items = me.getAutoLayoutItems(config.items, { columns: colcount });
            }
        }

        me.callParent([config]);
    },

    initComponent: function () {
        var me = this;
        me.callParent(arguments);

        me.on('afterrender', function () {
            me.form.setValues(me.frmdata || {});

            if ('r'.equals(pgOp)) {
                me.setReadOnly(true);
            }
        })
    },

    setDisabled: function (b) {
        var me = this;
        var toolbars = me.getToolbars();
        Ext.each(toolbars, function (toolbar) { toolbar.setDisabled(b); });

        var fields = me.getForm().getFields().getRange();
        Ext.each(fields, function (f) {
            f.setDisabled(b);
        });
    },

    setReadOnly: function (b) {
        var me = this;
        var toolbars = me.getToolbars();
        Ext.each(toolbars, function (toolbar) { toolbar.setReadOnly(b); });

        var fields = me.getForm().getFields().getRange();

        Ext.each(fields, function (f) {
            f.setReadOnly(b);
            //var r_flag = true;

            //if (f.onRenderReadView) {
            //    r_flag = f.onRenderReadView();
            //}

            //if (r_flag) {
            //    var f_child = $(f.bodyEl.dom).children().first();
            //    f_child.css("visibility", (b ? "hidden" : "visible"));

            //    var text;
            //    if (f.getRawValue) { text = f.getRawValue(); }
            //    else text = f.getValue();

            //    var f_view = f_child.find("span.pis-field-view");
            //    if (b == true) {
            //        if (f_view.length > 0) {
            //            f_view.css("display", "");
            //        } else {
            //            f_child.before("<span class='pis-field-view'>" + text + "</span>");
            //        }
            //    } else {
            //        if (f_view.length > 0) {
            //            f_view.css("display", "none");
            //        }
            //    }
            //}
        });
    },

    findField: function (id) {
        return this.getForm().findField(id);
    },

    getToolbars: function () {
        var toolbars = this.getDockedItems('toolbar[dock="top"]');
        return toolbars;
    },

    reset: function () {
        this.getForm().reset();
    },

    submit: function (request) {
        var me = this;

        if (!me.form.isValid()) {
            return false;
        }

        request = Ext.apply({
        }, request);

        // 验证数据
        var frmdata = me.form.getValues();

        var params = request.params = Ext.apply(frmdata, (request.params || {}));

        var funccols = {
            callback: params.callback || me.callback || null,
            onrequest: params.onrequest || me.onrequest || null,
            afterrequest: params.afterrequest || me.afterrequest || null,
            onfailure: params.onfailure || me.onfailure || null,
            afterfailure: params.afterfailure || me.afterfailure || null
        };

        request.callback = function (opts, success, resp) {
            var flag = true;

            me.form.clearInvalid();

            if (funccols.callback) { flag = funccols.callback(opts, success, resp); }

            return flag;
        }

        request.onrequest = function (respdata, opts) {
            var flag = true;
            if (funccols.onrequest) { flag = funccols.onrequest(me, respdata, opts); }

            return flag;
        }

        request.afterexpection = function (respdata, opts) {
            return false
        }

        request.afterrequest = function (respdata, opts) {
            var flag = true;
            if (funccols.afterrequest) { flag = funccols.afterrequest(me, respdata, opts); }

            if (respdata.isexception) {
                return false;
            }

            if (flag !== false && params.openercallback) {
                var opener = window.opener || window.dialogArguments;
                if (opener && opener.pgDialogCallback) {
                    flag = opener.pgDialogCallback({ respdata: respdata, options: opts });  // 调用opener页面回调函数
                }

                if (flag != false) {
                    // 延迟关闭本页面
                    Ext.defer(function () {
                        window.close();
                    }, 100);
                }
            }

            return flag;
        }

        request.onfailure = function (resp, opts) {
            var flag = true;
            if (funccols.onfailure) { flag = funccols.onfailure(me, respdata, opts); }

            return flag;
        }

        request.afterfailure = function (resp, opts) {
            var flag = true;
            if (funccols.afterfailure) { flag = funccols.afterfailure(me, respdata, opts); }

            return flag;
        }

        PISAjaxRequest(request);
    }
});

Ext.define('PIS.FormToolbar', {
    extend: 'PIS.Toolbar',
    alias: 'widget.pis-formtoolbar',
    formpanel: null,
    constructor: function (config) {
        config = Ext.apply({}, config);

        this.callParent([config]);
    },

    initComponent: function () {
        this.callParent(arguments);
    }
});


Ext.define('PIS.FormDescPanel', {
    extend: 'PIS.Panel',
    alias: 'widget.pis-formdescpanel',
    constructor: function (config) {
        config = Ext.apply({
            id: 'form-desc',
            height: 80,
            margin: "0",
            border: false,
            frame: false,
            isfield: false,
            html: ""
        }, config);

        config.html = "<hr />" + config.html;

        this.callParent([config]);
    },

    initComponent: function () {
        this.callParent(arguments);
    }
});

Ext.define('PIS.GridFormPanel', {
    extend: 'PIS.FormPanel',
    alias: 'widget.pis-gridformpanel',
    grid: null,
    constructor: function (config) {
        var me = this;
        config = Ext.apply({
        }, config);

        me.callParent([config]);
    }
});

//------------------------PIS ExtJs 表单面板 结束------------------------//