@devrelease
Feature: Update VAT settings from Maintenance
    The user can change on VAT format type, is mandatory for and applies for

    Scenario: Set Applies For Customer and change on VAT Format Type
        Given the user logged in and open "VAT Settings" in maintenance menu
        And a VAT settings with the following details
            | AppliesFor     | Customers               |
            | VATFormatType  | Apply for All Countries |
            | IsMandatoryFor | Not Mandatory           |
            | VatSize        | 4                       |
        When update VAT Setting
        Then the VATSetting should update successfully

    Scenario: Set Applies For Customer and change on VAT Format Type
        Given the user logged in and open "VAT Settings" in maintenance menu
        And VAT settings with the following details
            | AppliesFor     | Customers     |
            | VATFormatType  | No Format     |
            | IsMandatoryFor | Not Mandatory |
        When update VAT Setting
        Then the VATSetting should update successfully