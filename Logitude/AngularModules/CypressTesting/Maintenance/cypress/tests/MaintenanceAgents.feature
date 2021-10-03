@release @all @dev @smoke
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

    Scenario: Edit the agent
        Given "Test edit agent" as agent notes
        When update agent
        Then the agent should update successfully