'---------------------------------------------------------------------------------------------------
'   功能描述: 遍历文件
'   创建人员：Ray Liu
'   创建日期: 2012-10-4 14:17
'   调用的过程与函数:                                            
'   修改人员:
'   修改日期:
'   修改原因:
'   修改结果:
'   版本说明: 1.0
'----------------------------------------------------------------------------------------------------

'----------------------------------------------------------------------------------------------------
'   变量定义及程序初始化部分
'----------------------------------------------------------------------------------------------------
'定义变量
Public currPath '更新文件路径'
Dim fso

'----------------------------------------------------------------------------------------------------
'   主程序逻辑部分
'----------------------------------------------------------------------------------------------------

Run()   '运行主程序

'主运行逻辑
Public Sub Run()
    currPath = InputBox("请输入当前更新文件路径。默认当前路径。")
    
    If IsEmpty(currPath) Or RTrim(LTrim(currPath))="" Then
        currPath = Left(WScript.ScriptFullname, InstrRev(WScript.ScriptFullname,"\")-1) 
    End If
	
	Set fso = CreateObject("Scripting.FileSystemObject")
	Set folder = fso.GetFolder(currPath)
	Set logfile = fso.OpenTextFile("log.txt", 8, True)	' ForReading = 1, ForWriting = 2, ForAppending = 8
	
	Dim tfname
	For Each f In folder.files
		tfname = GetFileName(f.Name)
		logfile.Write ".pis-icon-" & tfname
		logfile.Write " { background-image: url(/portal/images/common/icons/" & f.Name & ") !important; background-repeat: no-repeat; }"
		logfile.Write Chr(13) & Chr(10) 
	Next
	
	Set logfile = Nothing
	Set fso = Nothing
    
    MsgBox "遍历完毕，谢谢！",vbOKOnly,"提示："
End Sub

Function GetFileName(fname)
	Dim rtn
	rtn = left(fname,instrrev(fname,".")-1)
	
	GetFileName = rtn
End Function

