/*
* pgctrl对ext的扩展
* @author Ray Liu
*/

PIS.ButtonDict = {};
PIS.PageNoticeCt = null;    // 用于用户页面提示

//-----------------------------PIS  方法 开始--------------------------//

function PISGetDialogStyle(style) {
    style = style || {};

    if (typeof (style) != "string") {
        style.dialogWidth = (style.dialogWidth || style.width || "750px").toString();
        style.dialogHeight = (style.dialogHeight || style.height || "550px").toString();
        if (style.dialogWidth.indexOf("px") < 0) style.dialogWidth += "px";
        if (style.dialogHeight.indexOf("px") < 0) style.dialogHeight += "px";
        style.scroll = style.scroll || "yes";
        style.center = style.center || "yes";
        style.status = style.status || "no";
        style.resizable = style.resizable || "yes";
        style = $.getStrByJsonObj(style, ":", ";");
    }

    return style;
}

function PISOpenDialog(args, params, style, onProcessFinish) {
    if (typeof (args) == "string") {
        var url = args;
        args = { url: url, params: params, style: style, onProcessFinish: onProcessFinish };
    }

    var tparams = Ext.apply(args.params || {}, params || {});
    var tstyle = PISGetDialogStyle(args.style);
    var turl = $.combineQueryUrl(args.url, tparams);

    var win = null;

    if (tparams.modalDialog == true) {
        win = window.showModalDialog(turl, window, tstyle);
    } else {
        win = window.showModelessDialog(turl, window, tstyle);
    }

    if (win) {
        if (typeof (args.onProcessFinish) == "function") {
            args.onProcessFinish.call(win, args.params);
        }
    }

    return win;
}

function PISOpenLink(url) {
    window.open(url, "_BLANK", "width=1,height=1");
}

function OpenVersionDialog(params, style) {
    PISOpenDialog(VERSION_PAGE_URL, params, style || { height: 600, width: 465, help: 0, resizable: 0, status: 0, scroll: 0 });
}

function OpenHelpDialog(params) {
    PISOpenDialog(HELP_PAGE_URL, params, style || { height: 600, width: 465, help: 0, resizable: 0, status: 0, scroll: 0 });
}

function PISApplyXTemplate(xtmpl, data) {
    var xtmpl = xtmpl;
    if (typeof xtmpl == "string") {
        xtmpl = new Ext.XTemplate(xtmpl);
    }

    var rtn = xtmpl.apply(data);
    return rtn;
}

function PISRenderFormLink(data) {
    var rtn = new Ext.XTemplate(
                "<span class='pis-formpage-link' style='float:left; margin:2px; border:0px'>",
                "<a class='pis-ui-link' onclick='PISOpenDialog({params})'>{text}</a>",
                "</span>"
        ).apply(data);

    return rtn;
}

function PISRenderFileLink(data) {
    if (typeof data == "string") {
        data = PISParseFileFullName(data);
    }
    
    var rtn = new Ext.XTemplate(
            '<tpl for=".">',
                '<span class="pis-ctrl-file-link" linkfile="{fullname}" style="float:left; margin:2px; border:0px">',
                '<a style="margin:5px; cursor: hand" title="{name}" onclick=PISOpenDownloadWin("{id}")>{name}</a>',
                '</span>',
            '</tpl>'
        ).apply(data);

    return rtn;
}

// 解析文件全名
function PISParseFileFullName(ffname) {
    var tffname = ffname.trimEnd(",");
    var fnames = ffname.split(",");
    var data = [];
    Ext.each(fnames, function (fname) {
        var id_pos_end = fname.indexOf("_");
        var id = fname.substring(0, id_pos_end);
        var name = fname.substring(id_pos_end + 1);
        data.push({ id: id, name: name, fullname: fname });
    });

    return data;
}

//-----------------------------PIS  方法 结束--------------------------//

//-----------------------------PIS ExtJs 事件 开始--------------------------//

Ext.define('PIS.Util', {
    statics: {
        getModelIds: function (models) {
            var ids = [];

            Ext.each(models, function (rec) {
                ids.push(rec.internalId);
            }, this);

            return ids;
        }
    }
});

Ext.define('PIS.Observable', {
    extend: 'Ext.util.Observable',

    constructor: function (config) {
        config = Ext.apply({}, config);
        this.callParent([config]);
    }
});

Ext.define('PIS.PageObservable', {
    extend: 'PIS.Observable',

    constructor: function (config) {
        config = Ext.apply({}, config);
        this.callParent([config]);
    },

    initComponent: function () {
        this.callParent(arguments);
    },

    onPgInit: function () { PISPgObserver.fireEvent('pginit'); },
    onFrmLoad: function () { PISPgObserver.fireEvent('frmload'); },
    onPgLoad: function () { PISPgObserver.fireEvent('pgload'); },
    onPgTimer: function () { PISPgObserver.fireEvent('pgtimer'); },
    onPgUnload: function () { PISPgObserver.fireEvent('pgunload'); },
    onDgCallback: function (args) { PISPgObserver.fireEvent('dgcallback', args); }

}, function () {
    PISPgObserver = PISPageObserver = new this();

    PISPgObserver.addEvents('pginit', 'frmload', 'pgload', 'pgtimer', 'pgunload', 'dgcallback');
});

//-----------------------------PIS ExtJs 事件 结束--------------------------//


//------------------------PIS ExtJs数据组件扩展 开始------------------------//

function PISAjaxRequest(request) {
    request = Ext.apply({
        method : 'POST',
        success: function (response, opts) {
            Ext.defer(function () {
                PISMsgBox.hide();

                if ('function' === typeof request.onsuccess) {
                    request.onsuccess.call(this, response, opts);
                } else {
                    if (response.__MSG) {
                        PISMsgBox.alert(response.MSG);
                    }
                }
            }, 100);
        },
        failure: function (response, opts) {
            Ext.defer(function () {
                PISMsgBox.hide();

                if (response.status) {
                    if ('function' === typeof request.onfailure) {
                        request.onfailure.call(response, opts);
                    } else {
                        PISMsgBox.error('请求失败,错误代码：' + response.status);
                    }
                }
            }, 100);
        }
    }, request, {
    });

    PISMsgBox.wait();

    Ext.Ajax.request(request);
}

