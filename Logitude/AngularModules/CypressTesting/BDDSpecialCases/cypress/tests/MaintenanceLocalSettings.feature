@release @dev @all @salwa
Feature: Change Time Zone and Date Time Format from Maintenance
    The user changes time zone and date time format from the Maintenance Module.

    Scenario: Change time zone and date time format
        Given the user logged in and open "Local Settings" in maintenance menu
        And local settings with the following details
            | TimeZone       | (UTC-06:00) |
            | DateTimeFormat | MM/dd/yyyy  |
        When save local settings
        Then the local settings should update successfully

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

    Scenario: Validate HAWB Date in "MM/dd/yyyy"
        Given open the direct shipment and navigate to General tab
        When fill "Today" as HAWB Date
        Then the date format should be "MM/dd/yyyy"

    Scenario: Validate Shipment Updated with time in "UTC-06:00" timezone
        Given the user update the shipment
        When navigate to event tab
        Then the "Shipment Updated" event should include the time of "UTC-06:00" timezone

    Scenario: Change time zone and date time format To local timezone
        Given the user open "Local Settings" in maintenance menu
        And local settings with the following details
            | TimeZone       | (UTC+03:00) |
            | DateTimeFormat | dd/MM/yyyy  |
        When save local settings
        Then the local settings should update successfully

    Scenario: Validate HAWB Date in "dd/MM/yyyy"
        Given the user navigates to shipments workspace
        And open the direct shipment and navigate to General tab
        When fill "Today" as HAWB Date
        Then the date format should be "dd/MM/yyyy"

    Scenario: Validate Shipment Updated with time in "UTC+03:00" timezone
        Given the user update the shipment
        When navigate to event tab
        Then the "Shipment Updated" event should include the time of "UTC+03:00" timezone



