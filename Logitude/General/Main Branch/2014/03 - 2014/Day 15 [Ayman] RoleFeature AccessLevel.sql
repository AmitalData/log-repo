
-- Run this SQL after Updating Database
-- The Table FeatureAccessLevels must be created first (by update)

IF OBJECT_ID ('FeatureAccessLevels', 'U') is not null
begin

	if not exists (select * from FeatureAccessLevels where Code = 'NO')
	begin
	insert into FeatureAccessLevels(Code, Name, SearchFields)
	values ('NO','None','NO,None')
	end

	if not exists (select * from FeatureAccessLevels where Code = 'US')
	begin
	insert into FeatureAccessLevels(Code, Name, SearchFields)
	values ('US','User','US,User')
	end

	if not exists (select * from FeatureAccessLevels where Code = 'BU')
	begin
	insert into FeatureAccessLevels(Code, Name, SearchFields)
	values ('BU','Business Unit','BU,Business Unit')
	end

	if not exists (select * from FeatureAccessLevels where Code = 'PR')
	begin
	insert into FeatureAccessLevels(Code, Name, SearchFields)
	values ('PR','Parent','PR,Parent')
	end

	if not exists (select * from FeatureAccessLevels where Code = 'OR')
	begin
	insert into FeatureAccessLevels(Code, Name, SearchFields)
	values ('OR','Organization','OR,Organization')
	end


	IF COLUMNPROPERTY( OBJECT_ID(N'RoleFeatures'), 'FeatureAccessLevelCode', 'ColumnId') IS NULL
	begin

		ALTER TABLE RoleFeatures ADD FeatureAccessLevelCode varchar(2) not null CONSTRAINT DF_RoleFeatures_Fixed_Name default 'OR';

		ALTER TABLE RoleFeatures ADD CONSTRAINT FK_RoleFeatureAccessLevel
			FOREIGN KEY (FeatureAccessLevelCode)
			REFERENCES FeatureAccessLevels(Code)
			ON DELETE NO ACTION ON UPDATE NO ACTION;

		CREATE INDEX [IX_FK_RoleFeatureAccessLevel] ON RoleFeatures (FeatureAccessLevelCode);

		ALTER TABLE RoleFeatures DROP DF_RoleFeatures_Fixed_Name
	end
end

