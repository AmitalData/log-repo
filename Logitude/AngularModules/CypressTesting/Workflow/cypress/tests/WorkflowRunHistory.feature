@devTest
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

    Scenario: export run history
        When export instances run history
        Then the instances excel file should export successfully

    Scenario: sort run history instances
        When sort instances run history by 'Duration'
        Then the instances should sort successfully

    Scenario: search run history
        When search instance in run history
        Then the result instances should appear successfully

    Scenario: filter run history instances by current date
        When filter instances by current date
        Then the instances should filter successfully

    Scenario: open single instance activity list
        When open instance activity list
        Then the activity list should appear successfully

    Scenario: refresh single instance activity list
        When refresh instance activity list
        Then the activity list should refresh successfully