import {Component, Output, EventEmitter, OnInit, AfterViewInit, ChangeDetectorRef}  from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ReconciliationPM} from '../../EntityPMs/ReconciliationPM';
import {AppTool} from '../../../Infrastructure/Tools';

@Component({
    selector: 'ReconciledMessage',
    template:
    `
    <style>
    .ConfirmIcon {
        margin-top: 17px;
        left: 10px;
        width: 60px;
        height: 60px;
        line-height: 64px;
        color: white;
        font-size: 36px;
        font-weight: bold;
        font-family: Arial;
        text-align: center;
        vertical-align: middle;
        -webkit-border-radius: 50px;
        -moz-border-radius: 50px;
        border-radius: 50px;
        /* Permalink - use to edit and share this gradient: http://colorzilla.com/gradient-editor/#429b30+0,b8ddb8+100 */
        background: #429b30; /* Old browsers */
        background: -moz-linear-gradient(top,  #429b30 0%, #b8ddb8 100%); /* FF3.6-15 */
        background: -webkit-linear-gradient(top,  #429b30 0%,#b8ddb8 100%); /* Chrome10-25,Safari5.1-6 */
        background: linear-gradient(to bottom,  #429b30 0%,#b8ddb8 100%); /* W3C, IE10+, FF16+, Chrome26+, Opera12+, Safari7+ */
        filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#429b30', endColorstr='#b8ddb8',GradientType=0 ); /* IE6-9 */

        }
    .RedButton{
        position: absolute;
        right: 10px;
        bottom: 10px;
        width: 65px;
    }
    </style>

    <div class="LeftCenter ConfirmIcon" >&#10003;</div>

    <div style= "padding: 35px 10px 10px 90px;font-size: 13px;" >
       Reconciliation <a href= "#"(click) = "OpenReco()" > {{RecoPM.Number }}</a> was created successfully.
    </div>

    <!--<button class="RedButton" (click)="OkButtonClicked()">Ok</button>-->

            `
})

export class ReconciledMessage {
    public RecoPM: ReconciliationPM;

    constructor() {
    }

    SetWindowArgs(args: any) {
        if (args != null) {
            this.RecoPM = args.ReconciliationPM;
        }
    }

    OpenReco() {
        if (!AppTool.IsNullOrEmpty(this.RecoPM.Id)) {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: this.RecoPM.Id, ObjectTableName: 'Reconciliation', BackButtonLabel: 'Back' });
                cmpRef.instance.BackCompleted.subscribe(bk => {
                    SessionLocator.CurrentSession.CloseCurrentWindow();
                });
                SessionLocator.CurrentSession.CloseCurrentWindow();
            });

        }
    }
    OkButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }
}
