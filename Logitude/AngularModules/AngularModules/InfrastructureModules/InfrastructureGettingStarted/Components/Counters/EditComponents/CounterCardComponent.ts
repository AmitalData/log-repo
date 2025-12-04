import { Component } from '@angular/core';
import { AppTool } from '../../../../../Infrastructure/Tools';
import { CounterPM } from '../../../../../Common/EntityPMs/CounterPM';
import { CounterDefinitionPM } from '../../../../../Common/EntityPMs/CounterDefinitionPM';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {
    CountersDomainService,
    CounterAPIHelper,
} from '../../../../../Common/Services/CountersDomainService';
import { Validator } from '../../../../../Infrastructure/Validators/Validator';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';
import { GroupByPipe } from '../../../../../Infrastructure/Pipes/GroupByPipe';
import { PartnerTypeListService } from 'Common/Services/StandardLists/PartnerTypeListService';
import { types } from 'util';
import { PartnerTypeList } from 'Common/EntityLists/PartnerTypeList';

@Component({
    templateUrl: './CounterCardComponent.html',
})
export class CounterCardComponent extends BaseComponent {
    public CounterId: string;
    public CounterPM: CounterPM;
    public EntityPM: CounterDefinitionPM;
    public DataContext = this;
    public ObjectTableName = 'CounterDefinition';
    public APIHelper: CounterAPIHelper;
    public IsCounterUsed: boolean = false;
    public IsResourcesReady: boolean = false;
    public ValidationErrorsList: string[] = [];
    public ItemsSource: CounterDefinitionPM[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    PartnerTypeListService: PartnerTypeListService =
        new PartnerTypeListService();
    PartnerTypeArray: Array<PartnerTypeList> = [];

    constructor() {
        super();
    }

    SetWindowArgs(args: any) {
        this.CounterId = args['CounterId'];

        if (this.CounterId) {
            this.CurrentSession.StartBusyIndicatorLoading();

            var myService = new CountersDomainService();
            this.PartnerTypeListService.getAll().subscribe((typesResponse) => {
                if (!typesResponse.HasError) {
                    this.PartnerTypeArray = typesResponse.Result;
                }
                myService
                    .GetCounterAPIHelper(this.CounterId)
                    .subscribe((myResponse: ServiceResponse) => {
                        if (myResponse.HasError) {
                            this.ValidationErrorsList = myResponse.ErrorsArray;
                        } else {
                            this.APIHelper = myResponse.Result;

                            if (this.APIHelper) {
                                this.CounterPM = this.APIHelper.CounterPM;
                                this.IsCounterUsed =
                                    this.APIHelper.IsCounterUsed;
                                this.InitializeDefinitions();
                                this.SetUIProperties();
                            }
                        }

                        this.IsResourcesReady = true;
                        this.CurrentSession.StopBusyIndicator();
                    });
            });
        }
    }
    SetUIProperties() {
        this.UIProperties.SetEnabled(
            'StartNumber',
            this.ObjectTableName,
            !this.IsCounterUsed
        );
        this.UIProperties.SetEnabled(
            'CounterSize',
            this.ObjectTableName,
            !this.IsCounterUsed
        );
    }
    InitializeDefinitions() {
        this.EntityPM = new CounterDefinitionPM();
        this.EntityPM.CounterId = this.CounterPM.Id;
        this.EntityPM.Tenant = this.CounterPM.Tenant;
        this.EntityPM.UniquePerPrefix = false;
        this.EntityPM.StartNumber = 1000;
        this.EntityPM.StartNumber_Old = 0;
        if (this.APIHelper.CounterDefinitions.length == 0) {
            this.APIHelper.CounterDefinitions.push(this.EntityPM);
        } else {
            var myPipe = new GroupByPipe();
            var myGroupbyCount = myPipe.transform(
                this.APIHelper.CounterDefinitions,
                'StartNumber'
            ).length;

            if (myGroupbyCount == this.APIHelper.CounterDefinitions.length) {
                this.sameForAllPartnersType = true;
            } else {
                this.sameForAllPartnersType = false;
            }
        }

        this.BuildItemsSource();
    }
    BuildItemsSource() {
        this.ItemsSource = [];
        this.APIHelper.CounterDefinitions.forEach((item) => {
            this.ItemsSource.push(item);
        });
    }
    getPartnerName(id: string): string {
        return this.PartnerTypeArray?.find((p) => p.Id === id)?.Name ?? '';
    }

    private sameForAllPartnersType: boolean = true;
    public get SameForAllPartnersType() {
        return this.sameForAllPartnersType;
    }
    public set SameForAllPartnersType(value: boolean) {
        if (this.sameForAllPartnersType != value) {
            this.sameForAllPartnersType = value;
            this.BuildItemsSource();
        }
    }

    public get CounterSize() {
        return this.EntityPM.CounterSize;
    }
    public set CounterSize(value: number) {
        if (this.EntityPM.CounterSize != value) {
            this.EntityPM.CounterSize = value;

            this.APIHelper.CounterDefinitions.forEach((item) => {
                item.CounterSize = value;
            });
        }
    }

    public get StartNumber() {
        return this.EntityPM.StartNumber;
    }
    public set StartNumber(value: number) {
        if (this.EntityPM.StartNumber != value) {
            this.EntityPM.StartNumber = value;

            this.APIHelper.CounterDefinitions.forEach((item) => {
                item.StartNumber = value;
            });
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var isValidGreaterStartNumber: boolean = true;

        this.APIHelper.CounterDefinitions.forEach((item) => {
            if (!AppTool.IsNullOrEmpty(item.StartNumber)) {
                if (item.StartNumber < item.StartNumber_Old) {
                    isValidGreaterStartNumber = false;
                }
            }
        });

        if (!isValidGreaterStartNumber) {
            var messageWindow = new MessageWindow();
            messageWindow.Show(
                'The new start number must be greater than current start number!'
            );
        } else {
            var errors: string[] = [];
            if (!this.SameForAllPartnersType) {
                this.APIHelper.CounterDefinitions.forEach((item) => {
                    if (item.CounterSize > 20) {
                        errors.push('Maximum size allowed for counter is 20');
                    }
                    Validator.TryValidateObject(
                        item,
                        this.ObjectTableName,
                        errors
                    );
                });
            } else {
                if (this.StartNumber.toString().length > 20) {
                    errors.push(
                        'Maximum length allowed for [Prefix + StartNumber] is 20'
                    );
                }                
            }
            this.ValidationErrorsList = errors;

            if (errors.length == 0) {
                this.CurrentSession.StartBusyIndicatorSaving();

                var myService = new CountersDomainService();
                myService
                    .Post(this.APIHelper)
                    .subscribe((myResponse: ServiceResponse) => {
                        this.CurrentSession.StopBusyIndicator();
                        if (myResponse.HasError) {
                            this.ValidationErrorsList = myResponse.ErrorsArray;
                        } else {
                            this.CurrentSession.CloseCurrentWindowEmit('Ok');
                        }
                    });
            }
        }
    }
}