Ext.define('PISValidator', {
    statics: {
        RegExps: {
            EMail: /^([\w]+)(.[\w]+)*@([\w-]+\.){1,5}([A-Za-z]){2,4}$/,
            Url: /(((^https?)|(^ftp)):\/\/((([\-\w]+\.)+\w{2,3}(\/[%\-\w]+(\.\w{2,})?)*(([\w\-\.\?\\\/+@&#;`~=%!]*)(\.\w{2,})?)*)|(localhost|LOCALHOST))\/?)/i,
            IPAddress: /^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$/
        }
    }
});

Ext.apply(Ext.data.validations, {
    number: function (config, value) {
        if (value === undefined) {
            return false;
        }
        var min = config.min,
            max = config.max;

        if ((min && value < min) || (max && value > max)) {
            return false;
        } else {
            return true;
        }
    },
    numberMessage: '数值范围错误。'
});

Ext.define('PIS.data.JsonReader', {
    extend: 'Ext.data.JsonReader',
    alias: 'reader.pisjson',
    alternateClassName: ['PIS.JsonReader'],

    constructor: function (config) {
        var me = this;

        this.callParent([config]);
    }
});

Ext.define('PIS.data.JsonWriter', {
    extend: 'Ext.data.JsonWriter',
    alias: 'writer.pisjson',

    constructor: function (config) {
        config = Ext.apply({}, config);

        this.callParent([config]);
    }
});

Ext.define('PIS.data.AjaxProxy', {
    extend: 'Ext.data.proxy.Ajax',
    alias: 'proxy.pisajaxproxy',

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
        }, config);

        this.callParent([config]);
    }
});

Ext.define('PIS.data.StoreMixin', {
    extend: 'Ext.data.Store',
    alias: 'store.pisstoremi',
    mixins: ['PIS.ExcelMixin'],

    getRangeData: function () {
        var recs = this.getRange();
        var data = [];

        Ext.each(recs, function (rec) {
            data.push(rec.data);
        });

        return data;
    },

    getField: function (f) {
        var fields = this.model.getFields();
        var field = null;

        if (typeof (f) == "string") {
            Ext.each(fields, function (tf) {
                if (tf.name = f) {
                    field = tf;
                    return false;
                }
            });
        } else if (typeof (f) == "number") {
            field = fields[f];
        }

        return field;
    },

    exportExcel: function (config) {
        var me = this;
        config = config || {};

        if (!config.columns) {
            var cols = [];

            var flds = me.model.getFields();
            Ext.each(flds, function (f) {
                var col = Ext.apply({
                    hidden: false,
                    dataIndex: f.name,
                    width: 100,
                    text: f.name
                }, f);

                cols.push(col);
            });

            config.columns = cols;
        }

        config.store = me;

        me.mixins['PIS.ExcelMixin'].exportExcel(config);
    }
});

Ext.define('PIS.data.Store', {
    extend: 'Ext.data.Store',
    mixins: ['PIS.data.StoreMixin'],

    alias: 'store.pisstore',
    alternateClassName: 'PIS.DataStore',

    constructor: function (config) {
        config = Ext.apply({
            root: 'Data',
            idProperty: 'Id',
            totalProperty: 'TotalCount',
            remoteSort: true,
            autoDestroy: true
        }, config);

        this.callParent([config]);
    }
});

Ext.define('PIS.data.ArrayStore', {
    extend: 'Ext.data.ArrayStore',
    alias: 'store.pisarray',
    mixins: ['PIS.data.StoreMixin'],

    constructor: function (config) {
        this.callParent([config]);
    },

    reload: function (args) {
        this.callParent([args]);
    }
});

Ext.define('PIS.data.EnumStore', {
    extend: 'PIS.data.ArrayStore',
    alias: 'store.pisenum',

    constructor: function (config) {
        config = Ext.apply({ enumdata: {}
        }, config);

        var tdata = config.enumdata;

        if (config.enumdata) {
            switch (config.enumtype) {
                case 'simple':
                default:
                    config.fields = config.fields || ['value', 'text'];
                    var tdata = [];
                    for (var key in config.enumdata) {
                        tdata[tdata.length] = [key, config.enumdata[key]];
                    }
                    config.data = tdata;
                    break;
            }
        }

        this.callParent([config]);
    }
});

//------------------------PIS ExtJs数据组件扩展 结束------------------------//

//-----------------------------PIS ExtJs 按钮 开始--------------------------//

// Ext findByType 不支持按钮, 这里是查找按钮方法
function ExtFindButtonsBase(c, params, array) {
    array = array || [];

    var bttype, xtype;

    if (typeof (params) == 'string') {
        bttype = params;
    } else {
        bttype = params.bttype;
        xtype = params.xtype;
    }

    if (c.toolbars) {
        Ext.each(c.toolbars, function (i) {
            var j = i.items.items;
            Ext.each(j, function (k) {
                if (bttype) {
                    if (k.bttype == bttype) {
                        array.push(k);
                    }
                } else if (xtype) {
                    if (k.xtype == xtype) {
                        array.push(k);
                    }
                } else if (k.xtype == 'button' || k.type == 'button') {
                    array.push(k);
                }
            })
        });
    }

    if (c.items && c.items.items) {
        var i = c.items.items;
        Ext.each(i, function (k) {
            if (bttype) {
                if (k.bttype == bttype) {
                    array.push(k);
                }
            } else if (xtype) {
                if (k.xtype == xtype) {
                    array.push(k);
                }
            } else if (k.xtype == 'button' || k.type == 'button') {
                array.push(k);
            }
        });
    }

    return array;
}

// 获取当前容器，以及下级所有容器的所有按钮（递归）
function ExtFindButtons(c, params, array) {
    array = array || [];

    //如果c不是叶子节点
    if ((c.items && c.items.items)) {
        ExtFindButtonsBase(c, params, array);

        if (c.items && c.items.items) {
            Ext.each(c.items.items, function (i) {
                ExtFindButtons(i, params, array);
            });
        }
    }

    return array;
}

function ExtRegPISBtType(bttype, type, xtype, config) {
    config = Ext.apply({
        type: type,
        xtype: xtype,
        bttype: bttype,
        pisexecutable: false
    }, config);

    if ('string' == typeof (bttype)) {
        if (PIS.ButtonDict[bttype]) { throw "bttype已注册！"; }
        else { PIS.ButtonDict[bttype] = config; }
    }
}

function ExtGetPISButtonInfoByBtType(bttype, field) {
    if (!field) { return PIS.ButtonDict[bttype]; }
    else if (PIS.ButtonDict[bttype]) {
        return PIS.ButtonDict[bttype][field];
    } else {
        return null;
    }
}

function ExtDefinePISButton(type, xtype, bttype, config) {
    var defname = ('PIS.' + type);

    config = Ext.apply({
        extend: 'PIS.Button',
        alias: ('widget.' + xtype),
        type: type,
        bttype: bttype,
        pisexecutable: false,
        text: '',
        iconCls: '',
        name: defname
    }, config);


    ExtRegPISBtType(bttype, type, xtype, config);

    Ext.define(defname, config);
}

Ext.define('PIS.Button', { extend: 'Ext.Button', alias: 'widget.pis-button' });

ExtDefinePISButton('ExtAddButton', 'pis-addbutton', 'add', { pisexecutable: true, text: '新建', iconCls: 'pis-icon-add' });
ExtDefinePISButton('ExtSaveButton', 'pis-savebutton', 'save', { pisexecutable: true, text: '保存', iconCls: 'pis-icon-save' });
ExtDefinePISButton('ExtSubmitButton', 'pis-submitbutton', 'submit', { pisexecutable: true, text: '提交', iconCls: 'pis-icon-submit' });
ExtDefinePISButton('ExtEditButton', 'pis-editbutton', 'edit', { pisexecutable: true, text: '编辑', iconCls: 'pis-icon-edit' });
ExtDefinePISButton('ExtDeleteButton', 'pis-deletebutton', 'delete', { pisexecutable: true, text: '删除', iconCls: 'pis-icon-delete' });
ExtDefinePISButton('ExtCancelButton', 'pis-cancelbutton', 'cancel', { text: '取消', iconCls: 'pis-icon-cancel' });
ExtDefinePISButton('ExtCloseButton', 'pis-closebutton', 'close', { text: '关闭', iconCls: 'pis-icon-close', handler: function () { window.top.close(); } });
ExtDefinePISButton('ExtHelpButton', 'pis-helpbutton', 'help', { text: '帮助', iconCls: 'pis-icon-help', handler: function () { if (typeof (OpenHelpWin) == 'function') { OpenHelpWin(); } } });
ExtDefinePISButton('ExtMoreButton', 'pis-morebutton', 'more', { text: '更多', iconCls: 'pis-icon-more' });
ExtDefinePISButton('ExtExcelButton', 'pis-excelbutton', 'excel', { text: '导出Excel', iconCls: 'pis-icon-xls' });
ExtDefinePISButton('ExtWordButton', 'pis-wordbutton', 'word', { text: '导出Word', iconCls: 'pis-icon-doc' });
ExtDefinePISButton('ExtSchButton', 'pis-schbutton', 'search', { text: '查询', iconCls: 'pis-icon-search' });
ExtDefinePISButton('ExtCQryButton', 'pis-cqrybutton', 'cquery', { text: '复杂查询', iconCls: 'pis-icon-tog-bottom' });
ExtDefinePISButton('ExtClrButton', 'pis-clrbutton', 'clear', { text: '清除', iconCls: 'pis-icon-erase' });
ExtDefinePISButton('ExtRefreshButton', 'pis-refreshbutton', 'refresh', { text: '刷新', iconCls: 'pis-icon-refresh' });


//-----------------------------PIS ExtJs 按钮 结束--------------------------//

//---------------------------PIS ExtJs 工具栏 开始--------------------------//

Ext.define('PIS.Toolbar', {
    extend: 'Ext.Toolbar',
    alias: 'widget.pis-toolbar',

    constructor: function (config) {
        config = Ext.apply({}, config);

        if (config.items) {
            for (var i = 0; i < config.items.length; i++) {
                var item = config.items[i];
                var itemcfg = item;
                var iteminfo;
                if (typeof (item) == "string") {
                    iteminfo = ExtGetPISButtonInfoByBtType(item.toLowerCase());
                    if (iteminfo) { itemcfg = iteminfo }
                } else if (typeof (item) == "object" && typeof (item.bttype) == "string") {
                    iteminfo = ExtGetPISButtonInfoByBtType(item.bttype.toLowerCase());
                    if (iteminfo) {
                        itemcfg = Ext.apply(iteminfo, itemcfg);
                    }
                }

                config.items[i] = itemcfg;
            }
        }

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;
        me.callParent(arguments);

        me.on('afterrender', function () {
            if ('r'.equals(pgOp)) {
                me.setReadOnly(true);
            }
        })
    },

    setDisabled: function (flag) {
        var me = this;
        me.setStatus({ disabled: flag, visible: true });
    },

    setReadOnly: function (flag) {
        var me = this;
        me.setStatus({ disabled: false, visible: !flag });
    },

    setStatus: function (params, menu) {
        var me = this;
        params = params || {};
        var menu = params.menu;
        params.readOnly = (params.readOnly == true);
        params.disabled = (params.disabled == true);
        params.visible = !(params.readOnly == false);

        if (!menu) {
            me.items.each(function () {
                me.setMenuItemStatus(this, params);

                if (this.menu) {
                    me.setStatus(params, this.menu);
                }
            });
        } else if (menu && menu.items) {
            menu.items.each(function () {
                me.setMenuItemStatus(this, params);
                if (this.menu) { me.setReadOnly(params, this.menu); }
            });
        }
    },

    setMenuItemStatus: function (menuitem, params) {
        if (menuitem && menuitem.pisautoset !== false && menuitem.pisexecutable === true) {
            if (menuitem.setVisible) { menuitem.setVisible(params.visible); }
            if (menuitem.setDisabled) { menuitem.setDisabled(params.disabled); }
            if (menuitem.setReadOnly) { menuitem.setDisabled(params.readOnly); }
        }
    },

    getButtons: function (params) {
        var btns = ExtFindButtons(this, params);

        return btns;
    },

    getButton: function (params) {
        var btns = this.getButtons(params);

        if (btns && btns.length > 0) {
            return btns[0];
        }

        return null;
    }
});

//---------------------------PIS ExtJs 工具栏 结束--------------------------//

//-----------------------------PIS ExtJs MessageBox 开始--------------------------//


Ext.define('PIS.MessageBox', {
    extend: 'Ext.window.MessageBox',
    alias: ['widget.pis-messagebox', 'widget.pis-msgbox'],
    alternateClassName: ['PIS.MsgBox'],

    constructor: function (config) {
        this.callParent([config]);
    },

    showAlert: function (msg, title, fn, scope, icon) {
        Ext.MessageBox.show({
            title: title,
            msg: msg,
            fn: fn,
            buttons: Ext.Msg.OK,
            icon: icon || null
        });
    },

    confirm: function (msg, fn, cfg, scope) {
        cfg = cfg || "确认";
        Ext.MessageBox.confirm(cfg, msg, fn, scope);
    },

    show: function (cfg) {
        Ext.MessageBox.show(cfg)
    },

    alert: function (msg, title, fn, scope) {
        this.showAlert(msg, title || "提示", fn, scope);
    },

    info: function (msg, title, fn, scope) {
        this.showAlert(msg, title || "消息", fn, scope, Ext.Msg.INFO);
    },

    warn: function (msg, title, fn, scope) {
        this.showAlert(msg, title || "警告", fn, scope, Ext.Msg.WARNING);
    },

    error: function (msg, title, fn, scope) {
        this.showAlert(msg, title || "错误", fn, scope, Ext.Msg.ERROR);
    },

    wait: function (progressText, msg) {
        Ext.MessageBox.show({
            msg: msg || "正在处理数据，请等待...",
            progressText: progressText || "处理中...",
            width: 300,
            wait: true,
            waitConfig: { interval: 200 }
        });
    },

    hide: function () { Ext.MessageBox.hide(); }
},

function () {
    PISExtMsgBox = PISExtMessageBox = new this();
    if (typeof PISMsgBox == "undefined") {
        PISMsgBox = PISExtMsgBox;
    }
});

//-----------------------------PIS ExtJs MessageBox 结束--------------------------//


//-----------------------------PIS ExtJs 表单域 开始--------------------------//

Ext.define('PIS.FrmField', {
    statics: {
        RequireLabelTextTpl: '<span style="color:red;font-weight:bold" data-qtip="Required">*</span>',

        InitConfig: function (config) {
            config = config || {};

            if (config.allowBlank == false) {
                config.afterLabelTextTpl = config.afterLabelTextTpl || PIS.FrmField.RequireLabelTextTpl
            }
        }
    }
});

Ext.define('PIS.TextField', {
    extend: 'Ext.form.TextField',
    alias: ['widget.pis-textfield', 'widget.pis-text', 'widget.pis-field'],

    constructor: function (config) {
        PIS.FrmField.InitConfig(config);

        this.callParent([config]);
    }
});

Ext.define('PIS.HiddentField', {
    extend: 'Ext.form.TextField',
    alias: ['widget.pis-hidden'],

    constructor: function (config) {
        config.hidden = true;

        this.callParent([config]);
    }
});

Ext.define('PIS.NumberField', {
    extend: 'Ext.form.NumberField',
    alias: ['widget.pis-numberfield', 'widget.pis-number'],

    constructor: function (config) {
        PIS.FrmField.InitConfig(config);

        this.callParent([config]);
    }
});

Ext.define('PIS.TextArea', {
    extend: 'Ext.form.TextArea',
    alias: 'widget.pis-textarea',

    constructor: function (config) {
        PIS.FrmField.InitConfig(config);

        this.callParent([config]);
    }
});

Ext.define('PIS.HtmlEditor', {
    extend: 'Ext.form.HtmlEditor',
    alias: 'widget.pis-htmleditor',

    constructor: function (config) {
        PIS.FrmField.InitConfig(config);

        this.callParent([config]);
    }
});

Ext.define('PIS.DateField', {
    extend: 'Ext.form.DateField',
    alias: 'widget.pis-datefield',

    constructor: function (config) {
        PIS.FrmField.InitConfig(config);

        this.callParent([config]);
    }
});

Ext.define('PIS.Checkbox', {
    extend: 'Ext.form.Checkbox',
    alias: 'widget.pis-checkbox',

    constructor: function (config) {
        config = Ext.apply({
        }, config);

        this.callParent([config]);
    }
});

Ext.define('PIS.CheckboxGroup', {
    extend: 'Ext.form.CheckboxGroup',
    alias: 'widget.pis-checkboxgroup',

    constructor: function (config) {
        config = Ext.apply({
        }, config);

        this.callParent([config]);
    }
});

Ext.define('PIS.RadioGroup', {
    extend: 'Ext.form.RadioGroup',
    alias: 'widget.pis-radiogroup',

    constructor: function (config) {
        config = Ext.apply({
        }, config);

        this.callParent([config]);
    }
});

//------------------------PIS ExtJs Picker类控件 开始------------------------//

Ext.define('PIS.ComboBox', {
    extend: 'Ext.form.ComboBox',
    alias: 'widget.pis-combobox',

    constructor: function (config) {
        PIS.FrmField.InitConfig(config);

        config = Ext.apply({
            editable: false,
            displayField: 'text',
            valueField: 'value',
            triggerAction: 'all',
            selectOnFocus: true,
            blankText: "请选择...",
            mode: 'local',
            value: ''
        }, config);

        this.callParent([config]);
    }
});

Ext.define('PIS.EnumSelect', {
    extend: 'PIS.ComboBox',
    alias: ['widget.pis-enumbox', 'widget.pis-enumselect'],

    constructor: function (config) {
        var _required = (config.allowBlank == false);

        if (!_required && config.enumdata) {
            var _enumdata = { '': config.blankText };

            for (var key in config.enumdata) {
                _enumdata[key] = config.enumdata[key];
            }

            config.enumdata = _enumdata;
        }

        if (!config.store && config.enumdata) {
            config.store = Ext.create('PIS.data.EnumStore', { enumtype: config.enumtype, enumdata: config.enumdata });
        }

        this.callParent([config]);
    }
});

Ext.define('PIS.GridComboBoxList', {
    extend: 'Ext.view.AbstractView',
    alias: 'widget.pis-gridcombolist',
    alternateClassName: 'PIS.view.ExtGridComboBoxList',
    renderTpl: ['<div class="list-ct" style="border: 1px solid #99BBE8"></div>'],
    initComponent: function () {
        var me = this;
        // 2012-05-07 Ext4.1下的Bug的解决
        me.itemSelector = "div.list-ct";
        //me.itemSelector = ".";
        me.tpl = Ext.create('Ext.XTemplate');
        me.callParent();
        Ext.applyIf(me.renderSelectors, {
            listEl: '.list-ct'
        });
        me.gridCfg.border = false;
        me.gridCfg.store = me.store;
        me.grid = Ext.create('Ext.grid.Panel', me.gridCfg);
        me.grid.store.addListener({
            beforeload: function () {
                me.owner.loading = true;
            },
            load: function () {
                me.owner.loading = false;
            }
        });
        var sm = me.grid.getSelectionModel();
        sm.addListener('selectionchange', function (a, sl) {
            var cbx = me.owner;
            if (cbx.loading)
                return;
            // var sv = cbx.multiSelect ? cbx.getValue() : {};
            var sv = {}
            var EA = Ext.Array, vf = cbx.valueField;

            if (!sl || !sl.length && !sl[vf]) {
                return
            }

            // al = [ 'G', 'Y', 'B' ]
            var al = EA.map(me.grid.store.data.items, function (r) {
                return r.data[vf];
            });
            var cs = EA.map(sl, function (r) {
                var d = r.data;
                if (d) {
                    var k = d[vf];
                    sv[k] = d;
                    return k;
                }
            });
            // cs = [ 'G' ]
            var rl = EA.difference(al, cs);
            EA.each(rl, function (r) {
                delete sv[r];
            });
            cbx.setValue(sv);
        });
        sm.addListener('select', function (m, r, i) {
            var cbx = me.owner;
            if (cbx.loading)
                return;
            if (!cbx.multiSelect)
                cbx.collapse();
        });
    },
    onRender: function () {
        this.callParent(arguments);
        this.grid.render(this.listEl);
    },
    bindStore: function (store, initial) {
        this.callParent(arguments);
        if (this.grid)
            this.grid.bindStore(store, initial);
    },
    onDestroy: function () {
        Ext.destroyMembers(this, 'grid', 'listEl');
        this.callParent();
    },
    highlightItem: function () {
    }
});

Ext.define('PIS.GridComboBox', {
    extend: 'Ext.form.field.Picker',
    requires: ['Ext.util.DelayedTask', 'Ext.EventObject', 'Ext.view.BoundList', 'Ext.view.BoundListKeyNav', 'Ext.data.StoreManager', 'Ext.grid.View'],
    alternateClassName: 'Ext.form.GridComboBox',
    alias: ['widget.pis-gridcombo', 'widget.pis-gridcombobox'],
    triggerCls: Ext.baseCSSPrefix + 'form-arrow-trigger',
    multiSelect: false,
    delimiter: ',',
    displayField: 'text',
    triggerAction: 'all',
    allQuery: '',
    queryParam: 'query',
    queryMode: 'remote',
    queryCaching: true,
    pageSize: 0,
    autoSelect: true,
    typeAhead: false,
    typeAheadDelay: 250,
    selectOnTab: true,
    forceSelection: false,
    defaultListConfig: {
        emptyText: '',
        loadingText: '正在加载...',
        loadingHeight: 70,
        minWidth: 70,
        maxHeight: 300,
        shadow: 'sides'
    },

    // private
    ignoreSelection: 0,

    initComponent: function () {
        var me = this, isDefined = Ext.isDefined, store = me.store, transform = me.transform, transformSelect, isLocalMode;
        // <debug>
        if (!store && !transform) {
            Ext.Error.raise('Either a valid store, or a HTML select to transform, must be configured on the combo.');
        }
        if (me.typeAhead && me.multiSelect) {
            Ext.Error.raise('typeAhead and multiSelect are mutually exclusive options -- please remove one of them.');
        }
        if (me.typeAhead && !me.editable) {
            Ext.Error.raise('If typeAhead is enabled the combo must be editable: true -- please change one of those settings.');
        }
        if (me.selectOnFocus && !me.editable) {
            Ext.Error.raise('If selectOnFocus is enabled the combo must be editable: true -- please change one of those settings.');
        }
        // </debug>
        this.addEvents('beforequery', 'select');
        // Build store from 'transform' HTML select element's
        // options
        if (!store && transform) {
            transformSelect = Ext.getDom(transform);
            if (transformSelect) {
                store = Ext.Array.map(Ext.Array.from(transformSelect.options), function (
						option) {
                    return [option.value, option.text];
                });
                if (!me.name) {
                    me.name = transformSelect.name;
                }
                if (!('value' in me)) {
                    me.value = transformSelect.value;
                }
            }
        }
        me.bindStore(store, true);
        store = me.store;
        if (store.autoCreated) {
            me.queryMode = 'local';
            me.valueField = me.displayField = 'field1';
            if (!store.expanded) {
                me.displayField = 'field2';
            }
        }
        if (!isDefined(me.valueField)) {
            me.valueField = me.displayField;
        }
        isLocalMode = me.queryMode === 'local';
        if (!isDefined(me.queryDelay)) {
            me.queryDelay = isLocalMode ? 10 : 500;
        }
        if (!isDefined(me.minChars)) {
            me.minChars = isLocalMode ? 0 : 4;
        }
        if (!me.displayTpl) {
            me.displayTpl = Ext.create('Ext.XTemplate', '<tpl for=".">'
					+ '{[typeof values === "string" ? values : values.' + me.displayField
					+ ']}' + '<tpl if="xindex < xcount">' + me.delimiter + '</tpl>'
					+ '</tpl>');
        } else if (Ext.isString(me.displayTpl)) {
            me.displayTpl = Ext.create('Ext.XTemplate', me.displayTpl);
        }
        me.callParent();
        me.doQueryTask = Ext.create('Ext.util.DelayedTask', me.doRawQuery, me);
        // store has already been loaded, setValue
        if (me.store.getCount() > 0) {
            me.setValue(me.value);
        }
        // render in place of 'transform' select
        if (transformSelect) {
            me.render(transformSelect.parentNode, transformSelect);
            Ext.removeNode(transformSelect);
            delete me.renderTo;
        }
    },

    beforeBlur: function () {
        var me = this;
        me.doQueryTask.cancel();
        if (me.forceSelection) {
            me.assertValue();
        } else {
            me.collapse();
        }
    },

    assertValue: function () {
        var me = this, value = me.getRawValue(), rec;
        rec = me.findRecordByDisplay(value);
        //only allow set single value for now.
        me.setValue(rec ? [rec.raw] : []);
        me.collapse();
    },

    onTypeAhead: function () {
        var me = this, df = me.displayField;
        var st = me.store, rv = me.getRawValue();
        var r = me.store.findRecord(df, rv);
        if (r) {
            var nv = r.get(df), ln = nv.length, ss = rv.length;
            if (ss !== 0 && ss !== ln) {
                me.setRawValue(nv);
                me.selectText(ss, nv.length);
            }
        }
    },

    // invoked when a different store is bound to this combo
    // than the original
    resetToDefault: function () {
    },

    bindStore: function (store, initial) {
        var me = this, oldStore = me.store;
        // this code directly accesses this.picker, bc invoking
        // getPicker
        // would create it when we may be preping to destroy it
        if (oldStore && !initial) {
            if (oldStore !== store && oldStore.autoDestroy) {
                oldStore.destroy();
            } else {
                oldStore.un({
                    scope: me,
                    load: me.onLoad,
                    exception: me.collapse
                });
            }
            if (!store) {
                me.store = null;
                if (me.picker) {
                    me.picker.bindStore(null);
                }
            }
        }
        if (store) {
            if (!initial) {
                me.resetToDefault();
            }
            me.store = Ext.data.StoreManager.lookup(store);
            me.store.on({
                scope: me,
                load: me.onLoad,
                exception: me.collapse
            });
            if (me.picker) {
                me.picker.bindStore(store);
            }
        }
    },

    onLoad: function () {
        var me = this, value = me.value;
        me.syncSelection();
    },

    /**
    * @private Execute the query with the raw contents within
    *		  the textfield.
    */
    doRawQuery: function () {
        this.doQuery(this.getRawValue());
    },

    doQuery: function (queryString, forceAll) {
        queryString = queryString || '';
        // store in object and pass by reference in
        // 'beforequery'
        // so that client code can modify values.
        var me = this, qe = {
            query: queryString,
            forceAll: forceAll,
            combo: me,
            cancel: false
        }, store = me.store, isLocalMode = me.queryMode === 'local';

        if (me.fireEvent('beforequery', qe) === false || qe.cancel) {
            return false;
        }
        // get back out possibly modified values
        queryString = qe.query;
        forceAll = qe.forceAll;
        // query permitted to run
        if (forceAll || (queryString.length >= me.minChars)) {
            // expand before starting query so LoadMask can
            // position itself correctly
            me.expand();
            // make sure they aren't querying the same thing
            if (!me.queryCaching || me.lastQuery !== queryString) {
                me.lastQuery = queryString;
                store.clearFilter(!forceAll);
                if (isLocalMode) {
                    if (!forceAll) {
                        store.filter(me.displayField, queryString);
                    }
                } else {
                    store.load({
                        params: me.getParams(queryString)
                    });
                }
            }
            // Clear current selection if it does not match the
            // current value in the field
            if (me.getRawValue() !== me.getDisplayValue()) {
                me.ignoreSelection++;
                me.picker.getSelectionModel().deselectAll();
                me.ignoreSelection--;
            }
            if (me.typeAhead) {
                me.doTypeAhead();
            }
        }
        return true;
    },

    // private
    getParams: function (queryString) {
        var p = {}, pageSize = this.pageSize;
        p[this.queryParam] = queryString;
        if (pageSize) {
            p.start = 0;
            p.limit = pageSize;
        }
        return p;
    },

    doTypeAhead: function () {
        if (!this.typeAheadTask) {
            this.typeAheadTask = Ext.create('Ext.util.DelayedTask', this.onTypeAhead, this);
        }
        if (this.lastKey != Ext.EventObject.BACKSPACE
				&& this.lastKey != Ext.EventObject.DELETE) {
            this.typeAheadTask.delay(this.typeAheadDelay);
        }
    },

    onTriggerClick: function () {
        var me = this;
        if (!me.readOnly && !me.disabled) {
            if (me.isExpanded) {
                me.collapse();
            } else {
                me.onFocus({});
                if (me.triggerAction === 'all') {
                    me.doQuery(me.allQuery, true);
                } else {
                    me.doQuery(me.getRawValue());
                }
            }
            me.inputEl.focus();
        }
    },

    // store the last key and doQuery if relevant
    onKeyUp: function (e, t) {
        var me = this, key = e.getKey();

        if (!me.readOnly && !me.disabled && me.editable) {
            me.lastKey = key;
            me.doQueryTask.cancel();

            // perform query w/ any normal key or backspace or
            // delete
            if (!e.isSpecialKey() || key == e.BACKSPACE || key == e.DELETE) {
                if (me.getRawValue() == '') {
                    me.clearValue();
                    return;
                }
                me.doQueryTask.delay(me.queryDelay);
            } else if (key == e.ENTER) {
                this.doQuery(this.getRawValue(), true);
            }
        }
    },

    initEvents: function () {
        var me = this;
        me.callParent();
        // setup keyboard handling
        me.mon(me.inputEl, 'keyup', me.onKeyUp, me);
    },

    createPicker: function () {
        var me = this, menuCls = Ext.baseCSSPrefix + 'menu';
        var opts = Ext.apply({
            selModel: {
                mode: me.multiSelect ? 'SIMPLE' : 'SINGLE'
            },
            floating: true,
            hidden: true,
            ownerCt: me.ownerCt,
            cls: me.el.up('.' + menuCls) ? menuCls : '',
            store: me.store,
            displayField: me.displayField,
            focusOnToFront: false,
            pageSize: me.pageSize,
            gridCfg: me.gridCfg,
            owner: me
        }, me.listConfig, me.defaultListConfig);
        var pk = me.picker = new PIS.GridComboBoxList(opts);
        me.mon(pk, {
            itemclick: me.onItemClick,
            refresh: me.onListRefresh,
            scope: me
        });
        me.mon(pk.getSelectionModel(), {
            selectionChange: me.onListSelectionChange,
            scope: me
        });
        return pk;
    },

    onListRefresh: function () {
        this.alignPicker();
        this.syncSelection();
    },

    onItemClick: function (picker, record) {
        /*
        * If we're doing single selection, the selection change
        * events won't fire when clicking on the selected
        * element. Detect it here.
        */
        var me = this, lastSelection = me.lastSelection, valueField = me.valueField, selected;

        if (!me.multiSelect && lastSelection) {
            selected = lastSelection[0];
            if (record.get(valueField) === selected.get(valueField)) {
                me.collapse();
            }
        }
    },

    onListSelectionChange: function (list, selectedRecords) {
        var me = this;
        // Only react to selection if it is not called from
        // setValue, and if our list is
        // expanded (ignores changes to the selection model
        // triggered elsewhere)
        if (!me.ignoreSelection && me.isExpanded) {
            if (!me.multiSelect) {
                Ext.defer(me.collapse, 1, me);
            }
            me.setValue(selectedRecords, false);
            if (selectedRecords.length > 0) {
                me.fireEvent('select', me, selectedRecords);
            }
            me.inputEl.focus();
        }
    },

    /**
    * @private Enables the key nav for the BoundList when it is
    *		  expanded.
    */
    onExpand: function () {
        var me = this, keyNav = me.listKeyNav;
        var selectOnTab = me.selectOnTab, picker = me.getPicker();

        // redo layout to make size right after reload store
        picker.grid.doLayout();
        // Handle BoundList navigation from the input field.
        // Insert a tab listener specially to enable
        // selectOnTab.
        if (keyNav) {
            keyNav.enable();
        } else {
            keyNav = me.listKeyNav = Ext.create('Ext.view.BoundListKeyNav', this.inputEl, {
                boundList: picker,
                forceKeyDown: true,
                home: function (e) {
                    return true;
                },
                end: function (e) {
                    return true;
                },
                tab: function (e) {
                    if (selectOnTab) {
                        this.selectHighlighted(e);
                        me.triggerBlur();
                    }
                    // Tab key event is allowed to propagate to
                    // field
                    return true;
                }
            });
        }
        // While list is expanded, stop tab monitoring from
        // Ext.form.field.Trigger so it doesn't short-circuit
        // selectOnTab
        if (selectOnTab) {
            me.ignoreMonitorTab = true;
        }
        // Ext.defer(keyNav.enable, 1, keyNav); //wait a bit so
        // it doesn't react to the down arrow opening the picker
        me.inputEl.focus();
        me.syncSelection();
    },

    /**
    * @private Disables the key nav for the BoundList when it
    *		  is collapsed.
    */
    onCollapse: function () {
        var me = this, keyNav = me.listKeyNav;
        if (keyNav) {
            keyNav.disable();
            me.ignoreMonitorTab = false;
        }
    },

    select: function (r) {
        this.setValue(r, true);
    },

    findRecord: function (field, value) {
        var ds = this.store, idx = ds.findExact(field, value);
        return idx !== -1 ? ds.getAt(idx) : false;
    },

    findRecordByValue: function (value) {
        return this.findRecord(this.valueField, value);
    },
    findRecordByDisplay: function (value) {
        return this.findRecord(this.displayField, value);
    },

    setValue: function (value, doSelect) {
        var me = this, txt = me.inputEl;
        me.value = value || {};
        if (me.store.loading)
            return me;
        me.setRawValue(me.getDisplayValue());
        if (txt && me.emptyText && !Ext.isEmpty(value))
            txt.removeCls(me.emptyCls);
        me.checkChange();
        if (doSelect)
            me.syncSelection(); //
        me.applyEmptyText();
        return me;
    },

    getDisplayValue: function () {
        var me = this, dv = [];
        Ext.Object.each(me.value, function (k, v) {
            var a = v[me.displayField];
            if (a)
                dv.push(a);
        });
        return dv.join(',');
    },

    getValue: function () {
        return this.value || [];
    },

    //keys, spliter, doSelect
    setSubmitValue: function (keys, sp, ds) {
        var me = this, v = {}, sp = sp || ',';
        if (keys) {
            Ext.Array.each(keys.split(sp), function (a) {
                var r = me.store.findRecord(me.valueField, a, 0, false, true, true);
                if (r)
                    v[a] = r.data;
            });
        }
        me.setValue(v, ds);
    },

    getSubmitValue: function () {
        var me = this, sv = [];
        Ext.Object.each(me.value, function (k, v) {
            sv.push(v[me.valueField]);
        });
        return sv;
    },

    isEqual: function (v1, v2) {
        var fa = Ext.Array.from, i, len;
        v1 = fa(v1);
        v2 = fa(v2);
        len = v1.length;
        if (len !== v2.length) {
            return false;
        }
        for (i = 0; i < len; i++) {
            if (v2[i] !== v1[i]) {
                return false;
            }
        }
        return true;
    },

    clearValue: function () {
        this.setValue({});
    },

    syncSelection: function () {
        var me = this, pk = me.picker;
        if (pk && pk.grid) {
            var EA = Ext.Array, gd = pk.grid, st = gd.store;
            var cs = [];
            var sv = this.getSubmitValue();
            EA.each(st.data.items, function (r) {
                if (EA.contains(sv, r.data[me.valueField])) {
                    cs.push(r);
                }
            });
            gd.getSelectionModel().select(cs, false, true);
        }
    }
});

Ext.define('PIS.data.SelectStore', {
    extend: 'PIS.data.Store',
    alias: 'store.pisselectstore',

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            pageSize: 50,
            autoLoad: false
        }, config);

        var url = config.url || location.href;
        url = $.combineQueryUrl(url, { asyncreq: true, qrytype: 'json' });

        if (!config.proxy) {
            config.proxy = {
                type: 'ajax',
                url: url,
                reader: config.reader
            };
        } else {
            if (config.proxy.url) {
                url = $.combineQueryUrl(config.proxy.url, { asyncreq: true, qrytype: 'json' });
            }

            config.proxy.url = url;
        }

        if (!config.proxy.reader) {
            config.proxy.reader = Ext.create('PIS.data.JsonReader');
        }

        this.callParent([config]);
    }
});

