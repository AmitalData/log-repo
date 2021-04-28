@release @stable @all
Feature: AWB Edit
   The user creates a Direct Export Air shipment, edits the AWB Wizard, 
   checks that the overview tabs appears, adds packages and saves.
   
    Scenario: Login and create direct export air shipment
        Given the user logged in and navigate to shipments workspace
        And a direct shipment with the following details
            | ShipmentLevel        | Direct            |
            | Direction            | Export            |
            | TransportMode        | Air               |
            | Shipper              | TestShipperExport |
            | MainCarriageFromPort | LHR               |
            | MainCarriageToPort   | MIA               |
        And main carriage airline is "AA" with random flight number and MAWB
        When create shipment
        Then the shipment should create successfully

    Scenario: Open direct AWB wizard to show overview tab
        Given the user open the direct shipment
        When open the AWB wizard
        Then the overview tab should appear successfully

    Scenario: Edit direct AWB by add packages
        Given the user in the AWB wizard packages tab
        And add the following packages
            | Quantity | Length | Width | Height | GrossWeight |
            | 1        | 20     | 40    | 60     | 100         |
            | 2        | 30     | 50    | 70     | 200         |
        When save the AWB wizard
        Then the shipment should update successfully