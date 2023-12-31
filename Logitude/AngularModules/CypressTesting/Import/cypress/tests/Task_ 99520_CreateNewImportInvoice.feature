@smoke
Feature: Create New Import Invoice
    The user create new Invoice, Fill all the Data and Save it

    Scenario: Create new Invoice 
        Given the user logged in and navigates to Import workspace
        And Search for file and enter to Importer Invoices 

        | File | 51330115 |
           

    Scenario: Open and Add details to Importer Invoice
        Given Fill Importer Invoice with the following details

            | AccountTypeCode          | חשבון מכר |
            | IssueDate                | TODAY |
            | ExporterNumber           | 510182462 |
            | InvoiceCurrencyTypeCode  | ILS |
            | IncotermCode             | CIF | 
      
            | InvoiceNumber            | 111222333 |    
            | InvoiceAmount            | 5000 |

            | PreferenceDocumentTypeCode | איחוד אירופי|
            | VendorId                   | DEEJAY.DE | 
           
            | FreightAmountCurrencyTypeCode | ILS |
            | FreightAmount            | 50 |   


Scenario: Open Add and Delete details for New Item
        Given Fill the New Item with the following details

            | ItemNo          | 111222333 |
            | ItemDescription | בדיקות אוטומטיות - TEST !@#$%^&*)("?1 |
            | Item            | 90229000006 |
            | TradeAgreementCode | 113 |
            | ProtocolCode    |  2  | 
            | UnitsQuantity   | 10  |
            | UnitType        | KGM |
            | ValueInForeignCurrency | 1000 |     
            | OriginCountryCode | DE |
            
        When saveing the Invoice
        Then the Invoice should save successfully


    Scenario: Delete Row
        Given the user delete the row 
        Then there is no row in the grid    
        

   