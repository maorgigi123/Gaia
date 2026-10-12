#!/bin/bash
set -euo pipefail
rm -f /opt/oracle/oradata/.gaia-user-ready
# Restrict characters so the password is safe inside the quoted SQL identifier.
if [[ ! "${ORACLE_APP_PASSWORD:-}" =~ ^[A-Za-z][A-Za-z0-9_]{11,63}$ ]]; then
  echo "ORACLE_APP_PASSWORD must be 12-64 letters, digits or underscores, starting with a letter." >&2
  exit 1
fi
sqlplus -s / as sysdba <<SQL
WHENEVER SQLERROR EXIT SQL.SQLCODE
SET ECHO OFF VERIFY OFF
ALTER SESSION SET CONTAINER = FREEPDB1;
DECLARE
  object_count NUMBER;
BEGIN
  SELECT COUNT(*) INTO object_count FROM DBA_TABLESPACES WHERE TABLESPACE_NAME = 'GAIA_DATA';
  IF object_count = 0 THEN
    EXECUTE IMMEDIATE 'CREATE TABLESPACE GAIA_DATA DATAFILE ''/opt/oracle/oradata/gaia.dbf'' SIZE 10M AUTOEXTEND ON NEXT 10M MAXSIZE 100M';
  END IF;
  SELECT COUNT(*) INTO object_count FROM DBA_USERS WHERE USERNAME = 'GAIA';
  IF object_count = 0 THEN
    EXECUTE IMMEDIATE 'CREATE USER GAIA IDENTIFIED BY "$ORACLE_APP_PASSWORD" DEFAULT TABLESPACE GAIA_DATA QUOTA 100M ON GAIA_DATA';
  END IF;
END;
/
GRANT CREATE SESSION, CREATE TABLE, CREATE SEQUENCE TO GAIA;
EXIT;
SQL
touch /opt/oracle/oradata/.gaia-user-ready
