import { OnInit, Component } from '@angular/core';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { DocumentTypeTemplatePM } from 'Common/EntityPMs/DocumentTypeTemplatePM';


@Component({
    selector: 'TemplateTypeComponent',
    templateUrl: './TemplateTypeComponent.html',
})


export class TemplateTypeComponent implements OnInit {

    private CurrentSession = SessionLocator.SelectedSession;
    DataViewModel: any;
    DocumentTypeTemplates: DocumentTypeTemplatePM[];
    SelectedTemplate: DocumentTypeTemplatePM;
    ValidationErrorsList: any[];

    ngOnInit(): void {

    }

    SetWindowArgs(args: any) {
        this.DataViewModel = args.DataViewModel;
        this.DocumentTypeTemplates = args.DocumentTypeTemplates;
        this.SelectedTemplate = this.DocumentTypeTemplates[0];
    }


    CloseButtonClicked() {
        this.CurrentSession.CurrentWindow.Close("");
    }

    SendClick() {
        this.ValidationErrorsList = [];
        if(!this.SelectedTemplate){
            this.ValidationErrorsList.push("Please select template");
            return;
        }

        this.DataViewModel.SendInvitaion(this.SelectedTemplate.Id);
        this.CurrentSession.CurrentWindow.Close("");
    }
}