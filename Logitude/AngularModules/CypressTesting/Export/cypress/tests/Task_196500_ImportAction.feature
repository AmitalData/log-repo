@smoke @task196500 @importAction @declarationForm
Feature: Request for Declaration Form (בקשה לקבלת טופס הצהרה)
    Verify that the user can request and open the Declaration Form from the Import Declarations workspace

    Background:
        Given the user is logged in
        And navigates to Import Declarations workspace

    Scenario: Open Declaration Form from first result
        When the user selects the first result from the grid
        And clicks on "Forms" menu
        And clicks on "Declaration Form"
        Then a popup dialog should appear
        When the user clicks OK/Confirm in the popup
        Then the action should complete successfully

