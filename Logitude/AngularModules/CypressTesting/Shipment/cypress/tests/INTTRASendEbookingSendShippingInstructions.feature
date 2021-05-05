@release @stable @all
Feature: INTTRA Sending E-Booking and Shipping Instructions
    The Customer Care user configures INTTRA for the tenant,
    a second regular user creates an Ocean Export FCL shipment,
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
            | Mode     | Test      |
            | INTTRAID | INTTRA123 |
            | Alias    | ALIAS123  |
        And fill the following out settings
            | UserName | c0464340           |
            | Password | 9Y5V9ila           |
            | Host     | ftp.cvt.inttra.com |
            | Folder   | inbound            |
        And fill the following in settings
            | UserName | c0464340           |
            | Password | 9Y5V9ila           |
            | Host     | ftp.cvt.inttra.com |
            | Folder   | outbound           |
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
            | ShipmentLevel        | Master    |
            | Direction            | Export    |
            | TransportMode        | Ocean     |
            | ShipmentType         | FCL       |
            | Agent                | TestAgent |
            | MainCarriageFromPort | LHR       |
            | MainCarriageToPort   | MIA       |
        When create shipment
        Then the shipment should create successfully

    Scenario: Open INTTRA e-booking wizard to ensure validation messages are appear
        Given the user open the master shipment
        When open INTTRA e-booking wizard
        Then the following validation messages for sending e-booking should appear
            | Message                                                   |
            | Main Carriage Carrier is required                         |
            | Contract Number is required                               |
            | ETD or Main-Carriage Vessel and Voyage must be provided   |
            | Shipment Description of Goods is required                 |
            | Shipment Order Packages or Shipment Packages are required |

    Scenario: Fill required information to send INTTRA e-booking
        Given the user fill the following information to send e-booking
            | BranchName         | Main Office        |
            | ShippingLine       | YMLU               |
            | ContractNumber     | 53454              |
            | DescriptionOfGoods | Send booking test  |
            | ETDDate            | Today              |
            | ETDTime            | 14:00              |
            | Vessel             | PT                 |
            | ShipperContact     | TestShipperContact |
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

    Scenario: Ensure shipping instructions validation messages
        Given the user open the master shipment
        When open INTTRA shipping instructions wizard
        Then the following validation messages for sending shipping instructions should appear
            | Message                                     |
            | Move type is required                       |
            | Booking Confirmation Number is required     |
            | All Containers should have Container Number |

    Scenario: Fill shipping instructions required information
        Given the user fill the following information to send shipping instructions
            | MoveType                  | Port to Port |
            | BookingConfirmationNumber | 123456       |
            | ContainerNumber           | AACC1234569  |
        And add an inside package with the following details
            | PackageType | Quantity | GrossWeight | Description       |
            | Carton      | 5        | 100         | TestInsidePackage |
        When save the shipment
        Then the shipment should save successfully

    Scenario: Send shipping instructions request
        Given the user in INTTRA shipping instructions wizard
        When send shipping instructions request
        Then the instructions should send successfully
        And booking request status should be "Shipping Instructions"