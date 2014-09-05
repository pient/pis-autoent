
var PISContentPanel = null;

//----------------------PIS ExtJs 门户菜单 开始----------------------//

MdlRecord = Ext.data.Record.create(
[{ name: 'ModuleID' }, { name: 'Code' }, { name: 'Name' }, { name: 'Type' }, { name: 'ParentID' },
{ name: 'Path' }, { name: 'PathLevel' }, { name: 'IsLeaf' }, { name: 'SortIndex' }, { name: 'Url' },
{ name: 'Icon' }, { name: 'Description' }, { name: 'Status' }, { name: 'IsSystem' }, { name: 'IsEntityPage'}]);

MdlColumns = [{ id: 'SortIndex', dataIndex: 'SortIndex', header: '排序号', hidden: true },
{ id: 'Name', dataIndex: 'Name', header: "名称", renderer: PISPortalMenuColRender}];

PISPortalMenuTopPathLevel = 0;

Ext.ux.PISPortalMenu = Ext.extend(Ext.ux.grid.PISTreeGridPanel, {
    constructor: function (config) {
        var menu = this;

        config = config || {};
        config.rootMenuItem = config.rootMenuItem || PISState["RootMenuItem"] || {};

        if (!config.store) {
            PISPortalMenuTopPathLevel = (config.rootMenuItem.PathLevel || 0) + 1;

            config.data = config.data || PISState["MdlList"] || [];
            $.each(config.data, function () {
                if (this.PathLevel === PISPortalMenuTopPathLevel) { this.ParentID = null; }
            })

            config.store = new Ext.ux.data.PISAdjacencyListStore({
                data: config.data,
                reader: new Ext.ux.data.PISJsonReader({ id: 'ModuleID', dsname: 'MdlList' }, MdlRecord)
            });
        }

        config.master_column_id = config.master_column_id || 'Name';
        config.hideHeaders = !(config.hideHeaders == false);
        config.collapsible = (config.collapsible == true);
        config.border = (config.border == true);
        config.split = (config.split == true);
        config.noline = !(config.noline == false);
        config.width = config.width || 180;
        config.minSize = config.width || 150;
        config.maxSize = config.width || 250;
        config.columns = config.columns || MdlColumns;
        config.autoExpandColumn = config.autoExpandColumn || "Name";
        // config.title = ('<div style="cursor:hand; vertical-align:middle; background-color:white; height: 60px;" onclick="doHelp();" align="left">' + (config.title || '') + '</div>');

        Ext.ux.PISPortalMenu.superclass.constructor.call(this, Ext.apply(config, {
        }));
    },
    initComponent: function () {
        var menu = this;

        Ext.ux.PISPortalMenu.superclass.initComponent.call(this);

        menu.on("afterrender", function () {
            menu.expandLoaded();
            // setTimeout(function () { menu.selectHome(); }, 500);
            setTimeout(function () { menu.selectWorkbench(); }, 500);
        });

        menu.on("rowclick", function (sm, ridx, e) {
            var rec = menu.store.getAt(ridx);
            // store.toggleNode(rec);
            var url = rec.get('Url');

            if (url && PISContentPanel) { PISContentPanel.loadContent(url); }
        });

        var sm = menu.getSelectionModel();
        if (sm) {
            sm.on("rowselect", function (sm, ridx, e) {
                var rec = menu.store.getAt(ridx);
                // store.toggleNode(rec);
                var url = rec.get('Url');

                if (url && PISContentPanel) { PISContentPanel.loadContent(url); }
            });
        }
    },
    selectHome: function () {
        var menu = this;
        menu.getSelectionModel().selectRow(1);
    },
    selectWorkbench: function () {
        var menu = this;
        menu.getSelectionModel().selectRow(2);
    }
});

Ext.reg('pis-portalmenu', Ext.ux.PISPortalMenu);

// 链接渲染
function PISPortalMenuColRender(val, p, rec) {
    var rtn = val || "";
    switch (this.id) {
        case "Name":
            if (val) {
                var icon = rec.get('Icon');
                if (icon) { icon = PISPortalMenuGetIconImg(icon); }

                if (rec.get("PathLevel") == PISPortalMenuTopPathLevel) {
                    rtn = '<span valign="bottom" style="cursor:hand; font-weight: bold; margin-top:10px; color:gray">' + icon + '&nbsp;' + val + '</span>';
                } else {
                    rtn = '<span valign="bottom" style="cursor:hand; height:18px; padding:1px;">' + icon + '&nbsp;' + val + '</span>';
                }
            } else {
                rtn = '';
            }

            if ('M_SYS_FAV_MSG'.equals(rec.get('Code'))) {
                rtn += '<span style="font-weight:bold; color: red; margin:2px;">(<span id="mitem_msgnew">0</span>)</span>'
            }

            if ('M_SYS_FAV_TASK'.equals(rec.get('Code'))) {
                rtn += '<span style="font-weight:bold; color: red; margin:2px;">(<span id="mitem_tasknew">0</span>)</span>'
            }
            break;
    }

    return rtn;
}

