-- Procedure Script From DeleteObjectTableMetadataProcedure.dxml
create or replace PROCEDURE usp_DeleteObjectTableMetadata(
v_pTableName IN VARCHAR2 )
AS
v_ObjectTableId VARCHAR2(15);
BEGIN
BEGIN
BEGIN
SELECT Id
INTO v_ObjectTableId
FROM ObjectTables
WHERE NAME  = v_pTableName
AND ROWNUM <= 1;
EXCEPTION
WHEN NO_DATA_FOUND THEN
v_ObjectTableId := NULL;
END;
--*--Delete--*--
--ObjectFields
DELETE objectfields
WHERE tenant      = 0
AND ObjectTableId = v_ObjectTableId;
DELETE querycolumns
WHERE tenant   = 0
AND userid    IS NULL
AND QueryCode IN
( SELECT UniqueCode FROM Queries WHERE ObjectTableId = v_ObjectTableId
) ;
DELETE AdvancedQueryFilters
WHERE tenant   = 0
AND userid    IS NULL
AND QueryCode IN
( SELECT UniqueCode FROM Queries WHERE ObjectTableId = v_ObjectTableId
) ;
DELETE RuleConditionFields
WHERE tenant               = 0
AND ObjectTableRuleId NOT IN
( SELECT id FROM ObjectTableRules WHERE SystemLevel = 0
)
AND ObjectTableRuleId IN
( SELECT id FROM ObjectTableRules WHERE ObjectTableId = v_ObjectTableId
) ;
DELETE ObjectTableRuleFields
WHERE tenant           = 0
AND systemlevel        = 1
AND ObjectTableRuleId IN
( SELECT id FROM ObjectTableRules WHERE ObjectTableId = v_ObjectTableId
) ;
DELETE ObjectTableRules
WHERE tenant    = 0
AND systemlevel = 1
AND Id         IN
( SELECT id FROM ObjectTableRules WHERE ObjectTableId = v_ObjectTableId
) ;
--Screens
DELETE ScreenFields
WHERE tenant    = 0
AND ScreenCode IN
( SELECT code FROM Screens WHERE ObjectTableId = v_ObjectTableId
) ;
DELETE screens WHERE tenant = 0 AND ObjectTableId = v_ObjectTableId;
--Queries
DELETE Queries
WHERE tenant      = 0
AND userid       IS NULL
AND systemlevel   = 1
AND ObjectTableId = v_ObjectTableId;
--TextCodes
DELETE MenuButtons
WHERE tenant           = 0
AND MenuButtonGroupId IN
( SELECT id FROM MenuButtonGroups WHERE ObjectTableId = v_ObjectTableId
) ;
DELETE ObjectTableTabs WHERE tenant = 0 AND ObjectTableId = v_ObjectTableId;
DELETE textcodes
WHERE tenant  = 0
AND code NOT IN
(SELECT NameTextCodeCode
FROM queries
WHERE tenant          = 0
AND userid           IS NOT NULL
AND SystemLevel       = 0
AND NameTextCodeCode IS NOT NULL
)
AND code NOT IN
( SELECT ShortTextCodeCode FROM tips
)
AND ObjectTableId = v_ObjectTableId;
--Features
DELETE Features
WHERE tenant      = 0
AND ObjectTableId = v_ObjectTableId;-----------
END;
END;


