
update HelpResources set IsNew = 0 

update HelpResources set IsNew = 1, UpdateDate = GETDATE() where Code = '53'
update HelpResources set IsNew = 1, UpdateDate = GETDATE() where Code = '54'

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('REL013', 'May 2018 - Version R2.18', GETDATE(), GETDATE(), 'EN', 'REL', 'OPE', null, null, 'may_2018_release.pdf', 'May 2018 - Version R2.18', 1)

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('60', 'How to Build Master Packages?', GETDATE(), GETDATE(), 'EN', 'HOW', 'OPE', null, null, 'build_master_packages.pdf', 'How to Build Master Packages?', 1)

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('61', 'Managing Freight and Customs Shipments', GETDATE(), GETDATE(), 'EN', 'TUT', 'OPE', null, null, 'manage_customs_shipments.pdf', 'Managing Freight and Customs Shipments', 1)