import { Component } from '@angular/core';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { AppTool, FormatTool } from '../../../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { DeclarationPM } from '../../../../../Customs/EntityPMs/DeclarationPM';
import { ClientList } from '../../../../../Customs/EntityLists/ClientList';
import { CustomerIdentifyTypePM } from '../../../../../Customs/EntityPMs/CustomerIdentifyTypePM';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';
import { DeclarationPMService } from '../../../../../Customs/Services/StandardPMs/DeclarationPMService';

@Component({

    moduleId: module.id,
    templateUrl: './ExportDeclarationComponent.html',
    selector: 'ExportDeclarationComponent',

})
export class ExportDeclarationComponent extends BaseComponent {
    public EntityPM: DeclarationPM;
    public DataContext: any = this;
    type: string;
    public ObjectTableName: string = "Customs.Declaration";
    declarationPMService: DeclarationPMService = new DeclarationPMService();
    public OriginalEntityPM: DeclarationPM;
    public ClonedEntityPM: DeclarationPM;
    public IsDisplayOnly: boolean = false;

    constructor() {
        super();


    }


    //#region properties

   
    public get LoadingDateTime() { return this.EntityPM.LoadingDateTime; }
    public set LoadingDateTime(newValue: Date) { this.EntityPM.LoadingDateTime = newValue; }



    public get ShipCode() { return this.EntityPM.ShipCode; }
    public set ShipCode(newValue: string) {
        this.EntityPM.ShipCode = newValue;
    }

    _Ship: any;
    public get Ship() { return this._Ship; }
    public set Ship(newValue: string) {
        this._Ship;
    }
    

    public get DestinationCountryCode() { return this.EntityPM.DestinationCountryCode; }
    public set DestinationCountryCode(newValue: string) {
        this.EntityPM.DestinationCountryCode = newValue;
    }

    

    public get IsExporterConfirmation() { return this.EntityPM.IsExporterConfirmation; }
    public set IsExporterConfirmation(newValue: boolean) {
        this.EntityPM.IsExporterConfirmation = newValue;
    }


}
