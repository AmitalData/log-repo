-- General Script From 202005210107_TablesBackup.sxml File
DECLARE
StartTime TIMESTAMP;
EndTime TIMESTAMP;
BEGIN
SAVEPOINT ScriptSavePoint;
StartTime := SYSTIMESTAMP;
BEGIN
EXECUTE IMMEDIATE 'create table Bak_countries as select * from countries';
EXECUTE IMMEDIATE 'create table Bak_states as select * from states';
EXECUTE IMMEDIATE 'create table Bak_COUNTRYCITIES as select * from COUNTRYCITIES';
EXECUTE IMMEDIATE 'create table Bak_INCOTERMS as select * from INCOTERMS';
EXECUTE IMMEDIATE 'create table Bak_PORTS as select * from PORTS';
EXECUTE IMMEDIATE 'create table Bak_objectfields as select * from objectfields';
EXECUTE IMMEDIATE 'create table Bak_querycolumns as select * from querycolumns';
EXECUTE IMMEDIATE 'create table Bak_ScreenFields as select * from ScreenFields';
EXECUTE IMMEDIATE 'create table Bak_AdvancedQueryFilters as select * from AdvancedQueryFilters';
EXECUTE IMMEDIATE 'create table Bak_RuleConditionFields as select * from RuleConditionFields';
EXECUTE IMMEDIATE 'create table Bak_ObjectTableRuleFields as select * from ObjectTableRuleFields';
EXECUTE IMMEDIATE 'create table Bak_ObjectTableRules as select * from ObjectTableRules';
EXECUTE IMMEDIATE 'create table Bak_screens as select * from screens';
EXECUTE IMMEDIATE 'create table Bak_Queries as select * from Queries';
EXECUTE IMMEDIATE 'create table Bak_MenuButtons as select * from MenuButtons';
EXECUTE IMMEDIATE 'create table Bak_ObjectTableTabs as select * from ObjectTableTabs';
EXECUTE IMMEDIATE 'create table Bak_textcodes as select * from textcodes';
EXECUTE IMMEDIATE 'create table Bak_Features as select * from Features';
EXECUTE IMMEDIATE 'create table Bak_MenusTables as select * from MenusTables';
EXECUTE IMMEDIATE 'create table Bak_CustomsRequiredFields as select * from CustomsRequiredFields';
EXECUTE IMMEDIATE 'create table Bak_AirlineMessagingRules as select * from AirlineMessagingRules';
EXECUTE IMMEDIATE 'create table Bak_restrictions as select * from restrictions';
EXECUTE IMMEDIATE 'create table Bak_CustomerFieldsUpdateSettings as select * from CustomerFieldsUpdateSettings';
EXECUTE IMMEDIATE 'create table Bak_ObjectFieldValidations as select * from ObjectFieldValidations';
EXECUTE IMMEDIATE 'create table Bak_ObjectFieldModifications as select * from ObjectFieldModifications';
EXECUTE IMMEDIATE 'create table Bak_ScreenModifications as select * from ScreenModifications';
EXECUTE IMMEDIATE 'create table Bak_ObjectTables as select * from ObjectTables';
EXECUTE IMMEDIATE 'create table Bak_SharedUserQueries as select * from SharedUserQueries';
EXECUTE IMMEDIATE 'create table Bak_Tips as select * from Tips';
EXECUTE IMMEDIATE 'create table Bak_Translations as select * from Translations';
EXECUTE IMMEDIATE 'create table Bak_PackageFeatures as select * from PackageFeatures';
EXECUTE IMMEDIATE 'create table Bak_RoleFeatures as select * from RoleFeatures';
EXECUTE IMMEDIATE 'create table Bak_Reports as select * from Reports';
EXECUTE IMMEDIATE 'create table Bak_ObjectTableHelperControls as select * from ObjectTableHelperControls';
END;
EndTime:= SYSTIMESTAMP;
BEGIN
DECLARE ScriptBody NCLOB;
BEGIN
ScriptBody := 'EXECUTE IMMEDIATE ''create table Bak_countries as select * from countries'';
EXECUTE IMMEDIATE ''create table Bak_states as select * from states'';
EXECUTE IMMEDIATE ''create table Bak_COUNTRYCITIES as select * from COUNTRYCITIES'';
EXECUTE IMMEDIATE ''create table Bak_INCOTERMS as select * from INCOTERMS'';
EXECUTE IMMEDIATE ''create table Bak_PORTS as select * from PORTS'';
EXECUTE IMMEDIATE ''create table Bak_objectfields as select * from objectfields'';
EXECUTE IMMEDIATE ''create table Bak_querycolumns as select * from querycolumns'';
EXECUTE IMMEDIATE ''create table Bak_ScreenFields as select * from ScreenFields'';
EXECUTE IMMEDIATE ''create table Bak_AdvancedQueryFilters as select * from AdvancedQueryFilters'';
EXECUTE IMMEDIATE ''create table Bak_RuleConditionFields as select * from RuleConditionFields'';
EXECUTE IMMEDIATE ''create table Bak_ObjectTableRuleFields as select * from ObjectTableRuleFields'';
EXECUTE IMMEDIATE ''create table Bak_ObjectTableRules as select * from ObjectTableRules'';
EXECUTE IMMEDIATE ''create table Bak_screens as select * from screens'';
EXECUTE IMMEDIATE ''create table Bak_Queries as select * from Queries'';
EXECUTE IMMEDIATE ''create table Bak_MenuButtons as select * from MenuButtons'';
EXECUTE IMMEDIATE ''create table Bak_ObjectTableTabs as select * from ObjectTableTabs'';
EXECUTE IMMEDIATE ''create table Bak_textcodes as select * from textcodes'';
EXECUTE IMMEDIATE ''create table Bak_Features as select * from Features'';
EXECUTE IMMEDIATE ''create table Bak_MenusTables as select * from MenusTables'';
EXECUTE IMMEDIATE ''create table Bak_CustomsRequiredFields as select * from CustomsRequiredFields'';
EXECUTE IMMEDIATE ''create table Bak_AirlineMessagingRules as select * from AirlineMessagingRules'';
EXECUTE IMMEDIATE ''create table Bak_restrictions as select * from restrictions'';
EXECUTE IMMEDIATE ''create table Bak_CustomerFieldsUpdateSettings as select * from CustomerFieldsUpdateSettings'';
EXECUTE IMMEDIATE ''create table Bak_ObjectFieldValidations as select * from ObjectFieldValidations'';
EXECUTE IMMEDIATE ''create table Bak_ObjectFieldModifications as select * from ObjectFieldModifications'';
EXECUTE IMMEDIATE ''create table Bak_ScreenModifications as select * from ScreenModifications'';
EXECUTE IMMEDIATE ''create table Bak_ObjectTables as select * from ObjectTables'';
EXECUTE IMMEDIATE ''create table Bak_SharedUserQueries as select * from SharedUserQueries'';
EXECUTE IMMEDIATE ''create table Bak_Tips as select * from Tips'';
EXECUTE IMMEDIATE ''create table Bak_Translations as select * from Translations'';
EXECUTE IMMEDIATE ''create table Bak_PackageFeatures as select * from PackageFeatures'';
EXECUTE IMMEDIATE ''create table Bak_RoleFeatures as select * from RoleFeatures'';
EXECUTE IMMEDIATE ''create table Bak_Reports as select * from Reports'';
EXECUTE IMMEDIATE ''create table Bak_ObjectTableHelperControls as select * from ObjectTableHelperControls'';';
UPDATE "DBSCRIPTSHISTORY" SET "EXECUTIONDATE" = SYSDATE, "SCRIPTBODY" = ScriptBody, "ELAPSEDTIMEINMS" = EXTRACT(DAY FROM(EndTime - StartTime) * 24 * 60 * 60 * 1000), "HASHVALUE" = '45ab9dc5bd26c7fce2592d26e7b17fb8', "VERSION" = 2 WHERE "SXMLFILENAME" = '202005210107_TablesBackup.sxml';
END;
END;
EXCEPTION
WHEN OTHERS THEN
ROLLBACK TO ScriptSavePoint;
COMMIT;
END;

