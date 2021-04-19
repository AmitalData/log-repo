@release @all @dev
Feature:Change Password
The user changes password from the Maintenance Module 

    Scenario: Change Password
        Given the user logged in and navigates to change password window from maintenance menu
        And "Spec_Flow21" as a current password and "ahmed13!A15" as a new paswword
        When change password
        Then the password should change successfully
