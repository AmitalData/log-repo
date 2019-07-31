import {Component, ViewChild, ViewContainerRef} from '@angular/core';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ContactPM} from '../../../../Common/EntityPMs/ContactPM';
import {ContactItemClass} from '../../../../CommonModules/CommonPartners/Components/EditTabs/ContactsTabComponent';
import {ContactInputTemplate, ContactInputTemplateArgs} from '../../../../CommonModules/CommonPartners/Components/Templates/ContactInputTemplate';
import {AppTool} from '../../../../Infrastructure/Tools';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {PartnersDomainService, PartnerServicePM} from '../../../../Common/Services/PartnersDomainService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditContactComponent.html',
})

export class AddEditContactComponent {
    public ObjectTableName: string;
    public CardId: string = null;
    public EntityPM: ContactPM = null;
    public DataContext: ContactItemClass;
    public ValidationErrorsList: string[] = [];
    public DomainService: PartnersDomainService;
    @ViewChild('Child', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

    }

    SetDataContext(dataContext: ContactItemClass) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.ObjectTableName = dataContext.ObjectTableName;
      
        if (dataContext.fatherComponent) {
            this.CardId = dataContext.fatherComponent.EntityId;
            this.DomainService = dataContext.fatherComponent.DomainService;
        }
        else {
            this.CardId = dataContext.EntityPM.CardId;
            this.DomainService = new PartnersDomainService(); 
        }
            
        this.RunComponent();
        this.Clone();
    }

    SetWindowArgs(args: any) {
        if (args) {
            this.ShowSecondPartOfWindow = args.ShowSecondPartOfWindow;
        }
    }

    RunComponent() {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }

        else {
            this.RunComponentTimer();
        }
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 3) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    } 

    private ContactTemplate: ContactInputTemplate;
    private ShowSecondPartOfWindow: string = null;
    LoadChildComponent() {
        SessionLocator.DynamicLoader.Load("./CommonModules/CommonPartners/Components/Templates/ContactInputTemplate", this.viewContainerRef)
            .then(cmpRef => {
                this.ContactTemplate = cmpRef.instance;

                var args = new ContactInputTemplateArgs();
                args.CardId = this.CardId;
                args.ShowSearchContacts = true;
                args.IsNewEntity = this.DataContext.IsNewEntity;
                args.EntityPM = this.EntityPM;
                args.ShowSecondPartOfWindow = this.ShowSecondPartOfWindow;
                var isBlockingEmail = false;
                if (!this.DataContext.IsNewEntity) {
                    if (!AppTool.IsNullOrEmpty(this.EntityPM.Email)) {
                        isBlockingEmail = true;
                    }
                }         


                args.BlockEditingEmail = isBlockingEmail;


                this.ContactTemplate.InitTemplate(args);
                this.Clone();
            });
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindowEmit("cancel");
    }

    OkButtonClicked() {
        this.CurrentSession.StartBusyIndicatorSaving();

        this.ValidationErrorsList = this.ContactTemplate.Validate();
        if (this.ValidationErrorsList.length > 0) {
            this.CurrentSession.StopBusyIndicator();
        }

        else {
            if (!this.EntityPM.IsDirty) {
                this.CurrentSession.CloseCurrentWindowEmit(this.EntityPM.Id);
            }

            else {
                if (this.DataContext.IsNewEntity) {
                    if (this.DataContext.fatherComponent && this.DataContext.fatherComponent.ItemsSource.length == 0) {
                        this.DataContext.fatherComponent.EntityPM.IsFirstContactToAdd = true;
                        this.DataContext.fatherComponent.EntityPM.PrimaryContactId = this.EntityPM.Id;
                    }
                }

                this.Save();
            }
        }        
    }

    private LoadCompletedEvent: any = null;
    private Save() {
        var args = new PartnerServicePM();
        args.Tenant = this.EntityPM.Tenant;
        args.ContactId = this.EntityPM.Id;
        args.PartnerId = this.EntityPM.CardId;
        args.Contact = this.EntityPM;
        args.IsContactDirty = this.EntityPM.IsDirty;

        if (this.DataContext.fatherComponent) {
            args.IsPartnerDirty = this.DataContext.fatherComponent.EntityPM.IsDirty;
            args.PartnerTypeId = this.DataContext.fatherComponent.PartnerTypeId;
            this.DomainService.SetPartner(args, this.DataContext.fatherComponent.EntityPM);
        }

        else {
            this.DomainService.SetPartner(args, this.DataContext.EntityPM, "CO");
        }

        this.DomainService.PostPartnerAddress(args).subscribe((myResponse: ServiceResponse) => {

            if (myResponse.HasError) {
                this.CurrentSession.StopBusyIndicator();
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }

            else {
                this.DataContext.EntityPM = myResponse.Result.Contact;

                if (!this.CurrentSession.CurrentEditComponent) {
                    if (this.DataContext.IsNewEntity) {
                        this.DataContext.IsNewEntity = false;
                    }

                    this.CurrentSession.CloseCurrentWindow();
                }

                else {
                    if (!this.LoadCompletedEvent) {
                        this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(isSuccess => {

                            AppTool.KillEventEmitter(this.LoadCompletedEvent);
                            this.LoadCompletedEvent = null;

                            if (isSuccess == false) {
                                this.CurrentSession.StopBusyIndicator();
                            }

                            else {
                                if (this.DataContext.IsNewEntity) {
                                    this.DataContext.IsNewEntity = false;
                                    this.DataContext.fatherComponent.IsNoDataVisible = false;
                                    this.DataContext.fatherComponent.ItemsSource.push(this.DataContext);
                                }

                                this.CurrentSession.CloseCurrentWindowEmit(this.EntityPM.Id);
                            }
                        });

                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    }
                }
            }            
        });
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.ContactTemplate);
        this.myCloner.AddField('Email');
        this.myCloner.AddField('EnglishName');
        this.myCloner.AddField('LocalName');
        this.myCloner.AddField('BusinessPhone');
        this.myCloner.AddField('Mobile');
        this.myCloner.AddField('Fax');
        this.myCloner.AddField('Anniversary');
        this.myCloner.AddField('Birthday');
        this.myCloner.AddField('InActive');
        this.myCloner.AddField('Position');
        this.myCloner.AddField('IsAll');
        this.myCloner.AddField('IsAirExport');
        this.myCloner.AddField('IsAirImport');
        this.myCloner.AddField('IsOceanExport');
        this.myCloner.AddField('IsOceanImport');
        this.myCloner.AddField('IsCustomsImport');
        this.myCloner.AddField('IsInlandDomestic');
        this.myCloner.AddEntity(this.EntityPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}

