@release @all @dev
Feature: Create, Search, Open and Edit a Quote Template from Maintenance
    The user creates, searches for, opens and edits a quote template from Maintenance Module.

    Scenario: Create new Quote template
        Given the user logged in and open "Quote Templates" in maintenance menu
        And a quote template with "Random" name
        When create quote template
        Then the quote template should create successfully

    Scenario: Edit the quote template's Page Header
        And fill the "Page Header" settings with the following details
            | Width1 | 10 |
            | Width2 | 10 |
            | Width3 | 80 |
        When save quote template
        Then the quote template should update successfully

    Scenario: Edit the quote template's Quote Header
        Given drag and drop the following details in "Quote Header" settings
            | Field           | Column  |
            | Expiration Date | Column1 |
            | Customer        | Column1 |
        And edit "Customer" field in label tab to "Customer Name"
        When save quote header
        Then the quote header template should update successfully

    Scenario: Edit the quote template's Quote Introduction
        Given the user open the quote template
        And add "Shipper Name" field in "Quote Introduction" settings
        When save quote introduction template
        Then the quote template should update successfully

    Scenario: Edit the quote template's Quote Details
        Given drag and drop the following details in "Quote Details" settings
            | Field           | Column  |
            | Expiration Days | Column1 |
            | Shipper Name    | Column1 |
            | Consignee Name  | Column2 |
            | Shipper Address | Column2 |
        And edit "Shipper Name" field in label tab to "Shipper Name Test"
        When save quote header
        Then the quote header template should update successfully

    Scenario: Edit the quote template's Pricing Packages
        Given the user reopen the quote template
        And add the following columns fields in "Pricing Packages" settings
            | Column             |
            | Charge Code        |
            | Charge Description |
        When save quote pricing template
        Then the quote template should update successfully

    Scenario: Edit the quote template's Pricing Containers
        Given add the following columns fields in "Pricing Containers" settings
            | Column             |
            | Charge Code        |
            | Charge Description |
        When save quote pricing template
        Then the quote template should update successfully

    Scenario: Edit the quote template's Page Footer
        Given fill the "Page Footer" settings with the following details
            | Width1 | 10 |
            | Width2 | 10 |
            | Width3 | 80 |
        When save quote template
        Then the quote template should update successfully

