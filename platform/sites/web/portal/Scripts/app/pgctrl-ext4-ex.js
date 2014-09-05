
// extjs 第三方支持

//------------------------PIS ExtJs Excel支持 开始------------------------//

var EXT_EXCEL_STYLE_XML = '<ss:Styles>' +
    '<ss:Style ss:ID="Default">' +
        '<ss:Alignment ss:Vertical="Top" ss:WrapText="1" />' +
        '<ss:Font ss:FontName="arial" ss:Size="10" />' +
        '<ss:Borders>' +
            '<ss:Border ss:Color="#e4e4e4" ss:Weight="1" ss:LineStyle="Continuous" ss:Position="Top" />' +
            '<ss:Border ss:Color="#e4e4e4" ss:Weight="1" ss:LineStyle="Continuous" ss:Position="Bottom" />' +
            '<ss:Border ss:Color="#e4e4e4" ss:Weight="1" ss:LineStyle="Continuous" ss:Position="Left" />' +
            '<ss:Border ss:Color="#e4e4e4" ss:Weight="1" ss:LineStyle="Continuous" ss:Position="Right" />' +
        '</ss:Borders>' +
        '<ss:Interior />' +
        '<ss:NumberFormat />' +
        '<ss:Protection />' +
    '</ss:Style>' +
    '<ss:Style ss:ID="title">' +
        '<ss:Borders />' +
        '<ss:Font />' +
        '<ss:Alignment ss:WrapText="1" ss:Vertical="Center" ss:Horizontal="Center" />' +
        '<ss:NumberFormat ss:Format="@" />' +
    '</ss:Style>' +
    '<ss:Style ss:ID="headercell">' +
        '<ss:Font ss:Bold="1" ss:Size="10" />' +
        '<ss:Alignment ss:WrapText="1" ss:Horizontal="Center" />' +
        '<ss:Interior ss:Pattern="Solid" ss:Color="#A3C9F1" />' +
    '</ss:Style>' +
    '<ss:Style ss:ID="even">' +
        '<ss:Interior ss:Pattern="Solid" ss:Color="#CCFFFF" />' +
    '</ss:Style>' +
    '<ss:Style ss:Parent="even" ss:ID="evencenter">' +
        '<ss:Alignment ss:Horizontal="Center" ss:Vertical="Top" ss:WrapText="1"/>' +
    '</ss:Style>' +
    '<ss:Style ss:Parent="even" ss:ID="evendate">' +
//                    '<ss:NumberFormat ss:Format="[ENG][$-409]dd\-mmm\-yyyy;@" />' +
        '<ss:NumberFormat ss:Format="yyyy\-m\-d;@" />' +
    '</ss:Style>' +
    '<ss:Style ss:Parent="even" ss:ID="evenint">' +
        '<ss:NumberFormat ss:Format="0" />' +
    '</ss:Style>' +
    '<ss:Style ss:Parent="even" ss:ID="evenfloat">' +
        '<ss:NumberFormat ss:Format="0.00" />' +
    '</ss:Style>' +
    '<ss:Style ss:ID="odd">' +
        '<ss:Interior ss:Pattern="Solid" ss:Color="#CCCCFF" />' +
    '</ss:Style>' +
    '<ss:Style ss:Parent="odd" ss:ID="oddcenter">' +
        '<ss:Alignment ss:Horizontal="Center" ss:Vertical="Top" ss:WrapText="1"/>' +
    '</ss:Style>' +
    '<ss:Style ss:Parent="odd" ss:ID="odddate">' +
        '<ss:NumberFormat ss:Format="[ENG][$-409]dd\-mmm\-yyyy;@" />' +
    '</ss:Style>' +
    '<ss:Style ss:Parent="odd" ss:ID="oddint">' +
        '<ss:NumberFormat ss:Format="0" />' +
    '</ss:Style>' +
    '<ss:Style ss:Parent="odd" ss:ID="oddfloat">' +
        '<ss:NumberFormat ss:Format="0.00" />' +
    '</ss:Style>' +
'</ss:Styles>';

