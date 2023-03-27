import { Component } from '@angular/core';
import { SessionLocator } from '../../../../../../Infrastructure/Utilities/SessionLocator';
import { AppTool, FormatTool } from '../../../../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { DeclarationPM } from '../../../../../../Customs/EntityPMs/DeclarationPM';
import { ClientList } from '../../../../../../Customs/EntityLists/ClientList';
import { CustomerIdentifyTypePM } from '../../../../../../Customs/EntityPMs/CustomerIdentifyTypePM';
import { MessageWindow } from '../../../../../../Controls/Windows/MessageWindow';
import { DeclarationPMService } from '../../../../../../Customs/Services/StandardPMs/DeclarationPMService';
import { DeclarationExportRecipientPM } from '../../../../../../Customs/EntityPMs/DeclarationExportRecipientPM';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { ConsignmentPM } from '../../../../../../Customs/EntityPMs/ConsignmentPM';
import { BaseComponent } from '../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';

@Component({

    
    templateUrl: './ComprehensiveUpdateConsignment.html',
    selector: 'ComprehensiveUpdateConsignment',

})
export class ComprehensiveUpdateConsignment extends BaseComponent {

    public DeclarationPM: DeclarationPM;
 
    public ObjectTableName: string = "Customs.Consignment";

    public DataContext: any = this;
    
    constructor() {
        super();

    }


    //#region properties

    public _ExportLoadingPortCode:string;

    public get ExportLoadingPortCode() { return this._ExportLoadingPortCode; }
    public set ExportLoadingPortCode(newValue: string) {
        this._ExportLoadingPortCode = newValue;
      
    }

    public _StorageSiteCodeExport:string;

    public get StorageSiteCodeExport() { return this._StorageSiteCodeExport; }
    public set StorageSiteCodeExport(newValue: string) {
        this._StorageSiteCodeExport = newValue;
      
    }

    public _FinalDestinationPortCode:string;

    public get FinalDestinationPortCode() { return this._FinalDestinationPortCode; }
    public set FinalDestinationPortCode(newValue: string) {
        this._FinalDestinationPortCode = newValue;
       
    }

    public _ShipCode:string;

    public get ShipCode() { return this._ShipCode; }
    public set ShipCode(newValue: string) {
        this._ShipCode = newValue;
        
    }

    public _ExportUnloadingPortCode:string;

    public get ExportUnloadingPortCode() { return this._ExportUnloadingPortCode; }
    public set ExportUnloadingPortCode(newValue: string) {
        this._ExportUnloadingPortCode = newValue;
    }






    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.DeclarationPM = args.EntityPM;

        }
    }

    CancelButtonClicked() {
        SessionLocator.SelectedSession.CloseCurrentWindowEmit("cancel");
    }


    doDisable: boolean;


    OkButtonClicked() {

       this.DeclarationPM.Consignments.filter(y=>y.ConsignmentType == 'E').forEach(x=>{
        x.ExportLoadingPortCode=!AppTool.IsNullOrEmpty(this._ExportLoadingPortCode)?this._ExportLoadingPortCode:x.ExportLoadingPortCode
        x.StorageSiteCode=!AppTool.IsNullOrEmpty(this._StorageSiteCodeExport)?this._StorageSiteCodeExport:x.StorageSiteCodeExport
        x.FinalDestinationPortCode=!AppTool.IsNullOrEmpty(this._FinalDestinationPortCode)?this._FinalDestinationPortCode:x.FinalDestinationPortCode
        x.ShipCode=!AppTool.IsNullOrEmpty(this._ShipCode)?this._ShipCode:x.ShipCode
        x.ExportUnloadingPortCode=!AppTool.IsNullOrEmpty(this._ExportUnloadingPortCode)?this._ExportUnloadingPortCode:x.ExportUnloadingPortCode
    })

            SessionLocator.SelectedSession.CloseCurrentWindowEmit("ok");

    }

}
