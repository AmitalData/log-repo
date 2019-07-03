

update HelpResources set IsNew = 0 

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('REL019', 'July 2019 - Version R3.19', GETDATE(), GETDATE(), 'EN', 'REL', 'OPE', null, null, 'july_2019_release.pdf', 'July 2019 - Version R3.19', 1)

update HelpResources set IsNew = 1, UpdateDate = GETDATE() where Code = '12'

