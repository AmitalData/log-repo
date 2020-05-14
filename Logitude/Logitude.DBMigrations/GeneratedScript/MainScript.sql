-- Unset Nullable For Column FIELDCODE
ALTER TABLE "OBJECTFIELDS" MODIFY "FIELDCODE" NOT NULL;

DECLARE ScriptText NCLOB; BEGIN ScriptText := '-- Unset Nullable For Column FIELDCODE
ALTER TABLE "OBJECTFIELDS" MODIFY "FIELDCODE" NOT NULL;'; INSERT INTO "DBMIGRATIONSHISTORY"("ID", "DXMLFILENAME", "TABLENAME", "COLUMNNAME", "MIGRATIONTYPE", "EXECUTIONDATE", "MIGRATIONSCRIPT")VALUES('6df3c19a-821c-46e7-93e2-f223498db485', 'ObjectField.dxml', 'OBJECTFIELDS', 'FIELDCODE', 'Unset Column Nullable', SYSDATE, ScriptText); END;


-- Create Index On OBJECTFIELDS Table
CREATE INDEX "IX_OBJECTFIELDS_FIELDCODE" ON "OBJECTFIELDS"("FIELDCODE");

DECLARE ScriptText NCLOB; BEGIN ScriptText := '-- Create Index On OBJECTFIELDS Table
CREATE INDEX "IX_OBJECTFIELDS_FIELDCODE" ON "OBJECTFIELDS"("FIELDCODE");'; INSERT INTO "DBMIGRATIONSHISTORY"("ID", "DXMLFILENAME", "TABLENAME", "COLUMNNAME", "MIGRATIONTYPE", "EXECUTIONDATE", "MIGRATIONSCRIPT")VALUES('c3a73093-b6b8-4958-b3a2-8cf3db5588c7', 'ObjectField.dxml', 'OBJECTFIELDS', 'FIELDCODE', 'Create Index', SYSDATE, ScriptText); END;


-- Create Unique Constraint On OBJECTFIELDS Table
ALTER TABLE "OBJECTFIELDS" ADD CONSTRAINT "UQ_OBJECTFIELDS_TENANT_C_WUZAO" UNIQUE("TENANT","CODE","OBJECTTABLEID");

DECLARE ScriptText NCLOB; BEGIN ScriptText := '-- Create Unique Constraint On OBJECTFIELDS Table
ALTER TABLE "OBJECTFIELDS" ADD CONSTRAINT "UQ_OBJECTFIELDS_TENANT_C_WUZAO" UNIQUE("TENANT","CODE","OBJECTTABLEID");'; INSERT INTO "DBMIGRATIONSHISTORY"("ID", "DXMLFILENAME", "TABLENAME", "COLUMNNAME", "MIGRATIONTYPE", "EXECUTIONDATE", "MIGRATIONSCRIPT")VALUES('1845cf3a-a030-4f93-afb6-00c1a64ef0e0', 'ObjectField.dxml', 'OBJECTFIELDS', 'TENANT,CODE,OBJECTTABLEID', 'Create Unique Constraint', SYSDATE, ScriptText); END;


-- Procedure Script From IdCounter GetIdRange Procedure.dxml
create or replace PROCEDURE usp_GetNextTableIdsRange
(
v_pStartNumber OUT int,
v_pNumberOfIds IN int,
v_pEndNumber OUT int,
v_pTableName IN VARCHAR2,
v_DBStringNumber OUT VARCHAR2
)
AS
v_COUNTERINIT int;
v_Current int;
v_DataBaseNumber int;
--Declare @DBStringNumber As varchar(50)
v_StartStrNumber VARCHAR2(50);
v_EndStrNumber VARCHAR2(50);
v_temp int := 0;
BEGIN
v_COUNTERINIT := v_pNumberOfIds ;
v_pStartNumber := 1 ;
SELECT DataBaseNumber
INTO v_DataBaseNumber
FROM DataBaseProperties
WHERE ROWNUM     <= 1;
v_DBStringNumber := CAST (v_DataBaseNumber as varchar2);
BEGIN
SELECT 1 INTO v_temp
FROM DUAL
WHERE NOT EXISTS ( SELECT TableName
FROM DBIdCounters
WHERE  TableName = v_pTableName );
EXCEPTION
WHEN OTHERS THEN
NULL;
END;
IF v_temp = 1 THEN
BEGIN
INSERT INTO DBIdCounters
( TableName, LastIdNumber )
VALUES ( v_pTableName, v_COUNTERINIT );
v_Current := 1 ;
END;
ELSE
BEGIN
SELECT LastIdNumber
INTO v_Current
FROM DBIdCounters
WHERE  TableName = v_pTableName
FOR UPDATE ;
v_pEndNumber := v_Current + v_pNumberOfIds ;
v_pStartNumber := v_Current + 1 ;
UPDATE DBIdCounters
SET LastIdNumber = v_pEndNumber--LastIdNumber + v_pNumberOfIds
WHERE  TableName = v_pTableName;
END;
END IF;
END;


