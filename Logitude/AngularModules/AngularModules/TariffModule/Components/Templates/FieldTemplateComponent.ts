import { Component } from '@angular/core';
import { CurrencyListService } from '../../../Common/Services/StandardLists/CurrencyListService';
import { CurrencyList } from '../../../Common/EntityLists/CurrencyList';
@Component({
    moduleId: module.id,
    templateUrl: './FieldTemplateComponent.html',
})

export class FieldTemplateComponent {
    public Entity: any = null;
    public FieldName: string = null;
    public FieldValue: any = null;
    public ObjectTableName: string = null;
    private CurrencyListService: CurrencyListService;
    constructor() {
        this.CurrencyListService = new CurrencyListService();

    }

    public Run(args: any) {
        this.Entity = args['Entity'];
        this.FieldName = args['FieldName'];
        this.ObjectTableName = args['ObjectTableName'];
      
        if (this.Entity != null && this.FieldName != null) {
            this.FieldValue = this.Entity[this.FieldName];
            if (this.FieldValue != null) {
                this.CurrencyListService.getSingleFromCache(this.FieldValue).subscribe(res => {
                    if (!res.HasError) {
                        var Currency: CurrencyList = res.Result;
                        if (Currency) {
                            this.FieldValue = Currency.Code;
                        }
                    }
                });
            }
        }

        
    }
}
