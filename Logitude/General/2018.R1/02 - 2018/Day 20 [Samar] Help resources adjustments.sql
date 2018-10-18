
update HelpResources set IsNew = 0 

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('REL012', 'February 2018 - Version R1.18', GETDATE(), GETDATE(), 'EN', 'REL', 'OPE', null, null, 'february_2018_release.pdf', 'February 2018 - Version R1.18', 1)

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('58', 'How to Print Rate Confirmation?', GETDATE(), GETDATE(), 'EN', 'HOW', 'OPE', null, null, 'print_rate_confirmation.pdf', 'How to Print Rate Confirmation?', 1)

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('59', 'Containers Follow up', GETDATE(), GETDATE(), 'EN', 'TUT', 'OPE', null, null, 'containers_followup.pdf', 'Containers Follow up', 1)

update HelpResources set [Type] = 'HOW' where Code = '56'
update HelpResources set IsNew = 1, UpdateDate = GETDATE()  where Code = '52'