function PISPortalMenuGetIconImg(iconurl, title, handler, baseurl) {
    // 获取图标html代码
    var baseurl = baseurl || ICON_IMG_BASE;
    var rtn = "";
    if (iconurl) {
        var url = iconurl;

        // 已设置绝对路径
        if (iconurl.indexOf('/') < 0) {
            url = baseurl + iconurl;
        }
        rtn += "<span style='vertical-align:middle;'>";
        rtn += "<img style='width:15px; height:15px; padding:0px; margin:0px; border:0px; " + (handler ? "cursor:hand" : "") + "' src='" + url + "' ";

        if (title) { rtn += " title='" + title + "' "; }
        if (handler) { rtn += " onclick='" + handler + "' "; }

        rtn += " /></span>";
    }

    return rtn;
}

//----------------------PIS ExtJs 门户菜单 结束----------------------//

//----------------------PIS ExtJs 内容块 开始----------------------//

Ext.ux.PISContentPanel = Ext.extend(Ext.ux.PISPanel, {
    frameContent: null,
    constructor: function (config) {
        if (PISContentPanel != null) {
            throw "内容页面已经初始化";
        }

        config = config || {};
        config.title = config.title || "&nbsp;";

        Ext.ux.PISContentPanel.superclass.constructor.call(this, Ext.apply(config, {
        }));
    },
    initComponent: function () {
        var content = this;
        PISContentPanel = this;

        var frame_id = content.id + "_frame";
        var frame_html = '<iframe width="100%" height="100%" id="' + frame_id + '" name="' + frame_id + '" contentid="' + content.id + '" onload="PISOnPortalContentLoad(this)" frameborder="0"></iframe>'

        Ext.ux.PISContentPanel.superclass.initComponent.call(this);

        this.on('afterrender', function () {
            content.body.update(frame_html);
            frameContent = Ext.get(frame_id);
        })
    },
    loadContent: function (url) {
        frameContent.dom.src = url;
    },
    loadTitle: function (tit_data) {
        var content = this;
        if (!tit_data) {
            content.setTitle('<div align="left"><a href="/portal/home.aspx" target="' + frameContent.dom.id + '">主页</a></div>');
        }
    }
});

Ext.reg('pis-contentpanel', Ext.ux.PISContentPanel);

function PISRetrieveContentPanel(frame) {
    if (frame.contentid) {
        var cont_panel = Ext.getCmp(frame.contentid);
        return cont_panel;
    }

    return null;
}

function PISOnPortalContentLoad(frame) {
    // 加载路径
    if (!frame.contentWindow.PISPath) {
        PISContentPanel.loadTitle();
    }
}

//----------------------PIS ExtJs 内容块 结束----------------------//

//------------------------PIS ExtJs 门户 开始------------------------//

Ext.ux.PISPortal = Ext.extend(Ext.ux.Portal, {
    constructor: function (config) {
        config = config || {};
        this.layout = 'border';
        if (config.picdraggable == false) {
            $.each(config.items, function () {
                var item = this;
                $.each(item.items, function () {
                    var iitem = this;
                    iitem.draggable = false;
                });
            });
        }

        Ext.ux.PISPortal.superclass.constructor.call(this, Ext.apply(config, {
        }));
    }
});

Ext.reg('pis-portal', Ext.ux.PISPortal);

//------------------------PIS ExtJs 门户 结束------------------------//

//------------------------图片查看控件 开始------------------------//

