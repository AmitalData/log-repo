import {Component, OnInit} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator'; 
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {DateTool} from '../../../../Infrastructure/Tools';
import {CustomerProductPM} from '../../../../Common/EntityPMs/CustomerProductPM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {CustomerList} from '../../../../Common/EntityLists//CustomerList'; 
import {CustomerProductExtendedService} from  '../../../../Common/Services/ExtendedPMs/CustomerProductExtendedService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    selector: 'BlockedCustomerComponent',
    moduleId: module.id,
    templateUrl: './BlockedCustomerComponent.html',
})

export class BlockedCustomerComponent extends BaseComponent {
    public entityList: CustomerList;
    public ObjectTableName: string = "Customer";
    public DataContext: BlockedCustomerComponent = this;
    public ProductsObslist: ProductObslistItem[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.ProductsObslist = [];
      
    }
    SetWindowArgs(args: any) {
        if (args) {
            this.entityList = args.CustomerList;
            this.LoadData();
        }
    }

    // Properties 
    get Code() { return this.entityList.Code; }
    get Name() { return this.entityList.EnglishName; }
    get VAT() { return this.entityList.VatNumber; }
    get Status() { return this.entityList.CustomerStatusName; }
    get StatusCode() { return this.entityList.CustomerStatusCode; }
    get Salesman() { return this.entityList.SalesmanUserEnglishName; }
    get StartWorkingDate() { return this.entityList.StartWorkingDate; }
    get LastShipmentDate() { return this.entityList.LastShipmentDate; }
    get LastInteractionDate() { return this.entityList.LastInteractionDate; }

    //LoadDate
    private LoadData() {
        this.ProductsObslist = [];
        var service = new CustomerProductExtendedService();
        service.GetCustomerProducts(this.entityList.Id, SessionLocator.Tenant).subscribe((response: ServiceResponse) => {
            //SessionLocator.CurrentSession.CurrentWindow.StopBusyIndicator();
            if (!response.HasError) {
                var myResult = response.Result;
                if (myResult) {
                    myResult.forEach((item) => {
                        this.ProductsObslist.push(new ProductObslistItem(item));
                    });
                }
            }
        });
    }

    // Commands 
    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}

export class ProductObslistItem {
    public entityPM: CustomerProductPM;
    constructor(item: CustomerProductPM) {
        this.entityPM = item;
    }

    get Name() { return this.entityPM.ProductTypeName; }
    get LastShipmentDate()
    {
        var myResult = "No Shipments";
        //if (this.entityPM.LastShipmentDate != null) {
        //    var dateTime = this.entityPM.LastShipmentDate;
        //    var todayDate = DateTool.GetCurrentDateTimeAsUtc();
        //    if (dateTime.valueOf() == todayDate.valueOf()) {
        //        myResult = TextCodeTranslator.Translate("General.O.Today");
        //    }
        //    else if (dateTime.valueOf() == todayDate.Date.AddDays(-1)) {
        //        myResult = TextCodeTranslator.Translate("General.O.Yesterday");
        //    }
        //    else if (dateTime..valueOf() == todayDate.Date.AddDays(1)) {
        //        myResult = TextCodeTranslator.Translate("General.O.Tomorrow");
        //    }
        //    else {
        //        myResult = dateTime.ToString("d", CultureInfo.CurrentCulture);
        //    }
        //}
        return myResult;
    }
}
