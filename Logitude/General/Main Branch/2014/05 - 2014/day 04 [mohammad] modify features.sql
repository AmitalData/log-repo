
delete from RoleFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='AWBChargesCode'))
delete from PackageFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='AWBChargesCode'))
delete from Features  where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='AWBChargesCode')
update ObjectTables set EnableSecurity=0 where name='AWBChargesCode'

delete from RoleFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='AWBSpecialHandlingCode'))
delete from PackageFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='AWBSpecialHandlingCode'))
delete from Features  where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='AWBSpecialHandlingCode')
update ObjectTables set EnableSecurity=0 where name='AWBSpecialHandlingCode'


delete from RoleFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='DocumentTypeTemplate'))
delete from PackageFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='DocumentTypeTemplate'))
delete from Features  where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='DocumentTypeTemplate')
update ObjectTables set EnableSecurity=0 where name='DocumentTypeTemplate'

delete from RoleFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='Feed'))
delete from PackageFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='Feed'))
delete from Features  where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='Feed')
update ObjectTables set EnableSecurity=0 where name='Feed'

delete from RoleFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='FollowEntity'))
delete from PackageFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='FollowEntity'))
delete from Features  where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='FollowEntity')
update ObjectTables set EnableSecurity=0 where name='FollowEntity'


delete from RoleFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='Follower'))
delete from PackageFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='Follower'))
delete from Features  where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='Follower')
update ObjectTables set EnableSecurity=0 where name='Follower'

delete from RoleFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='Group'))
delete from PackageFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='Group'))
delete from Features  where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='Group')
update ObjectTables set EnableSecurity=0 where name='Group'


delete from RoleFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='GroupMember'))
delete from PackageFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='GroupMember'))
delete from Features  where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='GroupMember')
update ObjectTables set EnableSecurity=0 where name='GroupMember'

delete from RoleFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='post'))
delete from PackageFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='post'))
delete from Features  where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='post')
update ObjectTables set EnableSecurity=0 where name='post'


delete from RoleFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='IATACode'))
delete from PackageFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='IATACode'))
delete from Features  where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='IATACode')
update ObjectTables set EnableSecurity=0 where name='IATACode'

delete from RoleFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='QuoteTemplate'))
delete from PackageFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='QuoteTemplate'))
delete from Features  where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='QuoteTemplate')
update ObjectTables set EnableSecurity=0 where name='QuoteTemplate'

delete from RoleFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='QuoteTemplateSection'))
delete from PackageFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='QuoteTemplateSection'))
delete from Features  where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='QuoteTemplateSection')
update ObjectTables set EnableSecurity=0 where name='QuoteTemplateSection'

delete from RoleFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='QuoteTemplateSetting'))
delete from PackageFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='QuoteTemplateSetting'))
delete from Features  where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='QuoteTemplateSetting')
update ObjectTables set EnableSecurity=0 where name='QuoteTemplateSetting'


delete from RoleFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='QuoteTemplateTableDesign'))
delete from PackageFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='QuoteTemplateTableDesign'))
delete from Features  where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='QuoteTemplateTableDesign')
update ObjectTables set EnableSecurity=0 where name='QuoteTemplateTableDesign'

delete from RoleFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='QuoteTemplateTextCode'))
delete from PackageFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='QuoteTemplateTextCode'))
delete from Features  where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='QuoteTemplateTextCode')
update ObjectTables set EnableSecurity=0 where name='QuoteTemplateTextCode'

delete from RoleFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='QuoteTemplateTextDesign'))
delete from PackageFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='QuoteTemplateTextDesign'))
delete from Features  where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='QuoteTemplateTextDesign')
update ObjectTables set EnableSecurity=0 where name='QuoteTemplateTextDesign'

delete from RoleFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='Tenant'))
delete from PackageFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='Tenant'))
delete from Features  where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='Tenant')
update ObjectTables set EnableSecurity=0 where name='Tenant'

delete from RoleFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='AWBOCI'))
delete from PackageFeatures where FeatureId in (select id from Features where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='AWBOCI'))
delete from Features  where (code='Read' or code ='new' or code='update') and ObjectTableId = (select Id from objecttables where Name='AWBOCI')
update ObjectTables set EnableSecurity=0 where name='AWBOCI'


