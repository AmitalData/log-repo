Feature:  Select Phone, Fax and Address 1 Fields as Mandatory for Potential and Active Customers from Maintenance

    The user enables phone, fax and address 1 fields to be mandatory when creating a
    potential or activating a customer, creates a potential customer then activates it.

    Scenario: Enable phone, fax and address 1 to be mandatory
        Given the user logged in and open "Customer Settings" in maintenance menu
        And a Customer Settings with the following details
            | Phone Required for Active Customer    | Yes |
            | Phone Required for Potential Customer | Yes |
            | Fax Required for Active Customer      | Yes |
            | Fax Required for Potential Customer   | Yes |
            | Active Address1 Required              | Yes |
        When update customer settings
        Then the settings should update successfully

    Scenario: Create potential customer
        Given the user navigates to customer workspace from CRM menu
        And a potential customer with the following details
            | CompanyName         | TestCompany |
            | City                | Anchorage   |
            | Country             | US          |
            | State               | AK          |
            | Phone               | 09887665    |
            | Fax                 | 09987668    |
            | Add Primary Contact | No          |
        When create customer
        Then the customer should create successfully
        And the customer status should be "Potential"

    Scenario: Activate customer
        Given  a potential customer with the following details
            | CompanyName | TestCompany   |
            | City        | Anchorage     |
            | Country     | US            |
            | State       | AK            |
            | Phone       | 09887665      |
            | Fax         | 09987668      |
            | address1    | RandomAddress |
        When activate customer
        Then the customer activate create successfully
        And the customer status should be "Active"