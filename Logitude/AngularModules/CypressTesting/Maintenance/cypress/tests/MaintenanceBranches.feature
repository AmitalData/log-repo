@NewDev
Feature: Branch Create, Search and Edit from Maintenance
    The user creates a Branch, searches for and edits it from the Maintenance Module.

    Scenario: Create new Branch
        Given the user logged in and open "Branches" in maintenance menu
        And a branch with the following details
            | Name        | CurrentDate |
            | LocalName   | Test        |
            | Code        | random      |
            | Signature   | Test        |
            | CounterCode | random      |
        When create branch
        Then the branch should create successfully

    Scenario: Search for the Branch by number
        When search branch
        Then the branch should appear successfully

    Scenario: Open the Branch
        When open branch
        Then the branch should open successfully

    Scenario: Edit the Branch
        Given the user fill the following branch details
            | LocalName | EditBranchLocalNameTest |
        And  the user activate branch
        When edit branch
        Then the branch should update successfully
        And following event should appear in events tab
            | Event          | Notes              |
            | Branch Updated | Branch Inactivated |

    Scenario: Save and close the branch
        When save and close branch
        Then the branch should close successfully