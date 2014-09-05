
//------------------------PIS 页面处理 开始------------------------//

var PIS_OP_ACTION = { c: 'create', cs : 'createsub', cp: 'copy', r: 'read', u: 'update', d: 'delete', exec: 'execute' };
var PIS_TIMER_INTERVAL = 600000;  // 600s

var PISState = {};
var PISPath = null;
var PISVar = {};
var PISSearchCrit = {}; // 数据查询规则

var pgTimerEnabled = false; // 是否pgTimer生效

var pgOp, pgOperation, pgOpType, pgOperationType, pgAction;

// 页面初始化
Ext.onReady(function () {
    PISState = getPageState() || PISState;

    PISVar["UserInfo"] = PISState["UserInfo"];
    PISVar["SystemInfo"] = PISState["SystemInfo"];

    pgOp = pgOperation = $.getQueryString({ ID: "op" });   // 操作
    pgOpType = pgOperationType = $.getQueryString({ ID: "optype", DefaultValue: "s" });   // 操作执行位置(s, c)服务器端客户端
    pgAction = PIS_OP_ACTION[pgOp] || pgOp;    // 页面活动

    if (PISState) { PISSearchCrit = PISState[PIS_PAGE_SCH_CRIT_KEY] || {}; }
    
    pgInit();
});

// 获取PageState
function getPageState() {
    var pgState = null;
    var psstr = $("#" + PIS_PAGE_STATE_KEY).val();
    if (psstr != null) { pgState = $.getJsonObj(psstr); }

    return pgState;
}

// 初始化PIS页面
function pgInit() {
    initPISEvt();   // 初始化事件
    initPISUI();    // 初始化PIS界面
    initPISQry();   // 初始化PIS查询控件

    PISPgObserver.onPgLoad(); // 触发pgload事件
}

// 由页面打开的窗口回吊使用
function pgDialogCallback(args) {
    PISPgObserver.onDgCallback(args);
}

function pgTimeElapsed() {
    if (pgTimerEnabled == true) {
        PISPgObserver.onPgTimer(); // 触发pgtimer事件

        PISPgTimer = setTimeout(pgTimeElapsed, PIS_TIMER_INTERVAL);
    }
}

function enablePgTimer() {
    pgTimerEnabled = true;

    PISPgTimer = setTimeout(pgTimeElapsed, PIS_TIMER_INTERVAL);
}

function disablePgTimer() {
    pgTimerEnabled = false;

    if (PISPgTimer) { clearTimeout(PISPgTimer); }
}

// 初始化事件
function initPISEvt() {
    if (typeof (onPgInit) == "function") { PISPgObserver.addListener('pginit', onPgInit); }
    if (typeof (onFrmLoad) == "function") { PISPgObserver.addListener('frmload', onFrmLoad); }
    if (typeof (onPgLoad) == "function") { PISPgObserver.addListener('pgload', onPgLoad); }
    if (typeof (onPgTimer) == "function") { PISPgObserver.addListener('pgtimer', onPgTimer); }    // onPgTimer shouldn't include time-consuming or user interacted function
    if (typeof (onPgUnload) == "function") { PISPgObserver.addListener('pgunload', onPgUnload); }
    if (typeof (onDgCallback) == "function") { PISPgObserver.addListener('dgcallback', onDgCallback); }

    $(window).bind("beforeunload", function () {
        // 触发pgunload事件
        PISPgObserver.onPgUnload();
    });

    $(window).bind("beforeprint", function() {
        // 隐藏按钮（并保存当前按钮状态以便于还原）
    });

    $(window).bind("afterprint", function() {
        // 还原按钮
    });

    PISPgObserver.onPgInit(); // 触发pginit事件
}

// 初始化PIS样式
function initPISUI() {
    PISPgObserver.onFrmLoad(); // 触发frmload事件
}

// 初始化PIS查询控件
function initPISQry() {
    $("[picqry], [qryopts], [qrygrp]").each(function (i) {
        var qryopts = $.getJsonObj($(this).attr("qryopts")) || {};
        var qryevent = qryopts["event"] || "keyup";
        if (qryevent && !$(this).attr(qryevent)) {
            $(this).bind(qryevent, function (event) {
                var event = event || window.event;
                if (qryevent != "keyup" || (qryevent == "keyup" && event.keyCode == 13)) {
                    qryEventHandler(this, event);
                }
            });
        }
    });
}

// 操作类型
var OP_TYPE = {
    c: { name: "c", code: "create", text: "创建", action: "create" },
    r: { name: "r", code: "read", text: "读取", action: "read" },
    u: { name: "u", code: "update", text: "更新", action: "update" },
    d: { name: "d", code: "delete", text: "删除", action: "delete" },
    o: { name: "o", code: "other", text: "其他", action: "other" }
}

//------------------------PIS 页面处理 结束------------------------//


//------------------------PIS 查询规则处理 开始------------------------//

// 查询方式
var SEARCH_MODE = {
    Equal: ["C", 0],
    NotEqual: ["C", 1],
    In: ["C", 2],
    NotIn: ["C", 3],
    Like: ["C", 4],
    NotLike: ["C", 5],
    GreaterThan: ["C", 6],
    GreaterThanEqual: ["C", 7],
    LessThan: ["C", 8],
    LessThanEqual: ["C", 9],
    StartWith: ["C", 10],
    EndWith: ["C", 11],
    NotStartWith: ["C", 12],
    NotEndWith: ["C", 13],
    UnSettled: ["C", 14],
    IsEmpty: ["S", 0],    // 查询集合时使用
    IsNotEmpty: ["S", 1],    // 查询集合时使用
    IsNull: ["S", 2],
    IsNotNull: ["S", 3],
    UnSettled: ["S", 4]   // 未设定
}

