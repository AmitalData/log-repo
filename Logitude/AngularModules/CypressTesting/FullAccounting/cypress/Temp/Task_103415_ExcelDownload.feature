@smoke @CloudSmokeTestingTag
Feature: ExcelDownload
    The user Enter quary, go to All journal and download to Excel

    Scenario: Create new Journal
        Given the user logged in and navigates to Full Accounting workspace
        And navigate journal workspace
        And enter to All journal
        And click on the Excel button

        When save as draft

        Then the journal should create successfully

   