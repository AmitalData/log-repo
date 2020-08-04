-- Add New Column With Name CANCELREQUESTAPPROVEDATE
ALTER TABLE "DECLARATIONS" ADD "CANCELREQUESTAPPROVEDATE" TIMESTAMP(7) NULL;

DECLARE ScriptText NCLOB; BEGIN ScriptText := '-- Add New Column With Name CANCELREQUESTAPPROVEDATEALTER TABLE "DECLARATIONS" ADD "CANCELREQUESTAPPROVEDATE" TIMESTAMP(7) NULL;'; INSERT INTO "DBMIGRATIONSHISTORY"("ID", "DXMLFILENAME", "TABLENAME", "COLUMNNAME", "MIGRATIONTYPE", "EXECUTIONDATE", "MIGRATIONSCRIPT")VALUES('038548b1-c4aa-48a0-9abd-383d04d33027', 'Declaration.dxml', 'DECLARATIONS', 'CANCELREQUESTAPPROVEDATE', 'Add Column', SYSDATE, ScriptText); END;

-- Add New Column With Name REPLACINGREPAIRREQUEST
ALTER TABLE "DECLARATIONS" ADD "REPLACINGREPAIRREQUEST" VARCHAR2(9 CHAR) NULL;

DECLARE ScriptText NCLOB; BEGIN ScriptText := '-- Add New Column With Name REPLACINGREPAIRREQUESTALTER TABLE "DECLARATIONS" ADD "REPLACINGREPAIRREQUEST" VARCHAR2(9 CHAR) NULL;'; INSERT INTO "DBMIGRATIONSHISTORY"("ID", "DXMLFILENAME", "TABLENAME", "COLUMNNAME", "MIGRATIONTYPE", "EXECUTIONDATE", "MIGRATIONSCRIPT")VALUES('49891596-56f0-4862-ab22-e127fd413dbf', 'Declaration.dxml', 'DECLARATIONS', 'REPLACINGREPAIRREQUEST', 'Add Column', SYSDATE, ScriptText); END;


-- Unset Nullable For Column DROP_TENANT
ALTER TABLE "REFERANTTEAM" MODIFY "DROP_TENANT" NOT NULL;

DECLARE ScriptText NCLOB; BEGIN ScriptText := '-- Unset Nullable For Column DROP_TENANTALTER TABLE "REFERANTTEAM" MODIFY "DROP_TENANT" NOT NULL;'; INSERT INTO "DBMIGRATIONSHISTORY"("ID", "DXMLFILENAME", "TABLENAME", "COLUMNNAME", "MIGRATIONTYPE", "EXECUTIONDATE", "MIGRATIONSCRIPT")VALUES('c70c9b2d-665c-46fb-a0c5-9811cd478bc4', 'ReferantTeam.dxml', 'REFERANTTEAM', 'DROP_TENANT', 'Unset Column Nullable', SYSDATE, ScriptText); END;

-- Rename Column From DROP_TENANT To TENANT
ALTER TABLE "REFERANTTEAM" RENAME COLUMN "DROP_TENANT" TO "TENANT";

DECLARE ScriptText NCLOB; BEGIN ScriptText := '-- Rename Column From DROP_TENANT To TENANTALTER TABLE "REFERANTTEAM" RENAME COLUMN "DROP_TENANT" TO "TENANT";'; INSERT INTO "DBMIGRATIONSHISTORY"("ID", "DXMLFILENAME", "TABLENAME", "COLUMNNAME", "MIGRATIONTYPE", "EXECUTIONDATE", "MIGRATIONSCRIPT")VALUES('1d4290bc-231f-4618-801b-55df97ec4896', 'ReferantTeam.dxml', 'REFERANTTEAM', 'DROP_TENANT', 'Rename Column', SYSDATE, ScriptText); END;


-- General Script From 202007201100_ReferantSearchfield.sxml File
DECLARE
StartTime TIMESTAMP;
EndTime TIMESTAMP;
BEGIN
SAVEPOINT ScriptSavePoint;
StartTime := SYSTIMESTAMP;
BEGIN
update declarationreferantdatas set
declarationreferantdatas.SEARCHFIELDS=
(select   to_char(d.DeclarationNumber)||',' ||to_char(d.CustomFileNo) ||',' || to_char(s.localname) ||',' ||
to_char(c.ManifestNumber )  ||',' || c.SecondCargoID  ||',' || c.ThirdCargoID as SEARCHFIELDS
from  declarationreferantdatas r
inner join
declarations d on  r.DECLARATIONID= d.id
inner join
cards s on d.customerid=s.id
inner join  CONSIGNMENTS c
on d.id= c.DECLARATIONID  and c.CONSIGNMENTNUMBER=1
where  d.id= declarationreferantdatas.DECLARATIONID);
END;
EndTime:= SYSTIMESTAMP;
BEGIN
DECLARE ScriptBody NCLOB;
BEGIN
ScriptBody := 'update declarationreferantdatas set
declarationreferantdatas.SEARCHFIELDS=
(select   to_char(d.DeclarationNumber)||'','' ||to_char(d.CustomFileNo) ||'','' || to_char(s.localname) ||'','' ||
to_char(c.ManifestNumber )  ||'','' || c.SecondCargoID  ||'','' || c.ThirdCargoID as SEARCHFIELDS
from  declarationreferantdatas r
inner join
declarations d on  r.DECLARATIONID= d.id
inner join
cards s on d.customerid=s.id
inner join  CONSIGNMENTS c
on d.id= c.DECLARATIONID  and c.CONSIGNMENTNUMBER=1
where  d.id= declarationreferantdatas.DECLARATIONID);';
UPDATE "DBSCRIPTSHISTORY" SET "EXECUTIONDATE" = SYSDATE, "SCRIPTBODY" = ScriptBody, "ELAPSEDTIMEINMS" = EXTRACT(DAY FROM(EndTime - StartTime) * 24 * 60 * 60 * 1000), "HASHVALUE" = 'ac7d69e9000b872dd8988527eb711e2e', "VERSION" = 2 WHERE "SXMLFILENAME" = '202007201100_ReferantSearchfield.sxml';
END;
END;
EXCEPTION
WHEN OTHERS THEN
ROLLBACK TO ScriptSavePoint;
COMMIT;
END;

