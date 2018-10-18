

BEGIN;
INSERT INTO  DocumentsDataProviders VALUES ('AWDP','AWBDataProvider');
INSERT INTO  DocumentsDataProviders VALUES ('FBDP','FBLDataProvider');
INSERT INTO  DocumentsDataProviders VALUES ('SDDP','ShippingDeclarationDataProvider');
INSERT INTO  DocumentsDataProviders VALUES ('ALDP','AWBLabelsDataProvider');
INSERT INTO  DocumentsDataProviders VALUES ('DNDP','DeliveryNoteDataProvider');
INSERT INTO  DocumentsDataProviders VALUES ('INDP','InvoiceDataProvider');

INSERT INTO  DocumentsDataProviders VALUES ('CMDP','CMRDataProvider');
INSERT INTO  DocumentsDataProviders VALUES ('MADP','ManifestDataProvider');
INSERT INTO  DocumentsDataProviders VALUES ('SPDP','ShipmentProfitDataProvider');



UPDATE DocumentTypes SET DocumentsDataProviderCode='AWDP'
WHERE Code='740PP' or Code='740' or Code='714' or Code='714PP';


UPDATE DocumentTypes SET DocumentsDataProviderCode='FBDP'
WHERE Code='716';

UPDATE DocumentTypes SET DocumentsDataProviderCode='SDDP'
WHERE Code='COO' or Code='BCO' or Code='716SD' or Code='SFBL'  or  Code='BCS' or Code='IFI' or Code='GAPS' or Code='DOR'    or  Code='PGDF' or Code='REOR' ;


UPDATE DocumentTypes SET DocumentsDataProviderCode='ALDP'
WHERE Code='740L' or Code='740HL' or Code='LCLL';



UPDATE DocumentTypes SET DocumentsDataProviderCode='DNDP'
WHERE Code='784' or Code='781' or Code='DORE';



UPDATE DocumentTypes SET DocumentsDataProviderCode='INDP'
WHERE Code='999S' or Code='999M' or Code='999C';



UPDATE DocumentTypes SET DocumentsDataProviderCode='CMDP'
WHERE Code='CMR' or Code='SCMR';




UPDATE DocumentTypes SET DocumentsDataProviderCode='MADP'
WHERE Code='785A' or Code='785O';





UPDATE DocumentTypes SET DocumentsDataProviderCode='SPDP'
WHERE Code='PROF' ;



END