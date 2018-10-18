
update HelpResources set Category = 'ACC' where Code = '52'
update HelpResources set Category = 'ACC' where Code = '53'

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('54', 'SAT Profact Connection (Spanish)', GETDATE(), GETDATE(), 'SP', 'TUT', 'ACC', null, null, 'sat_profact_connection_spanish.pdf', 'SAT Profact Connection  (Spanish)', 1)
