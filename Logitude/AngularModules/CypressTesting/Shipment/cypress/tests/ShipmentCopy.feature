@devsmoke @stable @all
Feature: Copy Direct Export Air Shipment
    The user creates a Direct Export Air shipment and copies it.

    Scenario: Create direct export air shipment
        Given the user logged in and navigates to shipments workspace
        And a direct shipment with the following details
            | ShipmentLevel        | Direct            |
            | Direction            | Export            |
            | TransportMode        | Air               |
            | Shipper              | TestShipperExport |
            | MainCarriageFromPort | LHR               |
            | MainCarriageToPort   | MIA               |
            | Consignee            | 70005             |
            | DescriptionOfGoods   | toys              |
        When create shipment
        Then the direct should create successfully

    Scenario: Add Packages
        Given the user open the shipment and navigate to packages workspace
        And a package with the following details
            | Quantity | Length | Width | Height | GrossWeight |
            | 12       | 100    | 100   | 100    | 120         |
        When save shipment
        Then the direct shipment should save successfully

    Scenario: Copy Direct export air shipment
        When copy the shipment
        Then a shipment copy should create successfully

    Scenario: Assert packages tab
        When navigates packages tab
        Then the direct shipment should should has the following package details
            | Quantity | Volume | GrossWeight |
            | 12       | 12.000 | 120.000     |

    Scenario: Assert routing tab
        When navigates routing tab
        Then the pick up has "Heathrow Apt/London" as to port value
        And the delivery has "Miami" as from port value