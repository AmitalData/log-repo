



 
export class SearchCargoTracking {

 
}
describe('Search Cargo Tracking ', () => {
 
 
  it('Search Cargo Tracking Successfully', function () {

   
	  var URL = Cypress.env("TestCargoTrackingURL");
	  cy.visit(URL)
	  cy.get('#CargoTracking_Search').clear();
      cy.get('#CargoTracking_Search').type("Automation Cargo Tracking Search", { delay: 50 });
      cy.get('#Button_Search').click()
        
      
    });
      
 
});



