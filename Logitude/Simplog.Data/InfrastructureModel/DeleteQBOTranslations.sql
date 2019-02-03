
IF OBJECT_ID('[dbo].[usp_DeleteQBOTranslations]', 'P') IS NOT NULL
drop PROCEDURE [dbo].usp_DeleteQBOTranslations
GO

Create PROCEDURE [dbo].usp_DeleteQBOTranslations
(
	@Tenant int
)
AS

BEGIN

update ChargesTypes set ReceivablesChargesTypeExternalCode = NULL,PayablesChargesTypeExternalCode = NULL where Tenant = @Tenant
update vattypes set ExternalVATCard = NULL where Tenant = @Tenant
update Currencies set AccountingExternalCode = NULL where Tenant = @Tenant
update PaymentTerms set ExternalId = NULL where Tenant = @Tenant
update AccountingPaymentMethods set ARExternalId = NULL,APExternalId=NULL where Tenant = @Tenant
update CardExternalCodeByCurrencies set ExternalPayableTableId = NULL,ExternalRecievableTableId=NULL where Tenant = @Tenant
END