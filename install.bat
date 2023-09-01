sc.exe create "SenitronPrintHandlerService" binpath="%~dp0SenitronPrintHandlerService.exe"
sc.exe start "SenitronPrintHandlerService"
pause