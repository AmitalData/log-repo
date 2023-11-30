@release @stable 
Feature: Contact Create, Edit, Save and Anonymize
    The user creates a contact, search for, edits, saves and anonymizes it from the Maintenance Module.

    Scenario: Create new contact
        Given the user logged in and open contacts menu
        And a contact with the following details
            | Email               | Random              |
            | EnglishName         | TestContact         |
            | LocalName           | TestContact         |
            | Position            | QA                  |
            | BusinessPhone       | 9999999999          |
            | Mobile              | 9999999999          |
            | Fax                 | 999999              |
            | BirthdayDate        | 1/1/2000            |
            | AnniversaryDate     | Today               |
            | BirthdayReminder    | Yes                 |
            | AnniversaryReminder | Yes                 |
            | Notes               | Test create contact |
        When create contact
        Then the contact should create successfully

    Scenario: Search for the contact by email
        When search contact
        Then the contact should appear successfully

    Scenario: Open the contact
        When open contact
        Then the contact should open successfully

    Scenario: Edit the contact
        Given the user fill the following contact details
            | EnglishName   | EditTestContact   |
            | LocalName     | EditTestContact   |
            | Position      | Developer         |
            | BusinessPhone | 8888888888        |
            | Mobile        | 8888888888        |
            | Fax           | 888888            |
            | Notes         | Test edit contact |
        When edit contact
        Then the contact should edit successfully

    Scenario: Anonymize the contact
        When anonymize contact
        Then the contact should anonymize successfully
        And contact details should change successfully