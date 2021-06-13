@NewDev
Feature: Shipping Line Create, Search and Edit from Maintenance
    The user creates a ShippingLine, searches for and edits it from the Maintenance Module.

    Scenario: Create new Shipping Line
        Given the user logged in and open "Shipping Lines" in maintenance menu
        And a shippingLine with the following details
            | Code     | random                 |
            | SCACCode | random                 |
            | Name     | Test                   |
            | Notes    | Test shippingLine note |
        When create shipping Line
        Then the shipping Line should create successfully

    Scenario: Search for the Shipping Line by code
        When search shipping Line
        Then the shipping Line should appear successfully

    Scenario: Open the Shipping Line
        When open shipping Line
        Then the shipping Line should open successfully

    Scenario: Edit the Shipping Line
        Given check dim input in shipping line INTTRA
        And fill the following Address in Addresses Shipping line
            | AddressCountry | United States |
            | AddressCity    | las           |

        When create shipping line address
        Then the shipping line address should create successfully

        Given fill the following Area in Areas Shipping line
            | AreaName        | Area Shipping Test             |
            | AreaDescription | Area Shipping description Test |
            | AreaCountry     | GB                             |
            | AreaPort        | Miami                          |

        When create shipping line area
        Then the shipping line address should create successfully

        Given fill the following Tariff in Tariff Shipping line
            | Partner Code | random |
            | Port         | Miami  |
        When create shipping line Tariff translation
        Then the shipping line Tariff translation should create successfully

        Given the user activate shipping Line
        When edit shipping Line
        Then the shipping Line should update successfully
        And following event should appear in events tab
            | Event                 | Notes                     |
            | Shipping Line Updated | Shipping Line Inactivated |

    Scenario: Save and close the shippingLine
        When save and close shipping Line
        Then the shipping Line should close successfully