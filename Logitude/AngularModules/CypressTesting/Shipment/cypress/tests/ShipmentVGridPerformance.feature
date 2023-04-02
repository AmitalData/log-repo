@PerformanceVGrid
Feature: Shipment Update
  The user creates a Direct Export Air shipment and updates it.

  Scenario: Create direct export
    Given the user logged in and navigates to shipments workspace
    And a direct shipment with the following details
      | ShipmentLevel        | Direct            |
      | Direction            | Export            |
      | TransportMode        | Ocean             |
      | ShipmentType         | FCL               |
      | Shipper              | TestShipperExport |
      | MainCarriageFromPort | LHR               |
      | MainCarriageToPort   | MIA               |


  Scenario: Add Containers
    Given the user open the "AF5408" shipment and navigate to packages workspace
    Given  a container with the following details
      | PackageType | ContainerNumber | GrossWeight |
      | Bulk        | ABCD1234529     | 300.000     |
      | Flat Rack   | PQRS1875435     | 900.000     |
      | Bulk        | ABCD1234566     | 300.000     |
      | Flat Rack   | PQRS1875434     | 900.000     |
      | Bulk        | ABCD1234565     | 300.000     |
      | Flat Rack   | PQRS1875436     | 900.000     |
      | Bulk        | ABCD1234567     | 300.000     |
      | Flat Rack   | PQRS1875438     | 900.000     |
      | Bulk        | ABCD1234569     | 300.000     |
      | Flat Rack   | PQRS1875410     | 900.000     |
      | Bulk        | ABCD1234511     | 300.000     |
      | Flat Rack   | PQRS1875412     | 900.000     |
      | Bulk        | ABCD1234513     | 300.000     |
      | Flat Rack   | PQRS1875414     | 900.000     |
      | Bulk        | ABCD1234515     | 300.000     |
      | Flat Rack   | PQRS1875416     | 900.000     |
      | Bulk        | ABCD1234517     | 300.000     |
      | Flat Rack   | PQRS1875418     | 900.000     |
      | Bulk        | ABCD1234519     | 300.000     |
      | Flat Rack   | PQRS1875420     | 900.000     |
      | Bulk        | ABCD1234521     | 300.000     |
      | Flat Rack   | PQRS1875422     | 900.000     |
      | Bulk        | ABCD1234523     | 300.000     |
      | Flat Rack   | PQRS1875424     | 900.000     |
      | Bulk        | ABCD1234525     | 300.000     |
      | Flat Rack   | PQRS1875426     | 900.000     |
      | Bulk        | ABCD1234527     | 300.000     |
      | Flat Rack   | PQRS1875428     | 900.000     |
    When save shipment
    Then the direct shipment should save successfully


  Scenario: Update packages tab
     Given the user logged in and navigates to shipments workspace
    Given a direct shipment with the following details
      | ShipmentLevel        | Direct            |
      | Direction            | Export            |
      | TransportMode        | Air               |
      | Shipper              | TestShipperExport |
      | MainCarriageFromPort | LHR               |
      | MainCarriageToPort   | MIA               |
    Given the user open the "AF5403" shipment and navigate to packages workspace
    Given the user add package with the following details
      | Quantity | Length | Width | Height | GrossWeight |
      | 5        | 1      | 2     | 3      | 100         |
      | 5        | 1      | 2     | 3      | 100         |
      | 5        | 1      | 2     | 3      | 100         |
      | 5        | 1      | 2     | 3      | 100         |
      | 5        | 1      | 2     | 3      | 100         |
      | 5        | 1      | 2     | 3      | 100         |
      | 5        | 1      | 2     | 3      | 100         |
      | 5        | 1      | 2     | 3      | 100         |
      | 5        | 1      | 2     | 3      | 100         |
      | 5        | 1      | 2     | 3      | 100         |
      | 5        | 1      | 2     | 3      | 100         |
      | 5        | 1      | 2     | 3      | 100         |
      | 5        | 1      | 2     | 3      | 100         |
      | 5        | 1      | 2     | 3      | 100         |
      | 5        | 1      | 2     | 3      | 100         |
      | 5        | 1      | 2     | 3      | 100         |
      | 5        | 1      | 2     | 3      | 100         |
      | 5        | 1      | 2     | 3      | 100         |
      | 5        | 1      | 2     | 3      | 100         |
      | 5        | 1      | 2     | 3      | 100         |
      | 5        | 1      | 2     | 3      | 100         |
      | 5        | 1      | 2     | 3      | 100         |
      | 5        | 1      | 2     | 3      | 100         |
      | 5        | 1      | 2     | 3      | 100         |
    When update shipment
    Then the direct should update successfully

  Scenario: Update receivables tab
    Given  the user fill receivables with the following details
      | ChargesType | UOM  | Quantity | UnitPrice | Currency | ExchangeRate |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |

    When update shipment
    Then the direct should update successfully



  Scenario: Update payables tab
    Given the user add payables with the following details
      | ChargesType | UOM  | Quantity | UnitPrice | Currency | ExchangeRate |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
      | AFT         | GRWT | 1        | 10        | EUR      | 4            |
    When update shipment
    Then the direct should update successfully