Ext.define('PIS.GridSelect', {
    extend: 'PIS.GridComboBox',
    alias: 'widget.pis-gridselect',
    gridCfg: null,
    fieldMap: null, // 值与其他表单域关联({valuekey1: fieldid1, valuekey2: fieldid2...})

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            multiSelect: false,
            // displayField : 'name',
            // valueField : 'value',
            width: 300,
            labelWidth: 100,
            // labelAlign: 'right',
            typeAhead: true,
            queryMode: 'remote',
            matchFieldWidth: false,
            pickerAlign: 'bl'
        }, config);

        var gridCfg = Ext.apply({
        }, config.gridCfg || {});

        if (config.multiSelect === true) {
            config.typeAhead = false;
            config.suspendCheckChange = false;
            gridCfg.selModel = gridCfg.selModel || new Ext.selection.CheckboxModel({ checkOnly: true });
        }

        if (gridCfg) {
            if (!gridCfg.bbar) {
                gridCfg.bbar = Ext.create('PIS.PagingToolbar', { store: config.store })
            }
        }

        config.gridCfg = gridCfg;

        this.callParent([config]);
    },

    onChange: function (args, oldVal) {
        var me = this;

        this.callParent(arguments);

        if (me.fieldMap) {
            var field;
            Ext.Object.each(me.fieldMap, function (fid, kv) {
                field = Ext.getCmp(fid);

                if (field) {
                    var v_arr = [];
                    Ext.Object.each(args, function (k, v) {
                        v_arr.push(v[kv]);
                    });

                    field.setValue(v_arr.join(","));
                }
            });
        }
    },

    setValue: function (value, doSelect) {
        var me = this;
        var val = value;
        if (typeof (val) == "string") {
            vals = val.split(",");
            val = {};
            Ext.each(vals, function (v) {
                var vobj = {};
                vobj[me.valueField] = v;
                val[v] = vobj;
            })
        }
        
        return this.callParent([val, doSelect]);
    },

    getSubmitValue: function () {
        var me = this, sv = [];
        Ext.Object.each(me.value, function (k, v) {
            sv.push(v[me.valueField]);
        });
        return sv.join(',');
    }
});

