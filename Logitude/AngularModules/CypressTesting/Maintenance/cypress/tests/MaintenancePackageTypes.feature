@release @stable @all
Feature: Package Type Create, Search and Edit from Maintenance
    The user creates a Package Type, searches for and edits it from the Maintenance Module.

    Scenario: Create new package type
        Given the user logged in and open "Package Types" in maintenance menu
        And a package type with the following details
            | Code          | random |
            | Name          | Test   |
            | LocalName     | Test   |
            | TEU           | random |
            | ContainerSize | random |
            | Volume        | random |
            | PrintAs       | Test   |
            | Air           | Yes    |
            | InActive      | Yes    |
        When create package type
        Then the package type should create successfully

    Scenario: Search for the package type by number
        When search package type
        Then the package type should appear successfully

    Scenario: Open the package type
        When open package type
        Then the package type should open successfully

    Scenario: Edit the package type
        Given a "EditTestPackageType" as packageTypeLocalName
        And  the user activate package type
        When edit package type
        Then the package type should update successfully
        And following event should appear in events tab
            | Event                | Notes                  |
            | Package Type Updated | Package Type Activated |