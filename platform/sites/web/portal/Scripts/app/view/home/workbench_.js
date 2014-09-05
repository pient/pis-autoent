

//----------------------PIS ExtJs Icon列表块 开始----------------------//
Ext.define('PIS.WorkbenchViewport', {
    extend: 'PIS.Viewport',
    alias: 'widget.pis-workbenchviewport',

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            layout: { type: 'border', padding: '0', align: 'stretch' }
        }, config);

        var wbview = Ext.create('PIS.WorkbenchView', {
            id: 'flow_view',
            cls: 'flow_view',
            picdata: [{
                Index: '1',
                GCode: 'ADM',
                GName: '行政中心流程',
                List: [
                        { Index: '101', Code: 'OA_AdminFeeByMonthFlow', Name: '行政报销（月结）流程', Icon: 'flowform.gif', Url: "/biz/Flows/reimb/OA_ReimbList.aspx", Style: null },
                        { Index: '102', Code: 'OA_SEAL_USE', Name: '印鉴使用流程', Icon: 'flowform.gif', Url: "/biz/Flows/reimb/OA_ReimbList.aspx", Style: null },
                        { Index: '103', Code: 'OA_SEAL_LEND', Name: '印鉴外借流程', Icon: 'people_down.gif', Url: "/biz/Flows/reimb/OA_ReimbList.aspx", Style: null },
                        { Index: '104', Code: 'OA_TRIP_TICKET', Name: '教职工出差、订票流程', Icon: 'flow.gif', Url: "/biz/Flows/reimb/OA_ReimbList.aspx", Style: null },
                        { Index: '105', Code: 'OA_OVERTIME', Name: '教职工加班流程', Icon: 'sheet.gif', Url: "/biz/Flows/reimb/OA_ReimbList.aspx", Style: null },
                        { Index: '106', Code: 'OA_TIME_ADJ', Name: '教职工调休流程', Icon: 'flowform.gif', Url: "/biz/Flows/reimb/OA_ReimbList.aspx", Style: null },
                        { Index: '107', Code: 'OA_LEAVE_APY', Name: '教职工请假流程', Icon: 'flowform.gif', Url: "/biz/Flows/reimb/OA_ReimbList.aspx", Style: null },
                        { Index: '108', Code: 'OA_OUT_APY', Name: '教职工外出流程', Icon: 'flowform.gif', Url: "/biz/Flows/reimb/OA_ReimbList.aspx", Style: null },
                        { Index: '109', Code: 'OA_OFFICE_SUPPLY_APY', Name: '办公用品领用流程', Icon: 'flowform.gif', Url: "/biz/Flows/reimb/OA_ReimbList.aspx", Style: null },
                        { Index: '110', Code: 'OA_CAR_APY', Name: '车辆使用申请流程', Icon: 'flowform.gif', Url: "/biz/Flows/reimb/OA_ReimbList.aspx", Style: null },
                        { Index: '111', Code: 'OA_FA_SCRAP', Name: '固定资产报废流程', Icon: 'flowform.gif', Url: "/biz/Flows/reimb/OA_ReimbList.aspx", Style: null },
                        { Index: '112', Code: 'OA_FA_SCRAP', Name: '固定资产转移流程', Icon: 'flowform.gif', Url: "/biz/Flows/reimb/OA_ReimbList.aspx", Style: null },
                        { Index: '113', Code: 'OA_FA_ADD', Name: '固定资产新增流程', Icon: 'flowform.gif', Url: "/biz/Flows/reimb/OA_ReimbList.aspx", Style: null },
                        { Index: '114', Code: 'OA_MEETING_ROOM_APY', Name: '会议室使用申请流程', Icon: 'flowform.gif', Url: "/biz/Flows/reimb/OA_ReimbList.aspx", Style: null },
                        { Index: '115', Code: 'ADM_BIZ_CARD_APY', Name: '名片印刷申请流程', Icon: 'flowform.gif', Url: "/biz/Flows/reimb/OA_ReimbList.aspx", Style: null }
                    ]
            }, {
                Index: '2',
                GCode: 'HR',
                GName: '人事中心流程',
                List: [
                        { Index: '201', Code: 'HR_AdminFeeByMonthFlow', Name: '招聘需求申请流程', Icon: 'flowform.gif', Url: "/biz/Flows/reimb/OA_ReimbList.aspx", Style: null },
                        { Index: '202', Code: 'HR_YPWLFeeFlow', Name: '教职工转正申请流程', Icon: 'flowform.gif', Url: "/biz/Flows/reimb/OA_ReimbList.aspx", Style: null },
                        { Index: '203', Code: 'HR_KYBFeeFlow', Name: '教职工录用申请流程', Icon: 'people_down.gif', Url: "/biz/Flows/reimb/OA_ReimbList.aspx", Style: null },
                        { Index: '204', Code: 'HR_OtherFeeFlow', Name: '教职工转岗申请流程', Icon: 'flow.gif', Url: "/biz/Flows/reimb/OA_ReimbList.aspx", Style: null },
                        { Index: '205', Code: 'HR_YGFlow', Name: '培训需求申请流程', Icon: 'sheet.gif', Url: "/biz/Flows/reimb/OA_ReimbList.aspx", Style: null },
                        { Index: '206', Code: 'HR_ZSGTFeeFlow', Name: '教职工离职申请流程', Icon: 'flowform.gif', Url: "/biz/Flows/reimb/OA_ReimbList.aspx", Style: null },
                        { Index: '207', Code: 'HR_ZSGTFeeFlow', Name: '教职工入职申请流程', Icon: 'flowform.gif', Url: "/biz/Flows/reimb/OA_ReimbList.aspx", Style: null },
                        { Index: '208', Code: 'HR_ZSGTFeeFlow', Name: '教职工辞退流程', Icon: 'flowform.gif', Url: "/biz/Flows/reimb/OA_ReimbList.aspx", Style: null }
                    ]
            }, {
                Index: '3',
                GCode: 'FA',
                GName: '财务部流程',
                List: [
                        { Index: '301', Code: 'FA_LOAN', Name: '借款申请流程', Icon: 'money.gif', Url: "/biz/Flows/reimb/OA_ReimbList.aspx", Style: null },
                        { Index: '302', Code: 'FA_PAY', Name: '付款申请流程', Icon: 'money.gif', Url: "/biz/Flows/reimb/OA_ReimbList.aspx", Style: null },
                        { Index: '303', Code: 'FA_DAILY_BUDGET', Name: '日常预算申请流程', Icon: 'money.gif', Url: "/biz/Flows/reimb/OA_ReimbList.aspx", Style: null },
                        { Index: '304', Code: 'FA_PRJ_BUDGET', Name: '项目预算申请流程', Icon: 'money.gif', Url: "/biz/Flows/reimb/OA_ReimbList.aspx", Style: null }
                    ]
            }, {
                Index: '4',
                GCode: 'IT',
                GName: '信息部流程',
                List: [
                        { Index: '401', Code: 'IT_ACT_APY', Name: '信息系统账号申请流程', Icon: 'flowform.gif', Url: "/biz/Flows/reimb/OA_ReimbList.aspx", Style: null },
                        { Index: '402', Code: 'IT_APP_REQ', Name: '应用系统需求申请流程', Icon: 'flowform.gif', Url: "/biz/Flows/reimb/OA_ReimbList.aspx", Style: null },
                        { Index: '403', Code: 'IT_SRV_APY', Name: 'IT服务申请流程', Icon: 'people_down.gif', Url: "/biz/Flows/reimb/OA_ReimbList.aspx", Style: null },
                        { Index: '404', Code: 'IT_FILE_SPC_APY', Name: '文件服务器空间申请流程', Icon: 'flow.gif', Url: "/biz/Flows/reimb/OA_ReimbList.aspx", Style: null }
                    ]
            }, {
                Index: '5',
                GCode: 'Doc',
                GName: '收文和发文管理',
                List: [
                        { Index: '501', Code: 'DOC_ADM', Name: '公文管理流程', Icon: 'flowform.gif', Url: "/biz/Flows/reimb/OA_ReimbList.aspx", Style: null }
                    ]
            }, {
                Index: '6',
                GCode: 'CEO',
                GName: '院办流程',
                List: [
                        { Index: '601', Code: 'CEO_WORK', Name: '工作督办流程', Icon: 'flowform.gif', Url: "/biz/Flows/reimb/OA_ReimbList.aspx", Style: null },
                        { Index: '602', Code: 'CEO_PLAN', Name: '部门月度计划、汇报流程', Icon: 'flowform.gif', Url: "/biz/Flows/reimb/OA_ReimbList.aspx", Style: null }
                    ]
            }]
        });

        wbview.on('itemclick', function (view, rec, dom, idx, e, eOpts) {
            var item = e.getTarget('li.view_item', 2);
            var list = rec.get("List");

            var nIdx = $.toInt(item.getAttribute("nodeIndex"));

            var url = list[nIdx]["Url"];
            var style = list[nIdx]["Style"] || { width: 750, height: 600, scrollbars: 'yes' };

            if (parent.PISPortalContent) {
                parent.PISPortalContent.loadPage(url);
            }
        });

        var viewPanel = Ext.create('PIS.Panel', {
            layout: 'fit',
            title: '所有流程',
            border: false,
            items: wbview
        });

        var favview = Ext.create('PIS.WorkbenchView', {
            id: 'fav_flow_view',
            cls: 'flow_view',
            picdata: [{
                Index: '1',
                GCode: 'ADM',
                GName: '行政中心流程',
                List: [
                        { Index: '101', Code: 'FL_OA_AdminFeeByMonth', Name: '行政报销（月结）流程', Icon: 'flowform.gif', Url: "/biz/Flows/reimb/OA_ReimbList.aspx", Style: null },
                        { Index: '109', Code: 'FL_OA_OfficeSupplyApy', Name: '办公用品领用流程', Icon: 'flowform.gif', Url: "/biz/Flows/OfficeSupply/List.aspx", Style: null },
                        { Index: '114', Code: 'FL_OA_MeetingRoomApy', Name: '会议室使用申请流程', Icon: 'flowform.gif', Url: "/biz/Flows/MeetingRoom/List.aspx", Style: null }
                    ]
            }, {
                Index: '2',
                GCode: 'HR',
                GName: '人事中心流程',
                List: [
                        { Index: '201', Code: 'FL_HR_RecruitNeed', Name: '招聘需求申请流程', Icon: 'flowform.gif', Url: "/biz/Flows/RecruitNeeds/List.aspx", Style: null },
                        { Index: '203', Code: 'FL_HR_StaffHire', Name: '教职工录用申请流程', Icon: 'people_down.gif', Url: "/biz/Flows/StaffHire/List.aspx", Style: null },
                        { Index: '205', Code: 'FL_HR_TrainingNeed', Name: '培训需求申请流程', Icon: 'sheet.gif', Url: "/biz/Flows/TrainingNeeds/List.aspx", Style: null }
                    ]
            }, {
                Index: '3',
                GCode: 'FA',
                GName: '财务部流程',
                List: [
                        { Index: '303', Code: 'FL_FA_DailyBudget', Name: '日常预算申请流程', Icon: 'money.gif', Url: "/biz/Flows/DailyBudget/List.aspx", Style: null }
                    ]
            }]
        });

        favview.on('itemclick', function (view, rec, dom, idx, e, eOpts) {
            var item = e.getTarget('li.view_item', 2);
            var list = rec.get("List");

            var nIdx = $.toInt(item.getAttribute("nodeIndex"));

            var url = list[nIdx]["Url"];
            var style = list[nIdx]["Style"] || { width: 750, height: 600, scrollbars: 'yes' };

            if (parent.PISPortalContent) {
                parent.PISPortalContent.loadPage(url);
            }
        });

        var favPanel = Ext.create('PIS.Panel', {
            layout: 'fit',
            title: '常用流程',
            border: false,
            items: favview
        });

        var tabs = Ext.widget('tabpanel', {
            activeTab: 1,
            border: false,
            region: 'center',
            defaults: { bodyPadding: 5 },
            items: [viewPanel, favPanel]
        });

        config.items = [tabs]

        this.callParent([config]);
    }
});

