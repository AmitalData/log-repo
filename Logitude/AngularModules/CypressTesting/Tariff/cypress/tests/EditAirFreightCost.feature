@release @dev
Feature: Edit Air Freight Cost Tariff
    The authenticated user will create new air freight cost tariff,
    add new tariff lines in draft version tab,
    and edit general tab details.

    Scenario: Login and create new air freight cost
        Given the user logged in and navigate to tariff workspace
        And an air freight cost with the following details
            | Name      | TestAirFreightCost |
            | Seller    | AA                 |
            | StartDate | Today              |
            | Product   | General            |
        When create freight cost
        Then the freight cost should create successfully

    Scenario: Add tariff lines in draft version tab
        Given the user open the freight cost
        And add the following tariff lines
            | FromPort | ToPort | MinPrice | Step1Price | Step2Price | Step3Price | Step4Price | Step5Price | Step6Price | Notes       |
            | LHR      | MIA    | 10       | 20         | 30         | 40         | 50         | 60         | 70         | Test Line 1 |
            | AMM      | TLV    | 20       | 30         | 40         | 50         | 60         | 70         | 80         | Test Line 2 |
        When update freight cost
        Then the freight cost should update successfully

    Scenario: Edit general tab details
        Given the user in general tab
        And the freight cost with new following details
            | Name    | TestAirFreightCost2     |
            | Seller  | BA                      |
            | Product | Dangerous Goods         |
            | Notes   | Edit TestAirFreightCost |
        When update freight cost
        Then the freight cost should update successfully