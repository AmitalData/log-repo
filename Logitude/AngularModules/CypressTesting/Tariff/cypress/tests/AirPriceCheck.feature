@release @FeatureToggle
Feature: Air Price Check
    The authenticated user will create new air freight cost tariff,
    add tariff line in draft version tab, then approve it,
    and open price check wizard to show price offers.

    Scenario: Login and create new air freight cost
        Given the user logged in and navigate to tariff workspace
        And an air freight cost with the following details
            | Name           | TestAirFreightCost |
            | Seller         | Air Astana         |
            | StartDate      | Today              |
            | ExpirationDate | Today              |
            | Product        | General            |
        When create freight cost
        Then the freight cost should create successfully

    Scenario: Add tariff lines in draft version tab
        Given the user open the freight cost
        And add the following tariff line
            | FromPort | ToPort | MinPrice | Step1Price | Step2Price | Step3Price |
            | LHR      | MIA    | 10       | 20         | 30         | 40         |
        When approve version
        Then the version should approve successfully

    Scenario: create air surcharge cost
        Given an air surcharge cost with the following details
            | Name   | TestAirSurchargeCost |
            | Seller | Air Astana           |
        And add the following surcharges
            | Name             |
            | Agent Commission |
            | Air Waybill Fee  |
        When create surcharge cost
        Then the surcharge cost should create successfully

    Scenario: Edit Surcharge
        Given the user in "Air" surchage workspace
        And open surchage with "Air Astana" as seller
        And add the following surcharge line
            | FromPort | ToPort | StartDate | Step1Price | Step2Price |
            | LHR      | MIA    | Today     | 10         | 280        |
        When copy into new version if start date is not "Today"
        Then new version should approve successfully

    Scenario: Open price check wizard to show price offers
        Given the user back into tariff workspace and open price check wizard
        And fill the following price check details
            | FromPort         | LHR |
            | ToPort           | MIA |
            | ChargeableWeight | 10  |
        When search about prices
        Then air price should equal the following
            | AirFreight | 200.00 ₪ |
            | Surcharges | 300.00 ₪ |
            | Total      | 500.00 ₪ |