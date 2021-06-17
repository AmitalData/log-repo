@NewDev
Feature: Currency Create, Search and Edit from Maintenance
    The user creates a Currency, searches for and edits it from the Maintenance Module.

    Scenario: Search for the Currency by code
        Given the user logged in and open "Currencies" in maintenance menu
        And  get currency code
        When search currency
        Then the currency should appear successfully

    Scenario: Open the Currency
        When open currency
        Then the currency should open successfully

    Scenario: Edit the Currency
        Given the user fill the following currency details
            | LocalName | EditCurrencyLocalNameTest |
        And  the user active or inactive currency
        And fill the following currency Accounting External ID
            | AccountingExternalID | CurrencyTest |
        When edit currency
        Then the currency should update successfully
        And following event should appear in events tab
            | Event            | Notes                             |
            | Currency Updated | Currency Inactivated or Activated |

    Scenario: Save and close the Currency
        When save and close currency
        Then the currency should close successfully

    Scenario: Create existing Currency
        Given a currency with the following details
            | Currency | samerandom |
            | Rate     | 100        |
        When create currency
        Then the currency should not create successfully
