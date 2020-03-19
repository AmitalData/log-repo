
delete from screenfields where objectfieldid = (select id  from objectfields where FieldName = 'BankAccountId' and objecttableId = (select id from objecttables  where name = 'ReconcileExternalPage') ) 
delete from objectfields where FieldName = 'BankAccountId' and objecttableId = (select id from objecttables  where name = 'ReconcileExternalPage')
delete from textcodes where code = 'ReconcileExternalPage.F.BankAccountId'
delete from textcodes where code = 'ReconcileExternalPage.CH.BankAccountIdListLable'

