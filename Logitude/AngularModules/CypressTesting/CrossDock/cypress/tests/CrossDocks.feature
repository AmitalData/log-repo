@release @dev
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
        Given the user open the shipment and navigate to packages tab
        And a package with the following details
            | Quantity | Length | Width | Height | GrossWeight |
            | 2        | 100    | 100   | 100    | 200         |
        When save shipment
        Then the direct shipment should save successfully

    Scenario: Add cross dock entry
        Given the user in Connected Entities tab
        And a corss dock "Entry" with the following details
            | Warehouse         | TestWarehouse |
            | ExpectedEntryDate | Today         |
            | ExpectedEntryTime | 13:00         |
        When create cross dock entry
        Then the cross dock entry should create successfully
        And the shipment should contain the linked entry details
            | EntryNumber | "Created Entry Number" |
            | Status      | Created                |
            | EntryDate   | Today (expected)       |
        And the cross dock "Entry" should contain the linked shipment details
            | ShipmentNumber | "Created Shipment Number" |
            | Customer       | TestShipperExport         |
            | From           | LHR                       |
            | To             | MIA                       |
            | Status         | Order                     |

    Scenario: Cancel entry
        When cancel the entry
        Then the entry should cancel successfully
        And entry status should be "Cancelled"
        And entry fields should be disable
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
        And fill the entry with the following details
            | ActualEntryDate | Today |
            | ActualEntryTime | 00:00 |
        When save entry
        Then the entry should update sucessfully
        And entry status should be "Entered"
        And entry date should be " Today  (actual)"
        And the Warehouse Terminal in shipment routing tab should have the following details
            | ExpectedEntryDate | Today    |
            | ExpectedEntryTime | 02:00 PM |
            | ActualEntryDate   | Today    |
            | ActualEntryTime   | 12:00 AM |

    Scenario: Add cross dock release
        Given the user in Connected Entities tab
        And a corss dock "Release" with the following details
            | Warehouse           | TestWarehouse          |
            | ExpectedReleaseDate | Today                  |
            | ExpectedReleaseTime | 13:00                  |
            | Package             | PackageWithEntryNumber |
        When create cross dock release
        Then the cross dock release should create successfully
        And shipment should contain the linked release details
            | ReleaseNumber | "Created Release Number" |
            | Status        | Created                  |
            | ReleaseDate   | Today (expected)         |
        And the linked cross dock entry should contain this cross dock release details
            | ReleaseNumber  | "Created Release Number"  |
            | ShipmentNumber | "Created Shipment Number" |
            | ConnectedTo    | Shipment                  |
            | Status         | Created                   |
        And the cross dock "Release" should contain the linked shipment details
            | ShipmentNumber | "Created Shipment Number" |
            | Customer       | TestShipperExport         |
            | From           | LHR                       |
            | To             | MIA                       |
            | Status         | Storage Entry             |

    Scenario: Cancel Release
        When cancel the release
        Then the release should cancel successfully
        And release status should be "Cancelled"
        And release fields should be disabled
        And linked release status should be "Cancelled"

    Scenario: Add cross dock release
        Given the user in Connected Entities tab
        And a corss dock "Release" with the following details
            | Warehouse           | TestWarehouse          |
            | ExpectedReleaseDate | Today                  |
            | ExpectedReleaseTime | 14:00                  |
            | Package             | PackageWithEntryNumber |
        When create cross dock release
        Then the cross dock release should create successfully

    Scenario: Edit release
        Given the user open the created release
        And fill the release with the following details
            | ActualReleaseDate | Today |
            | ActualReleaseTime | 00:00 |
        When save release
        Then the release should update sucessfully
        And release status should be "Released"
        And release date should be " Today  (actual)"
        And the Warehouse Terminal in shipment routing tab should have the following release details
            | ExpectedReleaseDate | Today    |
            | ExpectedReleaseTime | 02:00 PM |
            | ActualReleaseDate   | Today    |
            | ActualReleaseTime   | 12:00 AM |

    Scenario: Add delivery
        When Add delivery
        Then the delivery should add successfully
        And the delivery leg should appear in the shipment routing tab with the following details
            | ToPartner        | TestShipperExport |
            | ETDDepartureDate | Today             |
            | ETDDepartureTime | 02:00 PM          |
            | ATDDepartureDate | Today             |
            | ATDDepartureTime | 12:00 AM          |