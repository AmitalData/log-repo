



 
export class Test_SearchCargoTracking {

 
}
describe('Search Cargo Tracking ', () => {
 
 
  it('Search Cargo Tracking Successfully', function () {

   
	  var URL = Cypress.env("TestStagingCargoTrackingURL");
	  cy.visit(URL)
	  cy.get('#CargoTracking_Search').clear();
      cy.get('#CargoTracking_Search').type("Cargo Tracking Warme", { delay: 50 });
      cy.get('#Button_Search').click();
      cy.get('#CargoTracking_BusyIndicator').should('not.be.visible');
      cy.get('#SearchResults_0').click();
    });
      
 
});



