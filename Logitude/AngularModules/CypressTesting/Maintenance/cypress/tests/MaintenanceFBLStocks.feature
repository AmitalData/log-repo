@NewDev
Feature: FBL Stock Add, Remove and Remove Series in Maintenance
    The user creates a FBLStock, searches for and edits it from the Maintenance Module.

    Scenario: Create new FBLStock
        Given the user logged in and open "FBL Stocks" in maintenance menu
        And the user adds FBL stock with following details
            | StartNumber | random |
            | EndNumber   | random |
            | ByEndNumber | Yes    |
            | ByAmount    | No     |
            | Amount      | -      |
        When create fblStock
        Then the fblStock should create successfully

    Scenario: Remove FBLStock
        When user removes one entry
        Then the fblStock should Remove successfully

    Scenario: Create new FBLStock by Amount
        Given user adds another FBL stock with the following details
            | StartNumber | random |
            | EndNumber   | random |
            | ByEndNumber | No     |
            | ByAmount    | Yes    |
            | Amount      | random |
        When create fblStock
        Then the fblStock should create successfully

    Scenario: Remove FBLStock series
        When user removes a series of entries
        Then the fblStock series should Remove successfully





