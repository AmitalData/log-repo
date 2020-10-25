





import { LoginComp } from "../../login/Login.e2e-spec";
export class VehicleSpec {

  private login: LoginComp = new LoginComp();
}
describe('New Vehicle', () => {


  it('New Vehicle Created Successfully', function () {

  
   
      cy.get('li[id=GeneralMHVehicles]').click();

            cy.get('button[id=NewButton_CustomsVehicle]').click();
           cy.get('input[id=Customs.Vehicle_ImporterPassportNumber]').type('10000')
          cy.get('input[id=Customs.Vehicle_VehicleChassisNumber]').type('4444')
          cy.get('input[id=textboxdiv_Customs.Vehicle_ModelCode]').type('2012')
          cy.get('input[id=textboxdiv_Customs.Vehicle_TotalVehicleWeight]').type('50000')
 cy.get('input[id=Customs.Vehicle_NumberOfWheels]').type('4')

        
      
    });
      
 
});



