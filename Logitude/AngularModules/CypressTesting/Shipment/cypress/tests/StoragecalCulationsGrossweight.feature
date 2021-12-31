@release @FeatureToggle 
Feature: Storage Calculations Gross Weight without Rounding

    The user sets up a warehouse with storage charges, creates a Direct Import Air shipment,
    adds a package with gross weight, adds a warehouse for storage calculation, adds dates,
    calculates fees/charges, checks receivables automatically added based on calculations of warehouse and creates an invoice.

    Scenario: Set up a warehouse with storage charges
        Given the user logged in and navigate to warehouse workspace
        And open warehouse with "Testwarehouse" warehouse
        And fill with the following storage details for "CFS" Type
            | Currency        | NIS |
            | StorageFreeDays | 2   |
        And "Air" weight details as following
            | Measurement | Gross Weight |
            | Rounding    | None         |
        And pricing defaults lines as following
            | StepFrom | NumberOfDays | StepTo | SalePrice |
            | 1        | 2            | 2      | 100       |
            | 3        | 2            | 4      | 200       |
        When update warehouse
        Then the warehouse should update successfully

    Scenario: Create direct import air shipment
        Given the user in shipment workspace
        And a shipment with the following details
            | ShipmentLevel        | Direct              |
            | Direction            | Import              |
            | TransportMode        | Air                 |
            | Consignee            | TestConsigneeImport |
            | MainCarriageFromPort | LHR                 |
            | MainCarriageToPort   | MIA                 |
        When create shipment
        Then the shipment should create successfully

    Scenario: Add packages
        Given the user open the shipment and navigate to packages workspace
        And a package with the following details
            | Quantity | Length | Width | Height | GrossWeight |
            | 1        | 1      | 1     | 1      | 9.5         |
        When update shipment
        Then the direct shipment should update successfully


    Scenario: Add warehouse leg and check the calculation
        Given the user in the shipment's rounting tab
        And a warehouse leg with "Testwarehouse" as terminal
        And fill "Today" as actual release and "8" days ago date as actual entry
        When calculate storage
        Then Storage pricing should have weight "9.5" and Amount as following
            | Amount   |
            | 1,900.00 |
            | 3,800.00 |
        And the Storage Fee should be "5,700.00"
        And a receivables line with the following details should appear
            | ChargesType    | Amount    |
            | Import Storage | 5,700.000 |

    Scenario: Create an ARInvoice
        Given an ARInvoice with the following details
            | PartnerType     | Customer    |
            | InvoiceCurrency | USD         |
            | InvoiceDate     | Today       |
            | PaymentTerms    | Cash        |
            | DueDate         | Today       |
            | VATNo           | Zero        |
            | Branch          | Main Office |
            | VATType         | Zero        |
        When create invoice
        Then the invoice should create successfully