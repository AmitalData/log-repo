/// <reference types="cypress" />

context('Actions', () => {
  /*beforeEach(() => {
    cy.visit('https://system.logitudeworld.com')
  })*/

  // https://on.cypress.io/interacting-with-elements

/*

        
       

*/
  it('redirect into logitude', () => {
	   
		//cy.visit('http://localhost:4200') 
		cy.visit('https://system.logitudeworld.com') 
		//cy.request('http://localhost:9996/LinksGateway.aspx') 
  })
  it('.type() - type into a DOM element', () => {
		cy.get('#Email').type('razantest@protractor.com') 
		cy.get('#Password').type('!R123j456') 
        cy.get('#cmdLogin').click().then(()=>{
           // cy.url().should('include', '/angular')
        })
  })
})
