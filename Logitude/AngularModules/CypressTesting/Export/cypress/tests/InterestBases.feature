@smoke
Feature: Interest Bases
    The user creates new Interest Base and add Interest Period

    Scenario: Create new Interest Base
        Given the user logged in and navigates to Full Accounting workspace
        And an interest base with the following details
            | Code        | Random               |
            | EnglishName | new Interest         |
            | LocalName   | new Interest         |
            | Description | Interest description |
        When create interest base
        Then the interest base should create successfully

    Scenario: Add new Interest Period
        Given interest period with the following details
            | InterestBaseRate      | 16       |
            | InterestBaseStartDate | 1/9/2021 |
        When save interest base
        Then the interest base should update successfully