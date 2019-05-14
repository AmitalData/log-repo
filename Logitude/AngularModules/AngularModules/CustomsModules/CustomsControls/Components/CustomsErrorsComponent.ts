declare var window: any;
import { Component, EventEmitter, Output, Input, OnInit, ElementRef, AfterViewInit } from '@angular/core';
import { ChangeDetectorRef } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AppTool, DateTool } from   '../../../Infrastructure/Tools';
import { SessionLocator } from      '../../../Infrastructure/Utilities/SessionLocator';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';




import { CustomsRequestsSheetList } from '../../../Customs/EntityLists/CustomsRequestsSheetList';
import { CustomsRequestsSheetStatusList } from '../../../Customs/EntityLists/CustomsRequestsSheetStatusList';
import { CustomsRequestsSheetStatusListService } from '../../../Customs/Services/StandardLists/CustomsRequestsSheetStatusListService';


import { TextCodeTranslator } from      '../../../Infrastructure/Utilities/TextCodeTranslator';
import { ApiQueryFilters } from         '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ObservableCollection } from    '../../../Infrastructure/Utilities/ObservableCollection';
import { EntityListService } from   '../../../Infrastructure/Services/EntityListService';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { Guid } from '../../../Infrastructure/Utilities/Guid';




@Component({
    selector: 'CustomsErrorsComponent',
    moduleId: module.id,
    templateUrl: './CustomsErrorsComponent.html',
})

export class CustomsErrorsComponent {
    Errors: string[];
    ErrorsCount: string;
    NoButtonVisibility: boolean;
    CancelButtonVisibility: boolean;
    CancelButtonText: string;
    NoButtonText: string;
    SaveButtonText: string;
    ComponentHeight: string;
    private CurrentSession = SessionLocator.SelectedSession;
    SetWindowArgs(windowArgs) {
        this.ComponentHeight = windowArgs.ComponentHeight;
        this.Errors = windowArgs.Errors;
        this.ErrorsCount = "Errors Found: ";
        if (this.Errors) {
            this.ErrorsCount = this.ErrorsCount + this.Errors.length;
        }
        this.NoButtonText = TextCodeTranslator.Translate('Customs.General.B.No');
        this.CancelButtonText = TextCodeTranslator.Translate('Customs.General.B.Cancel');
        this.SaveButtonText = TextCodeTranslator.Translate('Customs.General.B.OK');
        this.CancelButtonVisibility = windowArgs.CancelButtonVisibility;
        this.NoButtonVisibility = windowArgs.NoButtonVisibility;
        if (windowArgs.NoButtonText) {
            this.NoButtonText = windowArgs.NoButtonText;
        }
        if (windowArgs.CancelButtonText) {
            this.CancelButtonText = windowArgs.CancelButtonText;
        }
        if (windowArgs.SaveButtonText) {
            this.SaveButtonText = windowArgs.SaveButtonText;
        }
    }

    OkButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("ok");
    }

    NoButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("no");
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("cancel");
    }
}
