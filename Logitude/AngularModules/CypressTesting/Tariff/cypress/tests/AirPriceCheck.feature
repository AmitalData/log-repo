@release @all    
Feature: Air Price Check
    The authenticated user will create new air freight cost tariff,
    add tariff line in draft version tab, then approve it,
    and open price check wizard to show price offers.

    Scenario: Login and create new air freight cost
        Given the user logged in and navigate to tariff workspace
        And an air freight cost with the following details
            | Name               | Seller | StartDate | Product |
            | TestAirFreightCost | AA     | Today     | General |
        When create freight cost
        Then the freight cost should create successfully

    Scenario: Add tariff lines in draft version tab
        Given the user open the freight cost
        And add the follwing tariff line
            | FromPort | ToPort | Step3Price | Notes       |
            | LHR      | MIA    | 3          | Test Line 1 |
        When approve version
        Then the version should approve successfully

    Scenario: Edit air surcharge cost
        Given the user in air surchage workspace
        And open surchage with "AA" seller
        When create new version
        Then new version should create successfully

    Scenario: Add tariff lines in draft verstion tab
        Given the user remove the old tariff line 
        And add the follwing tariff line
            | FromPort | ToPort | StartDate |
            | LHR      | LAS    | Today     |
        When approve version
        Then the version should approve successfully

    Scenario: Open price check wizard to show price offers
        Given the user back into tariff workspace and open price check wizard
        And fill the following price check details
            | FromPort | ToPort | ChargeableWeight |
            | LHR      | MIA    | 240              |
        When search about prices
        Then air price should equal "720.00"