Ext.define('PIS.data.UserSelectStore', {
    extend: 'PIS.data.SelectStore',
    alias: 'store.pis-userselectstore',

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            fields: [{ name: 'UserID' }, { name: 'Name' }, { name: 'WorkNo' }, { name: 'LoginName'}],
            proxy: {
                type: 'ajax',
                url: USER_SELECT_URL,
                reader: { type: 'json', root: 'DtList', totalProperty: 'RecordCount' }
            }
        }, config);

        me.callParent([config]);

        me.on("change", function () {
            alert();
        })
    }
});

Ext.define('PIS.UserSelect', {
    extend: 'PIS.GridSelect',
    alias: 'widget.pis-userselect',
    gridCfg: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            displayField : 'Name',
            valueField: 'Name'
        }, config);

        if (!config.store) {
            config.store = Ext.create('PIS.data.UserSelectStore');
        }

        config.gridCfg = Ext.apply({
            viewConfig: { loadMask: false },
            height: 200,
            width: 240,
            columns: [
                { text: '姓名', width: 80, dataIndex: 'Name' },
                { text: '工号', width: 80, dataIndex: 'WorkNo' }
            ]
        }, config.gridCfg || {});

        this.callParent([config]);
    }
});

Ext.define('PIS.data.GroupSelectStore', {
    extend: 'PIS.data.SelectStore',
    alias: 'store.pis-groupselectstore',

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            fields: [{ name: 'GroupID' }, { name: 'Name' }, { name: 'Code'}],
            proxy: {
                type: 'ajax',
                params: { asyncreq: true, qrytype: "json" },
                url: GROUP_SELECT_URL,
                reader: { type: 'json', root: 'DtList', totalProperty: 'RecordCount' }
            }
        }, config);

        this.callParent([config]);
    }
});

