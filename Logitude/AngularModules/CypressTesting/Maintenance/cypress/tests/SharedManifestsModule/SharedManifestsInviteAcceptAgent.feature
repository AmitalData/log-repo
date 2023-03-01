@release3
Feature: Agent Create, Search and Edit from Maintenance
    The user creates an agent, searches for and edits it from the Maintenance Module.

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
        When search agent
        Then the agent should appear successfully

    Scenario: Open the agent
        When open agent
        Then the agent should open successfully

    Scenario: Accept Shared Logistics Invitation from the agent
        When Accept Invitation from Shared Logistics with the shared key from the previouse agent
        Then the Invitation should accepted successfully
