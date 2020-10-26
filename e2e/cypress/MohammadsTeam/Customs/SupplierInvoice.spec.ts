
import  { NewImportDeclaration } from './NewImportDeclaration.spec';
import { LoginComp } from "../../login/Login.po";
export class SupplierInvoice {
  private login: LoginComp = new LoginComp();
  private importer: NewImportDeclaration = new NewImportDeclaration();  
}
describe('New  SupplierInvoice', () => {
  it('New  SupplierInvoice Created Successfully', function () {


   cy.window().then(win=> {
      const CustomFileNo= win.sessionStorage.getItem('CustomFileNo')
 cy.get('li[id=GeneralMHDeclarations]').click();
 
cy.get('input[id=SearchFieldsId_0_0]').should('be.visible').then( a=> {
    cy.get('input[id=SearchFieldsId_0_0]').type(CustomFileNo,{ force: true });
    cy.get('div[id=ListDataLoaded]').then( a=> {
          cy.get('div[id=LogGrid_0_0row0]').click({ force: true });
      })});

cy.get('#CustomsDeclarationTHInvoices').click();
cy.get('#Add_2').click();

 
      

 
     
      
    
});
   

  });

    it('supplier invoice fields', () => {

        cy.get('input[id="Customs.SupplierInvoice_AccountTypeCode"]').type('325');
        cy.get('ul[id="mydatalist_Customs.SupplierInvoice_AccountTypeCode"]').contains('325').then(a => {
            a[0].click();
        });
        cy.get('input[id="date_Customs.SupplierInvoice_IssueDate"]').type('16/10/2020')
        cy.get('input[id="Customs.SupplierInvoice_InvoiceCurrencyTypeCode"]').type('ILS').should("have.value", 'ILS');
        cy.get('ul[id="mydatalist_Customs.SupplierInvoice_InvoiceCurrencyTypeCode"]').contains('ILS').then(a => {
            a[0].click();
        });
        cy.get('input[id="Customs.SupplierInvoice_IncotermCode"]').type('DES').should("have.value", 'DES');
        cy.get('ul[id="mydatalist_Customs.SupplierInvoice_IncotermCode"]').contains('DES').then(a => {
            a[0].click();
        });
        cy.get('input[id="Customs.SupplierInvoice_IsPreference"]').check({ force: true }).should('be.checked')
        cy.get('input[id="Customs.SupplierInvoice_AccumalationStateCode"]').type('3').should("have.value", '3');
        cy.get('ul[id="mydatalist_Customs.SupplierInvoice_AccumalationStateCode"]').contains('3').then(a => {
            a[0].click();
        });
        cy.get('input[id="Customs.SupplierInvoice_InvoiceNumber"]').type('2020')
        cy.get('input[id="Customs.SupplierInvoice_VendorId"]').type('321').should("have.value", '321');
        cy.get('ul[id="mydatalist_Customs.SupplierInvoice_VendorId"]').contains('321').then(a => {
            a[0].click();
        });
        cy.get('input[id="Customs.SupplierInvoice_InvoiceAmount"]').type('100')
        cy.get('input[id="Customs.SupplierInvoice_IssueCountryCode"]').type('AD').should("have.value", 'AD');
        cy.get('ul[id="mydatalist_Customs.SupplierInvoice_IssueCountryCode"]').contains('AD').then(a => {
            a[0].click();
        });
        cy.get('input[id="Customs.SupplierInvoice_PreferenceDocumentTypeCode"]').type('15').should("have.value", '15');
        cy.get('ul[id="mydatalist_Customs.SupplierInvoice_PreferenceDocumentTypeCode"]').contains('15').then(a => {
            a[0].click();
        });
    })

    it('freight amount', () => {
        cy.get('#Add_3').click();
        cy.get('#edit-log-grid_0_20_0_0').click();
        cy.get('input[id="Customs.SupplierInvoiceFreightAmount_CurrencyTypeCode"]').type('ILS');
        cy.get('ul[id="mydatalist_Customs.SupplierInvoiceFreightAmount_CurrencyTypeCode"]').contains('ILS').then(a => {
            a[0].click();
        });
        cy.get('#edit-log-grid_0_20_1_0').click();
        cy.get('input[id="Customs.SupplierInvoice_Amount"]').type('50').should("have.value", '50');
    })

    it('Add Supplier invoice item', () => {
        cy.get('#Add_4').click();
        cy.get('input[id="Customs.SupplierInvoice_ItemCode"]').type('10')
        cy.get('#edit-log-grid_0_30_2_0').click();
        cy.get('input[id="Customs.SupplierInvoice_ItemDescription"]').type('1A').should("have.value", '1A');
        cy.get('#edit-log-grid_0_30_3_0').click();
        cy.get('input[id="Customs.SupplierInvoiceItem_ClassificationCode"]').type('123456782');
        cy.get('#edit-log-grid_0_30_4_0').click();
        cy.get('input[id="Customs.SupplierInvoice_TradeAgreementCode"]').type('10').should("have.value", '10');
        cy.get('ul[id="mydatalist_Customs.SupplierInvoice_TradeAgreementCode"]').contains('10').then(a => {
            a[0].click();
        });
        cy.get('#edit-log-grid_0_30_5_0').click();
        cy.get('input[id="Customs.SupplierInvoice_InvoiceQuantity"]').type('500');
       cy.get('#edit-log-grid_0_30_6_0').click();
        cy.get('input[id="Customs.SupplierInvoice_InvoiceQuantityType"]').type('C26');
        cy.get('ul[id="mydatalist_Customs.SupplierInvoice_InvoiceQuantityType"]').contains('C26').then(a => {
            a[0].click();
        });
        cy.get('#edit-log-grid_0_30_7_0').click();
        cy.get('input[id="Customs.SupplierInvoice_ItemPrice"]').type('300');
        cy.get('#edit-log-grid_0_30_8_0').click();
        cy.get('input[id="Customs.SupplierInvoice_OriginCountryCode"]').type('AD');
        cy.get('ul[id="mydatalist_Customs.SupplierInvoice_OriginCountryCode"]').contains('AD').then(a => {
            a[0].click();
        });
    })

    it('Edit Supplier invoice item', () => {
        cy.get('#edit-log-grid_0_30_9_0').click();
        cy.get('#Edit_3').click();
        cy.get('input[id="Customs.SupplierInvoiceItem_StatisticQuantity"]').type('100');
        cy.get('input[id="Customs.SupplierInvoiceItem_StatisticQuantityType"]').type('ANN');
        cy.get('ul[id="mydatalist_Customs.SupplierInvoiceItem_StatisticQuantityType"]').contains('ANN').then(a => {
            a[0].click();
        });
        cy.get('input[id="Customs.SupplierInvoiceItem_AdditionalQuantity"]').type('100');

        cy.get('input[id="Customs.SupplierInvoiceItem_AdditionalQuantityType"]').type('DAY');
        cy.get('ul[id="mydatalist_Customs.SupplierInvoiceItem_AdditionalQuantityType"]').contains('DAY').then(a => {
            a[0].click();
        });

        cy.get('#Add_6').click();
        cy.get('#edit-log-grid_0_50_0_0').click();
        cy.get('input[id="Customs.SupplierInvoiceItemProcesType_ProcessTypeCode"]').type('4100103');
        cy.get('ul[id="mydatalist_Customs.SupplierInvoiceItemProcesType_ProcessTypeCode"]').contains('4100103').then(a => {
            a[0].click();
        });
        cy.get('#Declarations').click();
        cy.get('#Add_7').click();
        cy.get('#edit-log-grid_0_60_0_0').click();
        cy.get('input[id="Customs.SupplierInvoiceItemsConDeclar_DeclarationTypeCode"]').type('4');
        cy.get('ul[id="mydatalist_Customs.SupplierInvoiceItemsConDeclar_DeclarationTypeCode"]').contains('4').then(a => {
            a[0].click();
        });
        cy.get('#edit-log-grid_0_60_1_0').click();
        cy.get('input[id="Customs.SupplierInvoiceItemsConDeclar_DeclarationNumber"]').type('1074');
        cy.get('#edit-log-grid_0_60_2_0').click();
        cy.get('input[id="Customs.SupplierInvoiceItemsConDeclar_InvoiceNumber"]').type('2020');
        cy.get('#edit-log-grid_0_60_3_0').click();
        cy.get('input[id="Customs.SupplierInvoiceItemsConDeclar_ItemSequence"]').type('1');
        cy.get('#edit-log-grid_0_60_4_0').click();
        cy.get('input[id="Customs.SupplierInvoiceItemsConDeclar_Quantity"]').type('10');
        cy.get('#SerialNumbers').click();
        cy.get('#Add_8').click();
        cy.get('#edit-log-grid_0_70_0_0').click();
        cy.get('input[id="Customs.SupplierInvoiceItemsSerialNum_TypeCode"]').type('CN');
        cy.get('ul[id="mydatalist_Customs.SupplierInvoiceItemsSerialNum_TypeCode"]').contains('CN').then(a => {
            a[0].click();
        });
        cy.get('#edit-log-grid_0_70_1_0').click();
        cy.get('input[id="Customs.SupplierInvoiceItemsSerialNum_SerialNumber"]').type('222');
        cy.get('#Add_9').click();

        cy.get('#edit-log-grid_0_80_0_0').click();
        cy.get('input[id="Customs.SupplierInvoiceItemsDescript_TypeCode"]').type('4');
        cy.get('ul[id="mydatalist_Customs.SupplierInvoiceItemsDescript_TypeCode"]').contains('4').then(a => {
            a[0].click();
        });
        cy.get('#edit-log-grid_0_80_1_0').click();
        cy.get('input[id="Customs.SupplierInvoiceItemsDescript_Description"]').type('ASDF');
        cy.get('#Add_10').click();
        cy.get('#edit-log-grid_0_90_0_0').click();

        cy.get('input[id="Customs.SupplierInvoiceItemsProdIdent_TypeCode"]').type('SS');
        cy.get('ul[id="mydatalist_Customs.SupplierInvoiceItemsProdIdent_TypeCode"]').contains('SS').then(a => {
            a[0].click();
        });
        cy.get('#edit-log-grid_0_90_1_0').click();
        cy.get('input[id="Customs.SupplierInvoiceItemsProdIdent_Identification"]').type('ASDF');

        cy.get('#Levies').click();
        cy.get('#Add_11').click();
        cy.get('#edit-log-grid_0_100_0_0').click();
        cy.get('input[id="Customs.SupplierInvoiceItemsLevy_TradeLevyExamptCode"]').type('1');
        cy.get('ul[id="mydatalist_Customs.SupplierInvoiceItemsLevy_TradeLevyExamptCode"]').contains('1').then(a => {
            a[0].click();
        });
        cy.get('#edit-log-grid_0_100_1_0').click();
        cy.get('input[id="Customs.SupplierInvoiceItemsLevy_TradeLevyNumber"]').type('1');

        cy.get('#Packages').click();
        cy.get('input[id="Customs.SupplierInvoiceItem_PackageQuantity"]').type('100');
        cy.get('input[id="Customs.SupplierInvoiceItem_Weight"]').type('500');
        cy.get('#SaveItem').click();
    })

    it('Add certificate', () => {
        cy.get('#edit-log-grid_0_30_9_0').click();
        cy.get('#Certificate').click();

        cy.get('#Add_12').click();
        cy.get('#edit-log-grid_0_110_1_0').click();
        cy.get('input[id="Customs.SupplierInvioceItemCertificat_ConfirmationTypeCode"]').type('104');
        cy.get('ul[id="mydatalist_Customs.SupplierInvioceItemCertificat_ConfirmationTypeCode"]').contains('104').then(a => {
            a[0].click();
        });
        cy.get('#edit-log-grid_0_110_2_0').click();
        cy.get('input[id="Customs.SupplierInvioceItemCertificat_AttachmentTypeCode"]').type('4');
        cy.get('ul[id="mydatalist_Customs.SupplierInvioceItemCertificat_AttachmentTypeCode"]').contains('4').then(a => {
            a[0].click();
        });
        cy.get('#edit-log-grid_0_110_3_0').click();

        cy.get('input[id="Customs.SupplierInvioceItemCertificat_CertificateNumber"]').type('4558');
        cy.get('#edit-log-grid_0_110_4_0').click();
        cy.get('input[id="Customs.SupplierInvioceItemCertificat_CertificateExemptionTypeCode"]').type('97');
        cy.get('ul[id="mydatalist_Customs.SupplierInvioceItemCertificat_CertificateExemptionTypeCode"]').contains('97').then(a => {
            a[0].click();
        });
        cy.get('#edit-log-grid_0_110_5_0').click();

        cy.get('input[id="Customs.SupplierInvioceItemCertificat_ResConfirmationTypeCode"]').type('104');
        cy.get('ul[id="mydatalist_Customs.SupplierInvioceItemCertificat_ResConfirmationTypeCode"]').contains('104').then(a => {
            a[0].click();
        });

        cy.get('#edit-log-grid_0_110_6_0').click();
        cy.get('input[id="Customs.SupplierInvioceItemCertificat_CustomsAttachmentID"]').type('104');

        cy.get('#edit-log-grid_0_110_7_0').click();
        cy.get('input[id="Customs.SupplierInvioceItemCertificat_ExternalRequestTypeCode"]').type('D');

        cy.get('#edit-log-grid_0_110_8_0').click();
        cy.get('input[id="Customs.SupplierInvioceItemCertificat_ApprovalRequestNumber"]').type('1000');

        cy.get('#CreateCertificate').click();
        cy.get('#ConfirmWindow_Yes_0').click();
    })

    it('update proccess type', () => {
        cy.get('#toggleButton').click();
        cy.get('#Update').click();

        cy.get('input[id="Customs.SupplierInvoiceItemProcesType_ProcessTypeCode"]').type('2100102');
        cy.get('ul[id="mydatalist_Customs.SupplierInvoiceItemProcesType_ProcessTypeCode"]').contains('2100102').then(a => {
            a[0].click();
        });
        cy.get('#undefined_UpdateSelected').check({ force: true });
        cy.get('input[id="FromNumber"]').type('1');
        cy.get('input[id="ToNumber"]').type('1');
        cy.get('#Add_Lines').click();
        cy.get('#UpdateProccess').click();
        cy.get('#MessageWindow_Ok_0').click();

    })

    it('update certificate', () => {
        cy.get('#toggleButton').click();
        cy.get('#ikea').click();
        cy.get('input[id="Customs.SupplierInvioceItemCertificat_ExternalRequestTypeCode"]').type('D');
        cy.get('input[id="Customs.SupplierInvioceItemCertificat_ApprovalRequestNumber"]').type('1000');
        cy.get('input[id="Customs.SupplierInvioceItemCertificat_ReqConfirmationTypeCode"]').type('104');
        cy.get('ul[id="mydatalist_Customs.SupplierInvioceItemCertificat_ReqConfirmationTypeCode"]').contains('104').then(a => {
            a[0].click();
        });
        cy.get('input[id="Customs.SupplierInvioceItemCertificat_AttachmentTypeCode"]').type('4');
        cy.get('ul[id="mydatalist_Customs.SupplierInvioceItemCertificat_AttachmentTypeCode"]').contains('4').then(a => {
            a[0].click();
        });
        cy.get('input[id="Customs.SupplierInvioceItemCertificat_CertificateNumber"]').type('4558');
        cy.get('input[id="Customs.SupplierInvioceItemCertificat_CertificateExemptionTypeCode"]').type('97');
        cy.get('ul[id="mydatalist_Customs.SupplierInvioceItemCertificat_CertificateExemptionTypeCode"]').contains('97').then(a => {
            a[0].click();
        });
        cy.get('input[id="Customs.SupplierInvioceItemCertificat_ResConfirmationTypeCode"]').type('105');
        cy.get('ul[id="mydatalist_Customs.SupplierInvioceItemCertificat_ResConfirmationTypeCode"]').contains('105').then(a => {
            a[0].click();
        });
        cy.get('#UpdateCertificate').click();
        cy.get('#MessageWindow_Ok_0').click();
    })

    it('update origine country', () => {
        cy.get('#toggleButton').click();
        cy.get('#UpdateCountryOfOrigin').click();
        cy.get('input[id="Customs.SupplierInvoiceItem_OriginCountryCode"]').type('AE');
        cy.get('ul[id="mydatalist_Customs.SupplierInvoiceItem_OriginCountryCode"]').contains('AE').then(a => {
            a[0].click();
        });
        cy.get('#undefined_UpdateAll').check({ force: true });
        cy.get('#Update_Country').click();
    })

    it('add vehicle', () => {
        //cy.get('#edit-log-grid_0_30_9_0').click();
        //cy.get('#Vehicle').click();
        //cy.get('#Add_13').click();
        //cy.get('#edit-log-grid_0_140_1_0').click();
        //cy.get('input[id="Customs.SupplierInvoiceItemVehicle_RichbitFileNumber"]').type('225588');
        //cy.get('#Vehivle_OK').click();
    })

    it('copy invoice item', () => {
        cy.get('#edit-log-grid_0_30_9_0').click();
        cy.get('#copy').click();
    })

    it('MORE tab', () => {
        cy.get('#MORE').click();
        cy.get('input[id="Customs.SupplierInvoice_ActualPayedCurrencyTypeCode"]').type('ILS');
        cy.get('ul[id="mydatalist_Customs.SupplierInvoice_ActualPayedCurrencyTypeCode"]').contains('ILS').then(a => {
            a[0].click();
        });
        cy.get('input[id="Customs.SupplierInvoice_ActualPayedAmount"]').type('100');
    })

    it('save', () => {
        cy.get('#button-drp-down').click();
        cy.get('#CopyInvoiceWithItems').click();
        cy.get('#SaveSupplierInvoice').click();
    })
});





