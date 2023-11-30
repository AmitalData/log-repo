@release3
Feature:  Invite Customers In Shared Logistics
    The user opens the Shared Logistics, Enter the Invite customers view, add contact, invite this contact to shared logistics, and test a customer who is not invited and a customer who is accepted

    Scenario: Add contact in Shared Logistics
        Given the user logged in
        And the user navigates to Shared Logistics
        And insert new contact to the fisrt customer from Invite customers tab
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
        When the user Add the new contact
        Then the contact should created successfully

    Scenario: Invite the added contact
        When the user invite the added contact
        Then the contact should invited successfully
        And the following confirmation messeage "Invitation email sent to "+" with temporary password" appears
        And the customer is now in the invited customers list

    # test a customer who is not invited and make sure it is shown under not invited view
    Scenario: check a Not Invited customer should be shown in Invite Customers List
        Given the user get the first Customer in the Not Invited List
        When search for it in Invited list
        Then the search result shouldn't show the customer


    # test a customer who is accepted and be shown under the accepted view
    Scenario: check an Invited customer shouldn't be shown in Invite Customers List
        Given the user get the first Customer in the invited customers list
        When search for it in Not Invited list
        Then the search result shouldn't show the customer