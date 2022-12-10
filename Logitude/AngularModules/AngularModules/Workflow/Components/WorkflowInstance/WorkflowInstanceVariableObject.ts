import { Component } from '@angular/core';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';

@Component({
    templateUrl: './WorkflowInstanceVariableObject.html',
})

export class WorkflowInstanceVariableObject extends BaseComponent {
    public EntityId: string;
    public Value:string

    constructor(public entityArgs: EntityArgs) {
        super();
    }

    SetWindowArgs(args: any) {
        this.EntityId = args.EntityId;
        this.Value = args.Value;
    }

}