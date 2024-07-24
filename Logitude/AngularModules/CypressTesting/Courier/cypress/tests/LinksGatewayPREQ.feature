@release @stable @smoke @smokeAfterSwap
Feature: Login to LinksGatewayPREQ


    Scenario: Login to LinksGatewayPREQ
        Given the user logged in to LinksGatewayPREQ
        When logged in
        Then value = 1

    Scenario: End task

   