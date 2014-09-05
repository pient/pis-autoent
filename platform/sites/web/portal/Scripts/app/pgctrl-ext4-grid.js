
//------------------------PIS ExtJs PageGridPanel 开始------------------------//

Ext.define('PIS.data.PageReader', {
    extend: 'PIS.data.JsonReader',
    alias: 'reader.pis-page',
    root: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            idProperty: "Id",
            totalProperty: "TotalCount",
            root: "Data"
        }, config);

        this.callParent([config]);
    }
});

Ext.define('PIS.data.PageProxy', {
    extend: 'PIS.data.AjaxProxy',
    alias: 'proxy.pis-page',

    constructor: function (config) {
        config = Ext.apply({
            reader: {}
        }, config);

        if (!(config.reader instanceof Ext.data.reader.Reader)) {
            config.reader = Ext.apply({
                type: 'pis-page'
            }, config.reader);
        }

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;
        me.callParent(arguments);
        
        me.addEvents('request');
    },

    doRequest: function (operation, callback, scope) {
        var me = this;

        var params = operation.params || {};
        params.data = params.data || {};

        var qryOptions = {
            Start: operation.start || 0,
            Limit: operation.limit,
            Conditions: {},
            Sorters: []
        };

        if (Ext.isArray(operation.sorters)) {
            var sorters = [];
            Ext.each(operation.sorters, function (s) {
                sorters.push({ "FieldName": s.property, "Ascending": (s.direction == "ASC") });
            });

            qryOptions.Sorters = sorters;
        }

        var conditions = [];

        qryOptions.Conditions = { Conditions: conditions };

        if (scope && scope.loadQryOptions) {
            scope.loadQryOptions(qryOptions);
        }
        
        me.fireEvent("request", scope, qryOptions, params);

        var qryOptStr = Ext.encode(qryOptions);

        PISAjaxRequest({
            url: me.url,
            params: { QryRequest: qryOptStr },
            onsuccess: function (response, opts) {
                me.processResponse(true, operation, qryOptions, response, callback, scope);
            },
            onfailure: function (response, opts) {
                me.fireEvent("requestfailed", me, response, opts);
            }
        });

        operation.setStarted();
    }
});

Ext.define('PIS.data.PageGridStore', {
    extend: 'PIS.data.Store',
    alias: 'store.pis-pagegridstore',
    alternateClassName: 'PIS.PageGridStore',
    grid: null,
    qryOptions: {},

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            idProperty: "Id",
            data: [],
            proxy: {}
        }, config);

        if (!(config.proxy instanceof Ext.data.proxy.Proxy)) {
            config.proxy = Ext.apply({
                noCache: false,
                type: 'pis-page',
                listeners: {
                    request: function (reqdata, params) {
                        me.fireEvent("request", reqdata, params);
                    },
                    requestfailed: function (response, opts) {
                        me.fireEvent("requestfailed", response, opts);
                    }
                }
            }, config.proxy);
        }

        me.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        me.callParent(arguments);

        me.addEvents('request');
        me.addEvents('requestfailed');
    },

    createModel: function (record) {
        if (!record.isModel) {
            var id = null;
            if (this.idProperty) {
                id = record[this.idProperty];
            }
            record = Ext.ModelManager.create(record, this.model, id);
        }

        return record;
    },

    loadQryOptions: function (qryOptions) {
        var me = this;
        var trigger = me.schtrigger;

        if (trigger && trigger.loadQryOptions) {
            trigger.loadQryOptions(qryOptions);
        }
    },

    doSearch: function (trigger, opts) {
        var me = this;

        me.schtrigger = trigger;

        me.loadPage(1);
    },

    load: function (options) {
        var me = this;

        options = Ext.apply((options || {}), me.loadargs || {});

        me.callParent([options]);

        return me;
    }
});

Ext.define('PIS.PageGridSchToolbar', {
    extend: 'PIS.Toolbar',
    alias: 'widget.pis-pagegridschtoolbar',

    constructor: function (config) {
        this.callParent([config]);
    }
});

