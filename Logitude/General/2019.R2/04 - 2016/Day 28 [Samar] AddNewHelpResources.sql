
update HelpResources set IsNew = 0 

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('REL018', 'May 2019 - Version R2.19', GETDATE(), GETDATE(), 'EN', 'REL', 'OPE', null, null, 'may_2019_release.pdf', 'May 2019 - Version R2.19', 1)

