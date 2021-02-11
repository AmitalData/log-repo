/// <reference types="cypress" />



export class LoginComp {
}


it('Login Successfully', () => {


    //cy.visit('http://localhost:4200/')

    //cy.visit('https://test.logitudeworld.com/test')
    var Email = Cypress.env("CustomsEmail");
    var Password = Cypress.env("CustomsPassword");
    var URL = Cypress.env("CustomsAPIURL");
    var Env = Cypress.env("Env");

    if (Env == 'staging') {
        URL = Cypress.env("ProdStagingURL");
        Email = Cypress.env("ProdStagingEmail");
        Password = Cypress.env("ProdStagingPassword");
    }
    else if (Env == 'cloudStaging') {
        URL = Cypress.env("CloudStagingURL");
        Email = Cypress.env("CloudStagingEmail");
        Password = Cypress.env("CloudStagingPassword");
    }
    else if (Env == 'local') {
        URL = Cypress.env("LocalURL");
        Email = Cypress.env("LocalEmail");
        Password = Cypress.env("LocalPassword");
    }

    else if ( Env == 'FATest'){
        URL=Cypress.env("TestStagingURL");
        Email = Cypress.env("FATestEmail");
        Password = Cypress.env("FATestPassword");
       }
       else if ( Env == 'FACloud'){
         URL = Cypress.env("CloudStagingURL");
         Email = Cypress.env("FACloudEmail");
         Password = Cypress.env("FACloudPassword");
       }

       else if ( Env == 'ProdStagingTI'){
        URL = Cypress.env("ProdStagingTIURL");
        Email = Cypress.env("ProdStagingTIEmail");
        Password = Cypress.env("ProdStagingTIPassword");
      }

      else if ( Env == 'TestStagingTI'){
        URL = Cypress.env("TestStagingURL");
        Email = Cypress.env("TestStagingEmail");
        Password = Cypress.env("TestStagingPassword");
      }


      else if ( Env == 'TestStagingTIQ4'){
        URL = Cypress.env("TestStagingURLTIQ4");
        Email = Cypress.env("TestStagingEmailTIQ4");
        Password = Cypress.env("TestStagingPasswordTIQ4");
      }
       
          else if ( Env == 'FAPreCloud'){
         URL = Cypress.env("CloudPreURL");
         Email = Cypress.env("FACloudEmail");
         Password = Cypress.env("FACloudPassword");
       }
   

    else //test_staging
    {
        //URL = Cypress.env("TestURL");
        //Email = Cypress.env("TestEmail");
        //Password = Cypress.env("TestPassword");
    }

    cy.visit(URL)

    cy.get('#Email').clear();
    cy.get('#Password').clear();
    cy.get('#Email').type(Email, { delay: 50 });//.should('have.value', 'protractor2@test.com')

    cy.get('#Password').type(Password)
    cy.get('#cmdLogin').click()



    cy.server();
    //cy.route('test/api/ObjectTableLastUpdate/GetLastTableUpdateDate/?tenant=1102').as('LoadDataCompleted');
    cy.route('**/ObjectTableLastUpdate/**').as('LoadDataCompleted');

    cy.window().then(win => { win.sessionStorage.setItem('ControlledByCypress', 'true') });

    cy.wait('@LoadDataCompleted'); 




})



