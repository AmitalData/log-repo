@release @all
Feature: Update Surcharge Tariff
    The authenticated user will create new ocean FCL surcharge cost tariff.
    then Update it .

    Scenario: creare Shipping Line 
    Given the user logged in and navigate to maintenance workspace
    When create new shipping line
    Then the shipping line should create successfully

    Scenario: Create ocean FCL surcharge cost
        Given the user logged in and navigate to tariff workspace
        And create new seller
        And an ocean FCL surcharge cost with new seller and the following details
            | Name                      | Seller     |
            | TestOceanFCLSurchargeCost | SellerTest |
        And the follwing surcharges details
            | Name                     |
            | Bunker Adjustment Factor |
        When create surcharge cost
        Then the surcharge cost should create successfully

    Scenario: Add tariff lines
        Given the user open the created surcharge cost
        And add the follwing tariff lines
            | From | To  | StartDate |
            | LHR  | LAS | Today     |
        When add the tariff line
        Then the surcharge cost should update successfully

    Scenario: Update surcharges
        Given the follwing update details
            | From | To  | StartDate |
            | LHR  | LAS | Today     |
        And  the following price details
            | price1 | price2 | price3 |
            | 10     | 10     | 10     |
        When update
        Then the surcharge update should create successfully