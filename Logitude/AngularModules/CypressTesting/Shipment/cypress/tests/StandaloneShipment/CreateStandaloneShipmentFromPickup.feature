
Feature: Create standalone shipment from Pickup
    The user creates a Direct Import Ocean FCL shipment,Create standalone shipment when pickup from partner to port with the same countries,
    Create standalone shipment when the pickup is FullResponsibility and "To/From" are partners with different countries,
    Create standalone shipment when the pickup is FullResponsibility and "To/From" are partners with same countries,
    Create standalone shipment when the pickup is not FullResponsibility and "To/From" are partners with same countries
    Create standalone shipment when the pickup is FullResponsibility and from Partner to Casual Address with same countries,
    Create standalone shipment when the pickup is FullResponsibility and from port to port with same countries,
    Create standalone shipment when the pickup is FullResponsibility and from Casual Address to Casual Address with same countries,
    Create standalone shipment when the pickup is FullResponsibility and from Casual Address to port with same countries

    Scenario: Create import ocean FCL shipment
        Given the user logged in and navigates to shipments workspace
        And a direct shipment with the following details
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

    Scenario: Create standalone shipment when pickup from partner to port with the same countries
        Given the user open the shipment and navigate to RoutingsTab workspace
        And add a new pickup leg with the following details
            | FullResponsibility | True                      |
            | From               | Partner                   |
            | FromPartner        | ALS CUSTOMS SERVICES GMBH |
            | To                 | Port                      |
            | ToPort             | Brandscheid/Westerwald    |
        And save the pickup
        When create standalone shipment
        Then a domestic inland shipment should create
        And the cancel ,operational close Shipment,convert to custom file and Send Response action in more button shouldn't be dim
        And all other actions should be dim
        And all fields should be dim in pickup window
        And the link of standalon should display

    Scenario: Create standalone shipment when the pickup is FullResponsibility and "To/From" are partners with different countries
        Given the user in the shipment's routong tab
        And add a new pickup leg with the following details
            | FullResponsibility | True                      |
            | From               | Partner                   |
            | FromPartner        | Israeli Tenant            |
            | To                 | Partner                   |
            | ToPartner          | ALS CUSTOMS SERVICES GMBH |
        And save the pickup
        When click create Standalone Shipment
        Then a validation message with "Both Addresses must be in the same country since the direction is Domestic" error should appear

    Scenario: Create standalone shipment when the pickup is FullResponsibility and "To/From" are partners with same countries
        Given the user in the shipment's routong tab
        And add a new pickup leg with the following details
            | From        | Partner        |
            | FromPartner | Israeli Tenant |
            | To          | Partner        |
            | ToPartner   | Israeli Tenant |
        And save the pickup
        When create standalone shipment
        Then a domestic inland shipment should create
        And the cancel ,operational close Shipment,convert to custom file and Send Response action in more button shouldn't be dim
        And all other actions should be dim
        And all fields should be dim in pickup window
        And the link of standalon should display

    Scenario: Create standalone shipment when the pickup is not FullResponsibility and "To/From" are partners with same countries
        Given the user in the shipment's routong tab
        And add a new pickup leg with the following details
            | From        | Partner        |
            | FromPartner | Israeli Tenant |
            | To          | Partner        |
            | ToPartner   | Israeli Tenant |
        And unchecked the FullResponsibility
        When save the pickup
        Then the Create Standalone Shipment button Should be dim

    Scenario: Create standalone shipment when the pickup is FullResponsibility and from Partner to Casual Address with same countries
        Given the user in the shipment's routong tab
        And  add a new pickup leg with the following details
            | From        | Partner            |
            | FromPartner | Testagent          |
            | To          | CasualAddress      |
            | ToCountry   | State Of Palestine |
            | ToCity      | Bethlehem          |
        And save the pickup
        When create standalone shipment
        Then a domestic inland shipment should create
        And the cancel ,operational close Shipment,convert to custom file and Send Response action in more button shouldn't be dim
        And all other actions should be dim
        And all fields should be dim in pickup window
        And the link of standalon should display

    Scenario: Create standalone shipment when the pickup is FullResponsibility and from port to port with same countries
        Given the user in the shipment's routong tab
        And add a new pickup leg with the following details
            | From     | Port                   |
            | FromPort | Brandscheid/Westerwald |
            | To       | Port                   |
            | ToPort   | Brandshagen            |
        And save the pickup
        When create standalone shipment
        Then a domestic inland shipment should create
        And the cancel ,operational close Shipment,convert to custom file and Send Response action in more button shouldn't be dim
        And all other actions should be dim
        And all fields should be dim in pickup window
        And the link of standalon should display


    Scenario: Create standalone shipment when the pickup is FullResponsibility and from Casual Address to Casual Address with same countries
        Given the user in the shipment's routong tab
        And add a new pickup leg with the following details
            | From        | CasualAddress      |
            | FromCountry | State Of Palestine |
            | FromCity    | Ramallah           |
            | To          | CasualAddress      |
            | ToCountry   | State Of Palestine |
            | ToCity      | Bethlehem          |
        And save the pickup
        When create standalone shipment
        Then a domestic inland shipment should create
        And the cancel ,operational close Shipment,convert to custom file and Send Response action in more button shouldn't be dim
        And all other actions should be dim
        And all fields should be dim in pickup window
        And the link of standalon should display



    Scenario: Create standalone shipment when the pickup is FullResponsibility and from Casual Address to port with same countries
        Given the user in the shipment's routong tab
        And add a new pickup leg with the following details
            | From        | CasualAddress      |
            | FromCountry | State Of Palestine |
            | FromCity    | Ramallah           |
            | To          | Port               |
            | ToPort      | Palestina          |
        And save the pickup
        When create standalone shipment
        Then a domestic inland shipment should create
        And the cancel ,operational close Shipment,convert to custom file and Send Response action in more button shouldn't be dim
        And all other actions should be dim
        And all fields should be dim in pickup window
        And the link of standalon should display

