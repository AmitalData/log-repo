alter table Cards drop Constraint UQ_Tenant_Code_PartnerTypeId_Cards
go

alter table Cards add Constraint Tenant_Code_PartnerTypeId_Cards unique(Tenant, Code, PartnerTypeId)
go

