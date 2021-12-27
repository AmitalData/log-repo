@devrelease 
Feature: Update VAT settings from Maintenance 
    The user can change on VAT format type, is mandatory for and applies for 

    Scenario: Set Applies For Customer and change on VAT Format Type
        Given the user logged in and open "VAT Settings" in maintenance menu
        And a VAT settings with the following details
            | AppliesFor      | Customers                   |
            | VATFormatType   | Apply For All Countries     |
            | IsMandatoryFor  | Mandatory For All Countries |
            | VatSize         | 5                           |
        When update VAT Setting
        Then the VATSetting should update successfully
