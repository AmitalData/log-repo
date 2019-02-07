import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Component, OnInit}  from '@angular/core';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {DocumentTypePM} from '../../../../../Common/EntityPMs/DocumentTypePM';
import {DocumentTypeCopyPM} from '../../../../../Common/EntityPMs/DocumentTypeCopyPM';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';

@Component({
    moduleId: module.id,
    selector: 'SharedLogisticsTab',
    templateUrl: './PrintingOptionsComponent.html',
})

export class PrintingOptionsComponent extends BaseComponent implements OnInit {
    public EntityPM: DocumentTypePM;
    public DocumentTypeCopies: DocumentTypeCopyPM[];
    SelectedCopy: DocumentTypeCopyPM;
    public ComboBoxIsDisabled: boolean = false;
    constructor(public entityArgs: EntityArgs) {
        super();
    }

    ngOnInit() {
        this.EntityPM = this.entityArgs.EntityPM;

        if (this.EntityPM) {

            var iCode: string = this.EntityPM.Code;
            if (iCode) {
                iCode = iCode.toUpperCase();
                switch (iCode) {
                    case "999S":
                    case "999C":
                    case "999M":
                    case "999CI":
                    case "999MP":
                    case "999P":
                        {

                            if (SessionLocator.TenantPM.CountryCode == "IL" && SessionLocator.LoggedUserPM.IsCustomerCare == false) {
                                this.ComboBoxIsDisabled = true;
                                this.EntityPM.UIProperties.SetEnabled("IsDocumentOneTimePrintLimited", "DocumentType", false)
                            }

                            break;
                        }
                }
            }

            this.Run();
        }
    }



    Run() {

        if (this.EntityPM.DocumentTypeCopies) {
            this.DocumentTypeCopies = this.EntityPM.DocumentTypeCopies;
            this.SelectedCopy = this.DocumentTypeCopies.filter(d => d.Id == this.EntityPM.LimitedPrintCopyId)[0];
        }

      


    }




    DocumentTypeCopyValueChanged(Copy:any) {

        this.EntityPM.LimitedPrintCopyId = Copy.Id;
    }


 



}






