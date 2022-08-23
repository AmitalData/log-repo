Feature: Workflow list
  The user open workflow list, and open flow builder.

  Scenario: open flow builder
    Given the user logged in and navigates to automation workspace
    And open workflows 
    When open flow
    Then the flow builder open successfully