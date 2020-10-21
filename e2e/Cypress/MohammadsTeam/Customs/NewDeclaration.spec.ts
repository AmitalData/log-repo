
 import {CreateRandom} from "../Helper/CreateRandom";
import { LoginComp } from "../../login/Login.po";
export class APInvoiceSpec {
  private login: LoginComp = new LoginComp();
}
describe('New Declarations ', () => {

let R: CreateRandom= new CreateRandom();

  it('New Declarations Created Successfully', function () {

      var str = R.createrandomnum();
      cy.get('li[id=GeneralMHDeclarations]').click();
      cy.get('button[id=NewButton_CustomsDeclaration]').click();
      cy.get('input[id="Customs.Declaration_ExportFile"]').type(str,{ force: true });
	  cy.get('input[id="Customs.Declaration_CustomerId"]').type('test').should("have.value", 'test');
      cy.get('ul[id="mydatalist_Customs.Declaration_CustomerId"]').contains('test').then(a => {
          a[0].click();
      });
      cy.get('[type="radio"]').eq(2).check({force: true});
 
      cy.get('button[id=NewExportDeclarationOkButton]').click();
 
    });
 
});



