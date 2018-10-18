"%SystemDrive%\Windows\System32\inetsrv\appcmd.exe" set config -section:urlCompression /doDynamicCompression:true /commit:apphost
"%SystemDrive%\Windows\System32\inetsrv\appcmd.exe" set config -section:system.webServer/httpCompression -minFileSizeForComp:200 /commit:apphost
"%SystemDrive%\Windows\System32\inetsrv\appcmd.exe" set config /section:httpCompression /+dynamicTypes.[mimeType='application/msbin1',enabled='true'] /commit:apphost
"%SystemDrive%\Windows\System32\inetsrv\appcmd.exe" set config -section:system.webServer/httpCompression -dynamicCompressionDisableCpuUsage:100 /commit:apphost
"%SystemDrive%\Windows\System32\inetsrv\appcmd.exe" set config -section:system.webServer/httpCompression -dynamicCompressionEnableCpuUsage:100 /commit:apphost
"%SystemDrive%\Windows\System32\inetsrv\appcmd.exe" set config -section:httpCompression -[name='gzip'].dynamicCompressionLevel:7
"%SystemDrive%\Windows\System32\inetsrv\appcmd.exe" set config -section:system.webServer/httpCompression /+"dynamicTypes.[mimeType='application/json; charset=utf-8',enabled='True']" /commit:apphost
exit /b 0

e