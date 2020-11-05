/// <reference types="cypress" />

import { LoginComp } from "../../../login/Login.po";

    export class NewBIReportScenario {

        private login: LoginComp = new LoginComp();
    
     
    }



    it('CreateBIReportFolder', function () {

        cy.get('#GeneralMHReports').click()
        cy.get('#BI').should('be.visible')
        cy.get('#null_Search').should('be.visible')
        cy.get('#BI').click()
        cy.get('#null_Search_1').should('be.visible')
        cy.get('#BIReportFolder').should('be.visible')
        cy.get('#null_Search_1').click()
        cy.get('#null_Search_1').type('E2E-Automation')
        cy.get('#BIReportFolder').click()

    });


    it('CreateBIReport', function () {

        this.agentName = 'e2eAutomation-' + (Math.random() * 1000000000)
        cy.get('#NewButton_BIReport').click()
        cy.get('#BIReport_Name').click()
        cy.get('#BIReport_Name').type(this.agentName);
        cy.get('#BIReport_Description').click() 
        cy.get('#BIReport_Description').type('TestBIReportByCypress');
        cy.get('#ComboBox_0_1').click({ force: true })
        cy.get('#ComboBox_0_1').contains('Shipments').click({ force: true })
        cy.get('#OKButton').click()

        cy.server();
        cy.route('**/dwobjectfields/**').as('LoadDWObjectFieldsCompleted');
        cy.wait('@LoadDWObjectFieldsCompleted'); 
      
      
        cy.get('#DWQueryBuilderSearchFields_0_0').type('Shipment Number')
        //cy.get('#Tooltip_0_711').trigger('mousedown')
        //cy.get('#Tooltip_0_711').should('be.visible')

        //cy.get('div.container').should('be.hidden').invoke('show').should('be.visible').find('AddQBRootColumnShipmentNumber')


        cy.get('#AddQBRootColumnShipmentNumber').click({ force: true })
        cy.get('#DWQueryBuilderSearchFields_0_0').clear();
        cy.get('#DWQueryBuilderSearchFields_0_0').type('Incoterm')
        cy.get('#AddQBRootColumnIncoterm').click({ force: true })
        cy.get('#DWQueryBuilderSearchFields_0_0').clear();
        cy.get('#DWQueryBuilderSearchFields_0_0').type('Direct / House')
        cy.get('#AddQBRootColumnDirectHouse').click({ force: true })
        cy.get('#DWQueryBuilderSearchFields_0_0').clear();
        cy.get('#DWQueryBuilderSearchFields_0_0').type('Shipper')
        cy.get('#AddQBRootColumnShipper').click({ force: true })
        cy.get('#DWQueryBuilderSearchFields_0_0').clear();
        cy.get('#DWQueryBuilderSearchFields_0_0').type('Gross Weight (KG)')
        cy.get('#AddQBRootColumnGrossWeightKG').click({ force: true })
        cy.get('#DWQueryBuilderSearchFields_0_0').clear();
        cy.get('#DWQueryBuilderSearchFields_0_0').type('Total Volume (CBM)')
        cy.get('#AddQBRootColumnTotalVolumeCBM').click({ force: true })
        cy.get('#DWQueryBuilderSearchFields_0_0').clear();
        cy.get('#DWQueryBuilderSearchFields_0_0').type('accounted payables ( local )')
        cy.get('#AddQBRootColumnAccountedPayablesLocal').click({ force: true })
        cy.get('#DWQueryBuilderSearchFields_0_0').clear();
        cy.get('#DWQueryBuilderSearchFields_0_0').type('Accounted Receivables ( Local )')
        cy.get('#AddQBRootColumnAccountedReceivablesLocal').click({ force: true })
        cy.get('#DWQueryBuilderSearchFields_0_0').clear();
        cy.get('#DWQueryBuilderSearchFields_0_0').type('Create date Time')
        cy.get('#AddQBRootColumnCreateDateTime').click({ force: true })
        cy.get('#DWQueryBuilderSearchFields_0_0').clear();
        cy.get('#DWQueryBuilderSearchFields_0_0').type('Consignee')
        cy.get('#AddQBRootColumnConsignee').click({ force: true })
        cy.get('#DWQueryBuilderSearchFields_0_0').clear();
        cy.get('#DWQueryBuilderSearchFields_0_0').type('Main Carriage ATA')
        cy.get('#AddQBRootColumnMainCarriageATA').click({ force: true })
        cy.get('#DWQueryBuilderSearchFields_0_0').clear();
        cy.get('#DWQueryBuilderSearchFields_0_0').type('Tenant Number')
        cy.get('#AddQBColumnTenantNumber').click({ force: true })
        cy.get('#DWQueryBuilderSearchFields_0_0').clear();
        cy.get('#DWQueryBuilderSearchFields_0_0').type('Create Date')
        cy.get('#AddQBRootFilterCreateDate').click({ force: true })
        cy.get('#ComboBox_0_6').click({ force: true })
        cy.get('#ComboBox_0_6').contains('After').click({ force: true })
        cy.get('#date_DIM_Dates_DateValue').click({ force: true })
        cy.get('#date_DIM_Dates_DateValue').type('06-08-2017')
        cy.get('#DWQueryBuilderSearchFields_0_0').clear();
        cy.get('#DWQueryBuilderSearchFields_0_0').type('Direction')
        cy.get('#AddQBRootFilterDirection').click({ force: true })

        cy.get('.LogLovDIMTable').should('be.visible')
        cy.get('.LogLovDIMTable').click()
        cy.server();
        cy.route('**/dwobjectfields/**').as('LoadDWObjectFieldsCompleted');
        cy.wait('@LoadDWObjectFieldsCompleted'); 
        

        cy.get('#SearchFieldsId_0_1').click({ force: true })
        cy.get('#SearchFieldsId_0_1').type('Export')
       
        cy.server();
        cy.route('**/dwquerybuilder/**').as('LoaddwquerybuilderCompleted');
        cy.wait('@LoaddwquerybuilderCompleted'); 
        
        cy.get('.Link').click()
        cy.get('#OKButton').click()
        cy.get('#LoadPreviewData').click()
        cy.get('#SaveButton').click()
        
        

        
    });

    it('RunBIReport', function () {

        cy.get('#RunBIReport').click()
        cy.get('#myGrid').should('be.visible')
    });

    it('EditBIReport', function () {
  
    });



