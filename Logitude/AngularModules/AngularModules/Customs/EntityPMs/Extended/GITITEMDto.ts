
import{ Injectable, EventEmitter } from '@angular/core';
import { GITITEMCR } from '../../EntityPMs/Extended/GITITEMCR';
import { ServiceLocator } from '../../../Infrastructure/Locators/ServiceLocator';
import { PropertyChangedArgs } from '../../../Infrastructure/EventEmitterArgs/PropertyChangedArgs';
//import { Output, EventEmitter } from '@angular/core';
@Injectable()
export class GITITEMDto {
    //@Output() PropertyChanged: EventEmitter<PropertyChangedArgs> = new EventEmitter<PropertyChangedArgs>();
    public COUNTER: number;
    public PARTNERID: string;
    public ITEMNO: string;
    public SAPAKID: string;
    public OPENDATE: Date;
    public BRANCHID: string;
    public ACCOUNTINGCLOSE: boolean;
    public ITEMCLOSE: boolean;
    public ITEMCANCELLED: string;
    public ITEMOPENUSER: string;
    public ITEMUPDATEDATE: Date;
    public PRATID: string;
    public ITEMUPDATEUSER: string;
    public NOSTANDART: string;
    public APPROVTYPEID: string;
    public NAMEENG: string;
    public SEARCHENG: string;
    public ORIGINCOUNTRY: string;
    public UNITID: string;
    public TARIFFID: string;
    

    private gITITEMCRPMs: GITITEMCR[];
    get GITITEMCRPMs() {
        if (this.gITITEMCRPMs == null) {
            this.gITITEMCRPMs = [];
        }

        return this.gITITEMCRPMs;
    }
    set GITITEMCRPMs(newValue: GITITEMCR[]) {
        if (this.gITITEMCRPMs != newValue) {
            this.gITITEMCRPMs = newValue;
        }
    }
    public AddGITITEMCR(item: GITITEMCR) {
        if (item != null) {
            var index = this.GITITEMCRPMs.indexOf(item);
            if (index == -1) {
                this.GITITEMCRPMs.push(item);
                //this.MarkAsDirty();
            }
        }
    }
    public RemoveGITITEMCR(item: GITITEMCR) {
        if (item != null) {
            var index = this.GITITEMCRPMs.indexOf(item);
            if (index > -1) {
                this.GITITEMCRPMs.splice(index, 1);
                //this.MarkAsDirty();
            }
        }
    }

    public IsDirty: boolean;
    public DisableMarkAsDirty: boolean = false;
    /*
    MarkAsDirty(propertyName: string = null) {
        if (!this.DisableMarkAsDirty) {
            this.IsDirty = true;

            if (propertyName != null) {
                this.PropertyChanged.emit(new PropertyChangedArgs(propertyName, this));
                ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "Customs.GITITEM");

            }
        }
    }
    */
    
}
