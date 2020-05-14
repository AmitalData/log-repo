/// <reference types="cypress" />

context('Actions', () => {
  /*beforeEach(() => {
    cy.visit('https://system.logitudeworld.com')
  })*/

  // https://on.cypress.io/interacting-with-elements

/*

        
       

*/
it('redirect into logitude', () => {
		cy.visit('https://system.logitudeworld.com')
  })
  it('.type() - type into a DOM element', () => {
		cy.get('#Email').type('razan@fnarsoft.com') 
		cy.get('#Password').type('!R123j456')
        cy.get('#cmdLogin').click().then(()=>{
            cy.url().should('include', '/angular')

        })
  })
})
