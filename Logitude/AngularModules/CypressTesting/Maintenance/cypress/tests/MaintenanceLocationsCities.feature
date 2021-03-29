@release @dev @all
Feature: Create City, Inactivate and activate it from Maintenance
    The user creates a City, selects it to edit, 
    selects it to edit again and Inactivates it from the Maintenance module.

    Scenario: Add CityCode with lenght more than 15
        Given the user logged in and navigate to "Cities" in maintenance menu
        When add "CityCode12345678" as city code
        Then a validation message with "Code Field must be less than 15" error should appear

    Scenario: Add City
        Given a city with the following details
            | CityCode      | random   |
            | CityName      | random   |
            | CityLocalName | random   |
            | Country       | US       |
            | State         | AK       |
            | Notes         | TestNote |
        When add city
        Then the city should add successfully

    Scenario: Search for the city by name
        When search for city
        Then the city should appear successfully

    Scenario: Open the city
        When open city
        Then the city should open successfully

    Scenario: Edit the city
        Given a "random" as cityLocalName
        And the user inactivate the city
        When edit city
        Then the city should update successfully
        And following event should appear in events tab
            | Event        | Notes            |
            | City Updated | City Inactivated |
