
update HelpResources set IsNew = 0 

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('REL014', 'July 2018 - Version R3.18', GETDATE(), GETDATE(), 'EN', 'REL', 'OPE', null, null, 'july_2018_release.pdf', 'July 2018 - Version R3.18', 1)

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('64', 'How to Clear Logitude Cache?', GETDATE(), GETDATE(), 'EN', 'HOW', 'OPE', null, null, 'clear_logitude_cache.pdf', 'How to Clear Logitude Cache?', 1)

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('65', 'Users and Contacts Data Protection', GETDATE(), GETDATE(), 'EN', 'TUT', 'OPE', null, null, 'users_contacts_protection.pdf', 'Users and Contacts Data Protection', 1)

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('66', 'GCCA New FWB/ FHL Recommendations', GETDATE(), GETDATE(), 'EN', 'TUT', 'AWB', null, null, 'fwb_fhl_recommendationsn.pdf', 'GCCA New FWB/ FHL Recommendations', 1)
