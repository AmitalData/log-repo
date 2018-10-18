alter table communicationLogs add [EntityReference] varchar(15) null
alter table communicationLogs add [SearchFields] nvarchar(1000) null

alter table communicationLogTypes add [SearchFields] nvarchar(1000) null

alter table communicationStatusTypes add [SearchFields] nvarchar(1000) null

alter table passwordPolicies add [SearchFields] nvarchar(1000) null