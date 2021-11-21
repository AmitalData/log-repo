@devsmoke @stable @all

Feature: unexpected payable case
    The user creates a direct export air shipment, add unexpected payable.

Scenario: Create direct export air shipment
        Given the user navigates to shipments workspace
        And a direct shipment with the following details
            | ShipmentLevel        | Direct      |
            | Direction            | Export      |
            | TransportMode        | Air         |
            | Shipper              | TestCompany |
            | MainCarriageFromPort | LHR         |
            | MainCarriageToPort   | MIA         |
        When create shipment
        Then the direct should create successfully

    Scenario: Add AP invoice
        Given the user navigates to payable wizerd
        And the user creates an AP invoice
            | Vendor               | TestVendor  |
            | Invoice Number       | 1           |
            | Invoice Amount       | 100         |
            | invoice Currency     | EUR         |
            | Exchange Rate        | 4           |
            | Invoice Date         | .           |
            | Payment Terms        | Cash        |
            | Due Dates            | .           |
        When create invoice  
        Then the invoice should create successfully

Scenario: Add invoice line 
        Given the user creates an AP invoice line
            | Charges Type   | AFT        |
            | VAT Type       | EXMPT      |
            | VAT Percentage | 0          |
            | Vendor         | TestVendor |
            | Amount         | 100        |
        When save the invoice 
        Then the invoice is saved
        And a Payable line is created in the payable wizard 


 


