 Update CommunicationLogTypes
set SearchFields = 
isnull(Code,'') + ',' +
isnull(name,'') + ','

Update CommunicationStatusTypes
set SearchFields = 
isnull(Code,'') + ',' +
isnull(name,'') + ','

Update PasswordPolicies
set SearchFields = 
isnull(Code,'') + ',' +
isnull(PasswordStrength,'') + ','

 Update ObjectTables
set SearchFields = 
isnull(objtbls.DBTableName,'') + ',' +
 isnull(objtbls.name,'') + ',' +
  isnull(objtbls.KeyPropertyPath,'') + ',' +
   --isnull(objfields.FieldName,'') + ',' +
isnull(objtbls.NewWizardControlName,'') + ','
  FROM ObjectTables as objtbls,ObjectFields as objfields