-- General Script From 202005210107_TablesBackup.sxml File
DECLARE
StartTime TIMESTAMP;
EndTime TIMESTAMP;
BEGIN
SAVEPOINT ScriptSavePoint;
StartTime := SYSTIMESTAMP;
BEGIN
EXECUTE IMMEDIATE 'create table z_countries as select * from countries';
EXECUTE IMMEDIATE 'create table z_states as select * from states';
EXECUTE IMMEDIATE 'create table z_COUNTRYCITIES as select * from COUNTRYCITIES';
EXECUTE IMMEDIATE 'create table z_INCOTERMS as select * from INCOTERMS';
EXECUTE IMMEDIATE 'create table z_PORTS as select * from PORTS';
EXECUTE IMMEDIATE 'create table z_objectfields as select * from objectfields';
EXECUTE IMMEDIATE 'create table z_querycolumns as select * from querycolumns';
EXECUTE IMMEDIATE 'create table z_ScreenFields as select * from ScreenFields';
EXECUTE IMMEDIATE 'create table z_AdvancedQueryFilters as select * from AdvancedQueryFilters';
EXECUTE IMMEDIATE 'create table z_RuleConditionFields as select * from RuleConditionFields';
EXECUTE IMMEDIATE 'create table z_ObjectTableRuleFields as select * from ObjectTableRuleFields';
EXECUTE IMMEDIATE 'create table z_ObjectTableRules as select * from ObjectTableRules';
EXECUTE IMMEDIATE 'create table z_screens as select * from screens';
EXECUTE IMMEDIATE 'create table z_Queries as select * from Queries';
EXECUTE IMMEDIATE 'create table z_MenuButtons as select * from MenuButtons';
EXECUTE IMMEDIATE 'create table z_ObjectTableTabs as select * from ObjectTableTabs';
EXECUTE IMMEDIATE 'create table z_textcodes as select * from textcodes';
EXECUTE IMMEDIATE 'create table z_Features as select * from Features';
EXECUTE IMMEDIATE 'create table z_MenusTables as select * from MenusTables';
EXECUTE IMMEDIATE 'create table z_CustomsRequiredFields as select * from CustomsRequiredFields';
EXECUTE IMMEDIATE 'create table z_AirlineMessagingRules as select * from AirlineMessagingRules';
EXECUTE IMMEDIATE 'create table z_restrictions as select * from restrictions';
EXECUTE IMMEDIATE 'create table z_CustomerFieldsUpdateSettings as select * from CustomerFieldsUpdateSettings';
EXECUTE IMMEDIATE 'create table z_ObjectFieldValidations as select * from ObjectFieldValidations';
EXECUTE IMMEDIATE 'create table z_ObjectFieldModifications as select * from ObjectFieldModifications';
EXECUTE IMMEDIATE 'create table z_ScreenModifications as select * from ScreenModifications';
EXECUTE IMMEDIATE 'create table z_ObjectTables as select * from ObjectTables';
EXECUTE IMMEDIATE 'create table z_SharedUserQueries as select * from SharedUserQueries';
EXECUTE IMMEDIATE 'create table z_Tips as select * from Tips';
EXECUTE IMMEDIATE 'create table z_Translations as select * from Translations';
EXECUTE IMMEDIATE 'create table z_PackageFeatures as select * from PackageFeatures';
EXECUTE IMMEDIATE 'create table z_RoleFeatures as select * from RoleFeatures';
EXECUTE IMMEDIATE 'create table z_Reports as select * from Reports';
EXECUTE IMMEDIATE 'create table z_ObjectTableHelperControls as select * from ObjectTableHelperControls';
END;
EndTime:= SYSTIMESTAMP;
BEGIN
DECLARE ScriptBody NCLOB;
BEGIN
ScriptBody := 'EXECUTE IMMEDIATE ''create table z_countries as select * from countries'';
EXECUTE IMMEDIATE ''create table z_states as select * from states'';
EXECUTE IMMEDIATE ''create table z_COUNTRYCITIES as select * from COUNTRYCITIES'';
EXECUTE IMMEDIATE ''create table z_INCOTERMS as select * from INCOTERMS'';
EXECUTE IMMEDIATE ''create table z_PORTS as select * from PORTS'';
EXECUTE IMMEDIATE ''create table z_objectfields as select * from objectfields'';
EXECUTE IMMEDIATE ''create table z_querycolumns as select * from querycolumns'';
EXECUTE IMMEDIATE ''create table z_ScreenFields as select * from ScreenFields'';
EXECUTE IMMEDIATE ''create table z_AdvancedQueryFilters as select * from AdvancedQueryFilters'';
EXECUTE IMMEDIATE ''create table z_RuleConditionFields as select * from RuleConditionFields'';
EXECUTE IMMEDIATE ''create table z_ObjectTableRuleFields as select * from ObjectTableRuleFields'';
EXECUTE IMMEDIATE ''create table z_ObjectTableRules as select * from ObjectTableRules'';
EXECUTE IMMEDIATE ''create table z_screens as select * from screens'';
EXECUTE IMMEDIATE ''create table z_Queries as select * from Queries'';
EXECUTE IMMEDIATE ''create table z_MenuButtons as select * from MenuButtons'';
EXECUTE IMMEDIATE ''create table z_ObjectTableTabs as select * from ObjectTableTabs'';
EXECUTE IMMEDIATE ''create table z_textcodes as select * from textcodes'';
EXECUTE IMMEDIATE ''create table z_Features as select * from Features'';
EXECUTE IMMEDIATE ''create table z_MenusTables as select * from MenusTables'';
EXECUTE IMMEDIATE ''create table z_CustomsRequiredFields as select * from CustomsRequiredFields'';
EXECUTE IMMEDIATE ''create table z_AirlineMessagingRules as select * from AirlineMessagingRules'';
EXECUTE IMMEDIATE ''create table z_restrictions as select * from restrictions'';
EXECUTE IMMEDIATE ''create table z_CustomerFieldsUpdateSettings as select * from CustomerFieldsUpdateSettings'';
EXECUTE IMMEDIATE ''create table z_ObjectFieldValidations as select * from ObjectFieldValidations'';
EXECUTE IMMEDIATE ''create table z_ObjectFieldModifications as select * from ObjectFieldModifications'';
EXECUTE IMMEDIATE ''create table z_ScreenModifications as select * from ScreenModifications'';
EXECUTE IMMEDIATE ''create table z_ObjectTables as select * from ObjectTables'';
EXECUTE IMMEDIATE ''create table z_SharedUserQueries as select * from SharedUserQueries'';
EXECUTE IMMEDIATE ''create table z_Tips as select * from Tips'';
EXECUTE IMMEDIATE ''create table z_Translations as select * from Translations'';
EXECUTE IMMEDIATE ''create table z_PackageFeatures as select * from PackageFeatures'';
EXECUTE IMMEDIATE ''create table z_RoleFeatures as select * from RoleFeatures'';
EXECUTE IMMEDIATE ''create table z_Reports as select * from Reports'';
EXECUTE IMMEDIATE ''create table z_ObjectTableHelperControls as select * from ObjectTableHelperControls'';';
UPDATE "DBSCRIPTSHISTORY" SET "EXECUTIONDATE" = SYSDATE, "SCRIPTBODY" = ScriptBody, "ELAPSEDTIMEINMS" = EXTRACT(DAY FROM(EndTime - StartTime) * 24 * 60 * 60 * 1000), "HASHVALUE" = '56586ad5b96c1f0cba7315d568348615', "VERSION" = 2 WHERE "SXMLFILENAME" = '202005210107_TablesBackup.sxml';
END;
END;
EXCEPTION
WHEN OTHERS THEN
ROLLBACK TO ScriptSavePoint;
COMMIT;
END;

