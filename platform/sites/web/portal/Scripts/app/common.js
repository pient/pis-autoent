/// <reference path="jquery-1.4.1-vsdoc2.js" />

var CHS_WEEK_DICT = { 0: '日', 1: '一', 2: '二', 3: '三', 4: '四', 5: '五', 6: '六' };

//
//---------------------------------JQuery扩展开始--------------------------------------------------------------
//
$.extend({
    getJsonObj: function (jsonstr) {
        ///	<summary>
        /// 获取由Json字符串获取Json对象
        ///	</summary>
        ///	<param name="jsonstr" type="String">
        /// jsont字符串
        ///	</param>
        ///	<returns type="Object" />
        var obj = null;
        try {
            var jstr = jsonstr.replace(/\r/g, "\\r");
            obj = eval("(" + jstr + ")");
            return obj;
        } catch (e) {
            try {
                obj = eval("(" + jsonstr + ")");
            } catch (ei) {
                return null
            }

            return obj;
        }
    },

    // 转换对象为Json字符串
    getJsonString: function (obj, escapedflag) {
        ///	<summary>
        /// 获取指定对象的Json字符串
        ///	</summary>
        ///	<param name="jsonstr" type="Object">
        /// 对象
        ///	</param>
        ///	<returns type="Object">jsont字符串<returns>
        var type = typeof (obj);

        switch (type.toLowerCase()) {
            case 'string':
                var objstr = obj.toString();
                if (escapedflag == true) {
                    objstr = escape(obj.toString());
                } else if (escapedflag == false) {
                    objstr = unescape(obj.toString());
                }
                return "\"" + $.encodeJsonString(objstr) + "\"";
                //return "\"" + obj.toString() + "\"";
            case 'boolean':
            case 'number':
                return obj.toString();
            case 'object':
                if (obj.constructor == Array) {
                    var strArray = '[';
                    for (var i = 0; i < obj.length; ++i) {
                        var value = '';
                        if (obj[i]) {
                            value = $.getJsonString(obj[i]);
                        }
                        strArray += value + ',';
                    }
                    if (strArray.charAt(strArray.length - 1) == ',') {
                        strArray = strArray.substr(0, strArray.length - 1);
                    }
                    strArray += ']';
                    return strArray;
                } else if (obj.constructor == Date) {
                    return "\"" + $.encodeJsonString($.getAdjustedDate(obj)) + "\"";
                }
            default:
                {
                    var serialize = '{';
                    for (var key in obj) {
                        if (obj[key] != undefined && typeof (obj[key]) != "function") {
                            var subserialize = 'null';
                            subserialize = $.getJsonString(obj[key]);
                            serialize += '' + key + ' : ' + subserialize + ',';
                        }
                    }
                    if (serialize.charAt(serialize.length - 1) == ',') {
                        serialize = serialize.substr(0, serialize.length - 1);
                    }
                    serialize += '}';
                    return serialize;
                }
        }
    },

    // 对对象进行编码
    getEscapedData: function (obj, flag) {
        ///	<summary>
        /// 获取指定对象escape编码后对象
        ///	</summary>
        ///	<param name="obj" type="Object/String">
        /// 对象
        ///	</param>
        ///	<returns type="Object">Object/String<returns>
        var type = typeof (obj);

        switch (type) {
            case 'string':
                if (flag) {
                    return escape(obj);
                } else {
                    return unescape(obj);
                }
                break;
            case 'object':
                var cobj;
                if (obj.constructor == Array) {
                    cobj = [];
                    $.each(obj, function () {
                        cobj[i] = $.getEscapedData(this, flag);
                    });
                } else if (obj) {
                    cobj = {};
                    for (var key in obj) {
                        cobj[key] = $.getEscapedData(obj[key], flag);
                    }
                }
                return cobj;
                break;
            default:
                return obj;
        }
    },

    encodeJsonString: function (jstr) {
        ///	<summary>
        /// 格式化JsonString(转换特殊字符)
        ///	</summary>
        ///	<param name="jstr" type="String">
        /// 普通字符串
        ///	</param>
        ///	<returns type="jstr">jsont字符串<returns>
        jstr = jstr.toString().replace(/(\\)/g, "\\$1")
            .replace(/(\/)/g, "\\$1")
            .replace(new RegExp('(["\"])', 'g'), "\\\"");

        return jstr;
    },

    mergeJson: function (obj1, obj2) {
        ///	<summary>
        ///	合并json属性
        ///	</summary>
        for (var z in obj2) {
            if (obj2.hasOwnProperty(z)) {
                obj1[z] = obj2[z];
            }
        }
        return obj1;
    },

    getEval: function (evalstr) {
        ///	<summary>
        ///	得到eval后的返回值,若执行出错则返回null
        ///	</summary>
        ///	<param name="jsonstr" type="String">
        /// 要执行的语句
        ///	</param>
        try {
            return eval(evalstr);
        } catch (e) {
            return null;
        }
    },

    cloneObj: function (obj) {
        ///	<summary>
        ///	克隆对象
        ///	</summary>
        ///	<param name="jsonstr" type="String">
        /// 要克隆对象
        ///	</param>
        if (typeof (obj) != 'object') return obj;
        if (obj == null) return obj;

        var newObj = new Object();

        for (var i in obj) {
            newObj[i] = $.cloneObj(obj[i]);
        }

        return newObj;
    },

    getJsonObjByPairStr: function (str, sep, isep) {
        sep = sep || ":";
        isep = isep || ";";
        var item_strs = str.split(sep);
        var jsonobj = {};
        $.each(item_strs, function () {
            var pair = this.split(isep);
            json[pair[0]] = pair[1];
        });

        return jsonobj;
    },

    getStrByJsonObj: function (jsonobj, isep, sep) {
        var str = "";
        isep = isep || ":";
        sep = sep || ";";

        for (var key in jsonobj) {
            str += key + isep + jsonobj[key].toString() + sep
        }

        str = str.trimEnd(sep);

        return str;
    },

    isSetted: function (obj) {
        ///	<summary>
        ///	判断某对象是否已被设置值
        ///	</summary>
        ///	<param name="obj" type="Object">
        /// 要判断的对象
        ///	</param>
        return typeof (obj) != "undefined" && obj != null;
    },

    htmlEncode: function (input) {
        var converter = document.createElement("DIV");
        converter.innerText = input;
        var output = converter.innerHTML;
        converter = null;
        return output;
    },

    htmlDecode: function (input) {
        var converter = document.createElement("DIV");
        converter.innerHTML = input;
        var output = converter.innerText;
        converter = null;
        return output;
    },

    //------------------------url处理扩展开始-------------------------//

    getAllQueryStrings: function (options) {
        defaults = { DefaultValue: "undefined", URL: window.location.href };
        options = $.extend(defaults, options);
        var qs;
        var args = new Array();
        if (typeof (options.URL.split("?")[1]) != "undefined") {
            qs = options.URL.split("?")[1].replace(/\+/g, ' ').split('&');
            $.each(qs, function (i) {
                var currentArg = this.split('=');
                if (currentArg.length == 2) {
                    args[i] = { name: currentArg[0], value: currentArg[1] };
                    args[currentArg[0]] = { name: currentArg[0], value: currentArg[1] };
                } else {
                    args[i] = { name: currentArg[0], value: currentArg[1] };
                    args[currentArg[0]] = { name: currentArg[0], value: currentArg[0] };
                }
            });
        }
        if (args.length <= 0) { };
        return args;
    },

    getQueryString: function (options) {
        defaults = { DefaultValue: "", URL: window.location.href };
        options = $.extend(defaults, options);
        if (typeof ($.getAllQueryStrings({ URL: options.URL })[options.ID]) == "undefined") {
            return options.DefaultValue;
        } else {
            return $.getAllQueryStrings({ DefaultValue: options.DefaultValue, URL: options.URL })[options.ID].value;
        }
    },

    getQueryUrl: function (url) {
        url = url || document.location.href;
        var sepIdx = url.indexOf("?");
        if (sepIdx < 0) return url;

        return url.substr(0, sepIdx);
    },

    formatQueryUrl: function (url) {
        url = url || document.location.href;
        // 移除井号（防刷新符）
        return url.trimEnd("#").replace(/#\?/g, "?");
    },

    // 组合url
    combineQueryUrl: function (url, param) {
        url = url || document.location.href;

        if (!($.isSetted(param)) || !param) {
            return url;
        }

        var args = $.getAllQueryStrings({ URL: url }) || {};

        var parr = [];
        switch (typeof (param)) {
            case "object":
                if ($.isArray(param)) {
                    parr = param;
                } else if ($.isPlainObject(param)) {
                    for (var key in param) {
                        if (param[key] != undefined && param[key] != null) {
                            parr.push(key + "=" + param[key].toString());
                        }
                    }
                }
                break;
            case "string":
                parr = param.trimStart("&").split("&");
                break;
        }

        $.each(parr, function () {
            var iarr = this.split("=");
            args[iarr[0]] = { name: iarr[0], value: iarr[1] };
        });

        var qryurl = $.getQueryUrl(url) + "?";
        var pstr = "";

        for (var key in args) {
            if (args[key] && isNaN(key) && typeof (args[key]) != 'function') {
                pstr += "&" + args[key].name + "=" + args[key].value;
            }
        }

        qryurl += pstr.trimStart("&");

        return qryurl;
    },

    //------------------------url处理扩展结束-------------------------//

    //------------------------类型转换扩展开始-------------------------//

    toBool: function (value) {
        ///	<summary>
        ///	将字符串转化为bool类型
        ///	</summary>
        ///	<param name="str" type="String">
        /// 输入字符串
        ///	</param>
        ///	<returns type="Bool" />
        if (typeof (value) == "boolean") {
            return value;
        }
        if (!value) {
            return false;
        }
        value = (value + "").toLowerCase();
        if (value == "true" || value == "t" || value == "yes" || value == "on" || value == "ok") {
            return true;
        }
        else {
            return false;
        }
    },

    toInt: function (value) {
        ///	<summary>
        ///	将字符串转化为Int类型
        ///	</summary>
        ///	<param name="str" type="String">
        /// 输入字符串
        ///	</param>
        ///	<returns type="Integer" />
        return parseInt(value);
    },

    toFloat: function (value, sb) {
        ///	<summary>
        ///	转换为浮点数
        ///	</summary>
        ///	<param name="sb" type="Integer">
        /// 保留小数位数
        ///	</param>
        if (!sb || typeof (sb) != "number") {
            return parseFloat(value.toString());
        } else {
            var powInt = Math.pow(10, sb);
            return (parseInt(parseFloat(value.toString()) * powInt) / powInt);
        }
    },

    toDate: function (str, divChar) {
        ///	<summary>
        ///	将指定格式的字符串转化为日期对象
        ///	</summary>
        ///	<param name="str" type="String">
        /// 输入字符串,格式为（yyyy-mm-dd hh:mm:ss）
        ///	</param>
        ///	<returns type="Date"></returns>
        try {
            var divChar = divChar || "-";
            if (str.indexOf(divChar) < 0) {
                divChar = "/";
            }

            var parts = str.split(" ");
            var dp = parts[0].split(divChar);
            //var dt = new Date(); //parseInt("09")等有BUG
            //dt.setFullYear(parseInt(parseFloat(dp[0])),parseInt(parseFloat((dp[1])-1)),parseInt(parseFloat(dp[2])));
            var dt = new Date(dp[0], dp[1] - 1, dp[2]);
            if (parts.length > 1) {
                var tt = parts[1].split(":");
                dt.setHours(parseInt(tt[0]), parseInt(tt[1]), parseInt(tt[2]));
            }
            else
                dt.setHours(0, 0, 0, 0);
            return dt;
        } catch (e) {
            return null;
        }
    },

    //------------------------类型转换扩展结束-------------------------

    //------------------------字符串操作开始-------------------------

    getTab: function (len) {
        ///	<summary>
        ///	获得指定数目Tab字符
        ///	</summary>
        ///	<param name="len" type="Integer">Tab串长度</param>
        ///	<returns type="String">返回Tab串</returns>
        if (len == 0) return "";
        var str = "";
        for (var i = 0; i < len; i++)
            str += "\t";
        return str;
    },

    getSpace: function (len, ch) {
        ///	<summary>
        ///	获得指定数目Tab字符
        ///	</summary>
        ///	<param name="len" type="Integer">空字符串长度</param>
        ///	<param name="ch" type="String">填充字符</param>
        ///	<returns type="String">返回空字符串</returns>
        if (len == 0) return "";
        var str = "";
        if (!ch) ch = " ";
        for (var i = 0; i < len; i++) {
            str += ch;
        }
        return str;
    },

    stringLen: function (valuestr) {
        ///	<summary>
        ///	获取字符串长度（Unicode每个字符两个长度）
        ///	</summary>
        var strInput = valuestr;
        var count = strInput.length;
        var len = 0;
        if (count != 0)
            for (var i = 0; i < count; i++) {
                if (strInput.charCodeAt(i) >= 128)
                    len += 2;
                else
                    len += 1;
            }
        return len;
    },

    isDate: function IsDate(value, fm) {
        ///	<summary>
        ///	判断制定值是否符合制定日期格式
        ///	</summary>
        ///	<param name="value" type="String">给定用于判断的值</param>
        ///	<param name="fm" type="String">日期格式"YYYY-MM-DD","YY-MM-DD",默认"YYYY-MM-DD"</param>
        if (!fm) {
            fm = "YYYY-MM-DD";
        }
        if (fm == "YYYY-MM-DD") {
            var re = /^\d{4}-\d{1,2}-\d{1,2}$/;
            var r = value.match(re);
            if (r == null)
                return false;
            else {
                var s = value.split("-");
                if (s[0].substring(0, 2) < 19 || s[0].substring(0, 2) > 21 || s[1] > 12 || s[1] < 1 || s[2] > 31 || s[2] < 1)
                    return false;
            }
            return true;
        } else if (fm == "YY-MM-DD") {
            var re = /^\d{1,2}-\d{1,2}-\d{1,2}$/;
            var r = value.match(re);
            if (r == null)
                return false;
            else {
                var s = value.split("-");
                if (s[1] > 12 || s[1] < 1 || s[2] > 31 || s[2] < 1)
                    return false;
            }
            return true;
        }

    },

    isDateTime: function (value, minYear, maxYear, hassec) {
        ///	<summary>
        ///	判断制定值是否符合制定日期时间格式
        ///	</summary>
        ///<param name="value" type="String">给定用于判断的值</param>
        ///<param name="minYear" type="Integer">最小年份，默认0</param>
        ///<param name="maxYear" type="Integer">最大年份，默认9999</param>
        ///<param name="hassec" type="Boolean">是否验证秒，默认false</param>
        if (!maxYear) {
            maxYear = 9999;
        }
        if (!minYear) {
            maxYear = 0;
        }
        var reg;
        if (hassec)
            reg = /^(\d+)-(\d{1,2})-(\d{1,2}) (\d{1,2}):(\d{1,2}):(\d{1,2})$/;
        else
            reg = /^(\d+)-(\d{1,2})-(\d{1,2}) (\d{1,2}):(\d{1,2})$/;
        var r = value.match(reg);
        //如果不验证秒,则将秒默认设置为0
        if (r == null) return false;
        if (!r[6])
            r[6] = 0;
        r[2] = r[2] - 1;
        var d = new Date(r[1], r[2], r[3], r[4], r[5], r[6]);
        if (d.getFullYear() != r[1] || r[1] < minYear || r[1] > maxYear) return false;
        if (d.getMonth() != r[2]) return false;
        if (d.getDate() != r[3]) return false;
        if (d.getHours() != r[4]) return false;
        if (d.getMinutes() != r[5]) return false;
        if (d.getSeconds() != r[6]) return false;
        return true;
    },

    isTime: function (value, hassec) {
        ///	<summary>
        ///	判断指定值是否符合时间格式
        ///	</summary>
        ///<param name="value" type="String">给定用于判断的值</param>
        ///<param name="hassec" type="Boolean">是否验证秒，默认false</param>
        if (typeof (hassec) == "undefined")
            var hassec = false;
        if (hassec) //需要判断秒
        {
            var re = /^\d{1,2}:\d{1,2}:\d{1,2}$/;
            var r = value.match(re);
            if (r == null)
                return false;
            else {
                var s = value.split(":");
                if (s[0] < 0 || s[0] > 23 || s[1] < 0 || s[1] > 59 || s[2] < 0 || s[2] > 59)
                    return false;
            }
            return true;
        } else {
            var re = /^\d{1,2}:\d{1,2}$/;
            var r = value.match(re);
            if (r == null)
                return false;
            else {
                var s = value.split(":");
                if (s[0] < 0 || s[0] > 23 || s[1] < 0 || s[1] > 59)
                    return false;
            }
            return true;

        }
    },

    isFloat: function (value) {
        ///	<summary>
        ///	判断指定值是否符合浮点数格式
        ///	</summary>
        var re = /^\d{1,}$|^\d{1,}\.\d{1,}$/;
        var r = value.match(re);
        if (r == null)
            return false;
        else
            return true;
    },

    isInt: function (value) {
        ///	<summary>
        ///	判断指定值是否符合时间格式
        ///	</summary>
        var re = /^\d{0,}$/;
        var r = value.match(re);
        if (r == null)
            return false;
        else
            return true;
    },

    isEmail: function (value) {
        ///	<summary>
        ///	判断指定值是否符合Email地址格式
        ///	</summary>
        var re = /^\w+@\w+\.\w{2,3}/;
        var r = value.match(re);
        if (r == null)
            return false
        else
            return true;

    },

    isPhone: function (value) {
        ///	<summary>
        ///	判断指定值是否符合电话号码格式
        ///	</summary>
        var re = /^(([0-9]+)-)?\d{7,11}$/;
        var r = value.match(re);
        if (r == null)
            return false
        else
            return true;
    },

    isIDCard: function (value) {
        ///	<summary>
        ///	判断是否合法居民身份证号（中国）
        ///	</summary>
        var _re15 = /^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$/; // 15位
        var _re18 = /^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{4}$/; // 18位

        return (IsValueMatch(value, _re15) || IsValueMatch(value, _re18))
    },

    isValueMatch: function (value, re) {
        ///	<summary>
        ///	验证指定值是否符合指定正则表达式
        ///	</summary>
        var r = value.match(re)
        if (r == null)
            return false
        else
            return true;
    },

    getFileSize: function (fname) {
        ///	<summary>
        ///	得到指定文件的大小,以kb为单位
        ///	</summary>
        ///<param name="fname" type="string">指定文件</param>
        try {
            var FSO = new ActiveXObject("Scripting.FileSystemObject");
            var file = FSO.GetFile(fname);
            var fileSize = Math.ceil(file.Size / 1024);
            return fileSize;
        } catch (e) {
            return -1;
        }
    },

    matchExp: function (value, exp) {
        ///	<summary>
        ///	判断指定文本是否符合制定正则表达式
        ///	</summary>
        ///<param name="value" type="String">指定文本</param>
        ///<param name="exp" type="ExpStr">正则表达式</param>
        var expobj = new RegExp(exp, "i");
        var r = value.match(expobj);
        if (r == null)
            return false;
        else
            return true;
    },

    dateOnly: function (date) {
        ///	<summary>
        ///	获取指定字符串或Date类型的时间部分
        ///	</summary>
        ///<param name="date" type="String/Object">指定字符串</param>
        if (!date) return "";

        if (typeof date == 'string')
            return date.split(" ")[0];
        else if (typeof date == 'object')
            return $.getDatePart(date);
    },

    //------------------------字符串操作结束-------------------------

    //------------------------日期类型操作结束-------------------------

    isDateObj: function (obj) {
        if (typeof (obj) != "object") {
            return false;
        } else {
            return obj.constructor == Date;
        }
    },

    /**<doc type="classext" name="Date.getTime">
    <desc>取得当前日期的字符串（hh:mm:ss）</desc>
    <output>返回日期字符串</output>
    </doc>**/
    getTime: function () {
        var dt = new Date();
        return dt.getTimePart();
    },

    /**<doc type="classext" name="Date.getFullDate">
    <desc>取得当前日期的字符串（yyyy-mm-dd hh:mm:ss）</desc>
    <output>返回日期字符串</output>
    </doc>**/
    getFullDate: function (date) {
        date = date || new Date();
        return $.getDatePart(date) + " " + $.getTimePart(date);
    },

    /**<doc type="classext" name="Date.differ">
    <desc>取得两个日期的差值</desc>
    <input>
    <param name="dfrom" type="date">起始日期</param>
    <param name="dto" type="date">结束日期</param>
    <param name="type" type="enum">差值类型</param>
    </input>
    <output type="number">返回差值</output>
    <enum name="type">
    <item text="s">秒</item>
    <item text="n">分</item>
    <item text="h">小时</item>
    <item text="d">天</item>
    <item text="w">周</item>
    </enum>
    </doc>**/
    dateDiff: function (dfrom, dto, type) {
        var dtfrom, dtto;

        if (!type) {
            type = 'd';
        }

        if (typeof (dfrom) == "string" && typeof (dto) == "string") {
            dtfrom = Date.parseDate(dfrom);
            dtto = Date.parseDate(dto);
        }
        else {
            dtfrom = dfrom;
            dtto = dto;
        }

        switch (type) {
            case 's': return Math.floor((dtto - dtfrom) / (1000));
            case 'n': return Math.floor((dtto - dtfrom) / (1000 * 60));
            case 'h': return Math.floor((dtto - dtfrom) / (1000 * 60 * 60));
            case 'd': return Math.floor((dtto - dtfrom) / (1000 * 60 * 60 * 24));
            case 'w': return Math.floor((dtto - dtfrom) / (1000 * 60 * 60 * 24 * 7));
        }
    },

    /**<doc type="classext" name="Date.getTime">
    <desc>取得时间的字符串（ hh:mm:ss）</desc>
    <output>返回时间字符串</output>
    </doc>**/
    getTimePart: function (date) {
        date = date || new Date();
        var hour = date.getHours().toString();
        var minute = date.getMinutes().toString();
        var second = date.getSeconds().toString();
        if (hour.length == 1) hour = "0" + hour;
        if (minute.length == 1) minute = "0" + minute;
        if (second.length == 1) second = "0" + second;
        return (hour + ":" + minute + ":" + second);
    },

    /**<doc type="classext" name="Date.getDate">
    <desc>取得当前日期的字符串（yyyy-mm-dd）</desc>
    <output>返回日期字符串</output>
    </doc>**/
    getDatePart: function (date, divchar) {
        divchar = divchar || "/";

        date = date || new Date();
        var month = (parseInt(date.getMonth()) + 1).toString();
        var day = date.getDate().toString();
        if (month.length == 1) month = "0" + month;
        if (day.length == 1) day = "0" + day;
        return (date.getFullYear() + divchar + month + divchar + day);
    },

    /**<doc type="classext" name="Date.getAdjustedDate">
    <desc>获取调整时间（若存在时分秒则返回长fullDate否则只返回普通时间段）</desc>
    <output>返回日期字符串</output>
    </doc>**/
    getAdjustedDate: function (date) {
        date = date || new Date();
        if (!date.getHours() && !date.getMinutes() && !date.getSeconds()) {
            return $.getDatePart(date);
        } else {
            return $.getFullDate(date);
        }
    },

    /**<doc type="classext" name="Date.DateAdd">
    <desc>将当前时间加上差值返回日期对象</desc>
    <input>
    <param name="strInterval" type="enum">差值类型</param>
    <param name="Number" type="integer">差值</param>
    </input>
    <enum name="strInterval">
    <item text="s">秒</item>
    <item text="m">分</item>
    <item text="h">小时</item>
    <item text="d">天</item>
    </enum>
    </doc>**/
    dateAdd: function (date, strInterval, Number) {
        var dtTmp = date || new Date();
        switch (strInterval) {
            case 's': return new Date(Date.parse(dtTmp) + (1000 * Number));
            case 'n': return new Date(Date.parse(dtTmp) + (60000 * Number));
            case 'h': return new Date(Date.parse(dtTmp) + (3600000 * Number));
            case 'd': return new Date(Date.parse(dtTmp) + (86400000 * Number));
            case 'w': return new Date(Date.parse(dtTmp) + ((86400000 * 7) * Number));
            case 'q': return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + Number * 3, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
            case 'm': return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + Number, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
            case 'y': return new Date((dtTmp.getFullYear() + Number), dtTmp.getMonth(), dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
        }
    },

    /**<doc type="classext" name="Date.getFrom">
    <desc>获得当前日期在指定类型区间的开始日期对象</desc>
    <input>
    <param name="type" type="enum">区间类型</param>
    </input>
    <enum name="type">
    <item text="h">小时</item>
    <item text="d">天</item>
    <item text="w">周</item>
    <item text="m">月</item>
    <item text="y">年</item>
    </enum>
    <output>返回开始日期对象</output>
    </doc>**/
    getDateFrom: function (type, date)//type=day,week,month,year
    {
        date = date || new Date();
        switch (type.toLowerCase()) {
            case "year":
                return new Date(date.getFullYear(), 0, 1);
            case "quarter":
                return new Date(date.getFullYear(), ((Math.ceil((date.getMonth() + 1) / 3) - 1) * 3), 1);
            case "month":
                return new Date(date.getFullYear(), date.getMonth(), 1);
            case "week":
                return new Date(date.getFullYear(), date.getMonth(), date.getDate() - ((date.getDay() + 6) % 7));
            case "preyear":
                return new Date(date.getFullYear() - 1, 0, 1);
            case "prequarter":
                return new Date(date.getFullYear(), ((Math.ceil((date.getMonth() + 1) / 3) - 1) * 3) - 3, 1);
            case "premonth":
                return new Date(date.getFullYear(), date.getMonth() - 1, 1);
            case "preweek":
                return new Date(date.getFullYear(), date.getMonth(), date.getDate() - ((date.getDay() + 6) % 7) - 7);
            case "nextyear":
                return new Date(date.getFullYear() + 1, 0, 1);
            case "nextquarter":
                return new Date(date.getFullYear(), ((Math.ceil((date.getMonth() + 1) / 3) - 1) * 3) + 3, 1);
            case "nextmonth":
                return new Date(date.getFullYear(), date.getMonth() + 1, 1);
            case "nextweek":
                return new Date(date.getFullYear(), date.getMonth(), date.getDate() - ((date.getDay() + 6) % 7) + 7);
            case "uphalfyear":
                return new Date(date.getFullYear(), 0, 1);
            case "downhalfyear":
                return new Date(date.getFullYear(), 6, 1);
            default:
                return date;
        }
    },

    /**<doc type="classext" name="Date.getTo">
    <desc>获得当前日期在指定类型区间的结束日期对象</desc>
    <input>
    <param name="type" type="enum">区间类型</param>
    </input>
    <enum name="type">
    <item text="h">小时</item>
    <item text="d">天</item>
    <item text="w">周</item>
    <item text="m">月</item>
    <item text="y">年</item>
    </enum>
    <output>返回结束日期对象</output>
    </doc>**/
    getDateTo: function (type, date) {
        date = date || new Date();
        switch (type.toLowerCase()) {
            case "year":
                return new Date(date.getFullYear(), 11, 31);
            case "quarter":
                return new Date(date.getFullYear(), ((Math.ceil((date.getMonth() + 1) / 3) - 1) * 3) + 3, 0);
            case "month":
                return new Date(date.getFullYear(), date.getMonth() + 1, 0);
            case "week":
                return new Date(date.getFullYear(), date.getMonth(), date.getDate() + (7 - date.getDay()));
            case "preyear":
                return new Date(date.getFullYear() - 1, 11, 31);
            case "prequarter":
                return new Date(date.getFullYear(), ((Math.ceil((date.getMonth() + 1) / 3) - 1) * 3), 0);
            case "premonth":
                return new Date(date.getFullYear(), date.getMonth(), 0);
            case "preweek":
                return new Date(date.getFullYear(), date.getMonth(), date.getDate() - date.getDay());
            case "nextyear":
                return new Date(date.getFullYear() + 1, 11, 31);
            case "nextquarter":
                return new Date(date.getFullYear(), ((Math.ceil((date.getMonth() + 1) / 3) - 1) * 3) + 6, 0);
            case "nextmonth":
                return new Date(date.getFullYear(), date.getMonth() + 2, 0);
            case "nextweek":
                return new Date(date.getFullYear(), date.getMonth(), date.getDate() + (14 - date.getDay()));
            case "uphalfyear":
                return new Date(date.getFullYear(), 5, 30);
            case "downhalfyear":
                return new Date(date.getFullYear(), 11, 30);
            default:
                return date;
        }
    },

    //------------------------日期类型操作结束-------------------------

    //------------------------DHTML操作开始-------------------------

    getCssRule: function () {
        ///	<summary>
        ///	用 javascript 获取页面上有选择符的 CSS 规则 包括'内部样式块'和'外部样式表文件' 
        ///	</summary>
        var styleSheetLen = document.styleSheets.length; // 样式总数 

        if (!styleSheetLen) return;

        // 样式规则名称，IE 和 FireFox 不同 
        var ruleName = (document.styleSheets[0].cssRules) ? 'cssRules' : 'rules'; //FireFox:cssRules || IE:rules 

        // 初始结果 
        var result = {};
        var totalRuleLen = 0;
        var totalSelectorLen = 0;
        var totalProperty = 0;
        for (var i = 0; i < styleSheetLen; i++) {
            // 获取每个样式表定义 
            var styleSheet = document.styleSheets[i];
            // 每个样式表的规则数 
            var ruleLen = styleSheet[ruleName].length;
            totalRuleLen += ruleLen;
            for (var j = 0; j < ruleLen; j++) {
                // 获取当前规则的选择符 
                // 传入选择符转换为小写 
                var selectors = styleSheet[ruleName][j].selectorText.toLowerCase().split(",");
                var selectorLen = selectors.length;
                totalSelectorLen += selectorLen;
                for (var s = 0; s < selectorLen; s++) {
                    // 去除选择符左右的空格
                    selector = selectors[s].replace(/(^\s*)|(\s*$)/g, "");
                    // 初始化当前选择符
                    result[selector] = {};
                    // 获取当前样式 
                    var styleSet = styleSheet[ruleName][j].style;
                    for (property in styleSet) {
                        // 获取规则 
                        if (styleSet[property] && property != 'cssText') {
                            //alert(selector +'=>'+ property +':'+ styleSet[property]) 
                            result[selector][property] = styleSet[property];
                            totalProperty += 1;
                        }
                    }
                }
            }
        }
        // 统计数据 
        result.totalSheet = styleSheetLen;    //样式块 
        result.totalRule = totalRuleLen;    //规则数 
        result.totalSelector = totalSelectorLen;    //选择符 
        result.totalProperty = totalProperty;    //属性 
        return result;
    },

    // 清理iframe防止内存泄漏
    unloadPage: function () {
        var innerFrames = document.frames;
        if (innerFrames) {
            for (var i = 0; i < innerFrames.length; i++) {
                try {
                    if (innerFrames[i].jQuery && innerFrames[i].jQuery.unloadPage) {
                        innerFrames[i].jQuery.unloadPage();
                    }

                    innerFrames[i].document.write("");
                    innerFrames[i].document.clear();
                } catch (e) { }
            }

            try {
                CollectGarbage();
            } catch (e) { alert(e); }
        }
    },

    setDisabled: function (id, st) {
        ///	<summary>
        ///	将HTML元素设置为不可控/可控状态
        ///	</summary>
        ///	<param name="id" type="String">元素的ID</param>
        ///<param name="st" type="Boolean">状态</param>
        document.getElementById(id).disabled = st;
        $("#" + id).attr("disabled", st);
    },

    setReadonly: function (id, st) {
        ///	<summary>
        ///	将HTML元素设置为不可编辑/可编辑状态
        ///	</summary>
        ///	<param name="id" type="String">元素的ID</param>
        ///<param name="st" type="Boolean">状态</param>
        document.getElementById(id).disabled = st;
        $("#" + id).attr("readOnly", st);
    },

    ensureExec: function (func, interval, times) {
        ///	<summary>
        ///	确保执行某方法(当func返回true时停止执行，返回false则继续尝试)
        ///	</summary>
        ///	<param name="func" type="Function">执行方法</param>
        ///	<param name="interval" type="Integer">检查间隔</param>
        ///	<param name="times" type="Integer">尝试次数</param>
        interval = interval || 100;
        times = times || 10;

        var execed = false; // 是否已执行
        var triedtimes = 0; // 已尝试次数

        if (typeof (func) == "function") {
            var intervalID = setInterval(function () {
                try {
                    if (execed || triedtimes > times) {
                        clearInterval(intervalID);
                    }

                    if (func.call(this)) {
                        execed = true;
                        clearInterval(intervalID);
                    }
                } catch (ex) { }

                triedtimes++;
            }, interval);
        }
    },

    execOnWinReady: function (win, func, interval) {
        ///	<summary>
        ///	当目标窗口执行完毕时触发
        ///	</summary>
        ///	<param name="win" type="Object">目标窗口</param>
        ///	<param name="func" type="Function">执行方法</param>
        ///	<param name="interval" type="Integer">检查间隔</param>
        if (typeof (win) == "object" && typeof (func) == "function") {
            interval = interval || 100;

            var intervalID = setInterval(function () {
                try {
                    if (win.document.readyState == "complete") {
                        clearInterval(intervalID);

                        func.call(win);
                    }
                } catch (ex) { }
            }, 100);
        }
    },

    includeScript: function (file, type) {
        ///	<summary>
        ///	加载Script文件
        ///	</summary>
        ///	<param name="file"  type="String">文件地址</param>
        ///	<param name="type"  type="String">文件类型，默认(text/javascript)</param>
        var head = document.getElementsByTagName('head');
        var script = document.createElement('script');
        script.src = file;
        script.type = type || 'text/javascript';

        if (head) {
            head[0].appendChild(script);
        }
    },

    addSheetFile: function (path) {
        var fileref = document.createElement("link");
        fileref.rel = "stylesheet";
        fileref.type = "text/css";
        fileref.href = path;
        fileref.media = "screen";

        var headobj = document.getElementsByTagName('head')[0];
        headobj.appendChild(fileref);
    },

    createClass: function () {
        var doc, cssCode;
        if (arguments.length == 1) {
            doc = document;
            cssCode = arguments[0]
        } else if (arguments.length == 2) {
            doc = arguments[0];
            cssCode = arguments[1];
        } else {
            alert("createClass函数最多接受两个参数!");
        }

        if (! +"\v1") {//增加自动转换透明度功能，用户只需输入W3C的透明样式，它会自动转换成IE的透明滤镜 
            var t = cssCode.match(/opacity:(\d?\.\d+);/);
            if (t != null) {
                cssCode = cssCode.replace(t[0], "filter:alpha(opacity=" + parseFloat(t[1]) * 100 + ")")
            }
        }
        cssCode = cssCode + "\n"; //增加末尾的换行符，方便在firebug下的查看。 
        var headElement = doc.getElementsByTagName("head")[0];
        var styleElements = headElement.getElementsByTagName("style");
        if (styleElements.length == 0) {//如果不存在style元素则创建 
            if (doc.createStyleSheet) {    //ie 
                doc.createStyleSheet();
            } else {
                var tempStyleElement = doc.createElement('style'); //w3c 
                tempStyleElement.setAttribute("type", "text/css");
                headElement.appendChild(tempStyleElement);
            }
        }
        var styleElement = styleElements[0];
        var media = styleElement.getAttribute("media");
        if (media != null && !/screen/.test(media.toLowerCase())) {
            styleElement.setAttribute("media", "screen");
        }

        if (styleElement.styleSheet) {    //ie 
            styleElement.styleSheet.cssText += cssCode;
        } else if (doc.getBoxObjectFor) {
            styleElement.innerHTML += cssCode; //火狐支持直接innerHTML添加样式表字串 
        } else {
            styleElement.appendChild(doc.createTextNode(cssCode))
        }
    }

    //------------------------DHTML操作结束-------------------------
});

//
//---------------------------------全局函数定义结束--------------------------------------------------------------
//

//
//---------------------------------系统对象增强开始--------------------------------------------------------------
//

window.console = window.console || {
    log: function () { },
    warn: function () { },
    error: function () { }
};

/**<doc type="classext" name="Array.Find">
<desc>查找数组中指定值位置</desc>
<output>返回数组中指定值位置，没有则返回-1</output>
</doc>**/
Array.prototype.Find = function(value) {
    for (var i = 0; i < this.length; i++)
        if (this[i] == value) return i;
    return -1;
}

/**<doc type="classext" name="Array.Remove">
<desc>删除数组中指定值</desc>
<output>删除成功返回true，不存在此值没有则返回false</output>
</doc>**/
Array.prototype.Remove = function(value) {
    var exists = false;
    for (var i = 0, n = 0; i < this.length; i++) {
        if (this[i] != value) {
            this[n++] = this[i];
        } else {
            exists = true;
        }
    }
    if (exists) {
        this.length -= 1
        return true;
    }

    return false;
}

/**<doc type="classext" name="String.fromNull">
<desc>字符串过滤</desc>
<output>如果为null或null串，返回空，否则返回原字符串</output>
</doc>**/
String.prototype.filterNull = function() {
    if (this.toLowerCase() == "null") return "";
    return "" + this;
}

/**<doc type="protofunc" name="String.trim">
<desc>字符串对象定义修剪功能扩展</desc>
<output>返回去除前后空格的字符串</output>
</doc>**/
String.prototype.trim = function() {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}

/**<doc type="protofunc" name="String.trimStart">
<desc>字符串对象定义修剪功能扩展</desc>
<output>返回去除前指定字符串</output>
</doc>**/
String.prototype.trimStart = function(str) {
    if (this.indexOf(str) == 0) {
        return this.substr(str.length, this.length);
    }
    
    return "" + this;
}

/**<doc type="protofunc" name="String.trimEnd">
<desc>字符串对象定义修剪功能扩展</desc>
<output>返回去除后指定字符串</output>
</doc>**/
String.prototype.trimEnd = function(str) {
    if ((this.lastIndexOf(str) + str.length) == this.length) {
        return this.substring(0, this.lastIndexOf(str));
    }
    return "" + this;
}

/**<doc type="protofunc" name="String.bytelen">
<desc>返回字节长度，中文字符长度为2，英文长度为1</desc>
<output>返回字节长度</output>
</doc>**/
String.prototype.bytelen = function() {
    var count = this.length;
    var len = 0;
    if (count != 0)
        for (var i = 0; i < count; i++) {
        if (this.charCodeAt(i) >= 128)
            len += 2;
        else
            len += 1;
    }
    return len;
}

/**<doc type="protofunc" name="String.bytelen">
<desc>替换指定字符串</desc>
<output>返回替换后的字符串</output>
</doc>**/
String.prototype.replaceAll = function(search, replace) {
    var tmp = str = this;
    do {
        str = tmp;
        tmp = str.replace(search, replace);
    } while (str != tmp);
    return str;
}

/**<doc type="protofunc" name="String.bytelen">
<desc>判断String是否以指定字符串开始</desc>
<output>Boolean</output>
</doc>**/
String.prototype.startWith = function(str) {
    return this.indexOf(str) == 0;
}

/**<doc type="protofunc" name="String.bytelen">
<desc>判断String是否以指定字符串结尾</desc>
<output>Boolean</output>
</doc>**/
String.prototype.endWith = function(str){
    var reg=new RegExp(str+"$");
    return reg.test(this);
}

/**<doc type="protofunc" name="String.bytelen">
<desc>判断String是否相等</desc>
<output>Boolean</output>
</doc>**/
String.prototype.equals = function(str, ignoreCase) {
    ignoreCase = !(ignoreCase == false);    // 默认为true
    if (this == str) {
        return true;
    } else {
        if (ignoreCase && str) {
            return this.toLowerCase() == str.toString().toLowerCase();
        }
    }

    return false;
}

/**<doc type="protofunc" name="String.bytelen">
<desc>判断String是否包含指定字符</desc>
<output>Boolean</output>
</doc>**/
String.prototype.contains = function(str, ignoreCase) {
    if (!str) {
        return false;
    } else {
        if (ignoreCase) {
            return this.toLowerCase().indexOf(str.toLowerCase()) >= 0;
        }

        return this.indexOf(str) >= 0;
    }
}

/**<doc type="objdefine" name="Currency">
<desc>Currency定义, 货币类型</desc>	
</doc>**/
function Currency() { }

/**<doc type="protofunc" name="Currency.Format">
<desc>将数值四舍五入(保留2位小数)后格式化成金额形式</desc>
<output>金额格式的字符串,如'1,234,567.45'</output>
</doc>**/
Currency.Format = function(num) {
    var num = num.toString().replace(/\$|\,/g, '');
    if (isNaN(num)) num = "0";

    sign = (num == (num = Math.abs(num)));
    num = Math.floor(num * 100 + 0.50000000001);
    cents = num % 100;
    num = Math.floor(num / 100).toString();

    if (cents < 10)
        cents = "0" + cents;

    for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
        num = num.substring(0, num.length - (4 * i + 3)) + ',' +
        num.substring(num.length - (4 * i + 3));

    return (((sign) ? '' : '-') + num + '.' + cents);
}

//
//---------------------------------系统对象增强结束--------------------------------------------------------------
//