-- Drop Column DECLARATIONTYPECODE
ALTER TABLE "DECLARATIONS" RENAME COLUMN "DECLARATIONTYPECODE" TO "DROP_DECLARATIONTYPECODE";

DECLARE ScriptText NCLOB; BEGIN ScriptText := '-- Drop Column DECLARATIONTYPECODEALTER TABLE "DECLARATIONS" RENAME COLUMN "DECLARATIONTYPECODE" TO "DROP_DECLARATIONTYPECODE";'; INSERT INTO "DBMIGRATIONSHISTORY"("ID", "DXMLFILENAME", "TABLENAME", "COLUMNNAME", "MIGRATIONTYPE", "EXECUTIONDATE", "MIGRATIONSCRIPT")VALUES('4e516a8a-d321-48cb-b522-7b6de980d978', 'Declaration.dxml', 'DECLARATIONS', 'DECLARATIONTYPECODE', 'Drop Column', SYSDATE, ScriptText); END;


-- Drop Column VEHICLEIDTYPECODE
ALTER TABLE "SUPPLIERINVOICEITEMVEHICLES" RENAME COLUMN "VEHICLEIDTYPECODE" TO "DROP_VEHICLEIDTYPECODE";

DECLARE ScriptText NCLOB; BEGIN ScriptText := '-- Drop Column VEHICLEIDTYPECODEALTER TABLE "SUPPLIERINVOICEITEMVEHICLES" RENAME COLUMN "VEHICLEIDTYPECODE" TO "DROP_VEHICLEIDTYPECODE";'; INSERT INTO "DBMIGRATIONSHISTORY"("ID", "DXMLFILENAME", "TABLENAME", "COLUMNNAME", "MIGRATIONTYPE", "EXECUTIONDATE", "MIGRATIONSCRIPT")VALUES('25799c04-985f-45d4-ab0b-6e5b7e53340d', 'SupplierInvoiceItemVehicle.dxml', 'SUPPLIERINVOICEITEMVEHICLES', 'VEHICLEIDTYPECODE', 'Drop Column', SYSDATE, ScriptText); END;


