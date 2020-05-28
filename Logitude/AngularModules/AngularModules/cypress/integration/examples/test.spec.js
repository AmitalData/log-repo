/// <reference types="cypress" />

context('Actions', () => {
  /*beforeEach(() => {
    cy.visit('https://system.logitudeworld.com')
  })*/

  // https://on.cypress.io/interacting-with-elements

/*

        
       

*/
  it('redirect into logitude', () => {
	   
		//cy.visit('http://192.168.1.100/test') 
		//cy.visit('https://test.logitudeworld.com/staging') 
    //cy.request('http://localhost:9996/LinksGateway.aspx') 
    cy.visit('http://localhost:4200')
  })
  // it('Login to the test system', () => {
	// 	cy.get('#Email').type('protractor@test.com') 
	// 	cy.get('#Password').type('!P123t456') 
  //       cy.get('#cmdLogin').click().then(()=>{
	// 		//cy.contains('#errorsList')
  //          //cy.url().should('include', '/Angular')
  //          cy.wait(30000);
  //       })
  // })

  it('Login to the Local system', () => {
		//cy.get('#Email').type('angular@fnarsoft.com') 
		//cy.get('#Password').type('1') 
        cy.get('#cmdLogin').click().then(()=>{ 
           var test = cy.get('#GeneralMHOperations',{timeout:15000}).click()
           cy.get('#SHIP',{timeout:15000}).click()
           cy.get('#HelperNotes_0_0',{timeout:15000}).click()
           cy.get('#NEWDIRECT',{timeout:15000}).click({force:true})

        })
       
  })

  //it('Move To Accounting Menu', () => {
	 
          
         
          //test.click()
         
  //})

})
