update ObjectTables set NameField='EnglishName' where Name IN('port','Branch','Card','User','country', 'customer', 'agent', 'Currency', 'PaymentTerm', 'VATType', 'ChargesType','Packagetype', 'Vessel')
update ObjectTables set NameField='Name' where Name IN('Incoterm', 'CreditCardType','Measurement', 'ChargesGroup', 'WeightUnit', 'VolumeUnit')
	