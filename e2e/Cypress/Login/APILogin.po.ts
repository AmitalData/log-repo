/// <reference types="cypress" />



export class APILoginComp {
	cy.log('log out any message we want here')

}
 
it('APILogin Successfully', () => {
 
  var Email = Cypress.env("LocalEmail");
  var Password = Cypress.env("LocalPassword");

  cy.request({
    method: 'POST',
    url: Cypress.env("LocalAPIURL")+'Authentication',  
	 headers: {
      'Content-Type': 'application/json',
     },
    body: {
      Email: Email,
      Password: Password
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
	 });
  });

})



