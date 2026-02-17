import { Component } from '@angular/core';
import { ContactPM } from '../../../../../Common/EntityPMs/ContactPM';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { CommonDomainService } from '../../../../../Common/Services/CommonDomainService';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    moduleId: module.id,
    templateUrl: './ContactOccasionsTabComponent.html',
})

export class ContactOccasionsTabComponent {
    public EntityPM: ContactPM;
    public ObjectTableName: string = "Contact";
    public ItemsSource: any[] = [];
    public IsResourcesReady: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, entityResourceService: EntityResourceService) {

        this.EntityPM = entityArgs.EntityPM;

        entityResourceService.getEntityResourceByTableName("Occasion").subscribe(response => {
            this.IsResourcesReady = true;
            this.LoadOccasions();
        });
    }

    RefreshButtonClicked() {
        this.LoadOccasions();
    }
    
    LoadOccasions() {
        var service: CommonDomainService = new CommonDomainService();
        service.GetContactOccasions(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.ItemsSource = myResponse.Result;
            }
        });
    }

    ShowOccasionClicked(item: any) {
        if (item) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: item.Id, ObjectTableName: 'Occasion', BackButtonLabel: this.ObjectTableName + ": " + this.EntityPM.EnglishName });

                    let isEditComponentSaved = false;

                    cmpRef.instance.BackCompleted.subscribe(bk => {
                        if (isEditComponentSaved) {
                            this.LoadOccasions();
                        }
                    });

                    cmpRef.instance.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                        if (isSaveSuccess) {
                            isEditComponentSaved = true;
                        }
                    });

                    cmpRef.instance.SaveAndCloseCompleted.subscribe((isSaveSuccess: boolean) => {
                        if (isSaveSuccess) {
                            isEditComponentSaved = true;
                        }
                    });
                });
        }
    }
}
