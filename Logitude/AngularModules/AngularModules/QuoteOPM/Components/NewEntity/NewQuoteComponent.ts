import { Component } from "@angular/core";
import { FormGroup } from "@angular/forms";
import { CardList } from "Common/EntityLists/CardList";
import { CardListService } from "Common/Services/StandardLists/CardListService";
import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";
import { ServiceResponse } from "Infrastructure/DataContracts/ServiceResponse";
import { ObjectTablePM } from "Infrastructure/EntityPMs/ObjectTablePM";
import { EntityListService } from "Infrastructure/Services/EntityListService";
import { EntityResourceService } from "Infrastructure/Services/EntityResourceService";
import { QuoteOPPM } from "QuoteOPM/EntityPMs/QuoteOPPM";
import { Observable } from "rxjs";
import { transporations } from "./components/new-quote-left-side/new-quote-left-side.component";

declare var window: any;

@Component({
    templateUrl: './NewQuoteComponent.html',
    styleUrls: ['./NewQuoteComponent.scss'],
})
export class NewQuoteComponent {
    public EntityPM: QuoteOPPM;

    formGroup = new FormGroup({});
    transporations = transporations;

    constructor(
        public entityListService: EntityListService,
    ) { }

    async ngOnInit() {
    }

    
    // _entityResourceService: EntityResourceService = new EntityResourceService();
    // private getCards2() {
    //     const LookUpTableName: string = 'Card';
    //     this._entityResourceService.getEntityResourceByTableName(LookUpTableName, 0).subscribe(async () => {
    //         const LookUpTable: ObjectTablePM = window.ObjectTables.filter(d => d.Name === LookUpTableName)[0];
    //         const loadPr: any = (LookUpTable?.CacheOnClient) ?
    //             await this.entityListService.getAllFromCache(LookUpTableName, new ApiQueryFilters()) :
    //             await this.entityListService.getAll(LookUpTableName);

    //         const response: ServiceResponse = await (<Observable<Promise<ServiceResponse>>>loadPr).toPromise();
    //         console.log(response.Result);
    //     });
    // }

}
