--Run on Main

  BEGIN TRY  
  ALTER TABLE APPayments DROP CONSTRAINT [FK_dbo.APPayments_dbo.APPaymentMethods_PaymentMethodId]
END TRY  
BEGIN CATCH  
  print('Not Exist')
END CATCH  

	