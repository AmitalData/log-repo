@devsmoke @dev @all
Feature: Shipment View
    The user open shipment view, add new view, search for column, save query, edit added view and delete view

    Scenario: Create New Shipment View
        Given the user logged in and navigates to shipments workspace
        And navigates shipment view and fill "TestView" as view name
        And add "branch" column to the selected columns
        When create view
        Then the view should create successfully

    Scenario: Edit the Shipment View
        Given edit the view
        When update the view
        Then the view should update successfully

    Scenario: Delete the Shipment View
        When delete the view
        Then the view should delete successfully