
if not exists (select * from AWBCustomsInformations where Code = 'DL')
insert into AWBCustomsInformations(Code, Name) values('DL','Dangerous Goods')

if not exists (select * from AWBCustomsInformations where Code = 'SM')
insert into AWBCustomsInformations(Code, Name) values('SM','Screening Method')

if not exists (select * from AWBCustomsInformations where Code = 'SD')
insert into AWBCustomsInformations(Code, Name) values('SD','Security Status Date and Time')

if not exists (select * from AWBCustomsInformations where Code = 'SN')
insert into AWBCustomsInformations(Code, Name) values('SN','Security Status Name of Issuer')

if not exists (select * from AWBCustomsInformations where Code = 'SS')
insert into AWBCustomsInformations(Code, Name) values('SS','Security Status')

if not exists (select * from AWBCustomsInformations where Code = 'ST')
insert into AWBCustomsInformations(Code, Name) values('ST','Security Textual Statement')

if not exists (select * from AWBCustomsInformations where Code = 'ED')
insert into AWBCustomsInformations(Code, Name) values('ED','Expiry Date')

update AWBCustomsInformations set SearchFields = Code + ',' + Name


if not exists (select * from AWBInformations where Code = 'HWB')
insert into AWBInformations(Code, Name) values('HWB','House Waybill')

if not exists (select * from AWBInformations where Code = 'MAL')
insert into AWBInformations(Code, Name) values('MAL','Mail')

if not exists (select * from AWBInformations where Code = 'DCL')
insert into AWBInformations(Code, Name) values('DCL','Declarant')

if not exists (select * from AWBInformations where Code = 'BRK')
insert into AWBInformations(Code, Name) values('BRK','Broker')

if not exists (select * from AWBInformations where Code = 'OSS')
insert into AWBInformations(Code, Name) values('OSS','The Regulated Agent Accepting the Security Status for a Consignment Issued by Another Regulated Agent')

if not exists (select * from AWBInformations where Code = 'ISS')
insert into AWBInformations(Code, Name) values('ISS','The Regulated Agent Issuing the Security Status for a Consignment')

update AWBInformations set SearchFields = Code + ',' + Name
