import { Component } from '@angular/core';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { FormatOptions, prettyPrintJson } from 'pretty-print-json';

@Component({
    templateUrl: './WorkflowInstanceVariableObject.html',
})

export class WorkflowInstanceVariableObject extends BaseComponent {
    public ObjectHtml: string | null = null;

    constructor(public entityArgs: EntityArgs) {
        super();
    }

    SetWindowArgs(args: any) {
        if (args.Value) {
            try {
                let object = JSON.parse(args.Value);
                this.ObjectHtml = prettyPrintJson.toHtml(object);
            }catch(error){
                this.ObjectHtml = args.Value
            }
        }
    }

}
