
import {Component} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../../infrastructure/Tools';
import {TextCodeTranslator} from '../../../../infrastructure/Utilities/TextCodeTranslator';
import {ObjectFieldPM} from '../../../../infrastructure/EntityPMs/ObjectFieldPM';
import {ObjectTablePM} from '../../../../infrastructure/EntityPMs/ObjectTablePM';
import {ApiQueryFilters, FilterItem} from '../../../../infrastructure/DataContracts/ApiQueryFilters';
import {EntityResourceService} from '../../../../infrastructure/Services/EntityResourceService';
import {EntityListService} from '../../../../infrastructure/Services/EntityListService';
import {ServiceResponse} from '../../../../infrastructure/DataContracts/ServiceResponse';
import {Observable} from 'rxjs/Observable';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {GetStackWindowArgs} from '../../../../Common/Args';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';

@Component({
    moduleId: module.id,
    selector: 'FBLStockFieldComponent',
    templateUrl: './FBLStockFieldComponent.html',
})

export class FBLStockFieldComponent {
    private CurrentSession = SessionLocator.SelectedSession;
    public ObjectField: ObjectFieldPM;
    public ObjectTableName: string;
    public DataContext: ShipmentPM;
    public IsNewEntityCall: boolean;
    ObjectTable: ObjectTablePM;

    public IsDataReady: boolean = false;

    public IsFromStockVisible: boolean = false;
    public IsFromStockEnabled: boolean = false;
    public IsReturnStockVisible: boolean = false;
    public IsReturnStockEnabled: boolean = false;
    public HideCoumns: boolean = false;

    SetUIProperties_StockButton() {
        
        var setting = SessionLocator.TenantSettings.filter(s => s.SettingCode == "HAWBCounterO_E_D")[0];
        if (this.DataContext.TransportModeId == "O" && (this.DataContext.DirectionId == "E" || this.DataContext.DirectionId == "D") && this.DataContext.ShipmentLevelCode != "C" && setting != null && setting.SettingValue == "Stock") {
            var isFromStockVisible = false;
            var isReturnStockVisible = false;

            var isFromStockEnabled = true;
            var isReturnStockEnabled = true;

            if (this.DataContext.FBLIsFromStock || this.DataContext.FBLTakenFromStock) {
                isReturnStockVisible = true;
                isFromStockEnabled = false;
            }

            isFromStockVisible = !isReturnStockVisible;


            this.IsFromStockVisible = isFromStockVisible;
            this.IsFromStockEnabled = isFromStockEnabled;
            this.IsReturnStockVisible = isReturnStockVisible;
            this.IsReturnStockEnabled = isReturnStockEnabled;

            this.DataContext.UIProperties.SetEnabled("House", this.ObjectTableName, isFromStockEnabled);
            this.HideCoumns = true;
        }
        else {
            this.IsFromStockVisible = false;
            this.IsReturnStockVisible = false;
        }
    }

    public Run(args: any) {
        this.DataContext = args['DataContext'];
        this.ObjectField = args['ObjectField'];
        this.ObjectTableName = args['ObjectTableName'];
        this.IsNewEntityCall = args['IsNewEntityCall'];

        this.SetUIProperties_StockButton();
        this.IsDataReady = true;
         
    }

    private isGetFromStock: boolean = false;
    private myOldFBLStockNumber: string;
    
    GetFBLStockClicked() {
        var windowArgs = new GetStackWindowArgs();
       
        var logWindow = new LogitudeWindow();
        logWindow.Width = 750;
        logWindow.Height = 450;
        logWindow.WindowArgs = windowArgs;
        logWindow.Title = "Select FBL Number";
        logWindow.Show('./ShipmentModules/ShipmentStock/Components/FBLStock/FBLStackSelectionComponent');

        logWindow.WindowClosed.subscribe(s => {
            if (s) {
                if (windowArgs.SelectedFBLStock != null) {

                    var stackNumber: number = windowArgs.SelectedFBLStock.Number;
                    this.myOldFBLStockNumber = this.DataContext.FBLStockNumber;
                    this.DataContext.FBLTakenFromStock = true;
                    this.DataContext.FBLStockNumber = stackNumber.toString();

                    this.isGetFromStock = true;

                    this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                        if (isSaveSuccess) {
                            this.DataContext = this.CurrentSession.CurrentEditComponent.EntityPM;
                        }
                        else {

                            if (this.isGetFromStock) {
                                this.DataContext.FBLTakenFromStock = false;
                                this.DataContext.FBLStockNumber = this.myOldFBLStockNumber;
                                this.isGetFromStock = false;
                            }
                        }

                        this.SetUIProperties_StockButton();
                        
                    });
                    this.CurrentSession.CurrentEditComponent.SaveChanges();
                  
                }
            }
        });
    }
    ReturnFBLStockClicked() {
        if (this.DataContext.FBLIsFromStock || this.DataContext.FBLTakenFromStock) {
        
                //this.SetMAWBAirline();
            this.DataContext.FBLReturnedToStock = true;
            this.DataContext.FBLStockNumber = this.DataContext.House;
             this.isGetFromStock = false;

             this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                 if (isSaveSuccess) {
                     this.DataContext = this.CurrentSession.CurrentEditComponent.EntityPM;
                 }
                 else {

                     //if (this.isGetFromStock) {
                     //    this.DataContext.FBLTakenFromStock = false;
                     //    this.DataContext.FBLStockNumber = this.myOldFBLStockNumber;
                     //    this.isGetFromStock = false;
                     //}
                 }

                 this.SetUIProperties_StockButton();

             });
             this.CurrentSession.CurrentEditComponent.SaveChanges();

           
        }
    }

}