Ext.define('PIS.GroupSelect', {
    extend: 'PIS.GridSelect',
    alias: 'widget.pis-groupselect',
    gridCfg: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            displayField : 'Name',
            valueField: 'Name'
        }, config);

        if (!config.store) {
            config.store = Ext.create('PIS.data.GroupSelectStore');
        }

        config.gridCfg = Ext.apply({
            viewConfig: { loadMask: false },
            height: 200,
            width: 240,
            columns: [
                { text: '名称', width: 80, dataIndex: 'Name' },
                { text: '编号', width: 80, dataIndex: 'Code' }
            ]
        }, config.gridCfg || {});

        this.callParent([config]);
    }
});

//------------------------PIS ExtJs Picker类控件 结束------------------------//


//------------------------PIS ExtJs 文件控件 开始------------------------//

function PISOpenUploadDialog(params, onProcessFinish) {
    var url = UPLOAD_PAGE_URL;
    var style = style || "dialogHeight:405px; dialogWidth:465px; help:0; resizable:0; status:0;scroll:0;"; // 上传文件页面弹出样式
    params = params || {};
    params.modalDialog = true;

    PISOpenDialog({ url: UPLOAD_PAGE_URL, params: params, style: style, onProcessFinish: onProcessFinish });
}

function PISOpenDownloadWin(fid) {
    var url = $.combineQueryUrl(DOWNLOAD_PAGE_URL, { id: fid });

    PISOpenLink(url);
}

