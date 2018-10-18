
update HelpResources set IsNew = 0 where Code = 'REL001'

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('REL002', 'February 2016 - Version R1.16', GETDATE(), GETDATE(), 'EN', 'REL', 'OPE', null, null, 'february_2016_release.pdf', 'February 2016 - Version R1.16', 1)

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('25', 'Airline Account Number', GETDATE(), GETDATE(), 'EN', 'HOW', 'AWB', null, null, 'airline_account_number.pdf', 'Airline Account Number', 1)

update HelpResources set UpdateDate = '2016-02-28' 
where Code = '25' or Code = 'REL002'