@release @dev @all
Feature: Create State, Inactivate and activate it from Maintenance
    This scenario the user creates a State, Inactivates it,
    selects it to edit and activates it from the Maintenance module.

    Scenario: Add StateCode with lenght more than 10
        Given the user logged in and navigate to "States" in maintenance menu
        When add "StateCode123" as state code
        Then a validation message with "Code Field must be less than 10" error should appear

    Scenario: Add state
        Given a state with the following details
            | StateCode      | StateCode          |
            | StateName      | TestStateName      |
            | StateLocalName | TestStateLocalName |
            | Country        | United States      |
            | InactiveState  | Yes                |
            | Notes          | TestNote           |
        When add state
        Then the state should add successfully

    Scenario: Search for the city by name
        When search for "TestCity" state
        Then the "TestCity" state should appear successfully

    Scenario: Open the city
        When open state
        Then the state should open successfully

    Scenario: Edit the city
        Given a "random" as stateLocalName
        Given the user change Inactivestate check box
        When edit state
        Then the state should update successfully
        Then following event should appear in events tab
            | Event         | Notes          |
            | State Updated | State "status" |
