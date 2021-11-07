REM remove previously dumped data
rd webapi\PubCiterAPI\bin\Debug\netcoreapp3.1\dump /s /q
REM start backend
start cmd /k "cd webapi/PubCiterAPI/bin/Debug/netcoreapp3.1 && echo Starting API . . . && PubCiterAPI.exe"
REM serve client application
start cmd /k "cd client/pubciter && ng serve --host 0.0.0.0 --disableHostCheck"