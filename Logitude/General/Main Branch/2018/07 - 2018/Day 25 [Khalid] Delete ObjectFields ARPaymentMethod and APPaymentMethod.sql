delete from QueryColumns where ObjectFieldId In ( select Id  from ObjectFields where FieldName In('PaymentMethodId','PaymentMethodName','ARPaymentMethodId','APPaymentMethodId') and ObjectTableId In ( Select Id from ObjectTables where Name In ('ARPayment','APPayment')) )
delete from ObjectFields where FieldName In('PaymentMethodId','PaymentMethodName','ARPaymentMethodId','APPaymentMethodId') and ObjectTableId In ( Select Id from ObjectTables where Name In ('ARPayment','APPayment')) 

delete from TextCodes where code in ('APPayment.CH.PaymentMethodNameListLable','APPayment.F.PaymentMethodName','APPayment.PaymentMethodNameHelpText', 
'ARPayment.CH.PaymentMethodNameListLable', 'ARPayment.F.PaymentMethodName' , 'ARPayment.PaymentMethodNameHelpText')

select * from ObjectFields where FieldName In('PaymentMethodId','PaymentMethodCode','ARPaymentMethodId','APPaymentMethodId')  and  ObjectTableId In ( Select Id from ObjectTables where Name In ('ARPayment','APPayment')) 