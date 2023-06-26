import { Component, OnInit} from '@angular/core';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';
import { DateTool, AppTool } from 'Infrastructure/Tools';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ConfirmWindow } from 'Controls/Windows/ConfirmWindow';
import { CargoTrackingExtendedPMService, CargoTrackingArgs } from 'Accounting/Services/ExtendedPMs/CargoTrackingExtendedPMService';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
 

@Component({
    
    templateUrl: './CargoTrackingBuildShipmentComponent.html',
})

export class CargoTrackingBuildShipmentComponent extends BaseComponent implements OnInit {
   LayoutDirection: string = 'ltr';
  private CurrentSession = SessionLocator.SelectedSession;
  public ValidationErrorsList: string[] = [];
  public ObjectTableName: string = "CargoTrackingShipments";
  public iCargoTrackingExtendedPMService:CargoTrackingExtendedPMService = new CargoTrackingExtendedPMService();
  DataContext: any = this;
  constructor() {
       super();
       this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
       this.SetUIProperty();
   } 
  ngOnInit(): void {
    //  this.InitializeDate();
   }
  CancelButtonClicked() {
     this.CurrentSession.CloseCurrentWindow();
  }

  OkButtonClicked(){
    // this.ValidateCargoTrackingBuildScreen();
    // if(this.ValidationErrorsList == null || this.ValidationErrorsList.length==0){
         this.OpenConfirmMessage();
    // }
 }
 
//  ValidateCargoTrackingBuildScreen(){
//     this.ValidationErrorsList=[];
//     if(this.SelectedValue ==="S" && AppTool.IsNullOrEmpty(this.Tenant)){
//         this.ValidationErrorsList.push("Please enter the number of an existing Tenant");
//     }
//     else{
//         this.ValidateDate(true);
//     }
//  }

 private OpenConfirmMessage(){

    var MessageText:string = "This action will delete all cargo tracking shipments and their related records from all tenants in the system and will create new ones for all tenants only for the selected dates";
    if(this.SelectedValue ==="S"){
        MessageText = "This action will delete all cargo tracking shipments and their related records from (Tenant "+this.Tenant+") in the system and will create new ones in that Tenant only for the selected dates";
    }

      var confirmWindow = new ConfirmWindow();
      confirmWindow.Width = 390;
      confirmWindow.Show(MessageText);
      confirmWindow.WindowClosed.subscribe((event: any) => {
          if (confirmWindow.Yes) {
            this.CargoTrackingBuilder();
          }
      });
 }

//   private fromDate: Date;
//     public get FromDate() { return this.fromDate; }
//     public set FromDate(value: Date) {
//         if (this.fromDate != value) {
//             this.fromDate = value;
//             this.ValidateDate();

//         }
//     }
    private tenant: number;
    public get Tenant() { return this.tenant; }
    public set Tenant(value: number) {
        if (this.tenant != value) {
            this.tenant = value;
        }
    }
//   private toDate: Date;
//     public get ToDate() { return this.toDate; }
//     public set ToDate(value: Date) {
//         if (this.toDate != value) {
//             this.toDate = value;
//             this.ValidateDate();
           

//         }
//   }
  private selectedValue: string = "All";
  public get SelectedValue() {
  
      return this.selectedValue;

  }
  public set SelectedValue(value: string) {
      if (this.selectedValue != value) {
          this.selectedValue = value;
       }
  }
  itemClicked(itemValue: string) {
    if (this.SelectedValue != itemValue) {
        this.SelectedValue = itemValue;
     }
}
 
//   ValidateDate(fromButtonClick: boolean=false) {
//     if(!this.FromDate || !this.ToDate){
//       this.ValidateIsToDateAndFromDateFilled(fromButtonClick);
//     }
//     else if (DateTool.GetDateFromDate(this.FromDate, true) > DateTool.GetDateFromDate(this.ToDate, true)) {
//         this.ValidateIsToDateGreaterOrEqualFromDate(fromButtonClick);
//      }
//      else{
//         this.SetUIPropertiesDateValidation(true);
//      }
    
// }
// ValidateIsToDateAndFromDateFilled(fromButtonClick: boolean){
//     var toDateValidationText="''To date'' field is required";
//     var fromDateValidationText="''From date'' field is required";
//     var fromtoDateValidationText="Please Fill All Dates Fields";
//     if (fromButtonClick) {
//         this.ValidationErrorsList.push(fromtoDateValidationText);
//     }
//     this.SetUIPropertiesDateValidation(false,toDateValidationText,fromDateValidationText);
        
// }

// ValidateIsToDateGreaterOrEqualFromDate(fromButtonClick: boolean){
    // var toDateValidationText="''To date'' field must be greater than or equal to ''From date'' field";
    // var fromDateValidationText="''From date'' field must be less than or equal to ''To date'' field";
    // var fromtoDateValidationText="''To date'' field must be greater than or equal to ''From date'' field";
    // if (fromButtonClick) {
    //     this.ValidationErrorsList.push(fromtoDateValidationText);
    // }
    // this.SetUIPropertiesDateValidation(false,toDateValidationText,fromDateValidationText);
        
// }

SetUIPropertiesDateValidation(isValid:boolean,toDateText:string=null,fromDateText:string=null){
    // this.UIProperties.SetValidity("ToDate", this.ObjectTableName, isValid, toDateText);
    // this.UIProperties.SetValidity("FromDate", this.ObjectTableName, isValid, fromDateText);
}

SetUIProperty() {
     
}
 

CargoTrackingBuilder() {
    var iCargoTrackingArgs:CargoTrackingArgs=  new CargoTrackingArgs();  
    // iCargoTrackingArgs.FromDate = this.fromDate;
    // iCargoTrackingArgs.ToDate = this.toDate;
    iCargoTrackingArgs.Tenant = this.Tenant;
    this.CurrentSession.StartBusyIndicatorLoading();
    this.iCargoTrackingExtendedPMService.PostCargoTrackingBuilder(iCargoTrackingArgs).subscribe((response: ServiceResponse) => {
    this.CurrentSession.StopBusyIndicator();
    var  response: ServiceResponse = response;
      if (!response.HasError) {
          this.CurrentSession.CloseCurrentWindow();
      }
      else {
        this.ValidationErrorsList.push(response.ErrorsArray.toString());
      }

    });
}

// InitializeDate(){
  
//     var month = new Date().getMonth();
//     var Year = new Date().getFullYear();
//     var Day = new Date().getDate();
//     this.ToDate = this.SetDate(Year, month, Day);
//     this.FromDate = this.SetDate(Year, month, Day);
//     this.FromDate.setMonth(this.ToDate.getMonth()- 6);
//   }

//   SetDate(year: number, month: number, day: number) {
//     var date = new Date();
//     date.setUTCFullYear(year);
//     date.setUTCMonth(month);
//     date.setUTCDate(day);
//     date.setUTCHours(0);
//     date.setUTCMinutes(0);
//     date.setUTCSeconds(0);
//     date.setUTCMilliseconds(0);

//     return date;
// }
}



 