Ext.define('PIS.ExcelMixin', {
    alias: 'widget.pis-excelmixin',
    DefaultColumnWidth: 100,

    exportExcel: function (config) {
        var me = this;

        var tmpExportContent = me.getExcelXml(config);

        if (Ext.isIE || Ext.isSafari || Ext.isSafari2 || Ext.isSafari3) {   //在这几种浏览器中才需要，IE8测试不能直接下载了
            var encodedContent = $.htmlEncode(tmpExportContent);
            var fileName = (config.maintitle || config.title || "export") + "_" + new Date().toLocaleString() + ".xls";

            var url = (config.url || EXPORT_PAGE_URL);
            var params = { ExportContent: encodedContent, ExportFile: fileName, FileType: "Excel" };

            ExtDummyPost({
                params: params,
                isUpload: true,
                url: url,
                callback: config.callback || Ext.emptyFn
            });
        } else {
            document.location = 'data:application/vnd.ms-excel;base64,' + Base64.encode(tmpExportContent);
        }
    },

    getExcelXml: function (config) {
        var me = this;
        var worksheet = me.createWorksheet(config.includeHidden || false, config);
        var innertitle = '';

        if (config && config.title) {
            innertitle = config.title;
        } else {
            innertitle = me.title;
        }

        return '<xml version="1.0" encoding="utf-8">' +
            '<ss:Workbook xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns:o="urn:schemas-microsoft-com:office:office">' +
            '<o:DocumentProperties><o:Title>' + innertitle + '</o:Title></o:DocumentProperties>' +
            '<ss:ExcelWorkbook>' +
                '<ss:WindowHeight>' + worksheet.height + '</ss:WindowHeight>' +
                '<ss:WindowWidth>' + worksheet.width + '</ss:WindowWidth>' +
                '<ss:ProtectStructure>False</ss:ProtectStructure>' +
                '<ss:ProtectWindows>False</ss:ProtectWindows>' +
            '</ss:ExcelWorkbook>' +
            me.getExcelStyleXml() + worksheet.xml +
            '</ss:Workbook>';
    },

    getExcelStyleXml: function () {
        return EXT_EXCEL_STYLE_XML;
    },

    createWorksheet: function (includeHidden, config) {
        var me = this;
        var columns = config.columns || me.columns;

        // var postfix = me.id.replace(/-/g, '_');
        var postfix = config.postfix || "_";
        var WorksheetName = "";
        var PrintTitlesName = "PrintTitles_" + postfix;

        // Calculate cell data types and extra class names which affect formatting
        var cellType = [];
        var cellTypeClass = [];
        var totalWidthInPixels = 0;
        var colXml = '';
        var headerXml = '';
        var visibleColumnCountReduction = 0;
        var innertitle = '';
        var innerstore = null;

        if (config && config.title) {
            innertitle = config.title;
        } else {
            innertitle = me.title;
        }

        if (!innertitle || innertitle == '') {
            innertitle = '标题';
        }

        WorksheetName = innertitle;

        if (config && config.store) {
            innerstore = config.store;
        } else {
            innerstore = me.store;
        }

        Ext.each(columns, function (col, i, cols) {
            if (includeHidden || !col.hidden) {
                var w = col.width || DefaultColumnWidth;
                totalWidthInPixels += w;
                var cid = col.id;
                var cheader = col.text;

                if ((cheader === "") || (cid === "checker") || ((cid === "actions") && (cheader === "Actions"))) {
                    cellType.push("None");
                    cellTypeClass.push("");
                    ++visibleColumnCountReduction;
                } else {
                    var fld = innerstore.getField(col.dataIndex);
                    if (!fld) return true;

                    colXml += '<ss:Column ss:AutoFitWidth="1" ss:Width="' + w + '" />';
                    headerXml += '<ss:Cell ss:StyleID="headercell">' +
                        '<ss:Data ss:Type="String">' + cheader + '</ss:Data>' +
                        '<ss:NamedCell ss:Name="' + PrintTitlesName + '" /></ss:Cell>';

                    switch (fld.type) {
                        case "int":
                            cellType.push("Number");
                            cellTypeClass.push("int");
                            break;
                        case "float":
                            cellType.push("Number");
                            cellTypeClass.push("float");
                            break;
                        case "bool":
                        case "boolean":
                            cellType.push("String");
                            cellTypeClass.push("");
                            break;
                        case "date":
                            cellType.push("DateTime");
                            cellTypeClass.push("date");
                            break;
                        default:
                            cellType.push("String");
                            if (col.align == 'center') {
                                cellTypeClass.push("center");
                            } else {
                                cellTypeClass.push("");
                            }
                            break;
                    }
                }
            }
        });

        var visibleColumnCount = cellType.length - visibleColumnCountReduction;

        var result = { height: 9000, width: Math.floor(totalWidthInPixels * 30) + 50 };

        // Generate worksheet header details.
        var t = '<ss:Worksheet ss:Name="' + WorksheetName + '">' +
            '<ss:Names>' +
                '<ss:NamedRange ss:Name="' + PrintTitlesName + '" ss:RefersTo="=\'' + WorksheetName + '\'!R1:R2" />' +
            '</ss:Names>' +
            '<ss:Table x:FullRows="1" x:FullColumns="1"' +
                ' ss:ExpandedColumnCount="' + (visibleColumnCount) +
                '" ss:ExpandedRowCount="' + (innerstore.getCount() + 2) + '">' +
                colXml +
                '<ss:Row ss:Height="38">' +
                    '<ss:Cell ss:StyleID="title" ss:MergeAcross="' + (visibleColumnCount - 1) + '">' +
                      '<ss:Data xmlns:html="http://www.w3.org/TR/REC-html40" ss:Type="String">' +
                        '<html:B>' + innertitle + '</html:B></ss:Data><ss:NamedCell ss:Name="' + PrintTitlesName + '" />' +
                    '</ss:Cell>' +
                '</ss:Row>' +
                '<ss:Row ss:AutoFitHeight="1">' +
                headerXml +
                '</ss:Row>';

        // by ray - 2012/5/1 - selection export - start
        var expitems;

        if (config.isselection === true && me.getSelectionModel && me.getSelectionModel().getSelection) {
            expitems = me.getSelectionModel().getSelection()
        } else {
            expitems = innerstore.data.items;
        }
        // by ray - 2012/5/1 - selection export - end

        // Generate the data rows from the data in the Store
        for (var i = 0, it = expitems, l = it.length; i < l; i++) {
            t += '<ss:Row>';
            var cellClass = (i & 1) ? 'odd' : 'even';
            r = it[i].data;

            var k = 0;

            Ext.each(columns, function (col, j, cols) {
                if (includeHidden || !col.hidden) {
                    if (!col.dataIndex) return true;

                    var v = r[col.dataIndex];

                    // by ray - 2012/6/24 - exptrenderer
                    if (typeof col.exptrenderer == 'function') {
                        v = col.exptrenderer(v, {}, it[i], i, j, innerstore);
                    } else if (typeof col.renderer == 'function' && cellType[k] != 'DateTime') {
                        var m = {};
                        v = col.renderer(v, m, it[i], i, j, innerstore);
                        var re = /<[^>]+>/g;
                        if (v) {
                            v = v.toString().replace(re, '');
                        } else {
                            v = '';
                        }
                    }

                    if (cellType[k] !== "None") {
                        if (!v) {
                            t += '<ss:Cell ss:StyleID="' + cellClass + '"></ss:Cell>';
                        } else {
                            t += '<ss:Cell ss:StyleID="' + cellClass + cellTypeClass[k] + '"><ss:Data ss:Type="' + cellType[k] + '">';
                            if (cellType[k] == 'DateTime') {
                                t += v.format('Y-m-d\\TH:i:s.000'); // no space betwen  i: s
                            } else {
                                v = EncodeValue(v);

                                t += v;
                            }
                            t += '</ss:Data></ss:Cell>';
                        }
                    }

                    k++;
                }
            });

            t += '</ss:Row>';
        }

        result.xml = t + '</ss:Table>' +
            '<x:WorksheetOptions>' +
                '<x:PageSetup>' +
                    '<x:Layout x:CenterHorizontal="1" x:Orientation="Landscape" />' +
                    '<x:Footer x:Data="Page &amp;P of &amp;N" x:Margin="0.5" />' +
                    '<x:PageMargins x:Top="0.5" x:Right="0.5" x:Left="0.5" x:Bottom="0.8" />' +
                '</x:PageSetup>' +
                '<x:FitToPage />' +
                '<x:Print>' +
                    '<x:PrintErrors>Blank</x:PrintErrors>' +
                    '<x:FitWidth>1</x:FitWidth>' +
                    '<x:FitHeight>32767</x:FitHeight>' +
                    '<x:ValidPrinterInfo />' +
                    '<x:VerticalResolution>600</x:VerticalResolution>' +
                '</x:Print>' +
                '<x:Selected />' +
                '<x:DoNotDisplayGridlines />' +
                '<x:ProtectObjects>False</x:ProtectObjects>' +
                '<x:ProtectScenarios>False</x:ProtectScenarios>' +
            '</x:WorksheetOptions>' +
        '</ss:Worksheet>';

        //Add function to encode value,2009-4-21
        function EncodeValue(v) {
            var re = /[\r|\n]/g; //Handler enter key
            v = v.toString().replace(re, '&#10;');

            return v;
        };

        return result;
    }
});

//------------------------PIS ExtJs Excel支持 结束------------------------//