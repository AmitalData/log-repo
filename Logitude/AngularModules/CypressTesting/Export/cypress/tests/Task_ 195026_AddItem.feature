@smoke
Feature: Add Item
    The user Add Item, Fill all the Data and Save it

    Scenario: Add Item 
        Given the user logged in and navigates to Export workspace
        And Search for file and enter to ExporterInvoices 

        | File | 2937 |
           

    Scenario: Open Add and Delete details for New Item
        Given Fill the New Item with the following details

            | ItemNo          | 111222333 |
            | ItemDescription | בדיקות אוטומטיות - TEST !@#$%^&*)("?1 |
            | Item            | 90229000006 |
            | TradeAgreementCode | 110 |
            | ProtocolCode    |  2  | 
            | UnitsQuantity   | 10  |
            | UnitType        | KGM |
            | ValueInForeignCurrency | 1000 |     

            | ProcessTypeCode | מוחלט |
            
            
        When saveing the Invoice
        Then the Invoice should save successfully


        
        

        



    
   