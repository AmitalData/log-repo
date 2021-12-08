@release @FeatureToggle
Feature: Ocean LCL Price Check
    The authenticated user will create new ocean LCL freight cost tariff,
    add tariff line in draft version tab, then approve it,
    and open price check wizard to show price offers.

    Scenario: Login and create new ocean LCL freight cost
        Given the user logged in and navigate to tariff workspace
        And an ocean LCL freight cost with the following details
            | Name           | TestOceanLCLFreightCost        |
            | Seller         | Yangming marine transport corp |
            | StartDate      | Today                          |
            | ExpirationDate | Today                          |
        When create freight cost
        Then the freight cost should create successfully

    Scenario: Add tariff lines in draft version tab
        Given the user open the freight cost
        And add the follwing tariff line
            | FromPort | ToPort | MinPrice | Step1Price | Step2Price | Step3Price |
            | LHR      | MIA    | 10       | 20         | 30         | 40         |
        When approve version
        Then the version should approve successfully

    Scenario: Create new ocean LCL surcharge cost
        Given an ocean FCL surcharge cost with the following details
            | Name   | TestOceanFCLSurchargeCost      |
            | Seller | Yangming marine transport corp |
        And add the following surcharges
            | Name                     |
            | Bunker Adjustment Factor |
            | B/L Fee                  |
        When create surcharge cost
        Then the surcharge cost should create successfully

    Scenario: Edit Surcharge
        Given the user in "OceanLCL" surchage workspace
        And open surchage with "Yangming marine transport corp" as seller
        And add the following surcharge line
            | FromPort | ToPort | StartDate | Step1Price | Step2Price |
            | LHR      | MIA    | Today     | 30         | 30         |
        When copy into new version if start date is not "Today"
        Then new version should approve successfully

    Scenario: Open price check wizard to show price offers
        Given the user back into tariff workspace and open price check wizard
        And fill the following price check details
            | FromPort         | LHR |
            | ToPort           | MIA |
            | ChargeableWeight | 10  |
        When search about prices
        Then ocean LCL price should equal the following
            | AirFreight | 200.00 ₪ |
            | Surcharges | 60.00 ₪  |
            | Total      | 260.00 ₪ |