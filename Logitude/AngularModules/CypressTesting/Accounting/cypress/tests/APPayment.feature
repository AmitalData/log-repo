@devsmoke
Feature: AP Payment
    The user creates new AP Payment and approve the AP Payment

    Scenario: Create new AP Payment
        Given the user logged in and navigates to Full Accounting workspace
        And an AP Payment with the following details
            | Vendor          | TestVendor |
            | PaymentMethod   | Cash       |
            | PaymentAmount   | 100        |
            | PaymentCurrency | NIS        |
        When save the AP Payment
        Then the AP Payment should save successfully

    Scenario: Approve the AP Payment
        When approve the AP Payment
        Then the AP Payment should approve successfully