--Apply in global
update TenantManagements set BluesnapContractQTY=NumberOfUsers where NumberOfUsers is not null and (BluesnapContractQTY=0 OR BluesnapContractQTY is null)
update TenantManagements set BluesnapCRMContractQTY=1 where (BluesnapCRMContractQTY = 0 OR BluesnapCRMContractQTY is null)
update TenantManagements set BluesnapEAWBContractQTY=1 where (BluesnapEAWBContractQTY = 0 OR BluesnapEAWBContractQTY is null)
update TenantManagements set BluesnapEAWBSContractQTY=1 where (BluesnapEAWBSContractQTY = 0 OR BluesnapEAWBSContractQTY is null)
update TenantManagements set BluesnapOneTimeContractQTY=1 where (BluesnapOneTimeContractQTY = 0 OR BluesnapOneTimeContractQTY is null)
