import { Component, OnInit } from '@angular/core';
import { prettyPrintJson } from 'pretty-print-json';

@Component({
    templateUrl: './ObjectVariableComponent.html',
})

export class ObjectVariableComponent implements OnInit {
    public Object: any = null;
    public ObjectHtml: string | null = null;

    SetWindowArgs(args: any) {
        this.Object = args.Value ? JSON.parse(args.Value) : null;
    }

    ngOnInit() {
        if (this.Object) {
            try {
                this.ObjectHtml = prettyPrintJson.toHtml(this.Object);
            } catch (error) {}
        }
    }
}