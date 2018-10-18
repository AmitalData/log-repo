
delete from ObjectTableTabs where TabNameTextCodeId = (select Id from TextCodes where Code = 'Customer.TH.Potentials')
delete from TextCodes where Code = 'Customer.TH.Potentials'