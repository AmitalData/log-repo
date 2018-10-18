update Customs.CustomsVendors  
set SearchFields  = 
isnull( VendorNumber ,'') + ',' +
 isnull(VendorName ,'') + ','+
  isnull(DunsNumber ,'') + ','+
   isnull(VATNumber ,'') + ','