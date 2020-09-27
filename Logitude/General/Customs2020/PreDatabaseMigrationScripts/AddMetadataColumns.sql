
--------------------------------------------------------------Run automation alter table before delete 
alter table objectfields add FieldCode varchar2(1000) null;

alter table CustomsRequiredFields add ObjectfieldCode varchar2(1000) null;
--MetaData All Scripts: Never Apply these scripts
alter table Features add FeatureUniqeCode varchar2(1000) null;

--*--Before Delete--*--
--ObjectFields
alter table querycolumns add ObjectFieldCode varchar2(1000) null;
alter table ScreenFields add ObjectFieldCode varchar2(1000) null;
alter table AdvancedQueryFilters add ObjectFieldCode varchar2(1000) null;
alter table RuleConditionFields add ObjectFieldCode varchar2(1000) null;
alter table ObjectTableRuleFields add ObjectFieldCode varchar2(1000) null;
alter table ObjectTableRules add TriggerFieldCode varchar2(1000) null;
alter table AirlineMessagingRules add RuleFieldCode varchar2(1000) null;
alter table restrictions  add ObjectFieldCode varchar2(1000) null;
alter table CustomerFieldsalter tableaddtings  add ObjectFieldCode varchar2(1000) null;
alter table ObjectFieldValidations  add ObjectFieldCode varchar2(1000) null;
alter table ObjectFieldModifications  add ObjectFieldCode varchar2(1000) null;

--Screens
alter table ScreenModifications add ScreenCode varchar2(1000) null;
alter table ScreenFields add ScreenCode varchar2(1000) null;
alter table ObjectTables add HeaderScreenCode varchar2(1000) null;

--Queries
alter table Queries add UniqueCode varchar2(1000) null;

alter table Queries add OriginalQueryCode varchar2(1000) null;
alter table AdvancedQueryFilters add QueryCode varchar2(1000) null;
alter table QueryColumns add QueryCode varchar2(1000) null;
alter table SharedUserQueries add QueryCode varchar2(1000) null;

--TextCodes
alter table Queries add NameTextCodeCode varchar2(1000) null;
alter table ObjectTables add DescriptionTextCodeCode varchar2(1000) null;
alter table ObjectTables add NewButtonTextCodeCode varchar2(1000) null;
alter table Features add NameTextCodeCode varchar2(1000) null;
alter table Tips add ShortTextCodeCode varchar2(1000) null;
alter table ObjectTableTabs add TabNameTextCodeCode varchar2(1000) null;
alter table MenuButtons add LabelTextCodeCode varchar2(1000) null;
alter table Translations add TextCodeCode varchar2(1000) null;
alter table ObjectFields add FullNameTextCodeCode varchar2(1000) null;
alter table ObjectFields add HelpTextCodeCode varchar2(1000) null;
alter table ObjectFields add ShortNameTextCodeCode varchar2(1000) null;
alter table ObjectFields add ListTextCodeCode varchar2(1000) null;

--Features
alter table MenusTables add FeatureUniqeCode varchar2(1000) null;
alter table Queries add FeatureUniqeCode varchar2(1000) null;
alter table PackageFeatures add FeatureUniqeCode varchar2(1000) null;
alter table RoleFeatures add FeatureUniqeCode varchar2(1000) null;
alter table Reports add FeatureUniqeCode varchar2(1000) null;
alter table ObjectTableTabs add FeatureUniqeCode varchar2(1000) null;
alter table ObjectTableHelperControls add FeatureUniqeCode varchar2(1000) null;
alter table MenuButtons add FeatureUniqeCode varchar2(1000) null;


alter table MenusTables add FeatureUniqeCode varchar2(1000) null;
alter table MenuButtons add FeatureUniqeCode varchar2(1000) null;
alter table Queries add FeatureUniqeCode varchar2(1000) null;
alter table ObjectTableTabs add FeatureUniqeCode varchar2(1000) null;
alter table ObjectTableHelperControls add FeatureUniqeCode varchar2(1000) null;
alter table PackageFeatures add FeatureUniqeCode varchar2(1000) null;
alter table RoleFeatures add FeatureUniqeCode varchar2(1000) null;
alter table Reports add FeatureUniqeCode varchar2(1000) null;