Ext.define('PIS.WorkbenchItem', {
    extend: 'Ext.data.Model',
    fields: [
            { name: 'Index', type: 'string' },
            { name: 'GCode', type: 'string' },
            { name: 'GName', type: 'string' },
            { name: 'List' }
        ]
});

Ext.define('PIS.WorkbenchView', {
    extend: 'Ext.DataView',
    alias: 'widget.pis-workbenchview',

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            itemSelector: 'ul',
            multiSelect: false,
            autoScroll: true
        }, config);

        if (!config.store) {
            if (config.picdata) {
                var store = Ext.create('PIS.data.ArrayStore', {
                    model: 'PIS.WorkbenchItem',
                    sortInfo: { field: 'Index', direction: 'ASC' }
                });

                store.loadData(config.picdata);

                config.store = store;
            }
        }

        config.tpl = config.tpl || new Ext.XTemplate(
        '<tpl for=".">',
            '<div class="x-grid-group dataview">',
                '<div class="x-grid-group-hd dataview-group-header"><div>{GName}</div></div>',
                    '<div class="x-grid-group-body">',
                        '<ul>',
                            '<tpl for="List">',
                                '<li class="view_item" nodeIndex={[xindex-1]} title="{Name}">',
                                    '<img width="50" height="50" src="/portal/images/thumbnail/128x128/{Icon}" />',
                                    '<b style="font-size:12px; color:navy; line-height: 1.2">{Name}</b>',
                                '</li>',
                            '</tpl>',
                        '</ul>',
                    '</div>',
                '</div>',
            '</div>',
        '</tpl>'
        );

        this.callParent([config]);

        me.on('containerclick', function (dv, evt) {
            // expand/collapse group
            var group = evt.getTarget('div.dataview-group-header', 2, false);
            if (group) {
                $(group.nextSibling).toggleClass('x-grid-group-collapsed');
                if (group) $(group.parentNode).toggleClass('dataview-group-collapsed');
            }
        });

        me.on('viewready', function () {
            var items = me.el.query('li');

            Ext.each(items, function (it) {
                Ext.get(it).addClsOnOver('view_item-hover');
            });
        });
    }
});

//----------------------PIS ExtJs Icon列表块 结束----------------------//
