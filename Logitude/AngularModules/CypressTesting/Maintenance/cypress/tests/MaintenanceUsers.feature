@smoke1 @release
Feature: User Create, Search and Edit from Maintenance
    The user creates a User, searches for and edits it from the Maintenance Module.
    and Log in with inactive user.

    Scenario: Create new User
        Given the user logged in and open "Users" in maintenance menu
        And a User with the following details
            | Email          | @mail.com          |
            | Password       | 123                |
            | ReTypePassword | 123                |
            | Name           | TestUser           |
            | Department     | Management         |
            | Branch         | Main Office        |
            | Notes          | this user for test |
        When create User
        Then the User should create successfully

    Scenario: Search for the User by Email
        When search User
        Then the User should appear successfully

    Scenario: Open the User
        When open the User
        Then the User should open successfully

    Scenario: Edit the User
        Given a "new note for test" as note
        When save the User
        Then the User should update successfully

    Scenario: Inactivate the User
        Given inactive the user
        When save the User
        Then the User should update successfully
        And the following event should appear in events tab
            | Event        | Notes            |
            | User Updated | User Inactivated |

    Scenario: Log in with inactive user
        Given the user logged in with the inactive user
        And change old password "123" to new password "!Cypress1"
        And re login
        Then an error message with "Your account has been deactivated!" should appear