import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool } from '../../../Infrastructure/Tools';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { DateTimePipe } from '../../../Controls/Pipes/DateTimePipe';
import { ChangeDetectorRef, Component } from '@angular/core';
import { TaxReportPMService } from 'Accounting/Services/StandardPMs/TaxReportPMService';
import { TaxReportLinePMService } from 'Accounting/Services/StandardPMs/TaxReportLinePMService';
import { TaxReportLineList } from 'Accounting/EntityLists/TaxReportLineList';
import { AccountingEntityHelper } from 'Accounting/Utilities/AccountingEntityHelper';
import { APPaymentList } from 'Invoice/EntityLists/APPaymentList';
import { MasavInterfaceStatus } from '../EditTabs/MasavInterface/MasavInterfaceDetailsTabComponent';

@Component({

    templateUrl: './MasavInterfaceListTemplate.html',
})

export class MasavInterfaceListTemplate {

    public rowData: any;
    public fieldName: any;
    public AdditionalData: any;
    public UpdateMessage: string;

    public isRTL: boolean = false;
    public showLocal: boolean = false;

    private CurrentSession = SessionLocator.SelectedSession;
    taxReportStatusCode: string;
    public readonly : boolean = false;
    constructor(private CD: ChangeDetectorRef) {
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.showLocal = !SessionLocator.LoggedUserPM.DontShowLocal;
        this.readonly = this.CurrentSession.CurrentEditComponent.EntityPM?.StatusCode ===  MasavInterfaceStatus.Transmitted;
    }

    setVariables(rowData: any, fieldName: string, MyAdditionalData: any) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();   
        }



    }
    private isChecked: boolean;
    get IsChecked() { return this.isChecked; }
    set IsChecked(newValue: boolean) {        
        this.CurrentSession.PseventRowSelectEvent.emit(this.rowData);
    }
    getMissingBankDetails(rowData: APPaymentList): string {
        let errorMessage = '';
        if (AppTool.IsNullOrEmpty(rowData.VendorBankAccount)) {
            errorMessage = TextCodeTranslator.Translate("APPayment.F.VendorBankAccount") + " " + TextCodeTranslator.Translate("APPayment.O.Missing");
        }
        if(AppTool.IsNullOrEmpty(rowData.VendorBankBranch)) {
            if (!AppTool.IsNullOrEmpty(errorMessage)) {
                errorMessage += ' ,';
            }
            errorMessage += TextCodeTranslator.Translate("APPayment.F.VendorBankBranch") + " " + TextCodeTranslator.Translate("APPayment.O.Missing");
        }
        if(AppTool.IsNullOrEmpty(rowData.VendorBankCode)) {
            if (!AppTool.IsNullOrEmpty(errorMessage)) {
                errorMessage += ' ,';
            }
            errorMessage +=TextCodeTranslator.Translate("APPayment.F.VendorBankCode") + " " + TextCodeTranslator.Translate("APPayment.O.Missing");;
        }
        return errorMessage;
    }
    

    OpenVendor(id,vendorPartnerTypeId) {      
        var billingTab = AccountingEntityHelper.GetBillingTabByPartnerType(vendorPartnerTypeId);
        var partnerTypeName = AccountingEntityHelper.GetPartnerTypeObjectTableName(vendorPartnerTypeId);
        AccountingEntityHelper.OpenCard(id,partnerTypeName,billingTab)
        
    }
    OpenAPPayment(id) {
        if (!AppTool.IsNullOrEmpty(id)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'APPayment' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                    });
                });
        }
    }
    
   
}
