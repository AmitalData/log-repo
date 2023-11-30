@release3
Feature:  Adjust Documents Permissions In Shared Manifests Display Settings
    The user opens Shared Manifests tab in Operations and adjust the Documents Permissions

    Scenario: Update Documents Permissions in Shared Manifests for Direct shipments
        Given the user logged in
        And the user navigates to operations
        And chooses shared manifests tab
        And goes to Documents Permissions
        And update Documents Permissions for "Direct" shipments as follows
            | AirManifest      | yes |
            | AirwaybillLabels | no  |
            | ArrivalNotice    | yes |

        When the user save documents permissions changes
        Then the documents permissions should updated successfully

    Scenario: Update Documents Permissions in Shared Manifests for House shipments
        Given the user navigates to operations
        And chooses shared manifests tab
        And goes to Documents Permissions
        And update Documents Permissions for "House" shipments as follows
            | AirManifest      | no  |
            | AirwaybillLabels | no  |
            | ArrivalNotice    | yes |
        When the user save documents permissions changes
        Then the documents permissions should updated successfully

    Scenario: Update Documents Permissions in Shared Manifests for Master shipments
        Given the user navigates to operations
        And chooses shared manifests tab
        And goes to Documents Permissions
        And update Documents Permissions for "Master" shipments as follows
            | AirManifest      | yes |
            | AirwaybillLabels | no  |
            | ArrivalNotice    | yes |
        When the user save documents permissions changes
        Then the documents permissions should updated successfully

    Scenario: Update Documents Permissions in Shared Manifests for Direct shipments
        And the user navigates to operations
        And chooses shared manifests tab
        And goes to Documents Permissions
        And update Documents Permissions for "Direct" shipments as follows
            | AirManifest      | no  |
            | AirwaybillLabels | yes |
            | ArrivalNotice    | no  |
        When the user save documents permissions changes
        Then the documents permissions should updated successfully

    Scenario: Update Documents Permissions in Shared Manifests for House shipments
        And the user navigates to operations
        And chooses shared manifests tab
        And goes to Documents Permissions
        And update Documents Permissions for "House" shipments as follows
            | AirManifest      | no  |
            | AirwaybillLabels | yes |
            | ArrivalNotice    | no  |
        When the user save documents permissions changes
        Then the documents permissions should updated successfully

    Scenario: Update Documents Permissions in Shared Manifests for Master shipments
        And the user navigates to operations
        And chooses shared manifests tab
        And goes to Documents Permissions
        And update Documents Permissions for "Master" shipments as follows
            | AirManifest      | no |
            | AirwaybillLabels | no |
            | ArrivalNotice    | no |
        When the user save documents permissions changes
        Then the documents permissions should updated successfully