Feature: Cross Docks Entries and Releases

    The user creates a shipment, adds a cross dock entry, modifies it, cancels it,
    create a new entry, creates a cross dock release, modifies it, cancels it,
    creates a new cross dock release and creates a delivery.

    Scenario: Create direct export air shipment
        Given the user logged in and navigates to shipments workspace
        And a shipment with the following details
            | ShipmentLevel        | Direct            |
            | Direction            | Export            |
            | TransportMode        | Air               |
            | Shipper              | TestShipperExport |
            | MainCarriageFromPort | LHR               |
            | MainCarriageToPort   | MIA               |
        When create shipment
        Then the shipment should create successfully

    Scenario: Add packages
        Given the user open the shipment and navigate to packages workspace
        And a package with the following details
            | Quantity | Length | Width | Height | GrossWeight |
            | 2        | 100    | 100   | 100    | 200         |
        When save shipment
        Then the direct shipment should save successfully

    Scenario: Add cross dock entry
        Given the user in Connected Entities workspace
        And a corss dock "Entry" with the following details
            | Warehouse         | TestWarehouse |
            | ExpectedEntryDate | Today         |
            | ExpectedEntryTime | 13:00         |
        When create cross dock entry
        Then the cross dock entry should create successfully
        And shipment will contain the linked entry details
            | EntryNumber | "Created Entry Number" |
            | Status      | Created                |
            | EntryDate   | Today (expected)       |
        And cross dock entry will contain the linked shipment details
            | ShipmentNumber | "Created Shipment Number" |
            | Customer       | TestShipperExport         |
            | From           | LHR                       |
            | To             | MIA                       |
            | Status         | Order                     |

    Scenario: Cancel entry
        When cancel the entry
        Then the entry should cancel successfully
        And entry status should be "Cancelled"
        And entry fields will be disable
        And linked entry status should be "Cancelled"

    Scenario: Add cross dock entry
        Given a corss dock "Entry" with the following details
            | Warehouse         | TestWarehouse |
            | ExpectedEntryDate | Today         |
            | ExpectedEntryTime | 14:00         |
        When create cross dock entry
        Then the cross dock entry should create successfully

    Scenario: Edit entry
        Given the user open the created entry
        Given fill the entry with the following details
            | ActualEntryDate | Today |
            | ActualEntryTime | 00:00 |
        When save entry
        Then the entry should update sucessfully
