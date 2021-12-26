@devRelease
Feature: Edit Signature from Maintenance
    The user could edit signature  and add new data fields to the signature

    Scenario: Edit signature from maintenance
        Given the user logged in and navigate to "Signature" in maintenance menu
        And the user edits the HTML template as following
            | Date               | Date                 |
            | Signature          | Signature            |
            | Logo               | Logo                 |
            | SmallLogo          | Small Logo           |
            | WideLogo           | Wide Logo            |
            | LocalCurrency      | Local Currency       |
            | Company            | Company              |
            | Email              | Email                |
            | Website            | Website              |
            | IATA               | IATA                 |
            | VATNo              | VAT No.              |
            | AddressID          | Address ID           |
            | Contact            | Contact              |
            | SupporteMail       | Support e-mail       |
            | UserSignatureImage | User Signature Image |
        When the user saves the new Signature
        Then the Signature should update successfully