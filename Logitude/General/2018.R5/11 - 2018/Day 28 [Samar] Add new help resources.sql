

update HelpResources set IsNew = 0 

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('REL016', 'December 2018 - Version R5.18', GETDATE(), GETDATE(), 'EN', 'REL', 'OPE', null, null, 'december_2018_release.pdf', 'December 2018 - Version R5.18', 1)

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('68', 'Accounting Settings User''s Guide', GETDATE(), GETDATE(), 'EN', 'TUT', 'ACC', null, null, 'accounting_setting_guide.pdf', 'Accounting Settings User''s Guide', 1)

update HelpResources set IsNew = 1 where Code = '67'