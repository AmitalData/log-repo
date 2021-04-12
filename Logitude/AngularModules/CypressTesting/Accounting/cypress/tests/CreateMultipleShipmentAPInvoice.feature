@smoke @release @dev 
Feature: Create multiple shipment AP Invoice
  After the user logging in the system and navigate to customers workspace
  will create a customer as shipper in the new shipments
  after that create multiple shipment AP Invoice for these shipments.

  Scenario: Update Accounting System
    Given the user logged in
    Given accounting System as "None"
    When change the accounting system
    Then the accounting system should update successfully

  Scenario: Create customer
    Given the user navigates to customers workspace
    And a customer with the following details
      | CompanyName | TestCompany |
      | City        | Anchorage   |
      | Country     | US          |
      | State       | AK          |
      | PhoneNumber | 98765443    |
      | FaxNumber   | 98765443    |
    When create customer
    Then the customer should create successfully

  Scenario: Create first direct export air shipment
    Given the user in shipments workspace
    And a direct shipment with the following details
      | ShipmentLevel        | Direct      |
      | Direction            | Export      |
      | TransportMode        | Air         |
      | Shipper              | TestCompany |
      | MainCarriageFromPort | LHR         |
      | MainCarriageToPort   | MIA         |
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
      | ChargesType  | AFT        |
      | UOM          | GRWT       |
      | Quantity     | 5          |
      | UnitPrice    | 10         |
      | Currency     | EUR        |
      | ExchangeRate | 4          |
      | Vendor       | TestVendor |
    When update shipment
    Then the direct should update successfully

  Scenario: Create second direct export air shipment
    Given the user in shipments workspace
    And a direct shipment with the following details
      | ShipmentLevel        | Direct      |
      | Direction            | Export      |
      | TransportMode        | Air         |
      | Shipper              | TestCompany |
      | MainCarriageFromPort | LHR         |
      | MainCarriageToPort   | MIA         |
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
      | ChargesType  | AFT        |
      | UOM          | GRWT       |
      | Quantity     | 5          |
      | UnitPrice    | 10         |
      | Currency     | EUR        |
      | ExchangeRate | 4          |
      | Vendor       | TestVendor |
    When update shipment
    Then the direct should update successfully

  Scenario: Create multiple shipment AP Invoice
    Given the user in Accounts Payable workspace
    And a multiple AP invoice  with a random invoice number and the following details
      | Vendor              | TestVendor  |
      | InvoiceAmount       | 50          |
      | InvoiceCurrency     | EUR         |
      | InvoiceExchangeRate | 4           |
      | InvoiceDate         | Today       |
      | PaymentTerms        | Cash        |
      | DueDate             | Today       |
      | VatNo               | 5           |
      | VATType             | Zero        |
      | Branch              | Main Office |
    When create invoice
    Then the invoice should create successfully

  Scenario: Add and edit shipment lines
    When the user add and edit shipment lines
    Then the invoice should update successfully

  Scenario: Approve AP Invoice
    When approve invoice
    Then the invoice should approve successfully