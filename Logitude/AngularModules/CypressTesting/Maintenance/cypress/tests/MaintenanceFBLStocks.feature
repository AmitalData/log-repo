@NewDev
Feature: FBLStock Create, Search and Edit from Maintenance
    The user creates a FBLStock, searches for and edits it from the Maintenance Module.

    Scenario: Create new FBLStock
        Given the user logged in and open "FBLStockes" in maintenance menu
        And a fblStock with the following details
            | StartNumber | random |
            | EndNumber   | random |
            | ByEndName   | Yes    |
            | ByAmount    | No     |
        When create fblStock
        Then the fblStock should create successfully

        Given a fblStock with the following details
            | StartNumber | random |
            | EndNumber   | random |
            | ByEndName   | No     |
            | ByAmount    | Yes    |
        When create fblStock
        Then the fblStock should create successfully

    Scenario: Remove FBLStock 
        When Remove fblStock
        Then the fblStock should Remove successfully

    Scenario: Remove FBLStock series
        When Remove fblStock series
        Then the fblStock series should Remove successfully