// 数据类型
var DATA_TYPE = {
    Integer: "Int32",
    Int: "Int32",
    Date: "DateTime"
}

// 获取查询模式值
function getSearchModeValue(str) {
    return SEARCH_MODE[str] || SEARCH_MODE["Equal"];    // 默认等于查询
}

// 处理查询
function qryEventHandler(obj, event) {
    var qryopts = $.getJsonObj($(obj).attr("qryopts")) || {};
    var qryTarget = ($(qryopts["target"])[0] || {}).PIS || PISDefQryTarget;
    if (qryTarget) {
        var schCrit = getSchCriterion(obj);
        if (qryTarget.query) {
            qryTarget.query(schCrit["ccrit"], schCrit["ftcrit"]);
        }
    }
}

function getSchCriterionByGroup(pisgrp, crit) {
    var tcrit = { ccrit: [], ftcrit: [], jcrit: [] };
    
    crit = crit || tcrit;
    if (!pisgrp) return crit;

    var qryinputs = $("[pisgrp=" + pisgrp + "]");


    // 普通检索
    $.each(qryinputs, function (i) {
        var val;

        if (Ext && Ext.getCmp && this.id) {
            var cmp = Ext.getCmp(this.id);
            val = cmp.getValue();
        } else {
            val = $(this).val();
        }

        var _crit = getSingleSchCriterion($(this).attr("qryopts"), val);

        switch (_crit["type"]) {
            case "fulltext":
                crit["ftcrit"].push(_crit["crit"]);
                break;
            case "junc":
                crit["jcrit"].push(_crit["crit"]);
                break;
            default:
                crit["ccrit"].push(_crit["crit"]);
                break;
        }
    });

    return crit;
}

// 由Html元素和查询标准模板获取查询标准对象
function getSchCriterion(htmlele, crit) {
    
    var tcrit = { ccrit: [], ftcrit: [], jcrit: [] };

    if (!htmlele) return crit || tcrit;
    crit = crit || tcrit;

    var pisgrp = $(htmlele).attr("pisgrp"); // 查询组

    var qryinputs = [];
    if (!pisgrp) {  // 没有组，则为单字段查询
        qryinputs[qryinputs.length] = htmlele;
    } else {
        qryinputs = $("[pisgrp=" + pisgrp + "]");
    }

    // 普通检索
    $.each(qryinputs, function (i) {
        var val;

        if (Ext && Ext.getCmp && this.id) {
            var cmp = Ext.getCmp(this.id);
            val = cmp.getValue();
        } else {
            val = $(this).val();
        }

        var _crit = getSingleSchCriterion($(this).attr("qryopts"), val);

        switch (_crit["type"]) {
            case "fulltext":
                crit["ftcrit"].push(_crit["crit"]);
                break;
            case "junc":
                crit["jcrit"].push(_crit["crit"]);
                break;
            default:
                crit["ccrit"].push(_crit["crit"]);
                break;
        }
    });

    return crit;
}

function getSingleSchCriterion(obj, val) {
    var opts = {};

    if (typeof (obj) == "string") {
        opts = $.getJsonObj(obj) || {};
    } else if (typeof (obj) == "object") {
        opts = obj;
    }
    
    var tcrit = {}
    var isft = false;

    // 查询类型 fulltext或junc或default(全文查询或默认查询)
    var type = opts["type"] || "default";

    // junc查询
    if (isJuncSchOptions(opts)) {
        type = "junc";
    }

    if (type.toLowerCase() == "fulltext") {
        tcrit["Value"] = val;
        tcrit["ColumnList"] = (opts["field"] ? opts["field"].split(",") : null);
    } else if (type.toLowerCase() == "junc") {
        tcrit = getJuncSchCriterion(opts, val);
    } else {
        tcrit["PropertyName"] = opts["field"] || obj.id || $(obj).attr("name");
        tcrit["Value"] = val;
        if (opts["datatype"]) {
            tcrit["Type"] = DATA_TYPE[opts["datatype"]] || opts["datatype"];
        }
        var schMode = getSearchModeValue(opts["mode"]);
        if (schMode[0] == "C") {
            tcrit["SearchMode"] = schMode[1]
        } else {
            tcrit["SingleSearchMode"] = schMode[1];
        }
    }

    return {crit:tcrit, type:type};
}

function isJuncSchOptions(opts) {
    return !!(opts["juncmode"] || opts["items"] && $.isArray(typeof (opts["items"])) || $.isArray(opts));
}

// 获取连接查询
function getJuncSchCriterion(opts, val) {
    var tcrit = { JunctionMode: 'Or', Searches: [] };
    var items = opts["items"] || [];
    tcrit["JunctionMode"] = opts["juncmode"] || "Or";

    if ($.isArray(opts)) {
        items = opts;
    }

    $.each(items, function() {
        var _tcrit = getSingleSchCriterion(this, val);
        if (_tcrit["crit"]) {
            tcrit["Searches"].push(_tcrit["crit"]);
        }
    });

    return tcrit;
}

//------------------------PIS 查询规则处理 结束------------------------//