Ext.define('PIS.data.FileStore', {
    extend: 'PIS.data.Store',
    alias: 'store.pis-filestore',
    mode: 'multi',

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            isclient: true,
            idProperty: "id",
            autoLoad: false,
            fields: [{ name: 'id' }, { name: 'name' }, { name: 'fullname' }, { name: 'size'}]
        }, config);

        me.mode = config.mode || me.mode;

        if (config.data || config.value) {
            config.data = config.data || config.value;

            if (typeof (config.data) != "object") {
                var frecs = me.getFileRecords(config.data);
                config.data = { total: frecs.length, records: frecs };
            }
        }

        this.callParent([config]);
    },

    getFileRecords: function (value) {
        var me = this;
        var frecords = value;

        if (typeof (value) == "string") {
            var fstrs = value.split(",");

            if ('single'.equals(me.mode) && fstrs.length > 1) {
                fstrs = [fstrs[0]];
            }

            var frecs = [];

            $.each(fstrs, function () {
                var fstr = this;
                if (fstr && fstr.length > 0) {
                    var fname = fstr.substring(fstr.indexOf("_") + 1);
                    var fid = fstr.substring(0, fstr.indexOf("_"));
                    var frec = { id: fid, name: fname, fullname: fstr };
                    frec.size = 0;

                    frecs.push(frec);
                }
            });

            frecords = frecs;
        }

        return frecords;
    },

    getTotalSize: function () {
        var me = this;
        var tot_size = 0;

        var frecs = me.getRange();
        Ext.each(frecs, function (rec) {
            tot_size += rec.get('size') || 0;
        });
    },

    getMaxSizeRecord: function () {
        var frecs = me.getRange();
        var max_rec = null;
        var max_size = 0;
        Ext.each(frecs, function (rec) {
            if (!max_rec || rec.get('size') > max_size) {
                max_rec = rec;
                max_size = rec.get('size');
            }
        });
    },

    addFiles: function (fnames) {
        var me = this;

        var recs = me.getFileRecords(fnames);
        var frecs = [];

        Ext.each(recs, function (rec) {
            frecs.push(rec);
        });

        me.add(frecs);
    },

    getStringValue: function () {
        var me = this;
        var frecs = me.getRange();
        var strval = "";

        Ext.each(frecs, function (rec) {
            strval += rec.get('fullname').toString() + ",";
        });

        if (strval) {
            strval = strval.trimEnd(",");
        }

        return strval;
    }
});

