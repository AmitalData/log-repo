@release @stable @all
Feature: Create State, Inactivate and activate it from Maintenance
    The user creates a State, Inactivates it,
    selects it to edit and activates it from the Maintenance module.

    Scenario: Add StateCode with lenght more than 10
        Given the user logged in and navigate to "States" in maintenance menu
        When add "StateCode123" as state code
        Then a validation message with "Code Field must be less than 10" error should appear

    Scenario: Add state
        Given a state with the following details
            | StateCode      | random        |
            | StateName      | random        |
            | StateLocalName | random        |
            | Country        | United States |
            | InactiveState  | Yes           |
            | Notes          | TestNote      |
        When add state
        Then the state should add successfully

    Scenario: Search for the state by name
        When search for state
        Then the state should appear successfully

    Scenario: Open the state
        When open state
        Then the state should open successfully

    Scenario: Edit the state
        Given a "random" as stateLocalName
        And the user activate state
        When edit state
        Then the state should update successfully
        And following event should appear in events tab
            | Event         | Notes           |
            | State Updated | State Activated |