-- Drop Foreign Key Constraint For Column DECLARATIONTYPECODE In Table DECLARATIONS That Reference To Column CODE In Table LEADDOCUMENTTYPES
DECLARE ConstraintCount NUMBER; BEGIN SELECT COUNT(*) INTO ConstraintCount FROM USER_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_937603261'; IF (ConstraintCount <> 0) THEN EXECUTE IMMEDIATE 'ALTER TABLE "DECLARATIONS" DROP CONSTRAINT "FK_937603261"'; END IF; END;

DECLARE ScriptText NCLOB; BEGIN ScriptText := '-- Drop Foreign Key Constraint For Column DECLARATIONTYPECODE In Table DECLARATIONS That Reference To Column CODE In Table LEADDOCUMENTTYPESDECLARE ConstraintCount NUMBER; BEGIN SELECT COUNT(*) INTO ConstraintCount FROM USER_CONSTRAINTS WHERE CONSTRAINT_NAME = ''FK_937603261''; IF (ConstraintCount <> 0) THEN EXECUTE IMMEDIATE ''ALTER TABLE "DECLARATIONS" DROP CONSTRAINT "FK_937603261"''; END IF; END;'; INSERT INTO "DBMIGRATIONSHISTORY"("ID", "DXMLFILENAME", "TABLENAME", "COLUMNNAME", "MIGRATIONTYPE", "EXECUTIONDATE", "MIGRATIONSCRIPT")VALUES('e5393cee-6351-4a7f-9e85-d245bf62b59f', 'Declaration.dxml', 'DECLARATIONS', NULL, 'Drop Relation', SYSDATE, ScriptText); END;

-- Drop Index IX_N433128425 From Table DECLARATIONS
DECLARE IndexCount NUMBER; BEGIN SELECT COUNT(*) INTO IndexCount FROM USER_INDEXES WHERE INDEX_NAME = 'IX_N433128425'; IF (IndexCount <> 0) THEN EXECUTE IMMEDIATE 'DROP INDEX "IX_N433128425"'; END IF; END;

