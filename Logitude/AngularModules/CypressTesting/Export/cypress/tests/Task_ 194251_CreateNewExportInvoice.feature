@smoke
Feature: Create New Export Invoice
    The user creates new Invoice, Fill all the Data and Save it

    Scenario: Create new Invoice 
        Given the user logged in and navigates to Export workspace
        And Search for file and enter to ExporterInvoices 

        | File | 565652 |
           

    Scenario: Open and Add details to Exporter Invoice
        Given Fill Exporter Invoice with the following details

            | AccountTypeCode          | חשבון מכר |
            | IssueDate                | TODAY |
            | ExporterNumber           | 510182462 |
            | InvoiceCurrencyTypeCode  | ILS |
            | IncotermCode             |  CIF  | 
            | BuyerName                | ORIT123,"8ם|
            | BuyerAddress             | ORIT123,"{)&*#@.\/' שלום |
            | PartyRelationshipCode    | קשר בעלות  |     
            | BuyerCountryCode         | US|
            | BuyerRoleCode            | לקוח קונה שהוא המקבל |
            | InvoiceNumber            | 111222333 |    
            | InvoiceAmount            | 5000 |

            | PreferenceDocumentTypeCode | ארה |
            | DutyRegimeProtocolCode     | כללי/בילטרלי |
            | ExportModificationCurrency | USD | 

            | InsuranceCurrencyTypeCode  | ILS |
            | InsuranceAmount            | 50  |
            | TransportCurrencyTypeCode  | ILS |
            | TransportAmount            | 100 |
            | ExpenseCurrencyTypeCode    | ILS |
            | ExpenseAmount              | 10  |
            
            
        When saveing the Invoice
        Then the Invoice should save successfully


    Scenario: Delete Row
        Given the user delete the row 
        Then there is no row in the grid    
        

   