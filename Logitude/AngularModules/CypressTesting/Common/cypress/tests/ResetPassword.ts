describe("Reset Password Scenario", () => {
 
    it('Enter Password with no lower and upper case', function () {
        cy.OpenAndFillChangePasswordPage('!A123456','!A123456'); 
        cy.ValidateElementColor('#PasswordContainsCharactersDiv','rgb(128, 128, 128)'); 
    }); 

    it('Enter Password with Lenth Less than 8 Char', function () {
        cy.OpenAndFillChangePasswordPage('!A1234','!A1234');    
        cy.ValidateElementColor('#PasswordLenghtDiv','rgb(128, 128, 128)'); 
    }); 

    it('Enter Password with No Numbers', function () {
        cy.OpenAndFillChangePasswordPage('!Aasde','!Aasde');    
        cy.ValidateElementColor('#PasswordContainsNumberDiv','rgb(128, 128, 128)'); 
    }); 
     
    it('Enter Password and mismatch password confirmation', function () {
        cy.OpenAndFillChangePasswordPage('!Aa121sde','!Aa121sda'); 
        cy.Click("#cmdSubmit", null);   
        cy.get('#errorsList').should('have.html','The passwords you entered do not match.'); 
    }); 

    it('Enter a valid Password and password confirmation', function () {
        cy.OpenAndFillChangePasswordPage('!Aa121sde','!Aa121sde'); 
        cy.intercept(
            {
              method: 'POST',     
              url: '**/PostChangePassword/**',     
            },[true] 
          ) 
          
        cy.Click("#cmdSubmit", null);
        cy.RedirectToLogin();
    }); 
})