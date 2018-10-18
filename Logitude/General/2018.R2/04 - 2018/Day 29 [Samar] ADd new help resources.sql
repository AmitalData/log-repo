
update HelpResources set SearchFields = Name
update HelpResources set Name = 'SAT Profact Integration_Invoices', SearchFields = 'SAT Profact Integration_Invoices' where Code = '53'
update HelpResources set Name = 'SAT Profact Integration_Invoices (Spanish)', SearchFields = 'SAT Profact Integration_Invoices (Spanish)' where Code = '54'

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('62', 'SAT Profact Integration_Payments', GETDATE(), GETDATE(), 'EN', 'TUT', 'ACC', null, null, 'sat_profact_payment.pdf', 'SAT Profact Integration_Payments', 1)

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('63', 'SAT Profact Integration_Payments (Spanish)', GETDATE(), GETDATE(), 'SP', 'TUT', 'ACC', null, null, 'sat_profact_payment_spanish.pdf', 'SAT Profact Integration_Payments (Spanish)', 1)