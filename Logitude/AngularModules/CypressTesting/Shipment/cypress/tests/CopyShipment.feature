Feature:Copy Direct Export Air Shipment

    Scenario: Login And Open Shipments Workspace
        Given user has logged
        And the user has gone to the shipments workspace

    Scenario: Create Direct Export Air Shipment
        Given the user fills the required shipment details with valid data
            | ShipmentLevel | Direction | TransportMode | Shipper  | MainCarriageFromPort | MainCarriageToPort |
            | Direct        | E         | A             | Shipper1 | LHR                  | MIA                |
        When the user Clicks on create shipment button
        Then the create operation completed successfully

    Scenario: Copy Direct Export Air Shipment
        Given the user searches for the required shipment using quick search
        And the user clicks on copy shipment
        When the users clicks on Copy
        Then the copy operation completed successfully
