@NewDev @daily
Feature: Currency fake Create, Search and Edit from Maintenance
    The user creates a Currency, searches for and edits it from the Maintenance Module.

    Scenario: Creat Currency already exists
        Given the user logged in and open "Currencies" in maintenance menu
        And navigate currency wizard
        And  fill the following currency details
            | Currency         | USD |
            | ExchangeRate     | 10  |
            | ExchangeRateDate | .   |
        When create currency
        Then this validation message error "This currency already exists" should appear

    Scenario: Creat Currency
        And fill the following currency details
            | Currency         | DM |
            | ExchangeRate     | 10 |
            | ExchangeRateDate | .  |
        When create currency
        Then the currency should create successfully

    Scenario: Search for the currency by code
        When search for "" currency
        Then the "USD" currency should appear successfully

    Scenario: Open the Currency
        When open currency
        Then the currency should open successfully

    Scenario: Edit the Currency
        Given fill "new Notes" as notes currency
        And  the user Check the InActive Currency CheckBox
        And fill "externalId" as Accounting External ID
        When edit currency
        Then the currency should update successfully
        And following event should appear in events tab
            | Event            |
            | Currency Updated |

    Scenario: Save and close the Currency
        When save and close currency
        Then the currency should close successfully