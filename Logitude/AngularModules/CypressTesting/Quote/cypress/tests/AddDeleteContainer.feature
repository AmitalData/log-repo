@smoke @smoke3 @release @all
Feature: Add and delete container from a quote
  After the user logging in the system and navigate to quotes workspace
  after that add and delete container from this quote.

  Scenario: Create export ocean LCL quote
    Given the user logged in and navigates to quotes workspace
    And a quote with the following details
      | Direction            | Export      |
      | TransportMode        | Ocean       |
      | ShipmentType         | FCL         |
      | Shipper              | TestCompany |
      | MainCarriageFromPort | LHR         |
      | MainCarriageToPort   | MIA         |
    When create quote
    Then the quote should create successfully

  Scenario: Add container
    Given the user add a container with the following details
      | Quantity    | 100         |
      | PackageType | 20 Ft. Bulk |
    When update quote
    Then the quote should update successfully

  Scenario: Delete container
    Given the user delete the container
    When update quote
    Then the quote should update successfully