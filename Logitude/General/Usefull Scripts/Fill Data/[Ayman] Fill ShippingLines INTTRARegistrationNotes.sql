
declare @SCACCodes table
(
  Code varchar(10) not null,
  Name varchar(100) not null,
  RegistrationNotes varchar(250) not null
)

insert into @SCACCodes(Code) values('APLU')
insert into @SCACCodes(Code) values('COSU')
insert into @SCACCodes(Code) values('HDMU')
insert into @SCACCodes(Code) values('KKLU')
insert into @SCACCodes(Code) values('MAEU')
insert into @SCACCodes(Code) values('MCPU')
insert into @SCACCodes(Code) values('MOLU')
insert into @SCACCodes(Code) values('NYKS')
insert into @SCACCodes(Code) values('PABV')
insert into @SCACCodes(Code) values('SAFM')
insert into @SCACCodes(Code) values('SEJJ')
insert into @SCACCodes(Code) values('SEAU')
insert into @SCACCodes(Code) values('ARKU')
insert into @SCACCodes(Code) values('ANRM')
insert into @SCACCodes(Code) values('NGST')
insert into @SCACCodes(Code) values('ANNU')
insert into @SCACCodes(Code) values('ARPZ')
insert into @SCACCodes(Code) values('ACLU')
insert into @SCACCodes(Code) values('EISU')
insert into @SCACCodes(Code) values('CCNR')
insert into @SCACCodes(Code) values('CMDU')
insert into @SCACCodes(Code) values('CNCL')
insert into @SCACCodes(Code) values('CSOY')
insert into @SCACCodes(Code) values('NCLL')
insert into @SCACCodes(Code) values('DAL')
insert into @SCACCodes(Code) values('DAAE')
insert into @SCACCodes(Code) values('DOLQ')
insert into @SCACCodes(Code) values('ECUI')
insert into @SCACCodes(Code) values('EPIR')
insert into @SCACCodes(Code) values('SIIU')
insert into @SCACCodes(Code) values('EGLV')
insert into @SCACCodes(Code) values('GOSU')
insert into @SCACCodes(Code) values('ACL')
insert into @SCACCodes(Code) values('SUDU')
insert into @SCACCodes(Code) values('HLCU')
insert into @SCACCodes(Code) values('HLUS')
insert into @SCACCodes(Code) values('IILU')
insert into @SCACCodes(Code) values('IDMC')
insert into @SCACCodes(Code) values('CLIB')
insert into @SCACCodes(Code) values('CPLB')
insert into @SCACCodes(Code) values('MFUS')
insert into @SCACCodes(Code) values('MSCU')
insert into @SCACCodes(Code) values('NDAL')
insert into @SCACCodes(Code) values('NBLZ')
insert into @SCACCodes(Code) values('PASU')
insert into @SCACCodes(Code) values('31ZZ')
insert into @SCACCodes(Code) values('SBDU')
insert into @SCACCodes(Code) values('SUAU')
insert into @SCACCodes(Code) values('UASU')
insert into @SCACCodes(Code) values('NAOA')
insert into @SCACCodes(Code) values('WECC')
insert into @SCACCodes(Code) values('YMLU')
insert into @SCACCodes(Code) values('ZIMU')



--select Cards.Id, Cards.EnglishName, ShippingLines.SCACCode from
--ShippingLines join Cards on ShippingLines.Id = Cards.Id
--where ShippingLines.Tenant = 0 and ShippingLines.SCACCode in (select Code from @SCACCodes)



--update Cards set CreateDate = '2018-04-12 06:42:12.207', CreatedByUserId = '1-356' where id = '1-1723'


--select * from Cards where id = '1-1723'
--select * from ShippingLines where id = '1-1723'