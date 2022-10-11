Feature: Workflow run history
    The user open workflow list, open a flow, and open run history.

    Scenario: open workflow run history
        Given the user logged in and navigates to automation workspace
        And open workflows list
        And open a flow
        When open run history
        Then the instances should appear successfully

    Scenario: refresh run history
        When refresh run history
        Then the instances should appear successfully

    Scenario: search run history
        When search instance in run history
        Then the result instances should appear successfully