var ImgView = Ext.extend(Ext.ux.PISPanel, {
    height: 740,
    img_index: 0,
    img_view_id: this.id + '_img',
    set_img: function (offset) {
        this.img_index = this.img_index + offset;
        if (this.img_index < 0) {
            this.img_index = this.slides.length - 1;
        } else if (this.img_index >= this.slides.length) {
            this.img_index = 0;
        }

        Ext.get(this.img_view_id).dom.src = this.slides[this.img_index].ImageUriStr;
        pic_win.setTitle("图片 - " + this.slides[this.img_index].Title);
        // Ext.getCmp(this.id + '_pre_btn').setDisabled(((this.img_index) == 0) ? true : false);
        // Ext.getCmp(this.id + '_next_btn').setDisabled(((this.img_index) == this.slides.length - 1) ? true : false);
    },
    initComponent: function () {
        var cmp = this;
        this.html = '<img id=\'' + this.img_view_id + '\' style="height:400" ></img>';
        this.tbar = [{
            text: "上一张",
            id: this.id + '_pre_btn',
            handler: function () {
                cmp.set_img(-1);
                //旋转样式为默认
                //var img_obj = Ext.getDom('img1_img');
                //img_obj.style.filter = 'Progid:DXImageTransform.Microsoft.BasicImage(Rotation=1)';
            }
        }, {
            text: "下一张",
            id: this.id + '_next_btn',
            handler: function () {
                cmp.set_img(1);
                //var img_obj = Ext.getDom('img1_img');
                //img_obj.style.filter = 'Progid:DXImageTransform.Microsoft.BasicImage(Rotation=1)';
            }
        }/*, {
                        text: '旋转',
                        handler: function () {
                            var img_obj = Ext.getDom('img1_img');
                            arcSize += (arcSize != 4) ? 1 : -3;
                            //通过滤镜进行图片旋转
                            // img_obj.style.filter = 'Progid:DXImageTransform.Microsoft.BasicImage(Rotation=' + arcSize + ')';
                        }
                    }*/];

        ImgView.superclass.initComponent.call(this);
    },
    afterRender: function () {
        ImgView.superclass.afterRender.call(this);
        // Ext.get(this.img_view_id).parent = this;
        // Ext.get(this.img_view_id).center();
        // new Ext.dd.DD(Ext.get(this.img_view_id), 'pic');

        // Ext.get(this.img_view_id).dom.title='双击放大 右击缩小';

        Ext.get(this.img_view_id).on({
            //双击图片放大
            'dblclick': {
                fn: function () {
                    // Ext.get(this).parent.zoom(Ext.get(this), 1.5, true);
                }
            },
            //单击右键 图片缩小1.5倍
            'contextmenu': {
                fn: function () {
                    // Ext.get(this).parent.zoom(Ext.get(this), 1.5, false);
                }
            }
        });
    },
    // 放大、缩小
    zoom: function (el, offset, type) {
        var width = el.getWidth();
        var height = el.getHeight();
        var nwidth = type ? (width * offset) : (width / offset);
        var nheight = type ? (height * offset) : (height / offset);
        var left = type ? -((nwidth - width) / 2) : ((width - nwidth) / 2);
        var top = type ? -((nheight - height) / 2) : ((height - nheight) / 2);
        el.animate({
            height: { to: nheight, from: height },
            width: { to: nwidth, from: width },
            left: { by: left },
            top: { by: top }
        }, null, null, 'backBoth', 'motion');
    }
});

//------------------------图片查看控件 结束------------------------//

//------------------------load layout 开始------------------------//

function generateLayout(layoutdef) {
    var cfg = $.getJsonObj(layoutdef['Config']);
    var layout = cfg;

    var tools = [{
        id: 'maximize',
        qtip: '更多',
        handler: function (e, target, p) {
            if (p.script) { eval(p.script); }
        }
    }];

    // 默认禁止动画，否则会导致页面重新获取
    $.each(layout, function () {
        var col = this;
        if (col && col.items) {
            $.each(col.items, function () {
                var item = this;
                item.frame = false;
                item.tools = tools;
                // item.bbar = { xtype: 'pis-toolbar', items: ['->', { xtype: 'pis-morebutton', scale: 'small'}] };
                item.bodyStyle = "background-color:#f5f9f9";
                item.bbar = {};
                item.animate = (item.animate == true);
                item.animCollapse = (item.animCollapse == true);
            })
        }
    });

    return layout;
}

function renderLayout(portal, dtmdls) {
    var p, portlets;
    if (typeof (portal) == "string") {
        p = Ext.getCmp(portal);
    } else {
        p = portal;
    }

    portlets = p.find("pictype", "portlet");

    $.each(portlets, function () {
        var portlet = this;
        this.body.update("<div style='padding:5px;'>正在加载...</div>");

        $.ajaxExec('qryportlet', { code: portlet.code, isfooter: !!(portlet.footer) }, function (args) {
            renderPortlet(args.data["Portlet"], portlet, dtmdls);
        });
    });
}

