@devrelease 
#@release @FeatureToggle
Feature: Storage Calculations Chargeable Weight, Rounding and Receivable Correction

    The user sets up a warehouse with storage charges, creates a Direct Import Ocean FCL shipment,
    adds a container with chargeable weight, adds a warehouse for storage calculation, adds dates,
    calculates fees/charges, checks receivables automatically added based on calculations of the warehouse,
    creates an invoice, changes dates and check receivable added as correction based on the storage calculation changes.

    Scenario: Set up a warehouse with storage charges
        Given the user logged in and navigate to warehouse workspace
        And open "Testwarehouse" warehouse
        And fill the following storage details
            | Currency        | NIS |
            | StorageFreeDays | 0   |
        And the following "Ocean" weight details
            | Measurement | Chargeable Weight |
            | Rounding    | 1                 |
        And the following pricing defaults lines
            | StepFrom | NumberOfDays | StepTo | SalePrice |
            | 0        | 3            | 2      | 100       |
            | 3        |              |        | 200       |
        When update warehouse
        Then the warehouse should update successfully

    Scenario: Create a direct import ocean FCL shipment
        Given the user in shipment workspace
        And a shipment with the following details
            | ShipmentLevel        | Direct              |
            | Direction            | Import              |
            | TransportMode        | Ocean               |
            | ShipmentType         | FCL                 |
            | Consignee            | TestConsigneeImport |
            | MainCarriageFromPort | LHR                 |
            | MainCarriageToPort   | MIA                 |
        When create shipment
        Then the shipment should create successfully

    Scenario: Add a container with chargeable weight
        Given the user open the shipment and navigate to packages tab
        And a container with the following details
            | PackageType | GrossWeight | ChargeableWeight |
            | PC2         | 100         | 1.3              |
        When update shipment
        Then the shipment should update successfully

    Scenario: Add the warehouse with dates for storage calculation
        Given the user in the shipment's rounting tab
        And a warehouse leg with "Testwarehouse" as terminal
        And "Today" as actual entry and expected release after "5" days
        When calculate storage
        Then storage pricing should have weight "2" with the following amounts
            | Amount |
            | 600.00 |
            | 800.00 |
        And storage fee should be "1,400.00"
        And a receivables line with the following details should appear
            | ChargesType    | Amount    |
            | Import Storage | 1,400.000 |

    Scenario: Create an ARInvoice
        Given an ARInvoice with the following details
            | PartnerType     | Customer    |
            | InvoiceCurrency | NIS         |
            | InvoiceDate     | Today       |
            | PaymentTerms    | Cash        |
            | DueDate         | Today       |
            | VATNo           | Zero        |
            | Branch          | Main Office |
            | VATType         | Zero        |
        When create invoice
        Then the invoice should create successfully

    Scenario: Update the warehouse dates for new storage calculation
        Given the user in the shipment's rounting tab
        And edit the expected release date to be after "6" days
        When calculate storage
        Then storage pricing should have weight "2" with the following amounts
            | Amount   |
            | 600.00   |
            | 1,200.00 |
        And storage fee should be "1,800.00"
        And a second receivables line with the following details should appear
            | ChargesType    | Amount  |
            | Import Storage | 400.000 |




