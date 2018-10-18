update TenantManagements 
set PackageCode = 'BUSN'

UPDATE [Logitude2-2_Global].dbo.TenantManagements 
SET [Logitude2-2_Global].dbo.TenantManagements.PackageCode = [Logitude2-2_Main].dbo.tenants.PackageCode 
FROM [Logitude2-2_Global].dbo.TenantManagements INNER JOIN [Logitude2-2_Main].dbo.tenants ON [Logitude2-2_Global].dbo.TenantManagements.Id = [Logitude2-2_Main].dbo.Tenants.Id


UPDATE [Logitude2-2_Global].dbo.GlobalTenants 
SET [Logitude2-2_Global].dbo.GlobalTenants.[Version] = [Logitude2-2_Main].dbo.tenants.[Version] 
FROM [Logitude2-2_Global].dbo.GlobalTenants INNER JOIN [Logitude2-2_Main].dbo.tenants ON [Logitude2-2_Global].dbo.GlobalTenants.Id = [Logitude2-2_Main].dbo.Tenants.Id

UPDATE [Logitude2-2_Global].dbo.GlobalTenants 
SET [Logitude2-2_Global].dbo.GlobalTenants.IsActive = [Logitude2-2_Main].dbo.tenants.IsActive 
FROM [Logitude2-2_Global].dbo.GlobalTenants INNER JOIN [Logitude2-2_Main].dbo.tenants ON [Logitude2-2_Global].dbo.GlobalTenants.Id = [Logitude2-2_Main].dbo.Tenants.Id
