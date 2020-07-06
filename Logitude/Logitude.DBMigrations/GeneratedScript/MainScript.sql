-- General Script From 2020070560750_UQ_CARDS_TENANT_CODE_PAR_CXQOV.sxml File
DECLARE
StartTime TIMESTAMP;
EndTime TIMESTAMP;
BEGIN
SAVEPOINT ScriptSavePoint;
StartTime := SYSTIMESTAMP;
BEGIN
-- prepare ALTER TABLE "CARDS" ADD CONSTRAINT "UQ_CARDS_TENANT_CODE_PAR_CXQOV" UNIQUE("TENANT","CODE","PARTNERTYPEID");
-- create backup for non UNIQUE("TENANT","CODE","PARTNERTYPEID")
EXECUTE IMMEDIATE 'create table CARDS_20r01d as SELECT * fROM  CARDS c  where c.code in (SELECT  DISTINCT CODE FROM (select "TENANT","CODE","PARTNERTYPEID", count(*) from  CARDS c group by "TENANT","CODE","PARTNERTYPEID" having count(*)>1))';
-- make it UNIQUE by u.code  ||'x' || ROWNUM
Update  CARDS u
set u.code  = u.code  ||'x' || ROWNUM
where u.id in (
SELECT c.id fROM  CARDs c
left outer join   DECLARATIONS d on c.id = d.CUSTOMERID
left outer join   tapags t on c.id = t.CUSTOMERID
where c.code in
(
SELECT  DISTINCT CODE FROM
(select "TENANT","CODE","PARTNERTYPEID", count(*) from  CARDS c group by "TENANT","CODE","PARTNERTYPEID" having count(*)>1)
)
and d.CUSTOMERID is null
and t.CUSTOMERID is null
);
-- how 2 check ...
-- select * From amital.GNDCARD  cnd join aminet_main.CARDS_20r01d   c on  cnd.card_id =c.CODE where APPLC='2'
END;
EndTime:= SYSTIMESTAMP;
BEGIN
DECLARE ScriptBody NCLOB;
BEGIN
ScriptBody := '-- prepare ALTER TABLE "CARDS" ADD CONSTRAINT "UQ_CARDS_TENANT_CODE_PAR_CXQOV" UNIQUE("TENANT","CODE","PARTNERTYPEID");
-- create backup for non UNIQUE("TENANT","CODE","PARTNERTYPEID")
EXECUTE IMMEDIATE ''create table CARDS_20r01d as SELECT * fROM  CARDS c  where c.code in (SELECT  DISTINCT CODE FROM (select "TENANT","CODE","PARTNERTYPEID", count(*) from  CARDS c group by "TENANT","CODE","PARTNERTYPEID" having count(*)>1))'';
-- make it UNIQUE by u.code  ||''x'' || ROWNUM
Update  CARDS u
set u.code  = u.code  ||''x'' || ROWNUM
where u.id in (
SELECT c.id fROM  CARDs c
left outer join   DECLARATIONS d on c.id = d.CUSTOMERID
left outer join   tapags t on c.id = t.CUSTOMERID
where c.code in
(
SELECT  DISTINCT CODE FROM
(select "TENANT","CODE","PARTNERTYPEID", count(*) from  CARDS c group by "TENANT","CODE","PARTNERTYPEID" having count(*)>1)
)
and d.CUSTOMERID is null
and t.CUSTOMERID is null
);
-- how 2 check ...
-- select * From amital.GNDCARD  cnd join aminet_main.CARDS_20r01d   c on  cnd.card_id =c.CODE where APPLC=''2''';
INSERT INTO "DBSCRIPTSHISTORY"("SXMLFILENAME", "EXECUTIONDATE", "SCRIPTBODY", "ELAPSEDTIMEINMS", "HASHVALUE", "VERSION")VALUES('2020070560750_UQ_CARDS_TENANT_CODE_PAR_CXQOV.sxml', SYSDATE, ScriptBody, EXTRACT(DAY FROM(EndTime - StartTime) * 24 * 60 * 60 * 1000), '0368ee576c3ff23d233081e8e30bb32b', 1);
END;
END;
EXCEPTION
WHEN OTHERS THEN
ROLLBACK TO ScriptSavePoint;
COMMIT;
END;

-- Drop Default Value For Column CODE
ALTER TABLE "EXCEPTIONREASONS" MODIFY "CODE" DEFAULT NULL;

DECLARE ScriptText NCLOB; BEGIN ScriptText := '-- Drop Default Value For Column CODEALTER TABLE "EXCEPTIONREASONS" MODIFY "CODE" DEFAULT NULL;'; INSERT INTO "DBMIGRATIONSHISTORY"("ID", "DXMLFILENAME", "TABLENAME", "COLUMNNAME", "MIGRATIONTYPE", "EXECUTIONDATE", "MIGRATIONSCRIPT")VALUES('2a2f70a3-e4b6-4652-a239-bd0cddf0dd2b', 'ExceptionReason.dxml', 'EXCEPTIONREASONS', 'CODE', 'Drop Default Value', SYSDATE, ScriptText); END;

-- Add Default Value For Column ISACTIVE
ALTER TABLE "EXCEPTIONREASONS" MODIFY "ISACTIVE" DEFAULT 0;

DECLARE ScriptText NCLOB; BEGIN ScriptText := '-- Add Default Value For Column ISACTIVEALTER TABLE "EXCEPTIONREASONS" MODIFY "ISACTIVE" DEFAULT 0;'; INSERT INTO "DBMIGRATIONSHISTORY"("ID", "DXMLFILENAME", "TABLENAME", "COLUMNNAME", "MIGRATIONTYPE", "EXECUTIONDATE", "MIGRATIONSCRIPT")VALUES('9bd868f8-3f9c-4ef0-bd47-9a139a5a0bdf', 'ExceptionReason.dxml', 'EXCEPTIONREASONS', 'ISACTIVE', 'Add Default Value', SYSDATE, ScriptText); END;


