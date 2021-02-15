@release
Feature: Air Freight Cost Tariff Version History
    The authenticated user will create new air freight cost tariff,
    add new tariff lines in draft version tab, then approve it,
    copy it into new version with different values, then approve the copied version,
    and open version history tab to show the approved versions.

    Scenario: Login and create new air freight cost
        Given the user logged in and navigate to tariff workspace
        And an air freight cost with the following details
            | Name               | Seller | StartDate | Product |
            | TestAirFreightCost | AA     | Today     | General |
        When create freight cost
        Then the freight cost should create successfully

    Scenario: Add tariff lines in draft version tab
        Given the user open the freight cost
        And add the follwing tariff lines
            | FromPort | ToPort | MinPrice | Step1Price | Step2Price | Step3Price |
            | LHR      | MIA    | 10       | 20         | 30         | 40         |
            | AMM      | TLV    | 20       | 30         | 40         | 50         |
        When approve version
        Then the version should approve successfully

    Scenario: Copy into new version
        When copy version with start date "Tomorrow"
        Then the version should copy successfully

    Scenario: Edit tariff lines in the new version tab
        Given the follwing new values for the tariff lines
            | MinPrice | Step1Price | Step2Price | Step3Price |
            | 20       | 30         | 40         | 50         |
            | 30       | 40         | 50         | 60         |
        When approve version
        Then the version should approve successfully