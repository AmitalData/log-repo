Feature: AMANAC - Ocean Shipment

    Check Validation & Marked As Blocked For Transfer & Marked As Not Blocked For Transfer & New Transfer & Retransfer

    Scenario: AMANAC Setup
        Given the user logged in and navigates to "customs settings" in maintenance menu
        When set local customs interface to "AMANAC ( Mexico )"
        Then the AMANAC workspace should appear in operations menu

    Scenario: Create export ocean FCL shipment
        Given the user in shipment workspace
        And a shipment with the following details
            | ShipmentLevel | Direction | TransportMode | ShipmentType | Shipper           | MainCarriageFromPort | MainCarriageToPort |
            | Direct        | Export    | Ocean         | FCL          | TestShipperExport | LHR                  | MIA                |
        When create shipment
        Then the shipment should create successfully

    Scenario: Marked as blocked for transfer
        When marke the shipment as blocked for transfer
        Then the shipment should appear in the "Marked as blocked for transfer" view in the AMANAC workspace
        And AMANAC Status should be "Blocked For Sending"