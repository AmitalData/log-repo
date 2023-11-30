@release3
Feature:  Update Shared Logistics Settings
    The user opens the Shared Logistics Settings and update the Activation Settings, Display Settings, Documents Permissions, Partners Permissions, Money Permissions

    # Scenario: Activate Shared Logistics if isn't Activated
    #     Given the user logged in
    #     And the user Activate Shared Logistics
    #     When the user save changes
    #     Then the Shared Logistics should Activated successfully

    Scenario: Update Activation Settings in Shared Logistics Settings
        Given the user logged in
        And the user navigates to Shared Logistics
        And update Shared Logistics Settings as following
            | ActivateWebAccess                       | yes |
            | ActivateMobile                          | yes |
            | SendLinkInEmailMessagesToYourPartners   | no  |
            | DisplayDocumentsAndEventsInMessagesLink | yes |
            | EnableMastersDocumentsLinkForAgents     | no  |
        When the user save changes
        Then the Shared Logistics should updated successfully

    Scenario: Update Activation Settings in Shared Logistics Settings
        Given the user navigates to Shared Logistics
        And update Shared Logistics Settings as following
            | ActivateWebAccess                       | no  |
            | ActivateMobile                          | no  |
            | SendLinkInEmailMessagesToYourPartners   | yes |
            | DisplayDocumentsAndEventsInMessagesLink | no  |
            | EnableMastersDocumentsLinkForAgents     | yes |
        When the user save changes
        Then the Shared Logistics should updated successfully

    Scenario: Update Documents Permissions in Shared Logistics
        Given the user navigates to Shared Logistics
        And update Documents Permissions as following
            | AirManifest      | yes |
            | AirwaybillLabels | yes |
            | ArrivalNotice    | yes |
        When the user save Documents Permissions changes
        Then the Documents Permissions should updated successfully

    Scenario: Update Documents Permissions in Shared Logistics
        Given the user navigates to Shared Logistics
        And update Documents Permissions as following
            | AirManifest      | no |
            | AirwaybillLabels | no |
            | ArrivalNotice    | no |
        When the user save Documents Permissions changes
        Then the Documents Permissions should updated successfully

    Scenario: Update Partners Permissions in Shared Logistics
        Given the user navigates to Shared Logistics
        And update Partners Permissions as following
            | Shipper              | yes |
            | Consignee            | yes |
            | Agent                | yes |
            | ShipperNotExporter   | yes |
            | ConsigneeNotImporter | yes |
            | Notify1              | yes |
            | Notify2              | no  |
        When the user save Partners Permissions changes
        Then the Partners Permissions should updated successfully

    Scenario: Update Partners Permissions in Shared Logistics
        Given the user navigates to Shared Logistics
        And update Partners Permissions as following
            | Shipper              | no  |
            | Consignee            | no  |
            | Agent                | no  |
            | ShipperNotExporter   | no  |
            | ConsigneeNotImporter | no  |
            | Notify1              | no  |
            | Notify2              | yes |
        When the user save Partners Permissions changes
        Then the Partners Permissions should updated successfully

    Scenario: Update Money Permissions in Shared Logistics
        Given the user navigates to Shared Logistics
        And update Money Permissions as following
            | InvoicesMenu                | yes |
            | MoneyTab                    | no  |
            | AmountInLocalCurrencyColumn | yes |
        When the user save Money Permissions changes
        Then the Money Permissions should updated successfully

    Scenario: Update Money Permissions in Shared Logistics
        Given the user navigates to Shared Logistics
        And update Money Permissions as following
            | InvoicesMenu                | no  |
            | MoneyTab                    | yes |
            | AmountInLocalCurrencyColumn | no  |
        When the user save Money Permissions changes
        Then the Money Permissions should updated successfully