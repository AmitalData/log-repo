@dev @daily
Feature: Branch fake Create, Search and Edit from Maintenance
    The user creates a Branch, searches for and edits it from the Maintenance Module.

    Scenario: Add Branch Code with lenght more than 10
        Given the user logged in and open "Branches" in maintenance menu
        When add "12345678901" as branch code
        Then a validation message with "Code Field must be less than 10" error should appear

    Scenario: Add Branch Counter Code with lenght more than 5
        When add "123456" as branch counter code
        Then a validation message with "Code Field must be less than 5" error should appear

    Scenario: Create new Branch
        And a branch with the following details
            | Name        | CurrentDate |
            | LocalName   | Test        |
            | Code        | random      |
            | Signature   | Test        |
            | CounterCode | random      |
        When create branch
        Then the branch should create successfully

    Scenario: Search for the Branch by name
        When search for "Main" branch
        Then the "Main Office" branch should appear successfully

    Scenario: Open the Branch
        When open branch
        Then the branch should open successfully

    Scenario: Edit the Branch by adding address
        Given the user create address with the following details
            | Name    | Branch        |
            | Country | United States |
            | City    | Anchorage     |
            | State   | Alaska        |
        When create address
        Then the address should update successfully

    Scenario: Edit the Branch
        Given the user change the External ID in Accounting Tab
        Given the user Inactivate the Branch
        When save branch
        Then the branch should update successfully
        And the following event should appear in events tab
            | Event          |
            | Branch Updated |

    Scenario: Save and close the branch
        When save and close branch
        Then the branch should close successfully