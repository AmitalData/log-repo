#@devrelease
Feature: Create Custom Field ,Edit and add it to Shipment Screen
    The user creates a department, searches for and edits it from the Maintenance Module.

    Scenario: Open Shipment module to add new custom field 
        Given the user logged in and open Customization in setting menu
        When search for shipment module
        Then shipment module will appears successfully

     Scenario: Add new custom fields With Text Type
        Given the user click on Add button to add Text field
        Given a text field with the following details
            | FieldLabel | CustomField1   |
            | Code       | CustomField1   |
            | DataType   | Text           |
            | MinLength  | 0              |
            | MaxLength  | 255            |
        When create text custome field
        Then the text custom field should create successfully

        
     Scenario: Add new custom fields with Code already exist
        Given the user click on Add button to add new field with Code already exist
        Given a field with the following details already exists
            | FieldLabel | CustomField1   |
            | Code       | CustomField1   |
            | DataType   | Text           |
            | MinLength  | 0              |
            | MaxLength  | 255            |
        When create custome field
        Then a validation error message with "An Object Field with the same code already exists" should appear
       
     Scenario: Add new custom fields with Boolean Type
        Given the user click on Add button to add boolean field
        Given a boolean field with the following details
            | FieldLabel | BooleanField   |
            | Code       | BooleanField   |
            | DataType   | Boolean        |
        When create boolean custome field,it will create successfully

  Scenario: Add new custom fields with Decimal Type
        Given the user click on Add button to add Decimal field
        Given a decimal with the following details
            | FieldLabel | DecimalField   |
            | Code       | DecimalField   |
            | DataType   | Decimal        |
        When create Decimal custome field,it will create successfully