function renderPortlet(pdef, p, dtmdls) {
    var cfg = $.getJsonObj(pdef['Config']);
    cfg.datamodule = pdef["DataModule"] || "System";    // 默认系统模块

    if (!p.title) { p.setTitle(cfg.title); }
    if (cfg.script) { p.script = cfg.script; }
    
    switch (cfg.type) {
        case "list":
            renderList(cfg, p, dtmdls);
            break;
        default:
            renderDefault(cfg, p, dtmdls);
            break;
    }

    // p.bbar = { xtype: 'pis-toolbar', items: ['->', { xtype: 'pis-morebutton', scale: 'small'}] };
    /*$(p.bbar.id).val(""); 
    var t_bbar = new Ext.ux.PISToolbar({ items: ['->', { xtype: 'pis-morebutton', scale: 'small'}] });
    t_bbar.render(p.bbar); */
}

function renderDefault(cfg, p, dtmdls) {
    // var tp = new Ext.Panel(cfg);
    p.body.update(cfg.html);
}

function renderList(cfg, p, dtmdls) {
    var html = cfg.header || "";
    var tmpl = cfg.item;
    var rows = cfg.rows || 0;

    var qryurl = retrieveQryUrl(cfg, dtmdls);
    var hqlstr = translatePISVar(cfg.hql);

    $.ajaxExec('qrydata', { hql: hqlstr, rows: rows, subcode: (p.subcode || "") }, function (args) {
        var dt = args.data;
        rows = (rows < dt.length) ? rows : dt.length;

        for (var i = 0; i < rows; i++) {
            var item = dt[i];

            if (item['Grade']) {
                if (item['Grade'] == 'Important' && item['Title']) {
                    item['Title'] = "<b style='color:red'>[重要]</b>&nbsp;" + item['Title'];
                }
            } else if (item['Important']) {
                if (item['Important'] == 'Important' && item['Subject']) {
                    item['Subject'] = "<b style='color:red'>[重要]</b>&nbsp;" + item['Subject'];
                }
            }

            html += tmpl.replace(/\${([\w,.,_]+)\}/g, function (match, value) { return item[value]; });

            html = translatePISVar(html, item);
        }

        html += cfg.footer || "";
        html = '<div style="padding: 5px;">' + html + '</div>';

        p.body.update(html);

        postRenderList(p.body.dom);

        // 调整viewport,防止出现横向滚动条
        viewport.doLayout();

    }, null, qryurl);
}

// 后继处理List
function postRenderList(dom) {
    $(dom).find('.date_list').each(function (i) {
        var item = this;
        var date_str = $(item).text()
        var tdate = new Date(date_str);
        
        $(item).text((tdate.getMonth() + 1) + '-' + tdate.getDate());
    });

    var div_items = $(dom).find('.div_list');

    div_items.each(function (i) {
        var item = this;
        if (i == div_items.length - 1) {
            item.style.borderBottom = 0;
        }
    });
}

function retrieveQryUrl(cfg, dtmdls) {
    var mdlstr = cfg.datamodule;
    var qryurl = PORTAL_DATA_PAGE_URL;

    $.each(dtmdls, function() {
        if (this.Value.equals(mdlstr)) {
            qryurl = this.Tag || qryurl;
            return false;
        }
    });

    return qryurl;
}

// 替换相关值
function translatePISVar(expr) {
    var rtnstr = expr;

    rtnstr = expr.replace(/\#{([\w,.,_]+)\}/g, function(match, value) {
        var str = value;

        if (value) {
            var vals = value.split('.');
            if (vals.length == 2) {
                str = PISVar[vals[0]][vals[1]];
            } else if (vals.length == 1 && PISVar[vals[0]]) {
                str = PISVar[vals[0]];
            }
        }

        return str;
    });

    return rtnstr;
}

//------------------------load layout 结束------------------------//


//------------------------load popup page 开始------------------------//

function loadPopup() {
    var qryurl = BIZ_DATA_PAGE_URL;

    // 获取弹出窗口列表（后面可考虑设计成rss格式, 考虑系统框架的xml支持）
    $.ajaxExec('qrydata', { path: "data.portlet.publicinfo", code: "DAT_PI_POPUP" }, function (args) {
        var dt = args.data;

        // 挨个弹出窗口
        for (var i = 0; i < dt.length; i++) {
            var item = dt[i];
            if (item) {
                var id = item.id || item.Id;

                if (id) {
                    var qryurl = $.combineQueryUrl(BIZ_DATA_PAGE_URL, { path: "data.portlet.infocontent", id: id });

                    OpenInfoViewerWin(qryurl);
                }
            }
        }

    }, null, qryurl);

    $.ajaxExec('qryportlet', { code: 'DAT_MSG_POPUP' }, function (args) {
        // 弹出个人信息
        // renderPortlet(args.data["Portlet"], portlet, dtmdls);
    });

    var msg_count = null, task_count = null;

    msg_count = top.PISState["NewMsgCount"] || 0;

    $.ajaxExec('qrydata', { path: 'data.portal.msgtask' }, function (args) {
        task_count = args.data.NewTaskCount || 0;

        if (msg_count != null && task_count != null) {
            popWin({ msg_count: msg_count, task_count: task_count });
        }
    }, null, "/biz/CommonPages/PageData.aspx");
}