Ext.define('PIS.PageGridSchPanel', {
    extend: 'PIS.FormPanel',
    alias: 'widget.pis-pagegridschpanel',
    grid: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            hideCollapseTool: true,
            labelWidth: 60,
            collapsed: false
        }, config);

        config.columns = config.columns || 3;

        if (!me.tbar) {
            var schtbar = { xtype: 'pis-pagegridschtoolbar' };

            title = config.title || '<b class="x-grid-cquery-title">&nbsp;复杂查询框&nbsp;</b>';
            schtbar.items = [title, '->', '-'];
            schtbar.items.push({ xtype: 'pis-schbutton', handler: function () { me.grid(this); } });
            schtbar.items.push('-');
            schtbar.items.push({ xtype: 'pis-clrbutton', text: '清除查询条件', handler: function () { me.doClear(); } });
            config.tbar = schtbar;
        }

        me.callParent([config]);

        me.on('afterrender', function () {
            me.initSchItem(me);
        });
    },

    initSchItem: function (item) {
        var me = this;

        if (item.schopts) {
            item.on('specialkey', function (f, e) {
                if (e.getKey() == e.ENTER) {
                    me.doSearch(item);
                }
            }, item);
        } else {
            if (item.items) {
                var p = this;
                if ($.isArray(item.items)) {
                    $.each(item.items, function () {
                        p.initSchItem(this);
                    })
                } else if (item.items.items) {
                    if ($.isArray(item.items.items)) {
                        var p = this;
                        $.each(item.items.items, function () {
                            p.initSchItem(this);
                        })
                    }
                }
            }
        }
    },

    loadQryOptions: function (qryOptions) {
        var me = this;

        var conditions = qryOptions.Conditions.Conditions;

        var schflds = me.query("[schopts]");

        Ext.each(schflds, function (f) {
            var _cond = f.schopts.condition;

            if (f.getValue) {
                if (typeof _cond.Field === "string") {
                    _cond.Field = { Name: _cond.Field };
                }

                var val = f.getValue();
                _cond.Value = f.getValue();
                if (_cond) { conditions.push(_cond); }
            }
        }, this);
    },

    doSearch: function (trigger) {
        var me = this;

        me.grid.store.doSearch(me);
    },

    doClear: function () {
        var me = this;
        var fields = me.form.getFields();
        fields.each(function (f) {
            if (!f.disabled && !f.readOnly && !f.hidden) {
                f.setValue("");
            }
        });
    },

    doCollapse: function () {
        if (me.grid.schpanel) {
            me.schpanel.toggleCollapse(true);
        }

        Ext.defer(function () {
            me.grid.doLayout();
        }, 100);
    }
});

Ext.define('PIS.PageGridButton', {
    extend: 'PIS.Button',
    alias: 'widget.pis-pagegridbutton',
    bttype: null,
    grid: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({}, config);

        me.grid = config.grid;
        config.handler = config.handler || me.onbtnclick;

        me.bttype = config.bttype;
        this.callParent([config]);
    },

    onbtnclick: function () {
        var me = this;
        var grid = me.grid;

        switch (me.bttype) {
            case "add":
                grid.loadFrmWin("add");
                break;
            case "edit":
                var rec = grid.getFirstSelection();
                if (!rec) {
                    PISMsgBox.alert("请先选择要操作的记录！");
                    return;
                }

                grid.loadFrmWin("modify", rec);
                break;
            case "delete":
                var recs = grid.getSelection();
                if (!recs || recs.length <= 0) {
                    PISMsgBox.alert("请先选择要删除的记录！");
                    return;
                }

                PISMsgBox.confirm("确定删除所选记录？", function (btn) {
                    if ('yes'.equals(btn)) {
                        var params = {
                            afterrequest: function () {
                                grid.store.reload();
                            }
                        };

                        grid.batchOperate('delete', recs, {}, params);
                    }
                });
                break;
            case "excel":
                var title = me.title || grid.pistitle || grid.title;
                
                grid.exportExcel({ title: title });
                break;
            case "cquery":
                grid.toggleSchPanel();
                break;
        }
    }
});

// TwinTriggerFiled查询控件
Ext.define('PIS.PageGridSearchField', {
    extend: 'Ext.form.TwinTriggerField',
    alias: 'widget.pis-pagegridsearchfield',
    grid: null,

    constructor: function (config) {
        var me = this;
        config = Ext.apply({
            fieldLabel: '查询 ',
            hideLabel: false,
            labelWidth: 40,
            clearCls: 'x-form-clear-trigger',
            triggerCls: 'x-form-search-trigger',
            hideTrigger: false
        }, config);

        this.callParent([config]);

        me.grid = config.grid;
    },

    initComponent: function () {
        var me = this;
        me.callParent(arguments);

        me.on('specialkey', function (f, e) {
            if (e.getKey() == e.ENTER) {
                this.onTriggerClick();
            }
        }, me);
    },

    loadQryOptions: function (qryOptions) {
        var me = this,
            conditions = [],
            val = me.getValue();

        if (val !== "") {
            if (me.grid && me.grid.columns) {
                var cols = me.grid.columns;

                Ext.each(cols, function (c) {
                    if (c.schopts && c.schopts.condition) {
                        var _cond = c.schopts.condition;
                        _cond.Value = val;

                        if (typeof _cond.Field === "string") {
                            _cond.Field = { Name: _cond.Field };
                        }

                        conditions.push(_cond);
                    }
                }, this);
            }
        }

        qryOptions.Conditions = { Type: 'Or', Conditions: conditions };
    },

    onTriggerClick: function () {
        var me = this;

        me.grid.store.doSearch(me);
    }
});