-- Procedure Script From PreDeleteMetadataProcedure.dxml
create or replace PROCEDURE usp_PreDeleteMetadata(
v_pTableName IN VARCHAR2 )
AS
v_ObjectTableId VARCHAR2(15);
tbl_exist  PLS_INTEGER;
featuresCreation varchar2(500);
spellCheckedCreation varchar2(500);
BEGIN
----------------------------------------------------------------Run automation Update before delete
select count(*) into tbl_exist from user_tables where table_name = 'TEMPOLDFEATURES';
if tbl_exist = 1 then
execute immediate 'drop table TEMPOLDFEATURES';
dbms_output.put_line('drop table TEMPOLDFEATURES');
end if;
dbms_output.put_line('out of drop table TEMPOLDFEATURES' || tbl_exist);
--select * from TempOldFeatures
featuresCreation:= 'CREATE TABLE TEMPOLDFEATURES
AS
SELECT *
FROM
(SELECT Code ,
FeatureUniqeCode ,
IsOld ,
Tenant
FROM features
WHERE IsOld = 1
) t';
execute immediate featuresCreation;
select count(*) into tbl_exist from user_tables where table_name = 'TEMPISSPELLCHECKEDTEXTCODES';
if tbl_exist = 1 then
execute immediate 'drop table TEMPISSPELLCHECKEDTEXTCODES';
end if;
spellCheckedCreation:= 'CREATE TABLE TEMPISSPELLCHECKEDTEXTCODES
AS
SELECT * FROM
( SELECT * FROM TextCodes WHERE IsSpellChecked = 1
) t';--select * from TempIsSpellCheckedTextCodes
execute immediate spellCheckedCreation;
END;