Ext.define('PIS.FileField', {
    extend: 'Ext.form.FieldContainer',
    mixins: { field: 'Ext.form.field.Field' },
    alias: 'widget.pis-filefield',
    store: null,
    readOnly: false,
    restoredata: [],
    filepanel: null,
    ctrlpanel: null,
    fileview: null,
    mode: 'multi',
    EditViewTemplate: null,
    ReadViewTemplate: null,
    maxLength: null,
    maxSingleSize: null,
    maxTotalSize: null,
    maxCount: null,
    blankText: '该输入项为必输项',
    maxLengthText: '最大文件名总长度为 {0}',
    maxSingleSizeText: '最大单个文件大小为 {0}',
    maxTotalSizeText: '最大总文件大小为 {0}',
    maxCountText: '最大总文件数为 {0}',

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            height: 80,
            border: false,
            readOnly: 'r'.equals(pgOp) || false,
            margin: "0 0 5 0",
            mode: "multi",
            maxCount: 20
        }, config);

        config.layout = 'border';

        var uploadparams = config.uploadparams || {};
        me.store = config.store || Ext.create('PIS.data.FileStore', config);
        me.restoredata = me.store.getRange();

        me.EditViewTemplate = new Ext.XTemplate(
            '<tpl for=".">',
                '<div class="pis-ctrl-file-link" linkfile="{fullname}" style="float:left; width:130; height: 20; margin:2px; border:0px;">',
                '<span style="border:0px; display:' + (('single'.equals(me.mode) || me.readOnly) ? 'none' : '') + '"><input type="checkbox" value="{id}" /></span>',
                '<a style="margin:5px; cursor: hand" title="{name}" onclick=PISOpenDownloadWin("{id}")>{name}</a>',
                '</div>',
            '</tpl>'
        );

        me.ReadViewTemplate = new Ext.XTemplate(
            '<tpl for=".">',
                '<div class="pis-ctrl-file-link" linkfile="{fullname}" style="float:left; width:140; height: 20; margin:2px; border:0px">',
                '<a style="margin:5px; cursor: hand" title="{name}" onclick=PISOpenDownloadWin("{id}")>{name}</a>',
                '</div>',
            '</tpl>'
        );

        var uploadbtn, removebtn, clearbtn, restorebtn;

        if ('single'.equals(me.mode)) {
            uploadbtn = Ext.create("PIS.Button", {
                text: '上传',
                flex: 1,
                handler: function () {
                    PISOpenUploadDialog(uploadparams, function (args) {
                        if (this.length > 0) {
                            me.removeAllFiles();
                            me.addFiles(this || "");
                        }
                    })
                }
            });

            clearbtn = Ext.create("PIS.Button", {
                text: '清空',
                flex: 1,
                handler: function () { me.removeAllFiles(); }
            });

            clearbtn = Ext.create("PIS.Panel", {
                region: 'east',
                layout: { type: 'hbox' },
                defaults: { margins: '0 0 0 5' },
                align: 'middle',
                border: false,
                width: 100,
                items: [uploadbtn, clearbtn]
            });
        } else {
            uploadbtn = Ext.create("PIS.Button", {
                text: '上传',
                handler: function () {
                    PISOpenUploadDialog(uploadparams, function (args) {
                        me.addFiles(this || "");
                    });
                }
            });

            clearbtn = Ext.create("PIS.Button", { text: '清空', handler: function () { me.removeAllFiles(); } });
            restorebtn = Ext.create("PIS.Button", { text: '还原', hidden: true, handler: function () { me.restoreFiles(); } });
            removebtn = Ext.create("PIS.Button", { text: '删除', handler: function () {
                var chkitems = Ext.query("input:checked", me.el.dom);
                $.each(chkitems, function () { me.removeFile(this.value); })
            }
            });

            me.ctrlpanel = Ext.create("PIS.Panel", {
                region: 'east',
                layout: { type: 'vbox', padding: '5', align: 'stretch', pack: 'center' },
                defaults: { margins: '0 0 5 0' },
                align: 'stretch',
                border: false,
                width: 100,
                items: [uploadbtn, removebtn, clearbtn, restorebtn]
            });
        };

        me.fileview = Ext.create('PIS.DataView', {
            layout: 'fit',
            itemSelector: 'div.pis-ctrl-file-link',
            tpl: me.EditViewTemplate,
            store: me.store
        });

        me.filepanel = Ext.create("PIS.Panel", {
            region: 'center',
            border: !me.readOnly,
            autoScroll: true,
            items: me.fileview
        });

        config.items = [me.filepanel, me.ctrlpanel];

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;
        me.callParent(arguments);
        me.addEvents('change');
    },

    restoreFiles: function () {
        var me = this;

        me.store.removeAll();
        me.store.add(me.restoredata);

        me.fireEvent('change');
    },

    removeFile: function (fid) {
        var me = this;

        // var rec = store.getById(fid);    // 不能工作，store的id值总是自动生成
        var idx = me.store.findExact("id", fid);    // 不能工作，store的id值总是自动生成
        me.store.removeAt(idx);

        me.fireEvent('change');
    },

    addFiles: function (fnames) {
        var me = this;
        me.store.addFiles(fnames.toString());
        me.fireEvent('change');
    },

    removeAllFiles: function () {
        var me = this;
        me.store.removeAll();
        me.fireEvent('change');
    },

    setValue: function (fnames) {
        var me = this;
        me.removeAllFiles();
        me.addFiles(fnames);
        me.fireEvent('change');
    },

    getValue: function () {
        var me = this;
        return me.store.getStringValue();
    },

    setReadOnly: function (b) {
        var me = this;
        me.readonly = b;
        if (me.ctrlpanel) {
            me.ctrlpanel.setVisible(!b);
        }

        $(me.el.dom).find("input").css("display", "none");
    },

    onRenderReadView: function () {
        return false;
    },

    validate: function () {
        var me = this,
            errors,
            isValid,
            wasValid;

        if (me.disabled) {
            isValid = true;
        } else {
            errors = me.getErrors();
            isValid = Ext.isEmpty(errors);
            wasValid = !me.hasActiveError();
            if (isValid) {
                me.unsetActiveError();
            } else {
                me.setActiveError(errors);
            }
        }
        if (isValid !== wasValid) {
            me.fireEvent('validitychange', me, isValid);
            me.updateLayout();
        }

        return isValid;
    },

    renderActiveError: function () {
        var me = this,
            activeError = me.getActiveError(),
            hasError = !!activeError;

        if (activeError !== me.lastActiveError) {
            me.fireEvent('errorchange', me, activeError);
            me.lastActiveError = activeError;
        }

        if (me.rendered && !me.isDestroyed && !me.preventMark) {
            Ext.get(me.fileview.el.dom.parentNode)[hasError ? 'addCls' : 'removeCls']("pis-ctrl-file-invalid");

            me.getActionEl().dom.setAttribute('aria-invalid', hasError);

            if (me.errorEl) {
                me.errorEl.dom.innerHTML = activeError;
            }
        }
    },

    getErrors: function (value) {
        var me = this,
            errors = [],
            validator = me.validator,
            allowBlank = me.allowBlank,
            maxTotalSize = me.maxTotalSize,
            maxSingleSize = me.maxSingleSize,
            maxCount = me.maxCount,
            vtype = me.vtype,
            vtypes = Ext.form.field.VTypes,
            msg;

        var value = me.getValue();

        if (Ext.isFunction(validator)) {
            msg = validator.call(me, value);
            if (msg !== true) {
                errors.push(msg);
            }
        }

        if (value.length < 1) {
            if (!allowBlank) {
                errors.push(me.blankText);
            }

            return errors;
        }

        if (me.maxLength && value.length > me.maxLength) {
            errors.push(Ext.String.format(me.maxLengthText, me.maxLength));
        }

        if (me.maxCount && me.store.getCount() > me.maxCount) {
            errors.push(Ext.String.format(me.maxCountText, me.maxCount));
        }

        if (me.maxSingleSize && me.store.getSingleSize() > me.maxSingleSize) {
            errors.push(Ext.String.format(me.maxSingleSizeText, me.maxSingleSize));
        }

        if (me.maxTotalSize && me.store.getTotalSize() > me.maxTotalSize) {
            errors.push(Ext.String.format(me.maxTotalSizeText, me.maxTotalSize));
        }

        if (vtype) {
            if (!vtypes[vtype](value, me)) {
                errors.push(me.vtypeText || vtypes[vtype + 'Text']);
            }
        }

        return errors;
    }
}, function () {
    this.borrow(Ext.form.field.Base, ['markInvalid', 'clearInvalid']);

});

