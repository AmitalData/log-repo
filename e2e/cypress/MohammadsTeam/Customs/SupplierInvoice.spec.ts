 
import { LoginComp } from "../../login/Login.po";
import { LogHelper } from '../../Helpers/LogHelper';
import {NewAPIImportDeclaration} from './API/NewAPIImportDeclaration.spec';
export class SupplierInvoice {
    public login: LoginComp = new LoginComp();
}
describe('New  SupplierInvoice', () => {

    
    it('New API Import Declarations ', () => { 
      var newAPIImportDeclaration = new NewAPIImportDeclaration();
    })

  it('New  SupplierInvoice Created Successfully', function () {

   
   cy.window().then(win=> {
      const CustomFileNo= win.sessionStorage.getItem('CustomFileNo')
       cy.get('li[id=GeneralMHDeclarations]').click({force: true} );
   //    LogHelper.QuerySearchAndSelectFirst('91340214');
cy.get('input[id=SearchFieldsId_0_1]').should('be.visible').then( a=> {
    cy.get('input[id=SearchFieldsId_0_1]').type(CustomFileNo,{ force: true });
    cy.get('div[id=ListDataLoaded]').then( a=> {
          cy.get('div[id=LogGrid_0_1row0]').click({ force: true });
      })});
       cy.get('#CustomsDeclarationTHGeneral').click({force: true} );
       cy.get('#Add_1').should('be.visible').then(a => {         
              cy.get('#CustomsDeclarationTHInvoices').click({ force: true });      
       });
   
     
cy.get('#Add_2').click({force: true} );

});
   

  });

    it('supplier invoice fields', () => {
      //  LogHelper.LOVSearchAndSelectFirst("Customs.SupplierInvoice_AccountTypeCode",'חשבון מכר');
        //cy.get('input[id="Customs.SupplierInvoice_AccountTypeCode"]').type('325');
        //LogHelper.LOVSearchAndSelectFirst("Customs.SupplierInvoice_AccountTypeCode", '325');
        // cy.get('input[id="Customs.SupplierInvoice_AccountTypeCode"]').type('325');
        // cy.get('ul[id="mydatalist_Customs.SupplierInvoice_AccountTypeCode"]').contains('325').then(a => {
            // a[0].click({force: true} );
        // });
        cy.get('input[id="date_Customs.SupplierInvoice_IssueDate"]').type('16/10/2020')
       // LogHelper.LOVSearchAndSelectFirst("Customs.SupplierInvoice_InvoiceCurrencyTypeCode", 'ILS');

       // LogHelper.LOVSearchAndSelectFirst("Customs.SupplierInvoice_IncotermCode", 'DES');

        cy.get('input[id="Customs.SupplierInvoice_IsPreference"]').check({ force: true }).should('be.checked')
       // LogHelper.LOVSearchAndSelectFirst("Customs.SupplierInvoice_AccumalationStateCode", '3');       
        cy.get('input[id="Customs.SupplierInvoice_InvoiceNumber"]').type('2020')
       // LogHelper.LOVSearchAndSelectFirst("Customs.SupplierInvoice_VendorId", '31');

        cy.get('input[id="Customs.SupplierInvoice_InvoiceAmount"]').type('100')
       // LogHelper.LOVSearchAndSelectFirst("Customs.SupplierInvoice_IssueCountryCode", 'AD');

        LogHelper.LOVSearchAndSelectFirst("Customs.SupplierInvoice_PreferenceDocumentTypeCode", '14');

    })

    //it('freight amount', () => {
    //  //  cy.get('#Add_3').click({force: true} );
    //    cy.get('#edit-log-grid_0_20_0_0').click({force: true} );
    //    LogHelper.LOVSearchAndSelectFirst("Customs.SupplierInvoiceFreightAmount_CurrencyTypeCode", 'ILS');
    //    cy.get('#edit-log-grid_0_20_1_0').click({force: true} );
    //    cy.get('input[id="Customs.SupplierInvoice_Amount"]').type('50').should("have.value", '50');
    //})

    it('Add Supplier invoice item', () => {
      const edit_log_grid_Existent = Cypress.$('#edit-log-grid_0_3');
      if (edit_log_grid_Existent && edit_log_grid_Existent.length) {
        cy.get('#Add_4').click({force: true} );
        cy.get('input[id="Customs.SupplierInvoice_ItemCode"]').type('10')
        cy.get('#edit-log-grid_0_30_2_0').click({force: true} );
        cy.get('input[id="Customs.SupplierInvoice_ItemDescription"]').type('1A');
        cy.get('#edit-log-grid_0_30_3_0').click({force: true} );
        cy.get('input[id="Customs.SupplierInvoiceItem_ClassificationCode"]').type('123456782');
        cy.get('#edit-log-grid_0_30_4_0').click({force: true} );
        LogHelper.LOVSearchAndSelectFirst("Customs.SupplierInvoice_TradeAgreementCode", '10');
  
        cy.get('#edit-log-grid_0_30_5_0').click({force: true} );
        cy.get('input[id="Customs.SupplierInvoice_InvoiceQuantity"]').type('500');
        cy.get('#edit-log-grid_0_30_6_0').click({force: true} );
        LogHelper.LOVSearchAndSelectFirst("Customs.SupplierInvoice_InvoiceQuantityType", 'C26');
   
        cy.get('#edit-log-grid_0_30_7_0').click({force: true} );
        cy.get('input[id="Customs.SupplierInvoice_ItemPrice"]').type('300');


        }
    
  
  })

    //it('Edit Supplier invoice item', () => {
    //    cy.get('#edit-log-grid_0_30_9_0').click({force: true} );
    //    cy.get('#Edit_3').click({force: true} );
    //    cy.get('input[id="Customs.SupplierInvoiceItem_StatisticQuantity"]').type('100');
    //    LogHelper.LOVSearchAndSelectFirst("Customs.SupplierInvoiceItem_StatisticQuantityType", 'ANN')       
    //    cy.get('input[id="Customs.SupplierInvoiceItem_AdditionalQuantity"]').type('100');
    //    LogHelper.LOVSearchAndSelectFirst("Customs.SupplierInvoiceItem_AdditionalQuantityType", 'DAY')      
    //    cy.get('#Add_6').click({ force: true });
    //    cy.get('#edit-log-grid_0_50_0_0').click({ force: true });
    //    cy.get('input[id="Customs.SupplierInvoiceItem_StatisticQuantity"]').type('100');
    //    LogHelper.LOVSearchAndSelectFirst("Customs.SupplierInvoiceItemProcesType_ProcessTypeCode", '4100103')
    //    cy.get('#Declarations').click({force: true} );
    //    cy.get('#Add_7').click({force: true} );
    //    cy.get('#edit-log-grid_0_60_0_0').click({force: true} );
    //    LogHelper.LOVSearchAndSelectFirst("Customs.SupplierInvoiceItemsConDeclar_DeclarationTypeCode", '4')
    //    cy.get('#edit-log-grid_0_60_1_0').click({force: true} );
    //    cy.get('input[id="Customs.SupplierInvoiceItemsConDeclar_DeclarationNumber"]').type('1074');
    //    cy.get('#edit-log-grid_0_60_2_0').click({force: true} );
    //    cy.get('input[id="Customs.SupplierInvoiceItemsConDeclar_InvoiceNumber"]').type('2020');
    //    cy.get('#edit-log-grid_0_60_3_0').click({force: true} );
    //    cy.get('input[id="Customs.SupplierInvoiceItemsConDeclar_ItemSequence"]').type('1');
    //    cy.get('#edit-log-grid_0_60_4_0').click({force: true} );
    //    cy.get('input[id="Customs.SupplierInvoiceItemsConDeclar_Quantity"]').type('10');
    //    cy.get('#SerialNumbers').click({force: true} );
    //    cy.get('#Add_8').click({force: true} );
    //    cy.get('#edit-log-grid_0_70_0_0').click({force: true} );
    //    LogHelper.LOVSearchAndSelectFirst("Customs.SupplierInvoiceItemsSerialNum_TypeCode", 'CN')
    //    cy.get('#edit-log-grid_0_70_1_0').click({force: true} );
    //    cy.get('input[id="Customs.SupplierInvoiceItemsSerialNum_SerialNumber"]').click({force: true} ).type('222');
    //    cy.get('#Add_9').click({force: true} );

    //    cy.get('#edit-log-grid_0_80_0_0').click({force: true} );
    //    LogHelper.LOVSearchAndSelectFirst("Customs.SupplierInvoiceItemsDescript_TypeCode", '4')
    //    cy.get('#edit-log-grid_0_80_1_0').click({force: true} );
    //    cy.get('input[id="Customs.SupplierInvoiceItemsDescript_Description"]').click({force: true} ).type('ASDF');
    //    cy.get('#Add_10').click({force: true} );
    //    cy.get('#edit-log-grid_0_90_0_0').click({force: true} );
    //    LogHelper.LOVSearchAndSelectFirst("Customs.SupplierInvoiceItemsProdIdent_TypeCode", 'SS')
    //    cy.get('#edit-log-grid_0_90_1_0').click({force: true} );
    //    cy.get('input[id="Customs.SupplierInvoiceItemsProdIdent_Identification"]').click({force: true} ).type('ASDF');

    //    cy.get('#Levies').click({force: true} );
    //    cy.get('#Add_11').click({force: true} );
    //    cy.get('#edit-log-grid_0_100_0_0').click({force: true} );
    //    LogHelper.LOVSearchAndSelectFirst("Customs.SupplierInvoiceItemsLevy_TradeLevyExamptCode", '1')
    //    cy.get('#edit-log-grid_0_100_1_0').click({force: true} );
    //    cy.get('input[id="Customs.SupplierInvoiceItemsLevy_TradeLevyNumber"]').click({force: true} ).type('1');

    //    cy.get('#SaveItem').click({force: true} );
    //})

    //it('Add certificate', () => {
    //    cy.get('#edit-log-grid_0_30_9_0').click({force: true} );
    //    cy.get('#Certificate').click({force: true} );

    //    cy.get('#Add_12').click({force: true} );
    //    cy.get('#edit-log-grid_0_110_1_0').click({force: true} );
    //    LogHelper.LOVSearchAndSelectFirst("Customs.SupplierInvioceItemCertificat_ConfirmationTypeCode", '104')
    //    cy.get('#edit-log-grid_0_110_2_0').click({force: true} );
    //    LogHelper.LOVSearchAndSelectFirst("Customs.SupplierInvioceItemCertificat_AttachmentTypeCode", '4')
    //    cy.get('#edit-log-grid_0_110_3_0').click({force: true} );
    //    cy.get('input[id="Customs.SupplierInvioceItemCertificat_CertificateNumber"]').click({force: true} ).type('4558');
    //    cy.get('#edit-log-grid_0_110_4_0').click({force: true} );
    //    LogHelper.LOVSearchAndSelectFirst("Customs.SupplierInvioceItemCertificat_CertificateExemptionTypeCode", '97')

    //    cy.get('#edit-log-grid_0_110_5_0').click({force: true} );
    //    LogHelper.LOVSearchAndSelectFirst("Customs.SupplierInvioceItemCertificat_ResConfirmationTypeCode", '104')

    //    cy.get('#edit-log-grid_0_110_6_0').click({force: true} );
    //    cy.get('input[id="Customs.SupplierInvioceItemCertificat_CustomsAttachmentID"]').type('104');

    //    cy.get('#edit-log-grid_0_110_7_0').click({force: true} );
    //    cy.get('input[id="Customs.SupplierInvioceItemCertificat_ExternalRequestTypeCode"]').type('D');

    //    cy.get('#edit-log-grid_0_110_8_0').click({force: true} );
    //    cy.get('input[id="Customs.SupplierInvioceItemCertificat_ApprovalRequestNumber"]').type('1000');

    //    cy.get('#CreateCertificate').click({force: true} );
    //    cy.get('#ConfirmWindow_Yes_0').click({force: true} );
    //})

    //it('update proccess type', () => {
    //    cy.get('#toggleButton').click({force: true} );
    //    cy.get('#Update').click({force: true} );
    //    LogHelper.LOVSearchAndSelectFirst("Customs.SupplierInvoiceItemProcesType_ProcessTypeCode", '2100102')

    //    cy.get('#undefined_UpdateSelected').check({ force: true });
    //    cy.get('input[id="FromNumber"]').type('1');
    //    cy.get('input[id="ToNumber"]').type('1');
    //    cy.get('#Add_Lines').click({force: true} );
    //    cy.get('#UpdateProccess').click({force: true} );
    //    cy.get('#MessageWindow_Ok_0').click({force: true} );

    //})

    //it('update certificate', () => {
    //    cy.get('#toggleButton').click({force: true} );
    //    cy.get('#ikea').click({force: true} );
    //    cy.get('input[id="Customs.SupplierInvioceItemCertificat_ExternalRequestTypeCode"]').type('D');
    //    cy.get('input[id="Customs.SupplierInvioceItemCertificat_ApprovalRequestNumber"]').type('1000');
    //    LogHelper.LOVSearchAndSelectFirst("Customs.SupplierInvioceItemCertificat_ReqConfirmationTypeCode", '104')

    //    LogHelper.LOVSearchAndSelectFirst("Customs.SupplierInvioceItemCertificat_AttachmentTypeCode", '4')
   
    //    cy.get('input[id="Customs.SupplierInvioceItemCertificat_CertificateNumber"]').type('4558');
    //    LogHelper.LOVSearchAndSelectFirst("Customs.SupplierInvioceItemCertificat_CertificateExemptionTypeCode", '97')

    //    LogHelper.LOVSearchAndSelectFirst("Customs.SupplierInvioceItemCertificat_ResConfirmationTypeCode", '105')

    //    cy.get('#UpdateCertificate').click({force: true} );
    //    cy.get('#MessageWindow_Ok_0').click({force: true} );
    //})

    //it('update origine country', () => {
    //    cy.get('#toggleButton').click({force: true} );
    //    cy.get('#UpdateCountryOfOrigin').click({force: true} );
    //    LogHelper.LOVSearchAndSelectFirst("Customs.SupplierInvoiceItem_OriginCountryCode", 'AE')

    //    cy.get('#undefined_UpdateAll').check({ force: true });
    //    cy.get('#Update_Country').click({force: true} );
    //})

    //it('add vehicle', () => {
    //    //cy.get('#edit-log-grid_0_30_9_0').click({force: true} );
    //    //cy.get('#Vehicle').click({force: true} );
    //    //cy.get('#Add_13').click({force: true} );
    //    //cy.get('#edit-log-grid_0_140_1_0').click({force: true} );
    //    //cy.get('input[id="Customs.SupplierInvoiceItemVehicle_RichbitFileNumber"]').type('225588');
    //    //cy.get('#Vehivle_OK').click({force: true} );
    //})

    //it('copy invoice item', () => {
    //    cy.get('#edit-log-grid_0_30_9_0').click({force: true} );
    //    cy.get('#copy').click({force: true} );
    //})

    //it('More tab', () => {
    //    cy.get('#MORE').click({force: true} );
    //    LogHelper.LOVSearchAndSelectFirst("Customs.SupplierInvoice_ActualPayedCurrencyTypeCode", 'ILS')

    //    cy.get('input[id="Customs.SupplierInvoice_ActualPayedAmount"]').type('100');
    //})

    it('save', () => {
        //cy.get('#button-drp-down').click({force: true} );
       // cy.get('#CopyInvoiceWithItems').click({force: true} );
        cy.get('#SaveSupplierInvoice').click({force: true} );
    })
});





