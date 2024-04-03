@release @stable
Feature:Change Password
    The user changes password from the Maintenance Module

    Scenario: Change Password
        Given the user logged in and navigates to change password window from maintenance menu
        And change user password with the following details
            | CurrentPassword | LoggedInUserPassword |
            | NewPassword     | ahmed13!A15          |
            | RetypePassword  | ahmed13!A15          |
        When change password
        Then the password should change successfully
