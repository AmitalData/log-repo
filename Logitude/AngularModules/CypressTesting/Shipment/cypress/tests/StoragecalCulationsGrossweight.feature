@release @not-stable @all
Feature: Storage Calculations Gross Weight without Rounding

    The user sets up a warehouse with storage charges, creates a Direct Import Air shipment,
    adds a package with gross weight, adds a warehouse for storage calculation, adds dates,
    calculates fees/charges, checks receivables automatically added based on calculations of warehouse and creates an invoice.

    Scenario: Update warehouse
        Given the user logged in and navigate to warehouse workspace
        And open warehouse with "Testwarehouse" warehouse
        And fill with the following storage details for "CFS" Type
            | Currency | StorageFreeDays |
            | USD      | 2               |
        And "Air" weight details as following
            | Measurement  | Rounding |
            | Gross Weight | None     |
        And pricing defaults lines as following
            | StepFrom | NumberOfDays | StepTo | SalePrice |
            | 1        | 2            | 2      | 100       |
            | 3        | 2            | 4      | 200       |
        When update warehouse
        Then the warehouse should update successfully

    Scenario: Create direct export air shipment
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
            | 1        | 1      | 1     | 1      | 10          |
        When update shipment
        Then the direct shipment should update successfully


    Scenario: Add warehouse leg and check the calculation
        Given the user in the shipment's rounting tab
        And add new warehouse leg with "Testwarehouse" as Termina
        And fill "Today" as actual release and "8" days ago date as actual entry
        When calculate storage
        Then the Storage Fee should be "6,000.00"
        And Storage pricing should have weight "10" and Amount as following
            | Amount   |
            | 2,000.00 |
            | 4,000.00 |
        And a receivables line with the following details should appear
            | ChargesType    | Amount    |
            | Import Storage | 6,000.000 |


    Scenario: Add invoice
        When add new invoice with "Zero" vat type and number
        Then the invoice should add successfully

