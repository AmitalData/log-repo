@devrelease

Feature: Sharing Manifests with agent
    The user check Sharing Manifest with agent Prerequisites, then create a shipment and share it,
    from other side check shared Manifest and test actions of create, Cancel Manifest,and Mark as Completed

    Scenario: Create new Agent
        Given the user logged in and open "Agents" in maintenance menu
        And an agent with the following details
            | CompanyName | Testing agent Scenario |
            | LocalName   | Testing agent Scenario |
            | Address1    | 15 agent Street        |
            | Zip         | 0000                   |
            | City        | Anchorage              |
            | Country     | United States          |
            | State       | Alaska                 |
            | Phone       | 9999999999             |
            | Fax         | 999999                 |
        And an agent contact with the following details
            | Email         | Email       |
            | AddContact    | Yes         |
            | EnglishName   | TestContact |
            | Position      | Developer   |
            | BusinessPhone | 8888888888  |
            | Mobile        | 8888888888  |
            | Fax           | 888888      |
        When create agent
        Then the agent should create successfully

    Scenario: Create master export air shipment
        Given the user navigates to shipments workspace
        And a master Shipment with following details
            | ShipmentLevel        | Master                 |
            | Direction            | Export                 |
            | TransportMode        | Air                    |
            | Agent                | Testing agent Scenario |
            | MainCarriageFromPort | LHR                    |
            | MainCarriageToPort   | MIA                    |
        When create shipment
        Then the master should create successfully

    Scenario: Try sharing manifest in a shipment with no MAWB number
        When the user try to sharing manifest in a shipment with no MAWB number
        Then the following validation appears "The master number is missing"

    Scenario: update MAWB number for the shipment
        Given the user fill master number for the shipment, MainCarriageCarrier,and remove the consignee
            | masterNumber | 11111445 |
            | AgentCarrier | aa       |
        When the user save the shipment
        Then the master should updated successfully

    Scenario: add consignee to the shipment
        Given the user add consignee
        When the user save the shipment
        Then the master should updated successfully

    Scenario: Search for the agent
        When search agent
        Then the agent should appear successfully

    Scenario: Open the agent
        When open agent
        Then the agent should open successfully

    Scenario: send Shared Logistics Invitation from the agent
        When send Invitation from Shared Logistics to "email@test.com"
        Then the Invitation should sent successfully

    Scenario: Create new Agent
        Given the user logged in to another tenant and open "Agents" in maintenance menu
        And an agent with the following details
            | CompanyName | Testing agent Scenario |
            | LocalName   | Testing agent Scenario |
            | Address1    | 11 agent Street        |
            | Zip         | 0000                   |
            | City        | Anchorage              |
            | Country     | United States          |
            | State       | Alaska                 |
            | Phone       | 0123456789             |
            | Fax         | 123456                 |
        And an agent contact with the following details
            | Email         | Email       |
            | AddContact    | Yes         |
            | EnglishName   | TestContact |
            | Position      | Developer   |
            | BusinessPhone | 8888888888  |
            | Mobile        | 8888888888  |
            | Fax           | 123456      |
        When create agent
        Then the agent should create successfully

    Scenario: Search for the agent
        When search the second agent
        Then the second agent should appear successfully

    Scenario: Open the agent
        When open agent
        Then the agent should open successfully

    Scenario: Accept Shared Logistics Invitation from the agent
        When Accept Invitation from Shared Logistics with the shared key from the previouse agent
        Then the Invitation should accepted successfully


    Scenario: Share manifest from shipment successfully
        Given the user logged in to the first tenant and open the created shipment
        When the user shares the manifest from a shipment
        Then the Manifest should shared successfully

    Scenario: check shared manifests from the second tenant
        Given the user logged in to the second tenant
        And the user navigates to operations
        And chooses shared manifests tab
        When chooses Air Manifests and search by the manifest number
        Then the shared shipment should exist with same details as we send from the first agent side

    Scenario: Create the shipment from the second tenant(should be inserted as import )
        When the user create the shipment with "aa" as carrier
        Then an import shipment should be created

    Scenario: Change On Document Permission
        Given the user logged in to the First tenant
        And  Open Shared Logistics Module and open Documents Permissions
        When  Open Agent View and search for "Air Manifest"
        Then check master checkbox as true and Save

    Scenario: User Build Document "Air Manifest"
        Given the user enter the shipment
        When Open Document Out tab and search for "Air Manifest"
        Then Build the document

    Scenario: Share Document With Agent
        When the user click on share Document Option
        Then The document will Shared successfully


    Scenario: Share Document from shipment successfully
        Given the user logged in to the Second tenant and open the shipment
        When the user open DocIn Tab "Air Manifest"
        Then the Document should be shared successfully


    Scenario: delete Master number from shipment successfully
        Given the user logged in to the first tenant and open the created shipment
        Then delete Master number







