/// <reference types="cypress" />



var i = 0;
for (i = 0; i <= 100; i++) {
    describe("UseLocalStorage", () => {
        beforeEach(() => {

            cy.restoreLocalStorage()
        })


        it('Login Successfully', () => {
            cy.visit(Cypress.env("URL"))
            cy.get('#Email').type(Cypress.env("Email"), { delay: 50 })
            cy.get('#Password').type(Cypress.env("Password"))
            cy.get('#cmdLogin').click()
            cy.url().should('not.include', '/login')
        }

        )




        it('Create Shipment Successfully', () => {

            cy.get('#GeneralMHOperations').click()
            cy.get('.DefaultMenuItem:first').click();
            cy.contains('Operation').click()
            cy.get('#SHIP').click()

            cy.get('#NEWDIRECT').click({ force: true })

            cy.get('#DirectionRadio_0E', { timeout: 5000 }).click({ force: true })
            cy.get('#TransportModeRadio_0A', { timeout: 5000 }).click({ force: true })
            cy.get('#Shipment_ShipperId', { timeout: 5000 }).click({ force: true })
            cy.get('#Shipment_ShipperId').type('TestShipperExport1')
            cy.get('.DropDownListItem:first', { timeout: 5000 }).should('be.visible').click()
            cy.get('#Shipment_ShipperReference1', { timeout: 5000 }).type('Reference1')



            cy.get('#Shipment_ConsigneeId', { timeout: 5000 }).click({ force: true })
            cy.get('#Shipment_ConsigneeId').type('TestConsigneeExport1')
            cy.get('.DropDownListItem:first', { timeout: 5000 }).should('be.visible').click()
            cy.get('#Shipment_ShipperReference1', { timeout: 5000 }).type('Reference1')


            cy.get('#Shipment_MainCarriageFromPortId', { timeout: 5000 }).click()
            cy.get('#Shipment_MainCarriageFromPortId').type('eze')
            cy.get('.DropDownListItem:first', { timeout: 5000 }).should('be.visible').click()
            cy.get('#Shipment_MainCarriageToPortId', { timeout: 5000 }).click()
            cy.get('#Shipment_MainCarriageToPortId').type('mvd')
            cy.get('.DropDownListItem:first', { timeout: 5000 }).should('be.visible').click()
            cy.get('#Shipment_DescriptionOfGoods', { timeout: 5000 }).type('CreateShipmentFromCypress')





            cy.get('#ShipmentCreatebtn', { timeout: 5000 }).click()

            cy.get('#Shipments-O-Q', { timeout: 5000 }).click()

            cy.get('#LogGrid_0_0row0', { timeout: 5000 }).click()
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
