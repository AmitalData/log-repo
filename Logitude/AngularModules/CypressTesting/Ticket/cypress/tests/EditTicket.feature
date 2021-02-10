Feature: Edit Ticket Test

    Scenario: Create Direct Export Air Shipment
        Given the user logged in and navigates to shipments workspace
        And a direct shipment with the following details
            | ShipmentLevel | Direction | TransportMode | Shipper           | MainCarriageFromPort | MainCarriageToPort |
            | Direct        | Export    | Air           | TestShipperExport | LHR                  | MIA                |
        When create shipment
        Then the shipment should create successfully

    Scenario: Create new ticket
        Given the user logged in and navigated to ticket workspace
        And a ticket with the following details
            | EntityType | Company   | Contact      | Subject     | Description | MainClassification | Severity | EmployeeGroup | Owner        |
            | shipment   | TestAgent | Test Contact | Test Ticket | Test Ticket | Test               | Medium   | Tester Group  | specflowTest |
        When create ticket
        Then the ticket should create successfully

    Scenario: Edit the ticket
        Given the user fill the shipment number as Entity Number
        When save as open
        Then the ticket should update|save successfully