-- Procedure Script From ReconnectObjectMetaDataProcedure.dxml
create or replace PROCEDURE usp_ReconnectObjectTableMetada(
v_pTableName IN VARCHAR2 )
AS
v_ObjectTableId VARCHAR2(15);
BEGIN
BEGIN
BEGIN
SELECT Id
INTO v_ObjectTableId
FROM ObjectTables
WHERE NAME  = v_pTableName
AND ROWNUM <= 1;
EXCEPTION
WHEN NO_DATA_FOUND THEN
v_ObjectTableId := NULL;
END;
----MetaData All Scripts: Never Apply these scripts
--*--After Delete--*--
--ObjectFields
UPDATE CUSTOMSREQUIREDFIELDS
SET ObjectFieldId =
(SELECT Id
FROM objectfields
WHERE FieldCode = CUSTOMSREQUIREDFIELDS.ObjectFieldCode
);
UPDATE querycolumns
SET ObjectFieldId =
( SELECT Id FROM objectfields WHERE FieldCode = querycolumns.ObjectFieldCode
)
WHERE objectfieldcode IN
( SELECT fieldcode FROM ObjectFields
) ;
UPDATE ScreenFields
SET ObjectFieldId =
( SELECT Id FROM objectfields WHERE FieldCode = ScreenFields.ObjectFieldCode
)
WHERE objectfieldcode IN
( SELECT fieldcode FROM ObjectFields
) ;
UPDATE AdvancedQueryFilters
SET ObjectFieldId =
(SELECT Id
FROM objectfields
WHERE FieldCode = AdvancedQueryFilters.ObjectFieldCode
)
WHERE objectfieldcode IN
( SELECT fieldcode FROM ObjectFields
) ;
UPDATE RuleConditionFields
SET ObjectFieldId =
(SELECT Id
FROM objectfields
WHERE FieldCode = RuleConditionFields.ObjectFieldCode
);
UPDATE ObjectTableRuleFields
SET ObjectFieldId =
(SELECT Id
FROM objectfields
WHERE FieldCode = ObjectTableRuleFields.ObjectFieldCode
);
UPDATE ObjectTableRules
SET TriggerFieldId =
(SELECT Id
FROM objectfields
WHERE FieldCode = ObjectTableRules.TriggerFieldCode
);
UPDATE AirlineMessagingRules
SET RuleFieldId =
(SELECT Id
FROM objectfields
WHERE FieldCode = AirlineMessagingRules.RuleFieldCode
);
UPDATE restrictions
SET ObjectFieldId =
( SELECT Id FROM objectfields WHERE FieldCode = restrictions.ObjectFieldCode
);
UPDATE CustomerFieldsUpdateSettings
SET ObjectFieldId =
(SELECT Id
FROM objectfields
WHERE FieldCode = CustomerFieldsUpdateSettings.ObjectFieldCode
);
--UPDATE ObjectFieldValidations
--SET ObjectFieldId =
--  (SELECT Id
--  FROM objectfields
--  WHERE FieldCode = ObjectFieldValidations.ObjectFieldCode
--  );
UPDATE ObjectFieldModifications
SET ObjectFieldId =
(SELECT Id
FROM objectfields
WHERE FieldCode = ObjectFieldModifications.ObjectFieldCode
)
WHERE objectfieldcode IN
( SELECT fieldcode FROM ObjectFields
) ;
--Screens
UPDATE ScreenModifications
SET ScreenId =
( SELECT Id FROM Screens WHERE code = ScreenModifications.ScreenCode
)
WHERE screencode IN
( SELECT code FROM screens
) ;
UPDATE ScreenFields
SET ScreenId =
( SELECT Id FROM Screens WHERE code = ScreenFields.ScreenCode
)
WHERE screencode IN
( SELECT code FROM screens
) ;
UPDATE ObjectTables
SET HeaderScreenId =
( SELECT Id FROM Screens WHERE code = ObjectTables.HeaderScreenCode
);
--Queries
UPDATE Queries
SET OriginalQueryId =
(SELECT q1.Id
FROM Queries q1
WHERE q1.UniqueCode = Queries.OriginalQueryCode and q1.USERID is null
);
UPDATE AdvancedQueryFilters
SET QueryId =
( SELECT Id FROM Queries WHERE UniqueCode = AdvancedQueryFilters.QueryCode
)
WHERE QueryCode IN
( SELECT UniqueCode FROM Queries
) ;
UPDATE QueryColumns
SET QueryId =
( SELECT Id FROM Queries WHERE UniqueCode = QueryColumns.QueryCode
)
WHERE QueryCode IN
( SELECT UniqueCode FROM Queries
) ;
UPDATE SharedUserQueries
SET QueryId =
( SELECT Id FROM Queries WHERE UniqueCode = SharedUserQueries.QueryCode
);
--TextCodes
UPDATE Queries
SET NameTextCodeId =
(SELECT Id
FROM TextCodes
WHERE Code = Queries.NameTextCodeCode
AND tenant = Queries.Tenant
);
UPDATE ObjectTables
SET DescriptionTextCodeId =
(SELECT Id
FROM TextCodes
WHERE Code = ObjectTables.DescriptionTextCodeCode
AND tenant = ObjectTables.Tenant
);
UPDATE ObjectTables
SET NewButtonTextCodeId =
( SELECT Id FROM TextCodes WHERE Code = ObjectTables.NewButtonTextCodeCode
);
UPDATE Features
SET NameTextCodeId =
(SELECT Id
FROM TextCodes
WHERE Code = Features.NameTextCodeCode
AND tenant = Features.Tenant
);
UPDATE Tips
SET ShortTextCode =
( SELECT Id FROM TextCodes WHERE Code = Tips.ShortTextCodeCode
);
UPDATE ObjectTableTabs
SET TabNameTextCodeId =
(SELECT Id
FROM TextCodes
WHERE Code = ObjectTableTabs.TabNameTextCodeCode
AND tenant = ObjectTableTabs.Tenant
);
UPDATE MenuButtons
SET LabelTextCodeId =
( SELECT Id FROM TextCodes WHERE Code = MenuButtons.LabelTextCodeCode
);
UPDATE ObjectFields
SET FullNameTextCodeId =
(SELECT Id
FROM TextCodes
WHERE Code = ObjectFields.FullNameTextCodeCode
AND tenant = ObjectFields.Tenant
);
UPDATE ObjectFields
SET HelpTextCodeId =
(SELECT Id
FROM TextCodes
WHERE Code = ObjectFields.HelpTextCodeCode
AND tenant = ObjectFields.Tenant
);
UPDATE ObjectFields
SET ShortNameTextCodeId =
(SELECT Id
FROM TextCodes
WHERE Code = ObjectFields.ShortNameTextCodeCode
AND tenant = ObjectFields.Tenant
);
UPDATE ObjectFields
SET ListTextCodeId =
(SELECT Id
FROM TextCodes
WHERE Code = ObjectFields.ListTextCodeCode
AND tenant = ObjectFields.Tenant
);
UPDATE Translations
SET TextCodeId =
(SELECT Id
FROM TextCodes
WHERE Code   = Translations.TextCodeCode
AND ( tenant = Translations.Tenant
OR tenant    = 0 )
)
WHERE TextCodeCode IN
( SELECT code FROM textcodes
) ;
--Features
UPDATE MenusTables
SET FeatureId =
(SELECT Id
FROM features
WHERE FeatureUniqeCode = MenusTables.FeatureUniqeCode
);
UPDATE Queries
SET FeatureId =
( SELECT Id FROM features WHERE FeatureUniqeCode = Queries.FeatureUniqeCode
);
UPDATE ObjectTableTabs
SET FeatureId =
(SELECT Id
FROM features
WHERE FeatureUniqeCode = ObjectTableTabs.FeatureUniqeCode
);
UPDATE ObjectTableHelperControls
SET FeatureId =
(SELECT Id
FROM features
WHERE FeatureUniqeCode = ObjectTableHelperControls.FeatureUniqeCode
);
UPDATE MenuButtons
SET FeatureId =
(SELECT Id
FROM features
WHERE FeatureUniqeCode = MenuButtons.FeatureUniqeCode
);
UPDATE Reports
SET FeatureId =
( SELECT Id FROM features WHERE FeatureUniqeCode = Reports.FeatureUniqeCode
);
UPDATE PackageFeatures
SET FeatureId =
(SELECT Id
FROM features
WHERE FeatureUniqeCode = PackageFeatures.FeatureUniqeCode
)
WHERE PackageFeatures.FeatureUniqeCode IN
( SELECT FeatureUniqeCode FROM Features
) ;
UPDATE RoleFeatures
SET FeatureId =
(SELECT Id
FROM features
WHERE FeatureUniqeCode = RoleFeatures.FeatureUniqeCode
)
WHERE RoleFeatures.FeatureUniqeCode IN
( SELECT FeatureUniqeCode FROM Features
) ;
-------------
UPDATE features f
SET (f.ISOLD) = (SELECT temp.IsOld
FROM TempOldFeatures temp
WHERE f.FeatureUniqeCode = temp.FeatureUniqeCode)
WHERE EXISTS (
SELECT 1
FROM TempOldFeatures temp
WHERE f.FeatureUniqeCode = temp.FeatureUniqeCode);
UPDATE TextCodes tc
SET (DefaultText, DefaultTextPlural,SpellCheckDate,SpellCheckedByUserId,LocalDefaultText,IsSpellChecked) = (SELECT
temp.DefaultText,
temp.DefaultTextPlural,
temp.SpellCheckDate,
temp.SpellCheckedByUserId,
temp.LocalDefaultText,
temp.IsSpellChecked
FROM TempIsSpellCheckedTextCodes temp
WHERE tc.CODE = temp.CODE)
WHERE EXISTS (
SELECT 1
FROM TempIsSpellCheckedTextCodes temp
WHERE tc.CODE = temp.CODE);
END;
END;


