@release @all @dev @devsmoke
Feature: Edit Company Address Settings from Maintenance
    The user edits the Company Address Settings from Maintenance Module

    Scenario: Edit the Company Address Settings
        Given the user logged in and open "Company Address Settings" in maintenance menu
        And a "Ramallah" as Address2 and "99988" as ZipCode
        When update the Company Address Settings
        Then the Company Address Settings should update successfully