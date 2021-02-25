REM remove previously dumped data
rd webapi\PubCiterAPI\bin\Debug\netcoreapp3.1\dump /s /q
REM post changes
psql -c "\i webapi/PubCiterAPI/scripts/post_alter_tables.sql" "user=postgres dbname=pubciter password=postgres"
REM start backend
start cmd /k "cd webapi/PubCiterAPI/bin/Debug/netcoreapp3.1 && echo Starting API . . . && PubCiterAPI.exe"
REM serve client application (install missing python libs & node modules if needed)
start cmd /k "cd client/pubciter && echo Installing missing python libs . . . && install_python_packages.bat && echo Installing missing node modules . . . && npm install && echo Deploying . . . && ng serve --host 0.0.0.0 --disableHostCheck"