//------------------------PIS ExtJs 文件控件 结束------------------------//

//------------------------------PIS ExtJs 表单域 结束----------------------------//


//-----------------------------PIS ExtJs 面板 开始--------------------------//

Ext.define('PIS.PanelMixin', {
    extend: 'Ext.Panel',
    alias: 'widget.pis-panelmixin',

    getDockedItem: function (selector, beforeBody) {
        var items = this.getDockedItems(selector, beforeBody);
        if (items && items.length > 0) {
            return items[0];
        }

        return null;
    }
});

Ext.define('PIS.Panel', {
    extend: 'Ext.Panel',
    alias: 'widget.pis-panel',
    mixins: ['PIS.PanelMixin']
});

Ext.define('PIS.Viewport', {
    extend: 'Ext.Viewport',
    alias: 'widget.pis-viewport',

    constructor: function (config) {
        config = Ext.apply({
            layout: 'border'
        }, config);

        this.callParent([config]);
    }
});

Ext.define('PIS.DataView', {
    extend: 'Ext.DataView',
    alias: 'widget.pis-dataview'
});

Ext.define('PIS.Page', {
    extend: 'PIS.Panel',
    alias: 'widget.pis-page',

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            layout: 'fit',
            border: false
        }, config);

        me.callParent([config]);
    }
});

Ext.define('PIS.StagePanel', {
    extend: 'PIS.Panel',
    alias: 'widget.pis-stagepanel',

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            layout: 'fit',
            border: false
        }, config);

        me.callParent([config]);
    },

    loadUrl: function (url, titleText) {
        var me = this;
        me.removeAll();

        var frame_id = me.id + "_frame";
        var frame_html = '<iframe width="100%" height="100%" id="' + frame_id + '" name="' + frame_id + '" '
            + ' src="' + url + '" contentid="' + me.id + '" frameborder="0"></iframe>'

        me.add({
            title: titleText,
            html: frame_html
        });
    },

    loadPage: function (cmp) {
        var me = this;
        me.removeAll();

        me.add(cmp);
    }
});

//-----------------------------PIS ExtJs 面板 结束--------------------------//

//-----------------------------PIS ExtJs Grid Panel 开始--------------------------//

Ext.define('PIS.grid.RowNumberer', {
    extend: 'Ext.grid.RowNumberer',
    alias: 'widget.pis-rownumberer',

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            width: 25,
            title: '&nbsp;',
            sortable: false
        }, config);

        me.callParent([config]);
    }
});

Ext.define('PIS.selection.CheckboxModel', {
    extend: 'Ext.selection.CheckboxModel',
    alias: 'selection.pis-checkboxmodel',

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            mode: "multi"
        }, config);

        me.callParent([config]);
    }
});

Ext.define('PIS.PagingField', {
    extend: 'Ext.form.NumberField',
    alias: 'widget.pis-pagingfield',

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            width: 50,
            minValue: 1,
            maxValue: 1000,
            value: 20,
            enableKeyEvents: true
        }, config);

        me.callParent([config]);
    }
});

Ext.define('PIS.toolbar.PagingToolbar', {
    extend: 'Ext.PagingToolbar',
    alias: 'widget.pis-pagingtoolbar',
    alternateClassName: 'PIS.PagingToolbar',
    pgfield: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            displayInfo: true,
            displayMsg: '当前条目 {0} - {1}, 总条目 {2}',
            emptyMsg: '无条目'
        }, config);

        if (!config.items) {
            if (!config.pgfield) {
                me.pgfield = Ext.create("PIS.PagingField", { pgbar: me });
            } else {
                me.pgfield = config.pgfield;
            }
            config.items = ['-', { text: '页面大小：', xtype: 'tbtext' }, me.pgfield]
        }

        me.callParent([config]);
    }
});

Ext.define('PIS.GridPanelMixin', {
    extend: 'Ext.grid.GridPanel',
    alias: 'widget.pis-gridpanelmixin',

    getSelections: function () {
        var sm = this.getSelectionModel();
        return sm.getSelection() || [];
    },

    getFirstSelection: function () {
        var selrecs = this.getSelections();

        if (!selrecs || selrecs.length <= 0) {
            return null;
        } else {
            return selrecs[0];
        }
    },

    hasSelection: function () {
        var sm = this.getSelectionModel();
        return sm.hasSelection();
    }
});

Ext.define('PIS.grid.GridPanel', {
    extend: 'Ext.grid.GridPanel',
    alias: 'widget.pis-gridpanel',
    alternateClassName: 'PIS.GridPanel',

    mixins: ['PIS.PanelMixin', 'PIS.GridPanelMixin', 'PIS.ExcelMixin'],

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
        }, config);

        config.selModel = config.selModel || Ext.create('PIS.selection.CheckboxModel');

        me.callParent([config]);
    }
});

Ext.define('PIS.grid.EditorGridPanel', {
    extend: 'PIS.grid.GridPanel',
    alias: 'widget.pis-editorgridpanel',

    mixins: ['PIS.PanelMixin'],
    editing: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            plugins: [],
            columns: []
        }, config);

        config.selModel = config.selModel || Ext.create('PIS.selection.CheckboxModel');

        me.editing = Ext.create('Ext.grid.plugin.CellEditing');
        config.plugins.push(me.editing);

        me.callParent([config]);
    },

    startEditByPosition: function (args) {
        this.editing.startEditByPosition(args);
    }
});

//-----------------------------PIS ExtJs GridPanel 结束--------------------------//


//-----------------------------PIS ExtJs Treeanel 开始--------------------------//

Ext.define('PIS.data.TreeStore', {
    extend: 'Ext.data.TreeStore',
    alias: 'store.pis-tree',
    mixins: ['PIS.data.StoreMixin'],

    expanded: false,
    rootPathLevel: 0,
    idProperty: null,
    parentIdProperty: null,
    isLeafProperty: null,
    pathLevelProperty: null,
    sortIndexProperty: null,
    iconProperty: null,

    constructor: function (config) {
        var me = this;
        config = Ext.apply({
            rootPathLevel: 0,
            idProperty: 'Id',
            defaultRootProperty: 'Items',
            parentIdProperty: 'ParentId',
            isLeafProperty: 'IsLeaf',
            pathLevelProperty: 'PathLevel',
            sortIndexProperty: 'SortIndex',
            iconProperty: 'Icon',
            folderSort: false
        }, config);

        config.root = config.root || { Name: 'Root', Items: [] };

        me.callParent([config]);

        if (!config.root && config.data) {
            var rdata = me.adjustRootData(config.data);
            me.setRootNode(rdata);
        }
    },

    adjustRootData: function (data) {
        var me = this;
        var rdata = { text: '.' };
        if (Ext.isArray(data) && data.length > 0) {
            rdata.children = [];

            Ext.each(data, function (node) {
                if (node[me.pathLevelProperty] == me.rootPathLevel) {
                    var p = me.adjustNodeData(data, node);
                    rdata.children.push(p);
                }
            });
        }

        return rdata;
    },

    adjustNodeData: function (data, parent) {
        var me = this;

        if (parent && Ext.isArray(data)) {
            Ext.each(data, function (node) {
                if (parent[me.idProperty] == node[me.parentIdProperty]) {
                    parent.children = parent.children || [];
                    parent.children.push(node);

                    me.adjustNodeData(data, node);
                }
            });

            if (parent.children && parent.children.length > 0) {
                parent.expanded = parent.expanded || me.expanded;
                parent.leaf = false;
            } else {
                parent.leaf = parent[me.isLeafProperty] || true;
            }

            if (me.iconProperty && parent[me.iconProperty]) {
                parent.icon = parent[me.iconProperty];
            }
        }

        return parent;
    }
});

Ext.define('PIS.TreePanel', {
    extend: 'Ext.TreePanel',
    alias: 'widget.pis-treepanel',
    mixins: ['PIS.PanelMixin', 'PIS.GridPanelMixin'],
    editing: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            rootVisible: false,
            bodyStyle: 'background-color:#FFFFFF',
            columnLines: false,
            rowLines: true
        }, config);

        me.callParent([config]);
    }
});

//-----------------------------PIS ExtJs Treeanel 结束--------------------------//


