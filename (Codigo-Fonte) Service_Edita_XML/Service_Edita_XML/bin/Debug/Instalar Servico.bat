sc create "Editor_XML" binPath= "C:\Service_Edita_XML\Service_Edita_XML.exe" start= "auto" DisplayName= "Editor_XML"

pause
timeout 3

NET START "Editor_XML"

echo >> XML_Config.txt
