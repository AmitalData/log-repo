-- run later
delete from QueryColumns
delete from ScreenFields

delete from objectfields where FieldName = 'Remark' and ObjectTableId = (select Id from ObjectTables where Name = 'Contact')
delete from TextCodes where Code = 'Contact.RemarkHelpText'
delete from TextCodes where Code = 'Contact.CH.RemarkListLable'
delete from TextCodes where Code = 'Contact.F.Remark'

-- then update !!