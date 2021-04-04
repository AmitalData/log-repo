@release @stable 
Feature: Download Excel Tariff test
    The authenticated user will create new Ocean LCL freight cost tariff.
    then download excel file .

    Scenario: Create new ocean FCL freight cost
        Given the user logged in and navigate to tariff workspace
        And an "Ocean LCL" freight cost with the following details
            | Name      | TestOceanFCLFreightCost |
            | Seller    | MAEU                    |
            | StartDate | Today                   |
        And the following All-In charges
            | Name                     |
            | Bunker Adjustment Factor |
            | B/L Fee                  |
        When create freight cost
        Then the freight cost should create successfully

    Scenario: Add tariff lines in draft version tab
        Given the user open the freight cost
        And add the following "Ocean LCL" tariff line
            | FromPort | ToPort | Step1Price | Step2Price | Step3Price |
            | LHR      | MIA    | 10         | 20         | 30         |
        When approve version
        Then the version should approve successfully

    Scenario: Download Excel File
        When download excel file
        Then the file should download successfully

    