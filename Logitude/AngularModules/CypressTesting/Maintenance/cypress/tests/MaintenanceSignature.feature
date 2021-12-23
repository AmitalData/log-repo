@devRelease
Feature: Edit Signature from Maintenance
    The user could edit signature  and add new data fields to the signature

    Scenario: Edit signature from maintenance
        Given the user logged in and navigate to "Signature" in maintenance menu
        And the user edits the HTML template as following
            | Date               | Add      |
            | User               | don'tAdd |
            | Signature          | Add      |
            | Logo               | Add      |
            | SmallLogo          | Add      |
            | WideLogo           | Add      |
            | LocalCurrency      | Add      |
            | Company            | Add      |
            | Email              | Add      |
            | Website            | Add      |
            | IATA               | Add      |
            | VATNo              | Add      |
            | AddressID          | Add      |
            | Contact            | Add      |
            | SupporteMail       | Add      |
            | UserSignatureImage | Add      |
        When the user saves the new Signature
        Then the Signature should update successfully