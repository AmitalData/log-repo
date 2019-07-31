
update HelpResources set IsNew = 0 
update HelpResources set IsNew = 1 where Code = '11'

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('REL017', 'February 2019 - Version R1.19', GETDATE(), GETDATE(), 'EN', 'REL', 'OPE', null, null, 'february_2019_release.pdf', 'February 2019 - Version R1.19', 1)

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('69', 'Generic Payment Interface', GETDATE(), GETDATE(), 'EN', 'TUT', 'ACC', null, null, 'generic_payment_interface.pdf', 'Generic Payment Interface', 1)
