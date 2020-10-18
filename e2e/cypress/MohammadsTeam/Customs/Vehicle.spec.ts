




import  { CreateRandom } from './CreateRandom';
import { LoginComp } from "../../login/Login.po";
export class VehicleSpec {

  private login: LoginComp = new LoginComp();
}
describe('New Vehicle', () => {

let R: CreateRandom= new CreateRandom();
  it('New Vehicle Created Successfully', function () {

   var str = R.createrandomnum();
   
      cy.get('li[id=GeneralMHVehicles]').click();

            cy.get('button[id=NewButton_CustomsVehicle]').click();
            cy.get('li[id=General_1').click();
            

            cy.get('input[id="Customs.Vehicle_ImporterIdentityId"]').type('1').should("have.value", '1')

            cy.get('ul[id="mydatalist_Customs.Vehicle_ImporterIdentityId"]').contains('1').then(a => {
                a[0].click();
            })
          cy.get('input[id="Customs.Vehicle_VehicleChassisNumber"]').type(str,{ force: true })
  cy.get('input[id="Customs.Vehicle_VehiclePoolTypeCode"]').type('1').should("have.value", '1')

            cy.get('ul[id="mydatalist_Customs.Vehicle_VehiclePoolTypeCode"]').contains('1').then(a => {
                a[0].click();
            })
 cy.get('input[id="Customs.Vehicle_ModelCode"]').type('500')
cy.get('input[id="Customs.Vehicle_TotalVehicleWeight"]').type('4000')
cy.get('input[id="Customs.Vehicle_NumberOfWheels"]').type('4')

     cy.get('input[id="Customs.Vehicle_VehicleTypeCode"]').type('1').should("have.value", '1')

            cy.get('ul[id="mydatalist_Customs.Vehicle_VehicleTypeCode"]').contains('1').then(a => {
                a[0].click();  
            })
cy.get('input[id="Customs.Vehicle_RichbitFileNumber"]').type('4323')
cy.get('input[id="Customs.Vehicle_VehicleWindowNumber"]').type('4')
  cy.get('input[id="Customs.Vehicle_VehicleManufacturerCode"]').type('1').should("have.value", '1')

            cy.get('ul[id="mydatalist_Customs.Vehicle_VehicleManufacturerCode"]').contains('1').then(a => {
                a[0].click();  
            })

cy.get('input[id="Customs.Vehicle_ModelDescription"]').type('AZ')
cy.get('input[id="Customs.Vehicle_EngineCapacity"]').type('2000')
cy.get('input[id="Customs.Vehicle_ManufactureCountryCode"]').type('BE').should("have.value", 'BE')

            cy.get('ul[id="mydatalist_Customs.Vehicle_ManufactureCountryCode"]').contains('BE').then(a => {
                a[0].click();  
            })
cy.get('input[id="date_Customs.Vehicle_VehicleManufactureDate"]').type('16/10/2020')

   cy.get('li[id=VehicleMoreDetailsTabComponent').click();
     

  cy.get('#CheckBox_0_97').check({ force: true }).should('be.checked')   
cy.get('input[id="Customs.Vehicle_GreenIndexGroup"]').type('2')



cy.get('input[id="Customs.Vehicle_VehiclePriceListTypeCode"]').type('1').should("have.value", '1')

            cy.get('ul[id="mydatalist_Customs.Vehicle_VehiclePriceListTypeCode"]').contains('1').then(a => {
                a[0].click();  
            })

cy.get('input[id="Customs.Vehicle_VehicleTecnologyTypeCode"]').type('1').should("have.value", '1')

            cy.get('ul[id="mydatalist_Customs.Vehicle_VehicleTecnologyTypeCode"]').contains('1').then(a => {
                a[0].click();  
            })
cy.get('#CheckBox_0_100').check({ force: true }).should('be.checked')   

 cy.get('input[id="Customs.Vehicle_VehiclePowerKW"]').type('2000')
 cy.get('input[id="Customs.Vehicle_NumberOfSeats"]').type('5')
cy.get('input[id="date_Customs.Vehicle_IsraelEnterDate"]').type('1/1/2020')
 cy.get('input[id="Customs.Vehicle_VehicleCategory"]').type('P')

cy.get('input[id="Customs.Vehicle_AirBagsNumber"]').type('10')
cy.get('input[id="Customs.Vehicle_GreenIndex"]').type('5')

cy.get('#CheckBox_0_98').check({ force: true }).should('be.checked') 
cy.get('input[id="Customs.Vehicle_VehicleSafetyAccessoryPoints"]').type('8')


   cy.get('li[id=VehiclesOwnersAndSafetyTabComponent').click();

//cy.get('#AddRowSafeties').click();

 //cy.get('#edit-log-grid_0_00_0_0').click();

//cy.get('input[id=CustomsVehicleSafetyAccessories_VehicleSafAccessoryInstlTypCod]').type('1')
 // cy.get('ul[id=mydatalist_CustomsVehicleSafetyAccessories_VehicleSafAccessoryInstlTypCod]').contains('1').then(a => {
   //             a[0].click();  
     //       })


// cy.get('#edit-log-grid_0_00_1_0').click();

//cy.get('input[id=CustomsVehicleSafetyAccessories_VehicleSafAccessoryInstlTypCod]').type('2')
 // cy.get('ul[id=mydatalist_CustomsVehicleSafetyAccessories_VehicleSafAccessoryInstlTypCod]').contains('2').then(a => {
      //          a[0].click();  
      //      })



  cy.get('#AddOwner').click();

 cy.get('#edit-log-grid_0_10_0_0').click();
cy.get('input[id="Customs.Vehicle_ClientId"]').type('2000')

cy.get('#edit-log-grid_0_10_1_0').click();
cy.get('input[id="Customs.Vehicle_PassportNumber"]').type('3434530')
cy.get('#edit-log-grid_0_10_2_0').click();
cy.get('input[id="Customs.VehicleOwner_PassCountryCode"]').type('AL').should("have.value", 'AL')

            cy.get('ul[id="mydatalist_Customs.VehicleOwner_PassCountryCode"]').contains('AL').then(a => {
                a[0].click();  
            })

cy.get('#edit-log-grid_0_10_3_0').click();
cy.get('input[id="Customs.VehicleOwner_ImporterPassportTypeCode"]').type('1').should("have.value", '1')

            cy.get('ul[id="mydatalist_Customs.VehicleOwner_ImporterPassportTypeCode"]').contains('1').then(a => {
                a[0].click();  
            })
cy.get('#edit-log-grid_0_10_4_0').click();
cy.get('input[id="Customs.Vehicle_FirstName"]').type('AAA')
cy.get('#edit-log-grid_0_10_5_0').click();
cy.get('input[id="Customs.Vehicle_LastNameOrCorporationName"]').type('BBBB')
cy.get('#edit-log-grid_0_10_6_0').click();
  cy.get('#CheckBox_0_98').check({ force: true }).should('be.checked')   

           cy.get('button[id=OK-AddVehicle]').click();
      
    });
      
 
});