function popWin(params) {
    var msg_count = params.msg_count, task_count = params.task_count;
    var html = "";

    if (msg_count > 0) {
        html += "您有 " + msg_count + " 条新消息，<span onclick='top.OnFuncClick(\"msg\")' style='cursor:hand; color:blue'>查看</span><br><br>"
    }

    if (task_count > 0) {
        html += "您有 " + task_count + " 条新任务，<span onclick='top.OnFuncClick(\"task\")' style='cursor:hand; color:blue'>查看</span><br><br>"
    }

    if (!html) return;

    win = null, bodymask = null;

    // 如果有新任务或新消息则弹出提示信息
    if (!win) {
        win = new Ext.Window({
            layout: 'fit',
            width: 400,
            height: 200,
            closeAction: 'hide',
            padding: '5 5 5 5',
            title: '系统消息',
            html: html,
            buttons: [{ text: '确定', handler: function () { win.hide(); } }]
        });

        win.on("hide", function () {
            if (bodymask) { bodymask.hide(); }
        })
    }

    bodymask = new Ext.LoadMask(Ext.getBody(), { msgCls: 'x-mask-nomsg' });
    win.show();
    bodymask.show();
}

//------------------------load popup page 结束------------------------//

//------------------------for silverlight 开始------------------------//

var pic_win, pic_panel;

function OnSLTitleClick(current, slides) {
    var c = getPicObject(current);
    var s = [];
    for (var i = 0; i < slides.length; i++) {
        s.push(getPicObject(slides[i]));
    }

//    var p_str = $.getJsonString({ current: c, slides: s });
//    var p_str = Base64.encode(p_str);
//    alert(p_str.length);
//    var p_str = Base64.decode(p_str);
//    alert(p_str.length);

    // OpenWin(url, "_blank", style)
    showPicWin({current: c, slides: s});
}

function OnSLPictureClick(current, slides) {
    // alert("OnSLPictureClick");
}

function OnSLPageLoad() {
    // alert("OnSLPageLoad");
}

function getPicObject(pic) {
    var pic_obj = {};

    pic_obj.ImageUriStr = pic.ImageUriStr;
    pic_obj.RedirectLink = pic.RedirectLink;
    pic_obj.Order = pic.Order;
    pic_obj.Title = pic.Title;
    pic_obj.Description = pic.Description;

    return pic_obj;
}

function showPicWin(params) {
    pic_panel = new ImgView({
        id: "img1",
        region: 'center',
        img_index: params.current.Order,
        current: params.current,
        slides: params.slides
    });

    if (!pic_win) {
        pic_win = new Ext.Window({
            layout: 'border',
            width: 800,
            height: 500,
            closeAction: 'hide',
            padding: '5 5 5 5',
            title: '图片查看',
            items: [pic_panel],

            buttons: [{
                text: '确定',
                handler: function () {
                    pic_win.hide();
                }
            }]
        });
    }

    pic_win.show();

    pic_panel.img_index = (params.current.Order - 1) || 0.
    pic_panel.set_img(0);
}

function onSilverlightError(sender, args) {
    var appSource = "";
    if (sender != null && sender != 0) {
        appSource = sender.getHost().Source;
    }

    var errorType = args.ErrorType;
    var iErrorCode = args.ErrorCode;

    if (errorType == "ImageError" || errorType == "MediaError") {
        return;
    }

    var errMsg = "Unhandled Error in Silverlight Application " + appSource + "\n";

    errMsg += "Code: " + iErrorCode + "    \n";
    errMsg += "Category: " + errorType + "       \n";
    errMsg += "Message: " + args.ErrorMessage + "     \n";

    if (errorType == "ParserError") {
        errMsg += "File: " + args.xamlFile + "     \n";
        errMsg += "Line: " + args.lineNumber + "     \n";
        errMsg += "Position: " + args.charPosition + "     \n";
    }
    else if (errorType == "RuntimeError") {
        if (args.lineNumber != 0) {
            errMsg += "Line: " + args.lineNumber + "     \n";
            errMsg += "Position: " + args.charPosition + "     \n";
        }
        errMsg += "MethodName: " + args.methodName + "     \n";
    }

    throw new Error(errMsg);
}

//------------------------for silverlight 开始------------------------//