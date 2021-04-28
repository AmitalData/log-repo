@release @dev @all
Feature: Convert Shipment from House to Direct, Direct to House, FCL to LCL, LCL to FCL and Shipment Direction
    The user creates a Direct Export Ocean FCL shipment,
    changes direction to Import, changes type to House,
    then back to Direct, changes type to LCL, then back to FCL.

    Scenario: Create direct export ocean FCL shipment
        Given the user logged in and navigate to shipments workspace
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

    Scenario: Change shipment direction to import
        Given the user open the shipment
        And open direction conversion wizard
        And fill the following details for direction conversion
            | Direction | Import              |
            | Consignee | TestConsigneeImport |
        When convert shipment direction
        Then the direction should convert successfully
        And direction should be "Import"
        And the shipment should have a new number
        And the consignee "TestConsigneeImport" should appear in partners tab
        And following events should appear in events tab
            | Event                        | Notes                               |
            | Shipment Direction Converted | Converted from [Export] to [Import] |

    Scenario: Change shipment type to house
        Given the user open direct to house conversion wizard
        And fill notes "Convert direct to house test"
        When convert shipment type
        Then the type should convert successfully
        And type should be "FCL House"
        And following events should appear in events tab
            | Event                                 | Notes                        |
            | Convert Shipment From Direct To House | Convert direct to house test |

    Scenario: Change shipment type to direct
        Given the user open house to direct conversion wizard
        And fill notes "Convert house to direct test"
        When convert shipment type
        Then the type should convert successfully
        And type should be "FCL Direct"
        And following events should appear in events tab
            | Event                                 | Notes                        |
            | Convert Shipment From House To Direct | Convert house to direct test |

    Scenario: Change shipment type to LCL
        Given the user open FCL to LCL conversion wizard
        And fill notes "Convert FCL to LCL test"
        When convert shipment type
        Then the type should convert successfully
        And type should be "LCL Direct"
        And the button "Add Package" should appear in packages tab
        And following events should appear in events tab
            | Event                     | Notes                   |
            | Converted From FCL to LCL | Convert FCL to LCL test |

    Scenario: Change shipment type to FCL
        Given the user open LCL to FCL conversion wizard
        And fill notes "Convert LCL to FCL test"
        When convert shipment type
        Then the type should convert successfully
        And type should be "FCL Direct"
        And the button "Add Container" should appear in packages tab
        And following events should appear in events tab
            | Event                     | Notes                   |
            | Converted From LCL to FCL | Convert LCL to FCL test |
