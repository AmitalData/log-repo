@NewDev
Feature: FBLStock Create, Search and Edit from Maintenance
    The user creates a FBLStock, searches for and edits it from the Maintenance Module.

    Scenario: Create new FBLStock
        Given the user logged in and open "FBL Stocks" in maintenance menu
        And a fblStock with the following details
            | StartNumber | random |
            | EndNumber   | random |
            | ByEndNumber | Yes    |
            | ByAmount    | No     |
            | Amount      | -      |
        When create fblStock
        Then the fblStock should create successfully

    Scenario: Remove FBLStock
        When Remove fblStock
        Then the fblStock should Remove successfully

    Scenario: Create new FBLStock by Amount
        Given a fblStock with the following details
            | StartNumber | random |
            | EndNumber   | random |
            | ByEndNumber | No     |
            | ByAmount    | Yes    |
            | Amount      | random |
        When create fblStock
        Then the fblStock should create successfully

    Scenario: Remove FBLStock series
        When Remove fblStock series
        Then the fblStock series should Remove successfully