-- General Script From InsertDBMigrationSettingsData.sxml File
DECLARE
StartTime TIMESTAMP;
EndTime TIMESTAMP;
BEGIN
SAVEPOINT ScriptSavePoint;
StartTime := SYSTIMESTAMP;
BEGIN
INSERT INTO "DBMIGRATIONSETTINGS"("MODE", "MODULESLIST") VALUES('Include', 'Customs,Common,Infrastructure');
END;
EndTime:= SYSTIMESTAMP;
BEGIN
DECLARE ScriptBody NCLOB;
BEGIN
ScriptBody := 'INSERT INTO "DBMIGRATIONSETTINGS"("MODE", "MODULESLIST") VALUES(''Include'', ''Customs,Common,Infrastructure'');';
INSERT INTO "DBSCRIPTSHISTORY"("SXMLFILENAME", "EXECUTIONDATE", "SCRIPTBODY", "ELAPSEDTIMEINMS", "HASHVALUE", "VERSION")VALUES('InsertDBMigrationSettingsData.sxml', SYSDATE, ScriptBody, EXTRACT(DAY FROM(EndTime - StartTime) * 24 * 60 * 60 * 1000), 'a6b9e112ba7fac4e9000996028247e49', 1);
END;
END;
EXCEPTION
WHEN OTHERS THEN
ROLLBACK TO ScriptSavePoint;
COMMIT;
END;

