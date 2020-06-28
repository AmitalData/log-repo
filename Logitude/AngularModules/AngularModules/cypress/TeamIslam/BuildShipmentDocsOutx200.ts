/// <reference types="cypress" />



var i = 0;
for (i = 0; i <= 100; i++) {
    describe("UseLocalStorage", () => {
        beforeEach(() => {

            cy.restoreLocalStorage()
        })


        it('Login Successfully', () => {



            //cy.visit('http://localhost:4200/')

            //cy.visit('https://test.logitudeworld.com/test')
            cy.visit(Cypress.env("URL"))

            cy.get('#Email').type(Cypress.env("Email"), { delay: 50 }).should('have.value', 'protractor2@test.com')

            cy.get('#Password').type(Cypress.env("Password"))
            cy.get('#cmdLogin').click()



            cy.server();
            //cy.route('test/api/ObjectTableLastUpdate/GetLastTableUpdateDate/?tenant=1102').as('LoadDataCompleted');
            cy.route('**/ObjectTableLastUpdate/**').as('LoadDataCompleted');

            cy.wait('@LoadDataCompleted');


        }

        )




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



        it('OpenDocOutTab', function () {


            cy.get('#ShipmentTHDocsOut').click();

        });

        it('Successfully Printing Document', function () {


            cy.get('#SearchFieldsId_0_1').type('Export Trucking Order');
            cy.get('#ETO-L-DocsOut').click();
            cy.get('#ETO-P-DocsOut').click();
            cy.get('#BusyIndicator_0').should('be.visible')



            cy.get('#BusyIndicator_0').should('be.visible')
            cy.get('#BusyIndicator_0').should('not.be.visible')


            cy.get('#BuildDocumentSucceededDiv').click({ force: true })

            cy.get('#closeButtonId').click()


        })

        it('Failing Printing Document', function () {


            cy.get('#SearchFieldsId_0_1').clear();

            cy.get('#SearchFieldsId_0_1').type('Failure Test Document');
            cy.get('#FTDT-L-DocsOut').click();
            cy.get('#FTDT-P-DocsOut').click();

            cy.get('#MessageWindow_Ok_0').click()

            cy.clearLocalStorage();

        })


        afterEach(() => {
            cy.saveLocalStorage();

        });

    })
}
