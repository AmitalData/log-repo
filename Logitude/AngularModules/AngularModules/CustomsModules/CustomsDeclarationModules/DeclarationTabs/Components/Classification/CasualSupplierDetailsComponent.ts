import { Component } from '@angular/core';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { AppTool, FormatTool } from '../../../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { DeclarationPM } from '../../../../../Customs/EntityPMs/DeclarationPM';
import { DeclarationPMService } from '../../../../../Customs/Services/StandardPMs/DeclarationPMService';
import { ClientList } from '../../../../../Customs/EntityLists/ClientList';
import { CustomerIdentifyTypePM } from '../../../../../Customs/EntityPMs/CustomerIdentifyTypePM';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';

@Component({

    moduleId: module.id,
    templateUrl: './CasualSupplierDetailsComponent.html',
    selector: 'CasualSupplierDetailsComponent',

})
export class CasualSupplierDetailsComponent extends BaseComponent {
    public EntityPM: DeclarationPM;
    public DataContext: any = this;
    type: string;
    public ObjectTableName: string = "Customs.Declaration";
    declarationPMService: DeclarationPMService = new DeclarationPMService();
    public OriginalEntityPM: DeclarationPM;
    public ClonedEntityPM: DeclarationPM;

    constructor() {
        super();


    }


    //#region properties

   


    get CasualSupplierName() { return this.EntityPM.CasualSupplierName; }
    set CasualSupplierName(value: string) { this.EntityPM.CasualSupplierName = value; }
    
    get CasualSupplierAddress() { return this.EntityPM.CasualSupplierAddress; }
    set CasualSupplierAddress(value: string) { this.EntityPM.CasualSupplierAddress = value; }

    


    public IsEntitleImporterEnabled: boolean = true;
    public IsTransferImporterEnabled: boolean = true;
    public IsImporterEnabled: boolean = true;

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.EntityPM = args.EntityPM;
            this.OriginalEntityPM = args.EntityPM;
            this.ClonedEntityPM = this.CloneEntity(args.EntityPM);
            this.type = args.Type;
        }
    }

   
  
    CloneEntity(entityToClone: DeclarationPM) {

        var clonedEntity: DeclarationPM;
        clonedEntity = new DeclarationPM();

        this.MapEntitytoEntity(entityToClone, clonedEntity);


        return clonedEntity;
    }

    RejectChanges() {
        this.MapEntitytoEntity(this.ClonedEntityPM, this.OriginalEntityPM, true);
    }

    MapEntitytoEntity(srcEntity: any, targetEntity: any, takeKeysFromTarget: boolean = false) {
        var keys;
        keys = Object.keys(takeKeysFromTarget ? targetEntity : srcEntity);
        for (var key in keys) {
            var property = keys[key];
            targetEntity[property] = srcEntity[property];
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        SessionLocator.CurrentSession.CloseCurrentWindowEmit("cancel");
    }

    
    OkButtonClicked() {
        

        SessionLocator.CurrentSession.CurrentEditComponent.SaveChanges();
        SessionLocator.CurrentSession.CloseCurrentWindowEmit("ok");





    }

}
