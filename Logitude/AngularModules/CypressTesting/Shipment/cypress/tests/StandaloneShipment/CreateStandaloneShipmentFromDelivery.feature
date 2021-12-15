
Feature: Create standalone shipment from Delivery
    The user creates a Direct Import Ocean FCL shipment,Create standalone shipment when Delivery from partner to port with the same countries,
    Create standalone shipment when the Delivery is FullResponsibility and "To/From" are partners with different countries,
    Create standalone shipment when the Delivery is FullResponsibility and "To/From" are partners with same countries,
    Create standalone shipment when the Delivery is not FullResponsibility and "To/From" are partners with same countries
    Create standalone shipment when the Delivery is FullResponsibility and from Partner to Casual Address with same countries,
    Create standalone shipment when the Delivery is FullResponsibility and from port to port with same countries,
    Create standalone shipment when the Delivery is FullResponsibility and from Casual Address to Casual Address with same countries,
    Create standalone shipment when the Delivery is FullResponsibility and from Casual Address to port with same countries

    Scenario: Create import ocean FCL shipment
        Given the user logged in and navigates to shipments workspace
        And a shipment with the following details
            | ShipmentLevel        | Direct              |
            | Direction            | Import              |
            | TransportMode        | Ocean               |
            | ShipmentType         | FCL                 |
            | Shipper              | Israeli Tenant      |
            | Consignee            | TestConsigneeImport |
            | MainCarriageFromPort | LHR                 |
            | MainCarriageToPort   | MIA                 |
        When create shipment
        Then the shipment should create successfully

    Scenario: Create standalone shipment when Delivery from partner to port with the same countries
        Given the user in the shipment's routings tab
        And add a new Delivery leg with the following details
            | FullResponsibility | True                      |
            | FromName           | Partner                   |
            | PartnerName        | ALS CUSTOMS SERVICES GMBH |
            | ToName             | Port                      |
            | Port               | Brandscheid/Westerwald    |

        And save the Delivery
        When create standalone shipment
        Then a domestic inland shipment should create
        And the cancel ,operational close Shipment,convert to custom file and Send Response action in more button shouldn't be dim
        And all other actions should be dim
        And all fields should be dim in Delivery window
        And the link of standalon should display


    Scenario: Create standalone shipment when the Delivery is FullResponsibility and "To/From" are partners with different countries
        Given the user in the shipment's  routings tab
        And add a Delivery leg with the following details
            | From     | Partner           |
            | FromName | TestShipperExport |
            | To       | Partner           |
            | ToName   | TestCompany       |
        And save the Delivery
        When create standalone shipment
        Then a validation should disply that Both Addresses must be in the same country since the direction is Domestic

    Scenario: Create standalone shipment when the Delivery is FullResponsibility and "To/From" are partners with same countries
        Given the user in the shipment's routings tab
        And add a Delivery leg with the following details
            | From     | Partner           |
            | FromName | TestShipperExport |
            | To       | Partner           |
            | ToName   | TestShipperExport |
        And save the Delivery
        When create standalone shipment
        Then a domestic inland shipment should create
        And the cancel ,operational close Shipment,convert to custom file and Send Response action in more button shouldn't be dim
        And all other actions should be dim
        And all fields should be dim in Delivery window
        And the link of standalon should display

    Scenario: Create standalone shipment when the Delivery is not FullResponsibility and "To/From" are partners with same countries
        Given the user in the shipment's routings tab
        And add a new Delivery leg with the following details
            | From     | Partner           |
            | FromName | TestShipperExport |
            | To       | Partner           |
            | ToName   | TestShipperExport |
        When save the Delivery
        Then the shipment should update successfully
        And the "Create Standalone Shipment" is dim



    Scenario: Create standalone shipment when the Delivery is FullResponsibility and from Partner to Casual Address with same countries
        Given the user in the shipment's routong tab
        And  add a new Delivery leg with the following details
            | From     | Partner            |
            | FromName | khalid             |
            | To       | Casual Address     |
            | Country  | State Of Palestine |
            | City     | Ramallah           |
        And save the Delivery
        When create standalone shipment
        Then a domestic inland shipment should create
        And the cancel ,operational close Shipment,convert to custom file and Send Response action in more button shouldn't be dim
        And all other actions should be dim
        And all fields should be dim in Delivery window
        And the link of standalon should Delivery



    Scenario: Create standalone shipment when the Delivery is FullResponsibility and from port to port with same countries
        Given the user in the shipment's routong tab
        And add a new Delivery leg with the following details
            | From     | Port                   |
            | FromName | Brandscheid/Westerwald |
            | To       | Port                   |
            | ToName   | Brandshagen            |
        And save the pickup
        When create standalone shipment
        Then a domestic inland shipment should create
        And the cancel ,operational close Shipment,convert to custom file and Send Response action in more button shouldn't be dim
        And all other actions should be dim
        And all fields should be dim in Delivery window
        And the link of standalon should display


    Scenario: Create standalone shipment when the Delivery is FullResponsibility and from Casual Address to Casual Address with same countries
        Given the user in the shipment's routong tab
        And add a new Delivery leg with the following details
            | From    | Casual Address     |
            | Country | State Of Palestine |
            | City    | Ramallah           |
            | To      | Casual Address     |
            | Country | State Of Palestine |
            | City    | Bethlehem          |
        And save the Delivery
        When create standalone shipment
        Then a domestic inland shipment should create
        And the cancel ,operational close Shipment,convert to custom file and Send Response action in more button shouldn't be dim
        And all other actions should be dim
        And all fields should be dim in Delivery window
        And the link of standalon should display



    Scenario: Create standalone shipment when the Delivery is FullResponsibility and from Casual Address to port with same countries
        Given the user in the shipment's routong tab
        And add a new Delivery leg with the following details
            | From    | Casual Address     |
            | Country | State Of Palestine |
            | City    | Ramallah           |
            | To      | Port               |
            | ToName  | Palestina          |
        And save the Delivery
        When create standalone shipment
        Then a domestic inland shipment should create
        And the cancel ,operational close Shipment,convert to custom file and Send Response action in more button shouldn't be dim
        And all other actions should be dim
        And all fields should be dim in Delivery window
        And the link of standalon should display

