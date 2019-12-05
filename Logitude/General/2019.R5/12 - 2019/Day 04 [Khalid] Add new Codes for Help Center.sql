update HelpResources set IsNew = 0 

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('REL021', 'December 2019 - Version R5.19', GETDATE(), GETDATE(), 'EN', 'REL', 'OPE', null, null, 'december_2019_release.pdf', 'December 2019 - Version R5.19', 1)

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('73', 'Shared Logistics White Label Guide', GETDATE(), GETDATE(), 'EN', 'TUT', 'OPE', null, null, 'shared_logistics_white_label_guide.pdf', 'Shared Logistics White Label Guide', 1)

