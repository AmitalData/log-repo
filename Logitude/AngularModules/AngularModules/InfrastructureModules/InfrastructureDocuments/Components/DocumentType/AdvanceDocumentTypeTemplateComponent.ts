declare var System: any;
declare var window: any;
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {UserLoginLogList} from '../../../../Common/EntityLists/UserLoginLogList';
import {Component, OnInit}  from '@angular/core';
import {UserExtendedPMService} from '../../../../Common/Services/ExtendedPMs/UserExtendedPMService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {DocumentTypeTemplatePM} from '../../../../Common/EntityPMs/DocumentTypeTemplatePM';
import {AppTool} from '../../../../Infrastructure/Tools';
import {DocumentTypeTemplatePMService} from '../../../../Common/Services/StandardPMs/DocumentTypeTemplatePMService';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {UIProperty, UIProperties}  from '../../../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {CountryListService} from '../../../../Common/Services/StandardLists/CountryListService';
import {CountryList} from '../../../../Common/EntityLists/CountryList';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {FormBuilder, FormGroup, FormsModule} from '@angular/forms';

@Component({
    
    selector: 'AdvanceDocumentTypeTemplate',
    templateUrl: './AdvanceDocumentTypeTemplateComponent.html', 
    providers: [ DocumentTypeTemplatePMService]
})

export class AdvanceDocumentTypeTemplateComponent extends BaseComponent implements OnInit {

    public myForm: FormGroup;
    EntityPM: DocumentTypeTemplatePM;
    DataContext: any =  this;
    CountryId: string = "";
    private documentTypeTemplatePMService: DocumentTypeTemplatePMService;
    IsLoadPage: boolean = false;
    CountryLists: CountryList[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(fb: FormBuilder) {
        super();
        if (this.documentTypeTemplatePMService == null) {
            this.documentTypeTemplatePMService = new DocumentTypeTemplatePMService();
 
        }

        this.myForm = fb.group({});
    }



    ngOnInit(


    ) {

    }

    SetDataContext(entityPM: DocumentTypeTemplatePM) {
        this.EntityPM = entityPM;

        if (this.EntityPM) {
            this.SetUIProperties();
        }
        var myService: CountryListService = new CountryListService();
        myService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError && myResponse.Result) {
                    this.CountryLists = myResponse.Result;
                    if (this.CountryLists) {
                        if (!AppTool.IsNullOrEmpty(this.EntityPM.CountryCode)) {
                            var countryList: CountryList = this.CountryLists.filter(d => d.Code == this.EntityPM.CountryCode)[0];
                            if (countryList) {
                                this.CountryId = countryList.Id;
                            }
                        }
                    }
                }
                this.IsLoadPage = true;
            });
        

    }


    private SetUIProperties() {
        this.EntityPM.UIProperties.SetEnabled("IsSystem", "DocumentTypeTemplate", SessionLocator.Tenant == 0);
    }

    CountrySelectedChange(value: any) {
        if (value) {
            this.EntityPM.CountryCode = value.Code;
        }
        else this.EntityPM.CountryCode = "";
    }






    SaveButtonClicked() {
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
        this.documentTypeTemplatePMService.update(this.EntityPM).subscribe((res:any) => {
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            this.CloseButtonClicked();
        });


    }

    CloseButtonClicked() {
        this.EntityPM.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();

    }



}


