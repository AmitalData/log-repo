




import { LoginComp } from "../../login/Login.po";
export class Journal {

  private login: LoginComp = new LoginComp();
}
describe('New Journal ', () => {

 


  it('New InterestBase Created Successfully', function () {

      
    
      cy.get('li[id=GeneralMHFullAccounting]').click();
    
      cy.get('li[id=FAJournal]').click();
      cy.get('button[id=NewJournal]').click();
     // cy.get('button[id=edit-log-grid_0_00_1_0]').click();
      cy.get('input[id=JournalLine_ActionId]').type("3");
      
      cy.get('ul[id=mydatalist_JournalLine_ActionId]').contains("חובה+זכות").then(a => {
        a[0].click();
    });
    cy.get('#edit-log-grid_0_00_3_0').type("20/10/2020")
    cy.get('#edit-log-grid_0_00_4_0').type("20/10/2020")
    cy.get('#edit-log-grid_0_00_5_0').type("khawla")
    
    cy.get('ul[id=mydatalist_JournalLine_CreditAccountId]').contains("khawla").then(a => {
      a[0].click();

    });
      cy.get('#edit-log-grid_0_00_6_0').type("khawla")
      cy.get('ul[id= mydatalist_JournalLine_DebitAccountId]').contains("khawla").then(a => {
        a[0].click();
   
      
    });
    cy.get('#edit-log-grid_0_00_7_0').type("1500")
    cy.get('#JournalBApprove').click();
    cy.contains('Approved')
  
});



