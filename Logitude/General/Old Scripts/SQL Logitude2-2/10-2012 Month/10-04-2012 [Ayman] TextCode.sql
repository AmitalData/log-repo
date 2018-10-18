
--select * from TextCodes where Code = 'Quote.F.IsDangerous' and ObjectTableId = (Select Id from ObjectTables where Name = 'Quote')
update TextCodes set DefaultText = 'Dangerous Goods' where Code = 'Quote.F.IsDangerous' and ObjectTableId = (Select Id from ObjectTables where Name = 'Quote')

-- BY Jalal
update textcodes
set IsSpellChecked = 0
where defaulttext like '%Add Charge for Print only%'