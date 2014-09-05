
var FIELD_REQUIRED_CLSNAME = "pis-ui-required";
var FIELD_REQUIRED_BGCOLOR = "#FEFFBB";

var MSGSEND_PAGE_URL = "/portal/CommonPages/Util/MsgSend.aspx";
var UPLOAD_PAGE_URL = "/portal/CommonPages/File/Upload.aspx";
var HELP_PAGE_URL = "/portal/CommonPages/Help/HelpPage.aspx";
var VERSION_PAGE_URL = "/portal/CommonPages/Help/VersionPage.aspx";
var INFO_VIEWER_PAGE_URL = "/portal/CommonPages/Portal/InfoViewer.aspx";

var DOWNLOAD_PAGE_URL = "/portal/CommonPages/File/DownLoad.aspx";
var IMPORT_PAGE_URL = "/portal/CommonPages/Data/DataImport.aspx";
var EXPORT_PAGE_URL = "/portal/CommonPages/Data/DataExport.aspx";
var FLOT_CHART_PAGE_URL = "/portal/CommonPages/Chart/FlotChart.aspx";

var USER_SELECT_URL = "/portal/CommonPages/Select/UsrSelect/MUsrSelect.aspx";
var GROUP_SELECT_URL = "/portal/CommonPages/Select/GrpSelect/MGrpSelect.aspx";
var ROLE_SELECT_URL = "/portal/CommonPages/Select/RolSelect/MRolSelect.aspx";
var ENUM_SELECT_URL = "/portal/CommonPages/Select/EnumSelect/MEnumSelect.aspx";
var SHARED_ICON_BASEPATH = "/portal/images/shared/"
var FCK_EDITOR_BASEPATH = "/portal/js/fckeditor/"

var BLANK_PIS_URL = "/portal/images/portal/blank.gif";
var ICON_IMG_BASE = '/portal/images/shared/';

var PORTAL_DATA_PAGE_URL = "/portal/CommonPages/Data/PageData.aspx";
var BIZ_DATA_PAGE_URL = "/biz/CommonPages/PageData.aspx";

var PIS = PIS || {};

Ext.BLANK_IMAGE_URL = '/Images/s.gif';
Ext.FlashComponent.EXPRESS_INSTALL_URL = '/portal/js/ext4/resources/expressinstall.swf';
Ext.chart.Chart.CHART_URL = '/portal/js/ext4/resources/charts.swf';

PIS_PAGE_STATE_KEY = "__PAGESTATE";
PIS_PAGE_SCH_CRIT_KEY = "SearchCriterion";
PIS_QUERY_CRIT_KEY = "qrycrit";
PIS_FORM_DATA_KEY = "frmdata";
PIS_REQ_DATA_KEY = "reqdata";

PIS_RESP_SEX_KEY = "__SEXCEPTION";
PIS_RESP_EX_KEY = "__EXCEPTION";
PIS_RESP_MSG_KEY = "__MESSAGE";

//-----------------------------PIS ExtJs 配置信息 开始--------------------------//

Ext.define('PISConfig', {
    statics: {
        SubPortalPath: '/Home/SubPortal',
        StagePagePath: '/Home/StagePage',

        PageDataPath: '/api/portal/GetPageData',

        Init: function () {
            window.PISPortal = window.top.PISPortal;

            Ext.Loader.setConfig({
                enabled: true,
                paths: {
                    'PIS': '/Scripts/app'
                }
            });

            Ext.tip.QuickTipManager.init();

            Ext.Ajax.defaultHeaders = {
                'Accept': 'application/json'
            };
        }
    }
});

PISConfig.Init();

//-----------------------------PIS ExtJs 配置信息 结束--------------------------//