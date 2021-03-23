@release @dev @all
Feature: Create Country, Inactivate and activate it from Maintenance
    This scenario the user creates a Country, Inactivates it, selects it
    to edit and activates it from the Maintenance module.
    Scenario: Add Country
        Given the user logged in and navigate to "Countries" in maintenance menu
        And a country with the following details
            | CountryCode       | Random   |
            | CountryName       | Random   |
            | CountryLocalName  | Random   |
            | CountryGlobalZone | AF       |
            | InactiveCountry   | Yes      |
            | EC                | Yes      |
            | NorthAmerica      | Yes      |
            | IsStateRequired   | Yes      |
            | HasCities         | Yes      |
            | Notes             | TestNote |
        When add country
        Then the country should add successfully

    Scenario: Search for the Country by name
        When search for "Test" country
        Then the "Test" country should appear successfully

    Scenario: Open the country
        When open country
        Then the country should open successfully

    Scenario: Edit the country
        Given the user change InactiveCountry check box
        When edit country
        Then the country should update successfully
        Then following event should appear in events tab
            | Event           | Notes            |
            | Country Updated | Country "status" |