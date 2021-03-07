@release @all
Feature: Upload Excel Tariff test
    The authenticated user will create new Air freight cost tariff.
    then upload excel file from PC .

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

    Scenario: Upload Excel File
        Given the user open the created air freight cost
        When upload excel file
        Then the file should load successfully with the following details
            | FromPort | ToPort | MinPrice | Step1Price | Step2Price | Step3Price | Step4Price | Step5Price | Step6Price |
            | MIA      | JFK    | 100.000  | 100.000    | 450.000    | 490.000    | 560.000    | 1,023.000   | 3,309.000   |