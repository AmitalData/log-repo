@regression @bug117820 @security
Feature: Regression Bug 117820 - Tenant Parameter Validation
    Ensure declaration-related database requests include tenant parameters for proper multi-tenant data isolation

    Scenario: Export declaration list requests have valid tenant values
        Given the user is logged in to the system
        And network monitoring captures declaration API requests only
        When the user navigates to Export workspace and loads data
        Then all captured declaration requests should include tenant parameter in URL or body
        And all tenant parameters should have value 106

    Scenario: Export declaration detail operations have valid tenant values  
        Given the user is logged in to the system
        And network monitoring captures declaration API requests only
        When the user navigates to Export workspace and loads data
        And the user opens the first Export declaration
        Then all captured declaration requests should include tenant parameter in URL or body
        And all tenant parameters should have value 106

