
update DocumentTypes set IsEnabledForCustomers = 'True' , IsCopiedAtSignup= 'True'  where Tenant =0;
update DocumentTypeTemplates set IsEnabledForCustomers = 'True' , IsCopiedAtSignup= 'True'  where Tenant =0;