Ext.define('PIS.PageGridToolbar', {
    extend: 'PIS.Toolbar',
    alias: 'widget.pis-pagegridtoolbar',
    grid: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
        }, config);

        me.grid = config.grid;

        if (config.items) {
            Ext.each(config.items, function (it, i, its) {
                if (!it) return true;
                var itemcfg = it;

                if (typeof (it) == "string") {
                    switch (it) {
                        case "add":
                        case "save":
                        case "edit":
                        case "delete":
                        case "excel":
                        case "word":
                        case "cquery":
                        case "help":
                            itemcfg = { xtype: 'pis-pagegridbutton', bttype: it, grid: me.grid };
                            break;
                        case "schfield":
                            itemcfg = { xtype: 'pis-pagegridsearchfield', columns: config.fldcols || [], schbutton: true, clrbutton: true, grid: me.grid };
                            break;
                    }
                } else {
                    if (!it.xtype) {
                        itemcfg.xtype = 'pis-pagegridbutton';
                        itemcfg.grid = itemcfg.grid || me.grid;
                    }
                }

                its[i] = itemcfg;
            });
        }

        this.callParent([config]);
    }
});

Ext.define('PIS.PageGridTitlePanel', {
    extend: 'PIS.Panel',
    alias: 'widget.pis-pagegridtitlepanel',
    grid: null,
    schpanel: null,

    constructor: function (config) {
        config = Ext.apply({
            border: false
        }, config);
        config.tbar = config.tbar || config.tlbar;

        if (!config.items) {
            if (config.schpanel) {
                config.items = [config.schpanel];
            }
        }

        this.callParent([config]);
    }
});

Ext.define('PIS.GridPagingField', {
    extend: 'Ext.form.NumberField',
    alias: 'widget.pis-gridpagingfield',
    pgbar: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            width: 50,
            minValue: 1,
            maxValue: 1000,
            value: 25,
            enableKeyEvents: true
        }, config);

        me.callParent([config]);

        me.pgbar = config.pgbar;
        config.value = config.pageSize || me.pgbar.pageSize;
    },

    initComponent: function () {
        var me = this;
        me.callParent(arguments);

        me.on('keyup', function (f, e) {
            if (e.keyCode == "13") { // 回车
                me.onPageSizeChange();
            }
        })
    },

    onPageSizeChange: function () {
        var me = this;
        var currPageSize = me.getValue();
        if (currPageSize > me.maxValue || currPageSize < me.minValue) {
            return;
        }

        if (me.pgbar) {
            me.pgbar.store.pageSize = currPageSize;
            me.pgbar.store.loadPage(1, { start: 0, pageSize: currPageSize, limit: currPageSize });
        }
    }
});

Ext.define('PIS.toolbar.GridPagingToolbar', {
    extend: 'PIS.toolbar.PagingToolbar',
    alias: 'widget.pis-gridpagingtoolbar',
    alternateClassName: 'PIS.GridPagingToolbar',

    grid: null,
    pgfield: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({}, config);

        me.grid = config.grid;

        if (!config.pgfield) {
            config.pgfield = Ext.create("PIS.GridPagingField", {
                pgbar: me
            });
        }

        me.callParent([config]);
    },

    initComponent: function () {
        this.callParent(arguments);
    }
});

