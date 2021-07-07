@dev @daily
Feature: Package Type fake Create, Search and Edit from Maintenance
    The user creates a Package Type, searches for and edits it from the Maintenance Module.

    Scenario: Add Package Type Code with lenght more than 4
        Given the user logged in and open "Package Types" in maintenance menu
        When add "12345" as package type code
        Then a validation message with "Code Field must be less than 4" error should appear

    Scenario: Add Package Type PrintAs with lenght more than 20
        When add "123456789012345678901" as package type PrintAs
        Then a validation message with "Print As Field must be less than 20" error should appear

    Scenario: Create new package type with code already exists
        Given a package type with the following required details
            | Code    | 20BU         |
            | Name    | package type |
            | PrintAs | Test         |
            | Ocean   | Yes          |
        When create
        Then this validation message error "This package type already exists" should appear

    Scenario: Create new package type
        Given a package type with the following details
            | Code          | random        |
            | LocalName     | package type  |
            | TEU           | 100           |
            | ContainerSize | 100           |
            | Volume        | 100           |
            | Notes         | Package types |
            | IsContainer   | Yes           |
            | Refrigerated  | Yes           |
            | Air           | Yes           |
            | Inland        | Yes           |
        When create package type
        Then the package type should create successfully

    Scenario: Search for the package type by code
        When search for "20BU" package type
        Then the "20BU" package type should appear successfully

    Scenario: Open the package type
        When open package type
        Then the package type should open successfully

    Scenario: Edit the package type
        Given the user fill the following package type details
            | ContainerSize | 45     |
            | Volume        | 99     |
            | Notes         | random |
            | InActive      | Yes    |
        When edit package type
        Then the package type should update successfully
        And following event should appear in events tab
            | Event                |
            | Package Type Updated |

    Scenario: Save and close the package type
        When save and close package type
        Then the package type should close successfully