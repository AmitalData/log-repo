@release @all
Feature: Contact Create, Search, Open, Edit, Save and Anonymize in Maintenance Module
    The user creates a contact, search for, edits, saves and anonymizes it from the Maintenance Module.

    Scenario: Create new contact
        Given the user logged in and navigate to maintenance menu
        And open contacts list
        And open new contact wizard
        And fill the following contact details
            | Email               | Random              |
            | EnglishName         | TestContact         |
            | LocalName           | TestContact         |
            | Position            | QA                  |
            | BusinessPhone       | 9999999999          |
            | Mobile              | 9999999999          |
            | Fax                 | 999999              |
            | BirthdayDate        | Random              |
            | AnniversaryDate     | Today               |
            | BirthdayReminder    | Yes                 |
            | AnniversaryReminder | Yes                 |
            | Notes               | Test create contact |
        When create contact
        Then the contact should create successfully

    Scenario: Search about the contact
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
