ECHO OFF
chcp 65001
REM recreate model
psql -c "\i create_tables.sql" "user=postgres dbname=pubciter password=postgres"

