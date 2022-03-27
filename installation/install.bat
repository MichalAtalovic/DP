REM remove previously dumped data
rd ./../webapi/PubCiterAPI/bin/Debug/netcoreapp3.1\dump /s /q
REM install missing python libs & node modules
start cmd /k "reinit_db.bat && cd ./../client/pubciter && echo Installing missing node modules . . . && npm install -f -g @angular/cli && npm install && echo Installing missing python libs . . . && cd ./../../installation && install_python_packages.bat"
exit