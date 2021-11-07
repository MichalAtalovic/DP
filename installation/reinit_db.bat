ECHO OFF
chcp 65001
REM Disconnect all users
psql -c "REVOKE CONNECT ON DATABASE pubciter FROM public" "user=postgres dbname=postgres password=postgres"
psql -c "SELECT pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname = 'pubciter'" "user=postgres dbname=postgres password=postgres"
REM drop database
psql -c "DROP DATABASE IF EXISTS pubciter" "user=postgres dbname=postgres password=postgres"
REM recreate database
psql -c "CREATE DATABASE pubciter WITH ENCODING 'UTF8'" "user=postgres dbname=postgres password=postgres"
REM create model
psql -c "\i ./../webapi/PubCiterAPI/scripts/create_tables.sql" "user=postgres dbname=pubciter password=postgres"

