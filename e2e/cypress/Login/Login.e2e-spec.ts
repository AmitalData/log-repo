/// <reference types="cypress" />
import { LoginComp } from './Login.po';


describe('Login Module', () => {

  let visit: LoginComp = new LoginComp();



  it('Login Success', function () {
   // browser.ignoreSynchronization = true;
  cy.visit('http://localhost:4200/')
    var Email = Cypress.env("TestEmail");
    var Password = Cypress.env("TestPassword");
    var URL = Cypress.env("TestURL");
    var Env = Cypress.env("Env");
     // page.navigateTo(browser.params.Link);
     // page.DoLogin(browser.params.Login.Email, browser.params.Login.Password);
      //page.navigateTo('http://localhost:4200/');
      //page.DoLogin('sgautomation@pro.com', 'Sg0592463934!');
  });
});
