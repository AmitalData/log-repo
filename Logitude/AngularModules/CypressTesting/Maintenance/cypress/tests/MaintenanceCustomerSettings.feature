@release @stable 
Feature: Update customers settings from Maintenance 

    The user enables phone, fax and address 1 fields to be mandatory when creating an active customer 
    and enables phone, fax to be mandatory when creating a potential customer then activates it.

    Scenario: Enable phone, fax and address 1 to be mandatory
        Given the user logged in and open "Customer Settings" in maintenance menu
        And a customer settings with the following details
            | IsCustomerTelphoneRequired          | Yes |
            | IsPotentialCustomerTelphoneRequired | Yes |
            | IsCustomerFaxRequired               | Yes |
            | IsPotentialCustomerFaxRequired      | Yes |
            | IsCustomerAddress1Required          | Yes |
        When update customer settings
        Then the settings should update successfully

    Scenario: Add potential customer
        Given the user navigates to customer workspace in CRM menu
        And a potential customer with the following details
            | CompanyName | TestCompany |
            | City        | Anchorage   |
            | Country     | US          |
            | State       | AK          |
            | PhoneNumber | 98765443    |
            | FaxNumber   | 98765443    |
            | AddContact  | No          |
        When add potential customer
        Then the customer should add successfully
        And the customer status should be "Potential"

    Scenario: Activate customer
        Given  an active customer with the following details
            | CompanyName | TestCompany |
            | City        | Anchorage   |
            | Country     | US          |
            | State       | AK          |
            | PhoneNumber | 98765443    |
            | FaxNumber   | 98765443    |
            | Address1    | Chester Ct  |
        When activate customer
        Then the customer should activate successfully
        And the customer status should change to "Active"