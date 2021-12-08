@release @stable
Feature:  Run, Print, Save and Send Automation Test Report in Reports Module
    The user Runs, Prints, Saves and Sends Automation Test Report in Reports Module.

    Scenario:Run Report with exception
        Given the user logged in and navigates to "Automation Test Report" in reports menu
        Given a report settings with "Yes" as is exception
        When run report
        Then the report should run successfully
        And the information messagee with "Exception Test" message should appear

    Scenario:Run Report without exception
        Given a report settings with "No" as is exception
        When run report
        Then the report should run successfully
        And the report should appear

    Scenario: Print report
        When print the report
        Then the report should print successfully

    Scenario: Save report as excel file
        When save report as "Excel File"
        Then the excel file should download successfully

    Scenario: Save report as excel file advanced
        Given export advanced settings with the following details
            | ExportDataOnly            | No  |
            | ExportObjectFormatting    | Yes |
            | UseOnePageHeaderAndFooter | Yes |
        When save report as "Excel File (Advanced)"
        Then the excel file should download successfully

    Scenario: Send report as Excel File
        Given a send report with the following details
            | SendType | Send as Excel File |
            | SendTo   | logged in user     |
        When send report
        Then the report should send successfully
        And the send message window should disappear

    Scenario: Send report as Pdf File
        Given a send report with the following details
            | SendType | Send as Pdf File |
            | SendTo   | logged in user   |
        When send report
        Then the report should send successfully
        And the send message window should disappear