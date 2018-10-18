update Translations set UpdateDateGMT = SYSUTCDATETIME ()
update ObjectFieldModifications set UpdateDateGMT = SYSUTCDATETIME ()

-- on global
insert into SystemMetadataLastUpdates(id,ObjectFieldsUpdateDateGMT,TranslationsUpdateDateGMT) values(1,SYSUTCDATETIME (),SYSUTCDATETIME ())
