@release @stable @smoke @smokeAfterSwap
Feature: Login to LinksGatewayUID


    Scenario: Login to LinksGatewayUID
        Given the user logged in to LinksGatewayUID
        When logged in
        Then value = 1

    Scenario: End task

   