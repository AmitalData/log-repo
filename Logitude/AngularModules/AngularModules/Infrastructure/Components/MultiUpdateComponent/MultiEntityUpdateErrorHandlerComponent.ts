import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../LogitudeComponents/BaseComponent';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { EntityListService } from 'Infrastructure/Services/EntityListService';
import { MultiEntityUpdateLogPM } from '../../EntityPMs/MultiEntityUpdateLogPM';
import { MultiEntityUpdateDataEntity } from '../../DataContracts/MultiEntityUpdateDataEntity';

@Component({
    selector: 'MultiEntityUpdateErrorHandlerComponent',
    templateUrl: 'MultiEntityUpdateErrorHandlerComponent.html',
})

export class MultiEntityUpdateErrorHandlerComponent extends BaseComponent implements OnInit {
    private CurrentSession = SessionLocator.SelectedSession;
    MultiEntityUpdatedLogPM: MultiEntityUpdateLogPM;
    MultiEntityUpdateDataEntities: MultiEntityUpdateDataEntity[] = [];
    ItemsSource: any;
    ObjectTableName: string;
    constructor(private _entityListService: EntityListService) {
        super();
    }

    ngOnInit() {
    }

    SetWindowArgs(windowArgs: any) {
        this.MultiEntityUpdatedLogPM = windowArgs.multiEntityUpdateLogPM;
        this.ObjectTableName = this.MultiEntityUpdatedLogPM.MultiEntityUpdateData.ObjectTableName;
        this.FillItemsSource();
    }

    private FillItemsSource() {
        this.MultiEntityUpdateDataEntities = this.MultiEntityUpdatedLogPM.MultiEntityUpdateData.Entities;
        this.ItemsSource = this.MultiEntityUpdateDataEntities.filter(f => f.HasException);
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
