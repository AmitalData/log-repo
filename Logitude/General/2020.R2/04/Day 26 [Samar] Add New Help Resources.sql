

update HelpResources set IsNew = 0 

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('REL023', 'May 2020 - Version R2.20', GETDATE(), GETDATE(), 'EN', 'REL', 'OPE', null, null, 'may_2020_release.pdf', 'May 2020 - Version R2.20', 1)

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('76', 'Warehouse Invoicing', GETDATE(), GETDATE(), 'EN', 'HOW', 'ACC', null, null, 'warehouse_invoicing.pdf', 'Warehouse Invoicing', 1)

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('77', 'Regional Tax', GETDATE(), GETDATE(), 'EN', 'TUT', 'ACC', null, null, 'regional_tax.pdf', 'Regional Tax', 1)

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('78', 'INTTRA E-booking', GETDATE(), GETDATE(), 'EN', 'TUT', 'OPE', null, null, 'inttra_e_booking.pdf', 'INTTRA E-booking', 1)

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('79', 'Quotation Module', GETDATE(), GETDATE(), 'EN', 'TUT', 'OPE', null, null, 'quotation_module.pdf', 'Quotation Module', 1)

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('80', 'Cross Docks Quick Tour', GETDATE(), GETDATE(), 'EN', 'TUT', 'OPE', null, null, 'cross_docks_quick_tour.pdf', 'Cross Docks Quick Tour', 1)

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('81', 'BI Reports Guide', GETDATE(), GETDATE(), 'EN', 'TUT', 'OPE', null, null, 'bi_reports_guide.pdf', 'BI Reports Guide', 1)

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('82', 'Private DB Quick Tour', GETDATE(), GETDATE(), 'EN', 'TUT', 'OPE', null, null, 'private_db_quick_tour.pdf', 'Private DB Quick Tour', 1)
