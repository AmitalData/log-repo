@release @dev @all
Feature: AMANAC Ocean Shipment Validations, Marked as Blocked, Marked as Not Blocked, Transfers and Retransfer

    The user configures AMANAC customs for the tenant,
    creates Direct Export Ocean FCL shipment, adds a container,
    marks the shipment as blocked for transfer, marks the shipment as not blocked for transfer,
    transfers shipment, checks and fixes validation, transfers shipment again, edit shipment and retransfers.

    Scenario: AMANAC setup
        Given the user logged in and navigates to "customs settings" in maintenance menu
        When set local customs interface to "AMANAC ( Mexico )"
        Then the AMANAC workspace should appear in operations menu

    Scenario: Create export ocean FCL shipment
        Given the user in shipment workspace
        And a shipment with the following details
            | ShipmentLevel        | Direct            |
            | Direction            | Export            |
            | TransportMode        | Ocean             |
            | ShipmentType         | FCL               |
            | Shipper              | TestShipperExport |
            | MainCarriageFromPort | LHR               |
            | MainCarriageToPort   | MIA               |
        When create shipment
        Then the shipment should create successfully

    Scenario: Marked as blocked for transfer
        When the user click "Mark as blocked for transfer" in "New Transfer" view
        Then the shipment should appear in the "Marked as blocked for transfer" view in the AMANAC workspace
        And AMANAC and customs transmissions statuses should be "Blocked For Sending"

    Scenario: Marked as not blocked for transfer
        When the user click "Mark as not blocked for transfer" in "Marked as blocked for transfer" view
        Then the shipment should appear in the "New Transfer" view in the AMANAC workspace
        And should not appear in the "Marked as blocked for transfer" view in the AMANAC workspace
        And AMANAC and customs transmissions statuses should be "Not sent"

    Scenario: New transfer before fill all mandatory fields
        When the user export the shipment in "New Transfer" view
        Then a validation message "Please fill all the mandatory fields before exporting to AMANAC" should appear

    Scenario: Fill all mandatory fields
        Given the user add "MSCU" as shipping line
        And add package with the following details
            | PackageType | ContainerNumber | GrossWeight |
            | PC2         | ABCD1234560     | 100         |
        And edit main carriage leg with "123456" as voyage no and "PT" as vessel
        When update shipment
        Then the shipment should update successfully

    Scenario: New transfer after fill all mandatory fields
        When the user export the shipment in "New Transfer" view
        Then a validation message "Transferred Successfully" should appear
        And should not appear in the "New Transfer" view in the AMANAC workspace
        And AMANAC and customs transmissions statuses should be as following
            | Status   | Sent       |
            | LastSent | Today      |
            | SentBy   | LoggedUser |

    Scenario: Retransfer
        When the user retransfer the shipment
        Then AMANAC and customs transmissions statuses should be "Not sent"

    Scenario: Update packages tab
        Given the user edit package with "200" as GrossWeight
        When update shipment
        Then the shipment should update successfully

    Scenario: New transfer after update mandatory fields
        When the user export the shipment in "New Transfer" view
        Then a validation message "Transferred Successfully" should appear
        And should not appear in the "New Transfer" view in the AMANAC workspace
        And AMANAC and customs transmissions statuses should be as following
            | Status   | Sent         |
            | LastSent | Today        |
            | SentBy   | LoggedUser |