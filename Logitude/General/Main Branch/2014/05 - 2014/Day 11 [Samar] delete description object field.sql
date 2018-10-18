delete from ObjectFields where FieldName = 'Description' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity')
delete from TextCodes where Code = 'Opportunity.F.Description'
delete from TextCodes where Code = 'Opportunity.DescriptionHelpText'
delete from TextCodes where Code = 'Opportunity.CH.DescriptionListLable'