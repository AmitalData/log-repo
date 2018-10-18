
delete from querycolumns
where objectfieldid = (select id from objectfields 
where fieldname = 'remark' and objecttableid = (SELECT id FROM objecttables WHERE name = 'Currency'))

delete from objectfields 
where fieldname = 'remark' and objecttableid = (SELECT id FROM objecttables WHERE name = 'Currency')

delete from TextCodes where Code = 'currency.f.remark'

delete from TextCodes where Code = 'Currency.CH.RemarkListLable'

--update objectfields 
--set fieldname = 'Notes'
--where fieldname = 'remark' and objecttableid = (SELECT id FROM objecttables WHERE name = 'Currency')

--update TextCodes
--set Code = 'currency.f.notes'
--where Code = 'currency.f.remark'