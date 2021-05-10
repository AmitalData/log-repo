@release @dev @all
Feature: Create Country, Edit, inactivate or activate it from Maintenance
    The user creates a Country, Inactivates it, then selects a different country,
    edits it and activates or inactivates it from the Maintenance module.

    Scenario: Add CountryCode with lenght more than 2
        Given the user logged in and navigate to "Countries" in maintenance menu
        When add "123" as country code
        Then a validation message with "Code Field must be less than 2" error should appear

    Scenario: Add Country
        Given a country with the following details
            | CountryCode       | 29                   |
            | CountryName       | TestCountryName      |
            | CountryLocalName  | TestCountryLocalName |
            | CountryGlobalZone | AF                   |
            | InactiveCountry   | Yes                  |
            | EC                | Yes                  |
            | NorthAmerica      | Yes                  |
            | IsStateRequired   | Yes                  |
            | HasCities         | Yes                  |
            | Notes             | TestNote             |
        When add country
        Then the country should add successfully

    Scenario: Search for the Country by code in filter
        When search for "TS" country code
        Then the "Test" country should appear successfully

    Scenario: Open the country
        When open country
        Then the country should open successfully

    Scenario: Edit the country
        Given a "random" as CountryLocalName
        Given the user change InactiveCountry check box
        When edit country
        Then the country should update successfully
        Then following event should appear in events tab
            | Event           | Notes            |
            | Country Updated | Country "status" |