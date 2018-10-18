 Update Customs.CustomBanks
set SearchFields = 
isnull(AccountNumber,'') + ',' +
 isnull(BankCode,'') + ',' +
 isnull(BranchCode,'') + ',' + 
  isnull(InternalCode,'') + ',' + 
   isnull(LocalName,'') + ',' + 
isnull(EnglishName,'') + ',' +
isnull(BankAddress,'') + ',' 
