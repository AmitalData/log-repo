net stop AmitalCustomsServerCustoms
cd C:\Program Files (x86)\WinRAR
unrar x C:\inetpub\wwwroot\IIGFTP\AmitalCustomsWindowsService.rar *.* o C:\Amital\AmitalCustomsWindowsServiceCustoms\ -y
XCOPY  "C:\\inetpub\\wwwroot\\IIGFTP\\ConnectionStrings\\563" "C:\\inetpub\\wwwroot\\Customs56"  /Y /E
XCOPY  "C:\\inetpub\\wwwroot\\IIGFTP\\ConnectionStrings\\563" "C:\\Amital\\AmitalCustomsWindowsServiceCustoms"  /Y /E
net start AmitalCustomsServerCustoms



net stop AmitalCustomsWindowsServiceCustomsCustoms573
cd C:\Program Files (x86)\WinRAR
unrar x C:\inetpub\wwwroot\IIGFTP\AmitalCustomsWindowsService.rar *.* o C:\Amital\AmitalCustomsWindowsServiceCustomsCustoms573\ -y
XCOPY  "C:\\inetpub\\wwwroot\\IIGFTP\\ConnectionStrings\\573" "C:\\inetpub573\\wwwroot\\Customs573"  /Y /E
XCOPY  "C:\\inetpub\\wwwroot\\IIGFTP\\ConnectionStrings\\573" "C:\\Amital\\AmitalCustomsWindowsServiceCustomsCustoms573"  /Y /E
net start AmitalCustomsWindowsServiceCustomsCustoms573


net stop AmitalCustomsWindowsServiceCustomsCustoms581
cd C:\Program Files (x86)\WinRAR
unrar x C:\inetpub\wwwroot\IIGFTP\AmitalCustomsWindowsService.rar *.* o C:\Amital\AmitalCustomsWindowsServiceCustomsCustoms581\ -y
XCOPY  "C:\\inetpub\\wwwroot\\IIGFTP\\ConnectionStrings\\581" "C:\\inetpub581\\wwwroot\\Customs581"  /Y /E
XCOPY  "C:\\inetpub\\wwwroot\\IIGFTP\\ConnectionStrings\\581" "C:\\Amital\\AmitalCustomsWindowsServiceCustomsCustoms581"  /Y /E
net start AmitalCustomsWindowsServiceCustomsCustoms581

net stop AmitalCustomsWindowsServiceCustomsCourier58
cd C:\Program Files (x86)\WinRAR
unrar x C:\inetpub\wwwroot\IIGFTP\AmitalCustomsWindowsService.rar *.* o C:\Amital\AmitalCustomsWindowsServiceCustomsCourier58\ -y
XCOPY  "C:\\inetpub\\wwwroot\\IIGFTP\\ConnectionStrings\\584" "C:\\InetPub584Courier\\wwwroot\\Courier58"  /Y /E
XCOPY  "C:\\inetpub\\wwwroot\\IIGFTP\\ConnectionStrings\\584" "C:\\Amital\\AmitalCustomsWindowsServiceCustomsCourier58"  /Y /E
net start AmitalCustomsWindowsServiceCustomsCourier58