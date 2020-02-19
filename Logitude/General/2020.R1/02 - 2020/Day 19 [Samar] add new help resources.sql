

update HelpResources set IsNew = 0 

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('REL022', 'February 2020 - Version R1.20', GETDATE(), GETDATE(), 'EN', 'REL', 'OPE', null, null, 'february_2020_release.pdf', 'February 2020 - Version R1.20', 1)




