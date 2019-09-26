update HelpResources set IsNew = 0 

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('REL020', 'September 2019 - Version R4.19', GETDATE(), GETDATE(), 'EN', 'REL', 'OPE', null, null, 'september_2019_release.pdf', 'September 2019 - Version R4.19', 1)

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('70', 'INTTRA Setup', GETDATE(), GETDATE(), 'EN', 'TUT', 'OPE', null, null, 'inttra_setup_protection.pdf', 'INTTRA Setup', 1)
insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('71', 'INTTRA Quick Tour', GETDATE(), GETDATE(), 'EN', 'TUT', 'OPE', null, null, 'inttra_quicktour.pdf', 'INTTRA Quick Tour', 1)
insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('72', 'CASS Report Implementation', GETDATE(), GETDATE(), 'EN', 'TUT', 'OPE', null, null, 'cass_report_implementation.pdf', 'CASS Report Implementation', 1)
