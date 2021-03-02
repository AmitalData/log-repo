@release @all @abed
Feature: INTTRA Sending E-Booking and Shipping Instructions
    The Customer Care user configures INTTRA for the tenant,
    the second user creates an Ocean Export FCL shipment,
    sends e-booking request,
    fixes the validations preventing the sending of e-booking,
    sends the e-booking again,
    sends shipping instructions,
    fixes the validations preventing the sending of shipping instructions,
    and sends the shipping instructions again.

    Scenario: Login as customer care user and adjust INTTRA settings
        Given the customer care user logged in and navigate to maintenance menu
        And open INTTRA settings wizard
        And fill the following general settings
            | Mode | INTTRAID  | Alias    |
            | Test | INTTRA123 | ALIAS123 |
        And fill the following out settings
            | UserName | Password | Host               | Folder  |
            | c0464340 | 9Y5V9ila | ftp.cvt.inttra.com | inbound |
        And fill the following in settings
            | UserName | Password | Host               | Folder   |
            | c0464340 | 9Y5V9ila | ftp.cvt.inttra.com | outbound |
        And fill the following branches settings
            | BranchName  | INTTRAID | PartyAlias | Contact      |
            | Main Office | 1234     | 5678       | SpecflowTest |
        And fill the following registration settings
            | BranchName  | RegistrationCode |
            | Main Office | YMLU             |
        When save settings
        Then the settings should save successfully

    Scenario: Login and create master export ocean FCL shipment
        Given the user logged in and navigate to shipments workspace
        And a master shipment with the following details
            | ShipmentLevel | Direction | TransportMode | ShipmentType | Agent     | MainCarriageFromPort | MainCarriageToPort |
            | Master        | Export    | Ocean         | FCL          | TestAgent | LHR                  | MIA                |
        When create shipment
        Then the shipment should create successfully

    Scenario: Open INTTRA e-booking wizard to ensure validation messages are appear
        Given the user open the master shipment
        When open INTTRA e-booking wizard
        Then validation messages for sending e-booking should appear

    Scenario: Fill required information to send INTTRA e-booking
        Given the user fill the following information to send e-booking
            | BranchName  | ShippingLine | ContractNumber | DescriptionOfGoods | ETDDate | ETDTime | Vessel | ShipperContact     |
            | Main Office | MSCU         | 53454          | Send booking test  | Today   | 14:00   | PT     | TestShipperContact |
        And add the following package
            | PackageType | GrossWeight |
            | 40GP        | 200         |
        When save the shipment
        Then the shipment should save successfully

    Scenario: Open INTTRA e-booking wizard and send booking request
        Given the user in INTTRA e-booking wizard
        When send booking request
        Then the request should send successfully
        And booking request status should be "Sent"