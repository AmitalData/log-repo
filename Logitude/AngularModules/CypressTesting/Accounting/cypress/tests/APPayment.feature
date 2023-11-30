@smoke @smoke3 @smoke2MasterDeploymnet
Feature: AP Payment
    The user creates new AP Payment and approve the AP Payment

    Scenario: Create new AP Payment
        Given the user logged in and navigates to Accounting workspace
        And an AP Payment with the following details
            | Vendor          | TestVendor |
            | PaymentMethod   | Cash       |
            | PaymentAmount   | 100        |
            | PaymentCurrency | NIS        |
            | RegisterDate    | 01/10/2021 |
        When save the AP Payment
        Then the AP Payment should save successfully

    Scenario: Approve the AP Payment
        When approve the AP Payment
        Then the AP Payment should approve successfully