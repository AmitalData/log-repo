@release @all @dev @daily
Feature: Credit Card Type fake Create and Edit in Maintenance Module
    The user creates a Credit Card Type fake create, then selects a different credit card type,
    edits it and activates or inactivates it from the Maintenance module.
    
    Scenario:Add Credit Card Type Code with lenght more than 2
        Given the user logged in and navigate to "Credit Card Types" in maintenance menu
        When add "123" as credit card type code
        Then a validation message with "Code Field must be less than 2" error should appear

    Scenario: Create a new credit card type
        Given a credit card type with the following details
            | Code        | Random                                  |
            | EnglishName | Testing Credit Card Type Daily Scenario |
        When create credit card type
        Then the credit card type should create successfully

    Scenario: Search for the credit card type by name
        When search for "TestCreditCardType" credit card type
        Then the "TestCreditCardType" credit card type should appear successfully

    Scenario: Open the credit card type
        When open credit card type
        Then the credit card type should open successfully

    Scenario: Edit the credit card type
        Given the user inactivate the credit card type
        When update credit card type
        Then the credit card type should update successfully
        And the following event should appear in events tab
            | Event                    | Notes                     |
            | Credit Card Type Updated | Credit Card Type "status" |