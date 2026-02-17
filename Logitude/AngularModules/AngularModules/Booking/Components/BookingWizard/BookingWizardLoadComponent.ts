declare var window: any;
import {Component, AfterViewInit, ViewChild, ViewContainerRef} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {BookingPM} from '../../EntityPMs/BookingPM';
import {BookingPMService} from '../../Services/StandardPMs/BookingPMService';
import {EntityLastActivityService} from '../../../Infrastructure/Services/EntityLastActivityService';
import {BookingWizardArgs} from '../../Args';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';

@Component({
    moduleId: module.id,
    templateUrl: './BookingWizardLoadComponent.html',
})

export class BookingWizardLoadComponent implements AfterViewInit {
    public EntityId: string = null;
    public EntityPM: BookingPM;
    @ViewChild('WizardView', { read: ViewContainerRef }) target: ViewContainerRef;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        
    }

    SetWindowArgs(entityId: string) {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.EntityId = entityId;
        this.Load();
    }

    private isViewInited = false;
    ngAfterViewInit() {
        this.isViewInited = true;
        this.Load();
    }

    private Load() {
        if (this.EntityId != null && this.isViewInited) {

            var myService: BookingPMService = new BookingPMService();

            myService.get(this.EntityId).subscribe(myResult => {
                var myResponse: ServiceResponse = myResult;

                if (!myResponse.HasError) {
                    this.EntityPM = myResponse.Result;

                    if (this.EntityPM != null) {
                        this.ImportWizard();
                        this.SendActivityLog();
                    }
                }

                this.CurrentSession.StopBusyIndicator();
            });
        }
    }

    private ImportWizard() {
        var myBookingWizardArgs: BookingWizardArgs = new BookingWizardArgs();
        myBookingWizardArgs.EntityPM = this.EntityPM;
        this._entityResourceService.getEntityResourceByTableName("Booking", 0).subscribe(response=> {
            SessionLocator.DynamicLoader.Load('./Booking/Components/BookingWizard/BookingWizardComponent', this.target)
                .then(cmpRef => {
                    cmpRef.instance.SetWindowArgs(myBookingWizardArgs);
                    this.CurrentSession.StopBusyIndicator();
                });
        });
    }

    private SendActivityLog() {
        var ObjectTableName = "Booking";
        var ObjectTable = window.ObjectTables.filter(x => x.Name === ObjectTableName)[0];
        var ObjectTableId = ObjectTable.Id;

        var myService: EntityLastActivityService = new EntityLastActivityService();
        myService.AddActivityLog(this.EntityId, ObjectTableId, SessionLocator.LoggedUserId, 'V').subscribe();
    }
}
