import { OnDestroy, Component, ChangeDetectorRef } from '@angular/core';
import { BaseComponent } from '../../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AppTool } from '../../../../../../../Infrastructure/Tools';
import { ConsignmentPackDangerPM } from '../../../../../../../Customs/EntityPMs/ConsignmentPackDangerPM';
import { DecDangersContactPM } from '../../../../../../../Customs/EntityPMs/DecDangersContactsPM';
import { SessionLocator } from '../../../../../../../Infrastructure/Utilities/SessionLocator';


@Component({
    selector: 'ConsigmentPackagesDangerComponent',
    moduleId: module.id,
    templateUrl: './ConsigmentPackagesDangerComponent.html',
})

export class ConsigmentPackagesDangerComponent
    extends BaseComponent
    implements OnDestroy {
    public DataContext: any = this;
    public ObjectTableName: string = "Customs.ConsignmentPackDanger";
    public ObjectTableNameContact: string = "Customs.DecDangersContact";

    public OriginalConsignmentPackDangerPM: ConsignmentPackDangerPM; 
    public OriginalDecDangersContactPM: DecDangersContactPM; 
    private CurrentSession = SessionLocator.SelectedSession;

    
    SetWindowArgs(args: any) {
         if (!AppTool.IsNullOrEmpty(args)) {
            //this.IsDisplayOnly = args.IsDisplayOnly;
         
             this.OriginalConsignmentPackDangerPM = args.ConsignmentPackagesDangerPM;
             alert(this.OriginalConsignmentPackDangerPM.UNCode);
              //this.ClonedItemPM = this.CloneEntity(args.SupplierInvoiceItemPM);

            //this.CustomsItem = this.OriginalItemPM.TaxExemptCode;

            //this.FillGridsData(); // copy  grids data from entity PM to ItemSource arrays

 
            //if (this.IsDisplayOnly) {
            //    this.SetScreenFieldsEditability();
            //}
        }
       // console.log("--> EditSupplierInvoiceItem window argument passed: ", args);
        //this.CheckRequrierdFieldsForSend();
    }


    CancelButtonClicked() {
    //    this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    //RejectChanges() {
    //    this.MapEntitytoEntity(this.ClonedItemPM, this.OriginalItemPM, true);
    //}

    //MapEntitytoEntity(srcEntity: any, targetEntity: any, takeKeysFromTarget: boolean = false) {
    //    var keys;
    //    keys = Object.keys(takeKeysFromTarget ? targetEntity : srcEntity);
    //    for (var key in keys) {
    //        var property = keys[key];
    //        targetEntity[property] = srcEntity[property];
    //    }
    //}

    ngOnDestroy(): void {
       // throw new Error("Method not implemented.");
    }

}
