--select Code , SIGN from Currencies order by Code

-- USAGE: Update Common Currencies Sign

update Currencies set Sign = Code where Sign is null or Sign = ''
update Currencies set Sign = N'€' where Code = 'EUR'
update Currencies set Sign = N'$' where Code = 'USD'
update Currencies set Sign = N'₪' where Code = 'NIS'
update Currencies set Sign = N'¥' where Code = 'YEN'
update Currencies set Sign = N'د.إ' where Code = 'AED'
update Currencies set Sign = N'؋' where Code = 'AFN'
update Currencies set Sign = N'ALL' where Code = 'ALL'
update Currencies set Sign = N'$' where Code = 'AUD'
update Currencies set Sign = N'$' where Code = 'ARS'
update Currencies set Sign = N'KM' where Code = 'BAM'
update Currencies set Sign = N'$' where Code = 'BBD'
update Currencies set Sign = N'₪' where Code = 'ILS'
update Currencies set Sign = N'JD' where Code = 'JOD'
update Currencies set Sign = N'﷼' where Code = 'QAR'
update Currencies set Sign = N'﷼' where Code = 'SAR'
update Currencies set Sign = N'﷼' where Code = 'YER'
