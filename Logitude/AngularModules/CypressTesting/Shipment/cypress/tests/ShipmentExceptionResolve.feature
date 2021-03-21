@release @dev

Feature: Shipment Exception Resolve
    The user creates a shipment, adds an exception in the Events tab,
    resolves the exception and checks the status of the exception.

    Scenario: Create direct export air shipment
        Given the user logged in and navigates to shipments workspace
        And a direct shipment with the following details
            | ShipmentLevel        | Direct            |
            | Direction            | Export            |
            | TransportMode        | Air               |
            | Shipper              | TestShipperExport |
            | MainCarriageFromPort | LHR               |
            | MainCarriageToPort   | MIA               |
        When create shipment
        Then the direct should create successfully

    Scenario: Add exception
        Given the initial has exception status is "No"
        Given exception event with the following details
            | EventType  | Exception        |
            | EventDate  | Today            |
            | EventTime  | 12:00            |
            | EventNotes | Adding Exception |
        When add exception
        Then the exception should add successfully
        And the exception should appear in events tab
        And has exception should change to yes

    Scenario: Resolve the exception
        When resolve the exception due to "ExceptionResolvedNote"
        Then the exception should resolve successfully
        And resolve the exception should appear in events tab
        And has exception should change to no
        