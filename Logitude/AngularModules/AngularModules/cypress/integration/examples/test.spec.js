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
		cy.visit('https://test.logitudeworld.com/test') 
		//cy.request('http://localhost:9996/LinksGateway.aspx') 
  })
  it('Login to the system', () => {
		cy.get('#Email').type('protractor@test.com') 
		cy.get('#Password').type('!P123t456') 
        cy.get('#cmdLogin').click().then(()=>{
			//cy.contains('#errorsList')
           cy.url().should('include', '/Angular')

        })
  })
})