DECLARE ScriptText NCLOB; BEGIN ScriptText := '-- Drop Index IX_N433128425 From Table DECLARATIONSDECLARE IndexCount NUMBER; BEGIN SELECT COUNT(*) INTO IndexCount FROM USER_INDEXES WHERE INDEX_NAME = ''IX_N433128425''; IF (IndexCount <> 0) THEN EXECUTE IMMEDIATE ''DROP INDEX "IX_N433128425"''; END IF; END;'; INSERT INTO "DBMIGRATIONSHISTORY"("ID", "DXMLFILENAME", "TABLENAME", "COLUMNNAME", "MIGRATIONTYPE", "EXECUTIONDATE", "MIGRATIONSCRIPT")VALUES('b4f39ee1-f381-4270-a034-b00e48e11079', 'Declaration.dxml', 'DECLARATIONS', NULL, 'Drop Index', SYSDATE, ScriptText); END;


-- Drop Foreign Key Constraint For Column VEHICLEIDTYPECODE In Table SUPPLIERINVOICEITEMVEHICLES That Reference To Column CODE In Table CARGOIDENTITYQUALIFIERS
DECLARE ConstraintCount NUMBER; BEGIN SELECT COUNT(*) INTO ConstraintCount FROM USER_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_N640192686'; IF (ConstraintCount <> 0) THEN EXECUTE IMMEDIATE 'ALTER TABLE "SUPPLIERINVOICEITEMVEHICLES" DROP CONSTRAINT "FK_N640192686"'; END IF; END;

DECLARE ScriptText NCLOB; BEGIN ScriptText := '-- Drop Foreign Key Constraint For Column VEHICLEIDTYPECODE In Table SUPPLIERINVOICEITEMVEHICLES That Reference To Column CODE In Table CARGOIDENTITYQUALIFIERSDECLARE ConstraintCount NUMBER; BEGIN SELECT COUNT(*) INTO ConstraintCount FROM USER_CONSTRAINTS WHERE CONSTRAINT_NAME = ''FK_N640192686''; IF (ConstraintCount <> 0) THEN EXECUTE IMMEDIATE ''ALTER TABLE "SUPPLIERINVOICEITEMVEHICLES" DROP CONSTRAINT "FK_N640192686"''; END IF; END;'; INSERT INTO "DBMIGRATIONSHISTORY"("ID", "DXMLFILENAME", "TABLENAME", "COLUMNNAME", "MIGRATIONTYPE", "EXECUTIONDATE", "MIGRATIONSCRIPT")VALUES('0ae928a6-f6dc-4c25-8574-37ed4197f8bf', 'SupplierInvoiceItemVehicle.dxml', 'SUPPLIERINVOICEITEMVEHICLES', NULL, 'Drop Relation', SYSDATE, ScriptText); END;

-- Drop Index IX_N941339729 From Table SUPPLIERINVOICEITEMVEHICLES
DECLARE IndexCount NUMBER; BEGIN SELECT COUNT(*) INTO IndexCount FROM USER_INDEXES WHERE INDEX_NAME = 'IX_N941339729'; IF (IndexCount <> 0) THEN EXECUTE IMMEDIATE 'DROP INDEX "IX_N941339729"'; END IF; END;

DECLARE ScriptText NCLOB; BEGIN ScriptText := '-- Drop Index IX_N941339729 From Table SUPPLIERINVOICEITEMVEHICLESDECLARE IndexCount NUMBER; BEGIN SELECT COUNT(*) INTO IndexCount FROM USER_INDEXES WHERE INDEX_NAME = ''IX_N941339729''; IF (IndexCount <> 0) THEN EXECUTE IMMEDIATE ''DROP INDEX "IX_N941339729"''; END IF; END;'; INSERT INTO "DBMIGRATIONSHISTORY"("ID", "DXMLFILENAME", "TABLENAME", "COLUMNNAME", "MIGRATIONTYPE", "EXECUTIONDATE", "MIGRATIONSCRIPT")VALUES('c84a0997-28cf-4060-a9b5-8a60fdcf0cac', 'SupplierInvoiceItemVehicle.dxml', 'SUPPLIERINVOICEITEMVEHICLES', NULL, 'Drop Index', SYSDATE, ScriptText); END;


