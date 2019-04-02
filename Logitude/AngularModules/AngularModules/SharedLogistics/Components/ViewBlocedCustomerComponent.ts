import {Component, OnInit}  from '@angular/core';
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {Guid} from '../../Infrastructure/Utilities/Guid';
import {SharedLogisticsService} from '../Services/Others/SharedLogisticsService';

import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {SessionInfo} from '../../Infrastructure/Utilities/SessionInfo';
import {TextCodeTranslator} from '../../Infrastructure/Utilities/TextCodeTranslator';
import {CustomerProductExtendedService} from '../../Common/Services/ExtendedPMs/CustomerProductExtendedService';

import {CustomerPM} from '../../Common/EntityPMs/CustomerPM';
import {CustomerProductPM} from '../../Common/EntityPMs/CustomerProductPM';
@Component({
    moduleId: module.id,
    selector: 'ViewBlocedCustomerComponent',
    templateUrl: './ViewBlocedCustomerComponent.html',
    providers: [CustomerProductExtendedService],
})

export class ViewBlocedCustomerComponent implements OnInit {

    ProductsObslist: ProductObslistItemClass[]
    private entityPM: CustomerPM;


    Code :string;
    Name: string;
    VAT: string;
    Status: string;
    StatusCode: string;
    Salesman: string;
    StartWorkingDate: Date;
    LastShipmentDate: Date;
    LastInteractionDate: Date;
    NoProductsVisibility: boolean;
    StatusCodeColor: string = "#E483FB";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _customerProductExtendedService: CustomerProductExtendedService) {
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
    }

    ngOnInit(

    ) {

        this.Run();

    }


    Run() {

        if (this.entityPM) {
            this.Code = this.entityPM.Code;
            this.Name = this.entityPM.EnglishName;
            this.VAT = this.entityPM.VatNumber;
            this.Status = this.entityPM.CustomerStatusName;
            this.StatusCode = this.entityPM.CustomerStatusCode;
            this.Salesman = this.entityPM.SalesmanUserEnglishName;

            this.StartWorkingDate = this.entityPM.StartWorkingDate;
            this.LastShipmentDate = this.entityPM.LastShipmentDate;
            this.LastInteractionDate = this.entityPM.LastInteractionDate;

            if (this.StatusCode != null) {
           
                if (this.StatusCode == "POT") {
                    this.StatusCodeColor = "#E36C0A";
                }

                else if (this.StatusCode == "ACT") {
                    this.StatusCodeColor  = "#00B076";
                }

                else if (this.StatusCode == "WAC") {
                    this.StatusCodeColor  = "#FF0000";
                }

                else if (this.StatusCode == "INA") {
                    this.StatusCodeColor  = "#F40CB2";
                }
            }

            this.LoadData();
        }
    }

    LoadData() {
        this.ProductsObslist = [];
    
        this._customerProductExtendedService.GetCustomerProducts(this.entityPM.Id, SessionInfo.LoggedUserTenant).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    myResult.forEach((item) => {
                        this.ProductsObslist.push(new ProductObslistItemClass(item));
                    });

                    this.NoProductsVisibility = this.ProductsObslist.length == 0 ?true : false;
                }

            }
           
        });

    }


  

    CloseButtonClicked() {

        this.CurrentSession.CloseCurrentWindow();
    }





    SetWindowArgs(args: any) {
        this.entityPM = args.CustomerPM;
      

    }


}
class ProductObslistItemClass {
    LastShipmentDate: Date;
    Name: string;
    private entityPM: CustomerProductPM ;
    constructor(item: CustomerProductPM) {
        this.Name = item.ProductTypeName;
        this.LastShipmentDate = item.LastShipmentDate;
        this.entityPM = item; 

    }

}
