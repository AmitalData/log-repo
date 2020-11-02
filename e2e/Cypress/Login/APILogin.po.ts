/// <reference types="cypress" />


import {AppTool} from '../MohammadsTeam/Helper/AppTool'
export class APILoginComp {
 

}
 
it('APILogin Successfully', () => {
 
  var Email = Cypress.env("LocalEmail");
  var Password = Cypress.env("LocalPassword");
  var APIURL = Cypress.env("CustomsAPIURL");


  cy.window().then(win=> {
    const Token = win.sessionStorage.getItem('Token')
    cy.log("Old Token: "+Token+"\n");
    if(AppTool.IsNullOrEmpty(Token)){
      cy.request({
        method: 'POST',
        url: APIURL+'Authentication',  
       headers: {
          'Content-Type': 'application/json',
         },
        body: {
          Email: Email,
          Password: Password,
          ByToken : false,
          CardId : null,
          CardType : null,
          IsMobileLogin : false,
          IsUser : true,
          GetToken : true,
          IsAngularLogin : true,
          ClientType : "Web"
        },
       
      })
      .its('body')
      .then(identity => {
       cy.window().then(win => { 
       win.sessionStorage.setItem('Token', identity.Token);
       win.sessionStorage.setItem('LoginUserId', identity.Id);
       win.sessionStorage.setItem('LoginUserName', identity.UserName);
       assert.notEqual(identity.Token, null, 'Authonticatin error on  Email , Password or this user have more than one tenant')
       cy.log("New Token: "+identity.Token+"\n");
       cy.log("LoginUserId: "+identity.LoginUserId+"\n");
       cy.log("LoginUserName: "+identity.LoginUserName+"\n");
       });
      });
    }
  });

 

})



