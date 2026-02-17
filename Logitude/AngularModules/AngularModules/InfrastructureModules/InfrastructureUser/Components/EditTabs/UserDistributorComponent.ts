import {Component, OnDestroy}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {UserPM} from '../../../../Common/EntityPMs/UserPM';

import {UserRolesPM} from '../../../../Common/EntityPMs/UserRolesPM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {AppTool} from '../../../../Infrastructure/Tools';

@Component({
    moduleId: module.id,
    templateUrl: './UserDistributorComponent.html',
})

export class UserDistributorComponent extends BaseComponent {
    public EntityPM: UserPM;
  
    public ObjectTableName: string = "User";
    public DataContext = this;
    public IsSalesman: boolean;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;

    }

    public get DistributorCode() { return this.EntityPM.DistributorCode; }
    public set DistributorCode(value: string) {
        if (this.EntityPM.DistributorCode != value) {
            this.EntityPM.DistributorCode = value;
        }
    }

    public get IsDistributor() { return this.EntityPM.IsDistributor; }
    public set IsDistributor(value: boolean) {
        if (this.EntityPM.IsDistributor != value) {
            this.EntityPM.IsDistributor = value;
        }
    }


}