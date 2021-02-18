@release @all
Feature: Create New Freight Cost Tariff
    The authenticated user will create new air freight cost tariff.
    add tariff line and check the cost

    Scenario: Login and create new air freight cost
        Given the user logged in and navigate to tariff workspace
        And an air freight cost with the following details
            | Name               | Seller | StartDate | Product |
            | TestAirFreightCost | AA     | Today     | General |
        And the follwing All-In charges
            | Name             |
            | Agent Commission |
            | Air Waybill Fee  |
        When create freight cost
        Then the freight cost should create successfully

    Scenario: Add tariff lines in draft version tab
        Given the user open the freight cost
        And add the follwing tariff line
            | FromPort | ToPort | Step3Price | Notes       |
            | LHR      | MIA    | 3          | Test Line 1 |
        When update freight cost
        Then the freight cost should update successfully

    Scenario: Check Air Price Check
        Given the user in the air's price check workspace
        When enter the following details
            | FromPort | ToPort | ChargeableWeight |
            | LHR      | MIA    | 240              |
        Then the result should be "720.00"