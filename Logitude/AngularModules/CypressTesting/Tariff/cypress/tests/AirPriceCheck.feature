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
            | FromPort | ToPort | MinPrice | Step1Price | Step2Price | Step3Price |
            | LHR      | MIA    | 10       | 20         | 30         | 40         |
        When approve version
        Then the version should approve successfully

    Scenario: Edit air surcharge cost if need
        Given the user in "Air" surchage workspace
        And open surchage with "American Airlines" as seller
        When copy into new version if start date is not "Today"
        Then new version should approve successfully

    Scenario: Open price check wizard to show price offers
        Given the user back into tariff workspace and open price check wizard
        And fill the following price check details
            | FromPort | ToPort | ChargeableWeight |
            | LHR      | MIA    | 10               |
        When search about prices
        Then air price should equal the following
            | AirFreight | Surcharges | Total  |
            | 200.00     | 300.00     | 500.00 |
