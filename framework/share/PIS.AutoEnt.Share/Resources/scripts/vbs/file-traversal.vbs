'---------------------------------------------------------------------------------------------------
'   ��������: �����ļ�
'   ������Ա��Ray Liu
'   ��������: 2012-10-4 14:17
'   ���õĹ����뺯��:                                            
'   �޸���Ա:
'   �޸�����:
'   �޸�ԭ��:
'   �޸Ľ��:
'   �汾˵��: 1.0
'----------------------------------------------------------------------------------------------------

'----------------------------------------------------------------------------------------------------
'   �������弰�����ʼ������
'----------------------------------------------------------------------------------------------------
'�������
Public currPath '�����ļ�·��'
Dim fso

'----------------------------------------------------------------------------------------------------
'   �������߼�����
'----------------------------------------------------------------------------------------------------

Run()   '����������

'�������߼�
Public Sub Run()
    currPath = InputBox("�����뵱ǰ�����ļ�·����Ĭ�ϵ�ǰ·����")
    
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
    
    MsgBox "������ϣ�лл��",vbOKOnly,"��ʾ��"
End Sub

Function GetFileName(fname)
	Dim rtn
	rtn = left(fname,instrrev(fname,".")-1)
	
	GetFileName = rtn
End Function

