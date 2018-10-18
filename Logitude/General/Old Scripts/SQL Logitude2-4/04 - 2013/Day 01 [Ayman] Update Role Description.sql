

update TextCodes set DefaultText = 'Bill to VAT is required' where Code = 'AccountingSetting.CH.IsVatNumberMandatoryInAR'
go

update TextCodes set DefaultText = 'Bill to VAT is required' where Code = 'AccountingSetting.F.IsVatNumberMandatoryInAR'
go

update Roles set Description = 'All system feauters' where Code = 'ADMN'
go

update Roles set Description = 'All feauters except system setup' where Code = 'MANG'
go

update Roles set Description = 'Full Operation include A/R Invoicing .' + CHAR(13) + 'No Accounting, system setup and Manager Dashboards .' where Code = 'FROP'
go

update Roles set Description = 'Quotes Management and Operational view' where Code = 'SALE'
go

update Roles set Description = 'Full accounting include Payables and Payments .' + CHAR(13) + 'No operational features, system setup and Manager Dashboards .' where Code = 'ACCT'
go