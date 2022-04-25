#@devrelease
Feature: Customization Add Fields To Shipment General Tab
    The user open customization, add Custom Fields To Shipment General Tab, create new shipment, test the Added fields in the shipment, remove the fields

        Scenario:Add Fields To Shipment General Tab 
        Given the user logged in and choose customization
        And the user choose "Shipment" Object
        And the user clicks on Custom Fields
        And get first two fields 
       When drag and drop the following details in shipment general tab and save
            | Field  | Column  |
            | Field1 | Column_0 |
            | Field2 | Column_1 |
       Then the fields should saved successfully to the screen layout
     
    Scenario: Create direct export air shipment
        Given the user navigates to shipments workspace
        And a direct shipment with the following details
            | ShipmentLevel        | Direct            |
            | Direction            | Export            |
            | TransportMode        | Air               |
            | Shipper              | TestShipperExport |
            | MainCarriageFromPort | LHR               |
            | MainCarriageToPort   | MIA               |
        When create shipment
        Then the direct should create successfully

    Scenario: open shipment to verify the custom fields addition
        Given the user open the shipment and go to the general tab
        Then the custom fields should be added successfully

    Scenario:Remove Custom Fields From Shipment General Tab 
        Given the user choose customization
        And the user choose "Shipment" Object
        And the user clicks on Custom Fields
        And get first two fields 
       When delete the following fields details from shipment general tab and save
            | Field  | Column  |
            | Field1 | Column_0 |
            | Field2 | Column_1 |
       Then the fields should removed successfully from the screen layout