@release @stable 
Feature: Wrong Email and Password Login and Reset Password
    The user fails to log in using wrong email and password and requests a password reset.

    Scenario:Login using wrong email
        Given the user in the Login page
        And login information with the following details
            | Email    | WrongEmail@Wrong.com |
            | Password | Random               |
        When login
        Then the login should fail
        And a validation message with "Login failed! invalid user name or password." error should appear

    Scenario:Login using wrong password
        Given the user in the Login page
        And login information with the following details
            | Email    | specflowtest@logitudeworld.com |
            | Password | WrongPassword                  |
        When login
        Then the login should fail
        And a validation message with "Login failed! invalid user name or password." error should appear

    Scenario:Send a password reset request
        Given the user in the Password Reset Request Page
        And "specflowtest@logitudeworld.com" as email
        When send a password reset request
        Then the request should send successfully
        And the following messages should appear
            | Message                                 |
            | Submiting completed successfully.        |
            | A reset link has been sent to your Email |