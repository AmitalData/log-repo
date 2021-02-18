@release @all
Feature: Ocean FCL Price Check
    The authenticated user will create new ocean FCL freight cost tariff,
    add tariff line in draft version tab, then approve it,
    and open price check wizard to show price offers.

    Scenario: Login and create new ocean FCL freight cost
        Given the user logged in and navigate to tariff workspace
        And an ocean FCL freight cost with the following details
            | Name                    | Seller | StartDate |
            | TestOceanFCLFreightCost | MAEU   | Today     |
        And the following All-In charges
            | Name                     |
            | Bunker Adjustment Factor |
            | B/L Fee                  |
        When create freight cost
        Then the freight cost should create successfully

    Scenario: Add tariff lines in draft version tab
        Given the user open the freight cost
        Given add the following tariff line
            | FromPort | ToPort | Step1Price | Step2Price | Step3Price |
            | LHR      | MIA    | 10         | 20         | 30         |
        When approve version
        Then the version should approve successfully

    Scenario: Open price check wizard to show price offers
        Given the user back into tariff workspace and open price check wizard
        And fill the following price check details
            | FromPort | ToPort | Date  | Quantity1 | Quantity2 | Quantity3 |
            | LHR      | MIA    | Today | 2         | 3         | 4         |
        When search about prices
        Then ocean FCL price should equal "200.00"