Feature: Reset password test

    Scenario: Enter a password and mismatch password confirmation
        Given "!A123456" as a paswword without confirm password
        When sumbit
        Then validate message should appear successfully

    Scenario: Enter a password with more than 3 following characters
        Given "123" as a paswword and confirm password
        When sumbit
        Then validate message should appear successfully

    Scenario: Enter a password without lower and upper case
        Given "1212" as a paswword and confirm password
        When sumbit
        Then validate message should appear successfully

    Scenario: Enter a password without numbers
        Given "Test" as a paswword and confirm password
        When sumbit
        Then validate message should appear successfully

    Scenario: Enter a password with length Less than 8 characters
        Given "Test12" as a paswword and confirm password
        When sumbit
        Then validate message should appear successfully

    Scenario: Enter a new password same as the current password
        Given "ahmed13!A15" as a current ,paswword and confirm password
        When sumbit
        Then validate message should appear successfully

    Scenario: Enter a valid Password and password confirmation
        Given "ahmed13!A15" as a new paswword and confirm password
        When sumbit
        Then password should reset successfully
