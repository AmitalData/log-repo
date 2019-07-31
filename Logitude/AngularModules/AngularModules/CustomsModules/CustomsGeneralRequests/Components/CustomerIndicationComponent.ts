import { Component } from '@angular/core';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool, ArrayTool } from '../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { ClaimImporterDeclarsP3LoiPM } from '../../../Customs/EntityPMs/ClaimImporterDeclarsP3LoiPM';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';

@Component({
    moduleId: module.id,
    templateUrl: './CustomerIndicationComponent.html',
})

export class CustomerIndicationComponent extends BaseComponent {
    public DataContext: CustomerIndicationComponent = this;

    public CustomerIndicationList: ObservableCollection;
    private isControlEnabled: boolean = true;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();

        this.CustomerIndicationList = new ObservableCollection([]);
    }

    SetWindowArgs(args: any) {
        this.CustomerIndicationList = args.CustomerIndicationList;
    }

    public get IsControlEnabled() { return this.isControlEnabled; }
    public set IsControlEnabled(newValue: boolean) { this.isControlEnabled = newValue; }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    //#endregion
}
