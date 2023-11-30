@release3
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
            | masterNumber | 11111111 |
            | AgentCarrier | aa       |
        When the user save the shipment
        Then the master should updated successfully

    Scenario: Try sharing manifest in a shipment with no consignee
        When the user try to sharing manifest in a shipment with no consignee
        Then the following validation appears "The consignee partner is missing"

    Scenario: add consignee to the shipment
        Given the user add consignee
        When the user save the shipment
        Then the master should updated successfully

    Scenario: Try sharing manifest in a shipment with Agent who doesn't have sharing accept
        When the user try to sharing manifest in a shipment Agent who doesn't have sharing accept
        Then the following validation appears "Please connect with the agent from the agent’s shared logistics tab"


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

    Scenario: delete Master number from shipment successfully
        Given the user logged in to the first tenant and open the created shipment
        And delete Master number
        When the user save the shipment
        Then the master should updated successfully
        And the user exit the shipment

    Scenario: Create master export air shipment
        Given the user navigates to shipments
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
            | masterNumber | 22222222 |
            | AgentCarrier | aa       |
        When the user save the shipment
        Then the master should updated successfully

    Scenario: Try sharing manifest in a shipment with no consignee
        When the user try to sharing manifest in a shipment with no consignee
        Then the following validation appears "The consignee partner is missing"

    Scenario: add consignee to the shipment
        Given the user add consignee
        When the user save the shipment
        Then the master should updated successfully

    Scenario: Share manifest from shipment successfully
        When the user shares the manifest from a shipment
        Then the Manifest should shared successfully

    Scenario: check shared manifests from the second tenant
        Given the user logged in to the second tenant
        And the user navigates to operations
        And chooses shared manifests tab
        When chooses Air Manifests and search by the manifest number
        Then the shared shipment should exist with same details as we send from the first agent side

    Scenario: Cancel Manifest
        When the user click Cancel Manifest
        Then the Manifest should cancelled successfully

    Scenario: delete Master number from shipment successfully
        Given the user logged in to the first tenant and open the created shipment
        And delete Master number
        When the user save the shipment
        Then the master should updated successfully
        And the user exit the shipment

    Scenario: Create master export air shipment
        Given the user navigates to shipments
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
            | masterNumber | 33333333 |
            | AgentCarrier | aa       |
        When the user save the shipment
        Then the master should updated successfully

    Scenario: Try sharing manifest in a shipment with no consignee
        When the user try to sharing manifest in a shipment with no consignee
        Then the following validation appears "The consignee partner is missing"

    Scenario: add consignee to the shipment
        Given the user add consignee
        When the user save the shipment
        Then the master should updated successfully

    Scenario: Share manifest from shipment successfully
        When the user shares the manifest from a shipment
        Then the Manifest should shared successfully

    Scenario: check shared manifests from the second tenant
        Given the user logged in to the second tenant
        And the user navigates to operations
        And chooses shared manifests tab
        When chooses Air Manifests and search by the manifest number
        Then the shared shipment should exist with same details as we send from the first agent side

    Scenario: Mark as Completed Action in Shared Manifest
        When the user mark the Manifest as completed
        Then the Manifest should Marked as Completed successfully

    Scenario: delete Master number from shipment successfully
        Given the user logged in to the first tenant and open the created shipment
        And delete Master number
        When the user save the shipment
        Then the master should updated successfully
        And the user exit the shipment

