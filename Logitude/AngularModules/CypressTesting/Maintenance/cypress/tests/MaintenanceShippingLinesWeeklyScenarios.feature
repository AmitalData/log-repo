@newdev @weekly
Feature: Shipping Line Create, Search and Edit from Maintenance
    The user creates a ShippingLine, searches for and edits it from the Maintenance Module.

    Scenario: Import Shipping Line
        Given the user logged in and open "Shipping Lines" in maintenance menu
        When fake import shipping Line
        Then the shipping Line should import successfully

    Scenario: Add Shipping Line Code with lenght more than 4
        Given the user navigate shipping line wizerd
        When add "12345" as shipping line code
        Then a validation message with "Code Field must be less than 4" error should appear

    Scenario: Add Shipping Line SCAC Code with lenght more than 4
        When add "12345" as shipping line SCAC code
        Then a validation message with "SCAC Code Field must be less than 4" error should appear

    Scenario: Create new Shipping Line
        Given a shipping line with the following details
            | Code     | random                |
            | SCACCode | random                |
            | Name     | Testing shipping Line |
            | Notes    | Testing shipping Line |
        When create shipping Line
        Then the shipping Line should create successfully

    Scenario: Search for the Shipping Line by code
        When search shipping Line
        Then the shipping Line should appear successfully

    Scenario: Open the Shipping Line
        When open shipping Line
        Then the shipping Line should open successfully

    Scenario: Check if the fields in INTTRA Tab are dim
        Given navigate INTTRA Tab
        Then the INTTRA input fields should be dim

    Scenario: Create shipping line address
        Given fill the following Address in Addresses Shipping line
            | AddressCountry | United States |
            | AddressCity    | las           |
            | AddressState   | Arkansas      |
        When create shipping line address
        Then the shipping line address should create successfully

    Scenario: Add Shipping Line Area Country Port
        Given add "GB" as country port area in Areas Tab
        When add country port area
        Then the country port area should add successfully

    Scenario: Add Shipping Line Area Port
        Given add "Mia" as port area in Areas Tab
        When add port area
        Then the port area should add successfully

    Scenario: Add Shipping Line Area
        Given fill the following Area details in Areas Tab
            | AreaName        | Adding New Area |
            | AreaDescription | Adding new Area |
        When create shipping line area
        Then the shipping line area should create successfully

    Scenario: Add Shipping Line Tariff Partner Code with lenght more than 50
        Given the user navigate shipping line tariff wizerd
        When add "012345678901234567890123456789012345678901234567891" as shipping line tariff partner code
        Then a validation message with "Partner Code Field must be less than 50" error should appear

    Scenario: Add Shipping Line Tariff
        Given fill the following Tariff Translation details
            | Partner Code | random |
            | TariffPort   | Miami  |
        When create shipping line Tariff translation
        Then the shipping line Tariff translation should create successfully

    Scenario: Inactivate Shipping Line and save changes
        Given the user Inactivate shipping line
        When edit shipping line
        Then the shipping line should update successfully
        And following event should appear in events tab
            | Event                 | Notes                     |
            | Shipping Line Updated | Shipping Line Inactivated |

    Scenario: Save and close the shippingLine
        When save and close shipping line
        Then the shipping line should close successfully