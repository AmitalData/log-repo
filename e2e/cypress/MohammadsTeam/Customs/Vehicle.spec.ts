





import { LoginComp } from "../../login/Login.po";
export class VehicleSpec {

  private login: LoginComp = new LoginComp();
}
describe('New Vehicle', () => {


  it('New Vehicle Created Successfully', function () {

  
   
      cy.get('li[id=GeneralMHVehicles]').click();

            cy.get('button[id=NewButton_CustomsVehicle]').click();
            cy.get('li[id=VehicleMoreDetailsTabComponent').click();
            
cy.wait(3000);
         
          cy.get('input[id=CustomsVehicle_GreenIndexGroup]').type('4444')
          cy.get('input[id=textboxdiv_Customs.Vehicle_ModelCode]').type('2012')
          cy.get('input[id=textboxdiv_Customs.Vehicle_TotalVehicleWeight]').type('50000')
 cy.get('input[id=Customs.Vehicle_NumberOfWheels]').type('4')

        
      
    });
      
 
});



