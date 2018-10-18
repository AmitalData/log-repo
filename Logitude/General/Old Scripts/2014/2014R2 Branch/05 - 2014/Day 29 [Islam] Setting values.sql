
-- execute on global

insert into Settings(Id,LogitudeURL,ChampURL,DeploymentStage,ChampEnv,CustomerCareIP,TotangoServiceId,UsingAzure,IsLogEnabled,storageaccountname,storageaccountkey,storagetype)
values('1','','http://54.200.22.12:80','Dev','TEST','82.213.2.230,213.6.5.182,213.6.5.176,192.116.221.91','SP-11460-01',0,0,'devstoreaccount1','Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==','azureemulator')


select * from Settings

-- storagetype values = azureemulator, azure