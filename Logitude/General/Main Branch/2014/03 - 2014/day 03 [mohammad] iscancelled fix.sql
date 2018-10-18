update objectfields set fieldname ='IsCancelled' where FieldName= 'isCancelled' and ObjectTableId = (select id from ObjectTables where name = 'GroupMember')



update textcodes set code='GroupMember.F.IsCancelled'
where code ='GroupMember.F.isCancelled'
go

update textcodes set code='GroupMember.IsCancelledHelpText'
where code ='GroupMember.IsCancelledHelpText'
go

update textcodes set code='GroupMember.CH.IsCancelledListLable'
where code ='GroupMember.CH.IsCancelledListLable'
go
