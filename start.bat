rd webapi\PubCiterAPI\bin\Debug\netcoreapp3.1\dump /s /q
start cmd /k "cd webapi/PubCiterAPI/bin/Debug/netcoreapp3.1 && echo Starting API . . . && PubCiterAPI.exe"
start cmd /k "cd client/pubciter && echo Deploying . . . && ng serve --host 0.0.0.0 --disableHostCheck"