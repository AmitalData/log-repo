@release @all
Feature: Update Surcharge Tariff
    The authenticated user will create new ocean FCL surcharge cost tariff.
    then Update it .

    Scenario: Temporarily closed

    # Scenario: Create ocean FCL surcharge cost with new seller 
    #     Given the user logged in and navigate to tariff workspace
    #     And an ocean FCL surcharge cost with the following details
    #         | Name                      | Seller     |
    #         | TestOceanFCLSurchargeCost | SellerTest |
    #     And the follwing surcharges details
    #         | Name                     |
    #         | Bunker Adjustment Factor |
    #     When create surcharge cost
    #     Then the surcharge cost should create successfully

    # Scenario: Add tariff lines
    #     Given the user open the created surcharge cost
    #     And add the follwing tariff lines
    #         | FromPort | ToPort | StartDate |
    #         | LHR      | LAS    | Today     |
    #     When add the tariff line
    #     Then the surcharge cost should update successfully

    # Scenario: Update surcharges
    #     Given the user in update tab
    #     And the follwing surcharge cost update details
    #         | FromPort | ToPort | StartDate | Step1Price | Step2Price | Step3Price |
    #         | LHR      | LAS    | Today     | 10         | 10         | 10         |
    #     When update
    #     Then the surcharge update should create successfully