@release @all @dev @weekly 
Feature:Credit Card Type Create and Edit it in Maintenance Module
    The user creates a credit card type and edits it from the Maintenance Module.

    Scenario:Add Credit Card Type Code with lenght more than 2
        Given the user logged in and navigate to "Credit Card Types" in maintenance menu
        When add "123" as credit card type code
        Then a validation message with "Code Field must be less than 2" error should appear

    Scenario: Create a new credit card type
        Given a credit card type with the following details
            | Code        | Random                                   |
            | EnglishName | Testing Credit Card Type Weekly Scenario |
        When create credit card type
        Then the credit card type should create successfully

    Scenario: Search for the credit card type by code
        When search for credit card type
        Then the credit card type should appear successfully

    Scenario: Open the credit card type
        When open credit card type
        Then the credit card type should open successfully

    Scenario: Edit the credit card type
        Given "Test edit Name credit card type" as credit card type name
        When update credit card type
        Then the credit card type should update successfully
        And the following event should appear in events tab
            | Event                    |
            | Credit Card Type Updated |