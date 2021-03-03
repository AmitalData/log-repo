@release @all @open

Feature: Shipment Exception Resolve
    The user creates a shipment, adds an exception in the Events tab,
    resolves the exception and checks the status of the exception.

    Scenario: Create direct export air shipment
        Given the user logged in and navigates to shipments workspace
        And a direct shipment with the following details
            | ShipmentLevel | Direction | TransportMode | Shipper           | MainCarriageFromPort | MainCarriageToPort |
            | Direct        | Export    | Air           | TestShipperExport | LHR                  | MIA                |
        When create shipment
        Then the direct should create successfully

    Scenario: Add exception
        Given the initial has exception status is "No"
        Given exception event with the following details
            | EventType | EventDate | EventTime | EventNotes       |
            | Exception | Today     | 12:00     | Adding Exception |
        When add exception
        Then the exception should add successfully
        And the exception should appear in events tab
        And has exception status should change to "Yes" successfully

    Scenario: resolve the exception
        When resolve the exception due to "ExceptionResolvedNote"
        Then the exception should resolve successfully
        And has exception status should back to "No" successfully
