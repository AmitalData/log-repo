@smoke @smoke3 @release @stable @smoke2MasterDeploymnet
Feature: Add and delete package from a quote
  After the user logging in the system and navigate to quotes workspace
  will create a customer as shipper in the new quote
  after that add and delete package from this quote.

  Scenario: Create customer
    Given the user logged in and navigates to customers workspace
    And a customer with the following details
      | CompanyName | TestCompany |
      | City        | Anchorage   |
      | Country     | US          |
      | State       | AK          |
      | PhoneNumber | 98765443    |
      | FaxNumber   | 98765443    |
    When create customer
    Then the customer should create successfully

  Scenario: Create export ocean LCL quote
    Given the user in quotes workspace
    And a quote with the following details
      | Direction            | Export      |
      | TransportMode        | Ocean       |
      | ShipmentType         | LCL         |
      | Shipper              | TestCompany |
      | MainCarriageFromPort | LHR         |
      | MainCarriageToPort   | MIA         |
    When create quote
    Then the quote should create successfully

  Scenario: Add package
    Given the user add a package with the following details
      | Quantity | PackageType | Volume | GrossWeight |
      | 5        | PP1         | 50     | 100         |
    When update quote
    Then the quote should update successfully

  Scenario: Delete package
    Given the user delete the package
    When update quote
    Then the quote should update successfully