Ext.define('PIS.grid.PageGridPanel', {
    extend: 'PIS.grid.GridPanel',
    alias: 'widget.pis-pagegridpanel',
    alternateClassName: 'PIS.PageGridPanel',

    loadargs: null,
    titlepanel: null,
    schpanel: null,

    formWin : null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            border: false,
            columns: [],
            viewConfig: { loadMask: false }
        }, config);

        me.formparams = config.formparams || {};
        
        me.initStore(config);
        me.initColumns(config); // 初始化columns
        me.initToolbar(config); // 初始化工具栏
        me.initBottomBar(config);   // 初始化bbar

        me.callParent([config]);

        me.titlepanel = me.getDockedItem('pis-pagegridtitlepanel');

        if (me.titlepanel) {
            me.schpanel = me.titlepanel.schpanel;
        }
    },

    initComponent: function () {
        var me = this;

        me.callParent(arguments);
    },

    initStore: function (config) {
        var me = this;

        if (!(config.store instanceof Ext.data.Store)) {
            config.store = Ext.create('PIS.PageGridStore', {
                model: config.mode,
                proxy: { url: config.url }
            });
        }

        config.store.grid = config.store.grid || me;
    },

    initToolbar: function (config) {
        var me = this;

        if (!config.tbar) {
            if (!config.titbar && (config.tlbar || config.tlitems || config.schpanel || config.schitems)) {
                config.titbar = { xtype: 'pis-pagegridtitlepanel', grid: me };

                if (!config.tlbar && config.tlitems) {
                    config.tlbar = { xtype: 'pis-pagegridtoolbar', items: config.tlitems, fldcols: config.columns, grid: me }
                }

                config.titbar.tbar = config.tlbar;

                if (!config.schpanel) {
                    if (config.schitems) {
                        config.schpanel = {
                            xtype: 'pis-pagegridschpanel',
                            items: config.schitems,
                            columns: config.schcols,
                            grid: me
                        }
                    }
                }

                if (config.schpanel) {
                    config.schpanel.grid = config.schpanel.grid || me;

                    if (config.schpanel) {
                        if (!(config.schpanel instanceof Ext.Component)) {
                            config.schpanel.hidden = config.schpanel.hidden || true;
                            config.schpanel.items = config.schpanel.items || config.schpanel.schitems;
                            config.schpanel.columns = config.schpanel.columns || config.schpanel.schcols;

                            config.titbar.schpanel = Ext.create('PIS.PageGridSchPanel', config.schpanel);
                        } else {
                            config.titbar.schpanel = config.schpanel;
                        }
                    }
                } else {
                    config.titbar.schpanel = Ext.create('PIS.PageGridSchPanel', { hidden: true, grid: me });
                }
            }

            config.tbar = config.titbar;
        }
    },

    initColumns: function (config) {
        var me = this;
        if (Ext.isArray(config.columns)) {
            Ext.each(config.columns, function (col, i, cols) {
                if (true === col.schfield) {
                    col.schopts = col.schopts || { condition: { Type: 'Like' } };
                }

                if (col.schopts) {
                    if (col.dataIndex) {
                        col.schopts.condition = col.schopts.condition || { Type: 'Like' };
                        col.schopts.condition.Field = col.schopts.condition.Field || col.dataIndex;
                    }
                }

                if (col.enumdata) {
                    col.renderer = col.renderer || function (val, p, rec) {
                        return col.enumdata[val];
                    }
                } else if (col.formlink) {
                    col.renderer = col.renderer || function (val, p, rec) {
                        var params = me.getFormWinParams('r', rec.internalId);

                        var rtn = PISRenderFormLink({text: val, params: $.getJsonString(params)});
                        
                        return rtn;
                    };
                } else if (col.filelink) {
                    col.renderer = col.renderer || function (val, p, rec) {
                        var rtn = PISRenderFileLink(val);
                        
                        return rtn;
                    };
                } else if (col.btnparams) {
                    this.renderer = col.renderer || function (val, p, rec) {
                        var params = Ext.apply({
                            recid: rec.internalId,
                            colid: col.id,
                            id: "btn_" + col.id,
                            width: "100%",
                            value: val,
                            text: val || col.header
                        }, col.btnparams);

                        var rtn = PISApplyXTemplate('<button id={id} style="width: {width};" onclick="{handler}(\'{colid}\', \'{recid}\', \'{value}\')" class="pis-grid-button">{text}</button>', params);
                        
                        return rtn;
                    };
                } else if (col.renderparams) {
                    col.renderer = col.renderer || function (val, p, rec) {
                        return PISApplyXTemplate(col.renderparams, {data: rec.getData(), params: col.renderparams.params, value: val});
                    }
                } else if (col.dateonly) {
                    col.renderer = col.renderer || function (val, p, rec) {
                        return $.dateOnly(val);
                    }
                }
            })
        }
    },

    initBottomBar: function (config) {
        var me = this;

        if (!config.bbar) {
            if (!config.pgbar && config.pgbar != false) {
                config.pgbar = Ext.create('PIS.GridPagingToolbar', {
                    grid: me,
                    store: config.store
                });
                config.bbar = config.pgbar;
            }
        }
    },

    loadFrmWin: function (action, rec, refrec, params) {
        var me = this;

        me.formWin.load(me, action, rec, params);
    },

    batchOperate: function (action, recs, data, params) {
        params = params || {};
        var url = params.url || null;

        data = data || {};

        var idlist = [],
            dtlist = [];

        if (recs != null) {
            Ext.each(recs, function (r) {
                idlist.push(r.internalId);
                dtlist.push(r.getData());
            })
        }

        var listParams = (params.uploaddata === true ? dtlist : idlist);
        var listParamStr = Ext.encode(listParams);

        PISAjaxRequest({
            method: 'DELETE',
            url: url,
            params: { Params: listParamStr },
            onsuccess: function (response, opts) {
                me.store.reload();
            }
        });
    },

    toggleSchPanel: function () {
        var me = this;

        if (me.schpanel) {
            me.schpanel.setVisible(me.schpanel.hidden);
        }
    }
});

//------------------------PIS ExtJs PageGridPanel 结束------------------------//
