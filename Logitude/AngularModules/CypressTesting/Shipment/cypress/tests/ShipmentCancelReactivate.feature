@smoke @release @not-stable @all
#cancel it's smoke and relese
#reactivate is release
Feature: Cancel and Reactivate Shipment
  The user creates a shipment, cancels the shipment,
  makes sure that the system does not allow the user to edit the shipment,
  reactivates the shipment and makes sure that the system allows the user to edit the shipment again.

  Scenario: Create direct export air shipment
    Given the user logged in and navigates to shipments workspace
    And a direct shipment with the following details
      | ShipmentLevel | Direction | TransportMode | Shipper           | MainCarriageFromPort | MainCarriageToPort |
      | Direct        | Export    | Air           | TestShipperExport | LHR                  | MIA                |
    When create shipment
    Then the direct should create successfully

  Scenario: Cancel direct shipment
    Given the user open the shipment
    When cancel the shipment with "Cancel The Shipment" Note
    Then the shipment should cancel successfully

  Scenario: Reactivate direct shipment
    When reactivate the shipment with "Reactivate The Shipment" Note
    Then the shipment should reactivate successfully