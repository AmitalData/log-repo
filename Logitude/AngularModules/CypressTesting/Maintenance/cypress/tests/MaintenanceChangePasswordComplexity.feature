@release @all @stable
Feature: Change Password Complexity
    The user enters different combinations in the Change Password screen from the Maintenance Module

    Scenario: change password with empty fields
        Given the user logged in and navigates to change password window from maintenance menu
        When change user password with the following details
            | CurrentPassword |  |
            | NewPassword     |  |
            | RetypePassword  |  |
        Then these validation should be with the following colors
            | PasswordLengh                  | gray |
            | PasswordContainsUpperLowercase | gray |
            | PasswordContainsNumber         | gray |
        When change password
        Then the validation message with "Confirm your password" message should appear

    Scenario: enter password with more than 3 following characters with correct format
        Given the user in change password window
        When change user password with the following details
            | CurrentPassword |          |
            | NewPassword     | Abcd1234 |
            | RetypePassword  |          |
        Then the validation message with "Password should not contain more than 3 following characters" message should appear
        And these validation should be with the following colors
            | PasswordLengh                  | green |
            | PasswordContainsUpperLowercase | green |
            | PasswordContainsNumber         | green |

    Scenario: enter password with 7 following numbers
        Given the user in change password window
        When change user password with the following details
            | CurrentPassword |         |
            | NewPassword     | 1234567 |
            | RetypePassword  |         |
        Then the validation message with "Password should not contain more than 3 following characters" message should appear
        And these validation should be with the following colors
            | PasswordLengh                  | gray  |
            | PasswordContainsUpperLowercase | gray  |
            | PasswordContainsNumber         | green |

    Scenario: change password with 8 following numbers
        Given the user in change password window
        When change user password with the following details
            | CurrentPassword |          |
            | NewPassword     | 12345678 |
            | RetypePassword  |          |
        Then these validation should be with the following colors
            | PasswordLengh                  | green |
            | PasswordContainsUpperLowercase | gray  |
            | PasswordContainsNumber         | green |
        When change password
        Then the validation message with "The passwords you entered do not match." message should appear

    Scenario: enter password with 8 following small letters
        Given the user in change password window
        When change user password with the following details
            | CurrentPassword |          |
            | NewPassword     | abcdefgh |
            | RetypePassword  |          |
        Then the validation message with "Password should not contain more than 3 following characters" message should appear
        And these validation should be with the following colors
            | PasswordLengh                  | green |
            | PasswordContainsUpperLowercase | gray  |
            | PasswordContainsNumber         | gray  |

    Scenario: change password with wrong current password
        Given the user in change password window
        When change user password with the following details
            | CurrentPassword | WrongCurrentPassword |
            | NewPassword     | Aa11bb22             |
            | RetypePassword  | Aa11bb22             |
        Then these validation should be with the following colors
            | PasswordLengh                  | green |
            | PasswordContainsUpperLowercase | green |
            | PasswordContainsNumber         | green |
        When change password
        Then the validation message with "The current password is wrong!" message should appear


    Scenario: change password with length less than 8 characters
        Given the user in change password window
        When change user password with the following details
            | CurrentPassword | LoggedInUserPassword |
            | NewPassword     | Aa11bb2              |
            | RetypePassword  | Aa11bb2              |
        Then these validation should be with the following colors
            | PasswordLengh                  | gray  |
            | PasswordContainsUpperLowercase | green |
            | PasswordContainsNumber         | green |
        When change password
        Then the validation message with "Passwords must have a minimum length of 8 characters!" message should appear


    Scenario: change password without capital letters
        Given the user in change password window
        When change user password with the following details
            | CurrentPassword | LoggedInUserPassword |
            | NewPassword     | aa11bb22cc           |
            | RetypePassword  | aa11bb22cc           |
        Then these validation should be with the following colors
            | PasswordLengh                  | green |
            | PasswordContainsUpperLowercase | gray  |
            | PasswordContainsNumber         | green |
        When change password
        Then the validation message with "Your password must include an uppercase and lowercase letter." message should appear