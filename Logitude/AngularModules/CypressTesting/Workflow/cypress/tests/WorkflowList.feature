@devTest
Feature: Workflow list
  The user open workflow list, and open flow builder.

  Scenario: open flow builder
    Given the user logged in and navigates to automation workspace
    And open workflows
    When open flow
    Then the flow builder open successfully

  Scenario: refresh workflow list
    When refresh workflow list
    Then the list should refresh successfully

  Scenario: export workflow list
    When export workflow list
    Then the list should export successfully

  Scenario: search flow by flow name
    When search flow
    Then the flow should appear successfully


