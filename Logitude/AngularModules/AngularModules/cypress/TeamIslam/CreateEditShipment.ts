/// <reference types="cypress" />

import { LoginComp } from ".././login/Login.po";


export class CreateEditShipment {


  private login: LoginComp = new LoginComp();
  private CreateEditShipment: CreateEditShipment = new CreateEditShipment();

  constructor() {
  }
}

describe('CreateEditShipment Tests', () => {
  it('Create Shipment Successfully', () => {
  cy.get('#GeneralMHOperations').click()
  cy.get('.DefaultMenuItem:first').click();
  cy.contains('Operation').click()
  cy.get('#SHIP').click()

  cy.get('#NEWDIRECT').click({ force: true })

  cy.get('#DirectionRadio_0E').click({ force: true })
  cy.get('#TransportModeRadio_0A').click({ force: true })
  cy.get('#Shipment_ShipperId').click({ force: true })
  cy.get('#Shipment_ShipperId').type('TestShipperExport1')
  cy.get('.DropDownListItem:first').click()
  cy.get('#Shipment_ShipperReference1').type('Reference1')



  cy.get('#Shipment_ConsigneeId').click({ force: true })
  cy.get('#Shipment_ShipperId').type('TestConsigneeExport1')
  cy.get('.DropDownListItem:first').click()
  cy.get('#Shipment_ShipperReference1').type('Reference1')


  cy.get('#Shipment_MainCarriageFromPortId').click()
  cy.get('#Shipment_MainCarriageFromPortId').type('eze')
  cy.get('.DropDownListItem:first').click()
  cy.get('#Shipment_MainCarriageToPortId').click()
  cy.get('#Shipment_MainCarriageToPortId').type('mvd')
  cy.get('.DropDownListItem:first').click()
  cy.get('#Shipment_DescriptionOfGoods').type('CreateShipmentFromCypress')





  cy.get('#ShipmentCreatebtn').click()

  cy.get('#Shipments-O-Q').click()

  cy.get('#LogGrid_0_0row0').click()
  //cy.get('#HelperNotesButton_0_1').click()
  //cy.get('#HelperNotesContent_0_1').click()
  //cy.get('#HelperNotesContent_0_1').type('Test Crypress')


  //cy.get('#Shipment.TH.General').click()
  //cy.get('#Shipment_BookingConfirmationNotes').click()
  //cy.get('#Shipment_BookingConfirmationNotes').type('Test Crypress')

  //cy.get('#Shipment-Save').click()




})

})



  //})

