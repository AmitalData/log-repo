@smoke @task189500 @supplierAccount
Feature: Create and Delete Supplier Account
    The user creates a new Supplier Account, fills all the data including transport data and customs detail row, saves it, and then deletes it

    Scenario: Navigate to Import Declarations and open Supplier Accounts
        Given the user logged in and navigates to Import workspace
        And Search for file and enter to Supplier Accounts

            | File | 51840032 |

    Scenario: Create Supplier Account with all details
        Given Fill Supplier Account with the following details

            | AccountTypeCode          | חשבון מכר |
            | IssueDate                | TODAY |
            | InvoiceCurrencyTypeCode  | ILS |
            | IncotermCode             | CIF |
            | InvoiceNumber            | 111222333 |
            | InvoiceAmount            | 5000 |
            | IsPreference             | true |
            | PreferenceDocumentTypeCode | איחוד אירופי |
            | VendorId                 | 2152908 |

    Scenario: Fill Transport Data
        Given Fill Transport Data with the following details

            | TransportCurrencyTypeCode | ILS |
            | TransportAmount           | 50 |

    Scenario: Add and Fill Customs Detail Row
        Given Fill the New Customs Detail Row with the following details

            | ItemNo                  | 111222333 |
            | ItemDescription         | בדיקות אוטומטיות - TEST !@#$%^&*)("?1 |
            | Item                    | 90229000006 |
            | TradeAgreementCode      | 113 |
            | UnitsQuantity           | 10 |
            | UnitType                | KGM |
            | ValueInForeignCurrency  | 1000 |
            | OriginCountryCode       | DE |

        When saving the Supplier Account
        Then the Supplier Account should save successfully

    Scenario: Delete Supplier Account
        Given the user delete the Supplier Account row
        Then there is no Supplier Account row in the grid

