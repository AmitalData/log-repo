@smoke and @release
Feature: Create multiple shipment AP Invoice
  After the user logging in the system and navigate to customers workspace
  will create a customer as shipper in the new shipments
  after that create multiple shipment AP Invoice for these shipments.

  Scenario: Create customer
    Given the user logged in and navigates to customers workspace
    And a customer with the following details
      | CompanyName | City | Country | State |
      | TestCompany | LAS  | US      | AK    |
    When create customer
    Then the customer should create successfully

  Scenario: Update customer
    Given the user in the customer's billing tab
    When activate consolidated invoice option
    Then the customer should update successfully

  Scenario: Create first direct export air shipment
    Given the user in shipments workspace
    And a direct shipment with the following details
      | ShipmentLevel | Direction | TransportMode | Shipper     | MainCarriageFromPort | MainCarriageToPort |
      | Direct        | Export    | Air           | TestCompany | LHR                  | MIA                |
    When create shipment
    Then the first direct should create successfully

  Scenario: Update first shipment's packages tab
    Given the user add package with the following details
      | Quantity | Length | Width | Height | GrossWeight |
      | 5        | 1      | 2     | 3      | 100         |
    When update shipment
    Then the direct should update successfully

  Scenario: Update first shipment's payables tab
    Given the user add payable with the following details
      | ChargesType | UOM  | Quantity | UnitPrice | Currency | Vendor     |
      | AFT         | GRWT | 5        | 10        | EUR      | TestVendor |
    When update shipment
    Then the direct should update successfully

  Scenario: Create second direct export air shipment
    Given the user in shipments workspace
    And a direct shipment with the following details
      | ShipmentLevel | Direction | TransportMode | Shipper     | MainCarriageFromPort | MainCarriageToPort |
      | Direct        | Export    | Air           | TestCompany | LHR                  | MIA                |
    When create shipment
    Then the second direct should create successfully

  Scenario: Update second shipment's packages tab
    Given the user add package with the following details
      | Quantity | Length | Width | Height | GrossWeight |
      | 5        | 1      | 2     | 3      | 100         |
    When update shipment
    Then the direct should update successfully

  Scenario: Update second shipment's payables tab
    Given the user add payable with the following details
      | ChargesType | UOM  | Quantity | UnitPrice | Currency | Vendor     |
      | AFT         | GRWT | 5        | 10        | EUR      | TestVendor |
    When update shipment
    Then the direct should update successfully

  Scenario: Create multiple shipment AP Invoice
    Given the user in Accounts Payable workspace
    And a multiple AP invoice  with a random invoice number and the following details
      | Vendor     | InvoiceAmount | InvoiceCurrency | InvoiceExchangeRate | InvoiceDate | PaymentTerms | DueDate | VatNo | VATType |
      | TestVendor | 50            | EUR             | 4                   | Today       | Cash         | Today   | 5     | Zero    |
    When create invoice
    Then the invoice should create successfully

  Scenario: Add and edit shipment lines
    When the user add and edit shipment lines
    Then the invoice should update successfully

  Scenario: Approve AP Invoice
    When approve invoice
    Then the invoice should approve successfully