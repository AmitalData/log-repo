--
-- remove MTC items: Journal , GLAccount , JournalActionTypes
--
delete from MenusTables where ObjectTableId = (select Id from ObjectTables where Name='Journal') and Code = 'MTJN'
delete from MenusTables where ObjectTableId = (select Id from ObjectTables where Name='GLAccount') and Code = 'MTGA'
delete from MenusTables where ObjectTableId = (select Id from ObjectTables where Name='GLAccount') and Code = 'MTGC'
delete from MenusTables where ObjectTableId = (select Id from ObjectTables where Name='GLAccount') and Code = 'MTGV'
delete from MenusTables where ObjectTableId = (select Id from ObjectTables where Name='Journalactiontype') and Code = 'MTJA'

delete from RoleFeatures where FeatureId = (select id from Features where Code = 'JOURNALMENU')
delete from RoleFeatures where FeatureId = (select id from Features where Code = 'JOURNALACTIONTYPESMENU')
delete from PackageFeatures where FeatureId = (select id from Features where Code = 'JOURNALMENU')
delete from PackageFeatures where FeatureId = (select id from Features where Code = 'JOURNALACTIONTYPESMENU')

delete from Features where Code = 'JOURNALMENU'
delete from Features where Code = 'GLACCOUNTSMENU'
delete from Features where Code = 'CLIENTGLACCOUNTSMENU'
delete from Features where Code = 'VENDORGLACCOUNTSMENU'
delete from Features where Code = 'JOURNALACTIONTYPESMENU'