@smoke
Feature: Payment Submission Blocking on Declaration Changes
    The system should block payment submission when declaration changes are made (IsChanged field in DB)

    Scenario: Basic Payment Blocking Test - Verify payment submission functionality
        Given the user logged in and navigates to Import workspace
        And search for file number "241126"
        And enter the file
        And navigate to "כללי" tab in declaration
        And log all available buttons on the page
        When try to find payment submission button
        Then payment submission functionality should be verified
