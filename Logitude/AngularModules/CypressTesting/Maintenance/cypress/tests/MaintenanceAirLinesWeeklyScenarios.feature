@dev @weekly
Feature: Airlines Create, Search and Edit from Maintenance
    The user creates an airline, searches for and edits it from the Maintenance Module.

    Scenario: Import Air Line
        Given the user logged in and open "Airlines" in maintenance menu
        When fake import air line
        Then the air line should import successfully

    Scenario: Add Airline Code with lenght more than 2
        Given the user navigate air line Wizard
        When add "123" as air line code
        Then a validation message with "Code Field must be less than 2" error should appear

    Scenario: Add Airline ICAO with lenght 2
        When add "12" as air line ICAO
        Then a validation message with "ICAO Field must be less than 3 and more than 3" error should appear

    Scenario: Add Airline ICAO already exists
        When add "AAL" as airline ICAO
        Then a validation single error message with "An airline with same ICAO already exists!" should appear

    Scenario: Add Airline Prefix with lenght more than 3
        When add "test" as airline prefix
        Then a validation message with "Prefix Field must be less than 3" error should appear

    Scenario: Create new Airline
        And an air line with the following details
            | Code      | random          |
            | ICAO      | random          |
            | Name      | Testing airline |
            | Prefix    | 011             |
            | LocalName | Testing airline |
            | Notes     | Testing airline |
        When create air line
        Then the air line should create successfully

    Scenario: Search for the Airline by code
        When search the air line
        Then the air line should appear successfully

    Scenario: Open the Airline
        When open air line
        Then the air line should open successfully

    Scenario: Create Air line address
        Given fill the following Address details in Addresses air line tab
            | Name    | Testing Airlines |
            | Country | United States    |
            | City    | Anchorage        |
            | State   | Arkansas         |
        When create air line address
        Then the air line address should create successfully

    Scenario: Add Air Line Tariff Partner Code with lenght more than 50
        Given the user navigate air line tariff Wizard
        When add "012345678901234567890123456789012345678901234567891" as air line tariff partner code
        Then a validation message with "Partner Code Field must be less than 50" error should appear

    Scenario: Add Air Line Tariff
        Given fill the following tariff translation details
            | TariffPartnerCode | random |
            | TariffPort        | Miami  |
        When create air line tariff translation
        Then the air line tariff translation should create successfully

    Scenario: Add Air Line Surcharge Tariff
        Given fill the following surcharge tariff details
            | SurchargeTariffFromDate | current date |
            | SurchargeTariffToDate   | current date |
        And add tariff charge
            | TariffChargeType      | THC Origin |
            | TariffChargeUnitPrice | 100        |
        When create air line surcharge tariff
        Then the air line surcharge tariff should create successfully

    Scenario: Add Special Handling Code from Adaptations tab with lenght more than 4
        Given the user navigate Special Handling Codes Wizard
        When add "12345" as Special Handling Code
        Then a validation message with "Code Field must be less than 4" error should appear

    Scenario: Add Air Line AWB Special Handling Code
        Given add new special handling code from Adaptions Tab
            | AdaptionSpecialHandlingCode | 1234             |
            | AdaptionSpecialHandlingName | Handle With Care |
        When create air line awb special handling code
        Then the air line awb special handling code should create successfully

    Scenario: Inactivate Air Line and save changes
        Given the user Inactivate air line
        When save air line
        Then the air line should update successfully
        And the following event should appear in events tab
            | Event           | Notes               |
            | Airline Updated | Airline Inactivated |

    Scenario: Save and close the Airline
        When save and close air line
        Then the air line should close successfully