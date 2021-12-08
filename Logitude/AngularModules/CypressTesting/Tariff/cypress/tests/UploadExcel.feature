@release @FeatureToggle
Feature: Upload Excel Tariff test
    The authenticated user will create new Air freight cost tariff.
    then upload excel file from PC .

    Scenario: Login and create new air freight cost
        Given the user logged in and navigate to tariff workspace
        And an air freight cost with the following details
            | Name           | TestAirFreightCost |
            | Seller         | AA                 |
            | StartDate      | Today              |
            | ExpirationDate | Today              |
            | Product        | General            |
        And the follwing All-In charges
            | Name             |
            | Agent Commission |
            | Air Waybill Fee  |
        When create freight cost
        Then the freight cost should create successfully

    Scenario: Upload Excel File
        Given the user open the created air freight cost
        When upload excel file
        Then the file should load successfully with the following details
            | FromPort   | MIA       |
            | ToPort     | JFK       |
            | Via        | 100       |
            | MinPrice   | 100.000   |
            | Step1Price | 450.000   |
            | Step2Price | 490.000   |
            | Step3Price | 560.000   |
            | Step4Price | 1,023.000 |
            | Step5Price | 3,309.000 |