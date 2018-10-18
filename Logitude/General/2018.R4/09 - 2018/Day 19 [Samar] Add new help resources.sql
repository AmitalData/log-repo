
update HelpResources set IsNew = 0 

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('REL015', 'September 2018 - Version R4.18', GETDATE(), GETDATE(), 'EN', 'REL', 'OPE', null, null, 'september_2018_release.pdf', 'September 2018 - Version R4.18', 1)

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('67', 'Documents Filing Inbox', GETDATE(), GETDATE(), 'EN', 'TUT', 'OPE', null, null, 'documents_filing_inbox.pdf', 'Documents Filing Inbox', 1)