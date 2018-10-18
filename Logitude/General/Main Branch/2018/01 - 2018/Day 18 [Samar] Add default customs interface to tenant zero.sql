

if not exists (select Tenant from CustomsInterfaceSettings where Tenant = 0 )
begin 		           
	insert into CustomsInterfaceSettings(Tenant, LocalCustomsInterfaceCode, ImportToUSAInterfaceCode, ExportFromUSAInterfaceCode, LocalCompanyId, LocalPassword, LocalUserId, ActivateCustomsManagementInShipments, ArtemusInSettingsId, ArtemusOutSettingsId)
	values(0, 'NO', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL)                            
end 