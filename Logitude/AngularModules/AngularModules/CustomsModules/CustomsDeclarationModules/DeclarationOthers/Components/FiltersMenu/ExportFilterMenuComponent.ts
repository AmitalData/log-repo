import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { TextCodeTranslator } from "Infrastructure/Utilities/TextCodeTranslator";

@Component({
    selector: 'app-export-filter-menu',
    templateUrl: './ExportFilterMenuComponent.html',
    styles: [`
        .row {            
            display: flex;
            justify-content: space-evenly;
        }

        .row-button {            
            display: flex;
            justify-content: flex-end;
        }

        button {
            width: 100px;
            margin: 10px;
        }
    `],
})
export class ExportFilterMenuComponent extends BaseComponent {
    filters: { dbField: string, lookupField: string, placeHolder: string, chosenListHeader: string, lookUpTableName: string, lookupDisplayColumn: string }[] = [
        {            
            dbField: 'DeclarationStatusTypeCode',
            lookupField: 'Code',
            chosenListHeader: TextCodeTranslator.Translate('Customs.Declaration.O.StatusThatSelected'),
            placeHolder: TextCodeTranslator.Translate('Customs.Declaration.F.DeclarationStatusTypeCode'),
            lookUpTableName: 'Customs.DeclarationStatusType',
            lookupDisplayColumn: 'LocalName',
        },
        {            
            dbField: 'DestinationCountryCode',
            lookupField: 'Code',
            chosenListHeader: TextCodeTranslator.Translate('Customs.Declaration.O.DestantionThatSelected'),
            placeHolder: TextCodeTranslator.Translate('Customs.Declaration.F.DestinationCountryCode'),
            lookUpTableName: 'Customs.CustomsCountry',
            lookupDisplayColumn: 'LocalName',
        },
        {            
            dbField: 'ImporterCode',
            lookupField: 'Code',
            chosenListHeader: TextCodeTranslator.Translate('Customs.Declaration.O.ImporterThatSelected'),
            placeHolder: TextCodeTranslator.Translate('Customs.Declaration.F.ExporterImporterCode'),
            lookUpTableName: 'Customs.Client',
            lookupDisplayColumn: 'FullName',
        },
        {            
            dbField: 'ReferentUserId',
            lookupField: 'Id',
            chosenListHeader: TextCodeTranslator.Translate('Customs.Declaration.O.ReferentsThatSelected'),
            placeHolder: TextCodeTranslator.Translate('Customs.Declaration.F.ReferentUserName'),
            lookUpTableName: 'User',
            lookupDisplayColumn: 'LocalName',
        },
    ]
    filtersInputs = this.filters.map(x => x.dbField).reduce((a, v) => ({ ...a, [v]: []}), {}) 
    
    SetWindowArgs({ apiQueryFilters, filtersInputs }: { apiQueryFilters: ApiQueryFilters, filtersInputs: any }) {
        this.filtersInputs = filtersInputs;
    }

    cleanFliter() {
        this.filtersInputs = this.filters.map(x => x.dbField).reduce((a, v) => ({ ...a, [v]: []}), {}) 
    }

    closeWindow(value?: any) {
        SessionLocator.SelectedSession.CloseCurrentWindowEmit(value);
    }

    addFilters() {
        this.closeWindow({ apiQueryFilters: this.createApiQueryFilter(), filtersInputs: this.filtersInputs, filterActive: this.isExportFilterActive() });
    }

    isExportFilterActive(): boolean {
        for( const input in this.filtersInputs)
            if(this.filtersInputs[input]?.length)
                return true;

        return false;
    }

    createApiQueryFilter() {
        const apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();

        this.filters.forEach(filter => {
            apiQueryFilters.AdditionalFilters = apiQueryFilters.AdditionalFilters.filter(a => a.FieldName !== filter.dbField);
            apiQueryFilters.addAdditionalFilter(filter.dbField, this.filtersInputs[filter.dbField]?.map(x => x[filter.lookupField]).join(','), null, null, "InListExact", false, false, false, "string", this.filtersInputs[filter.dbField].length == 0);
        })

        apiQueryFilters.addAdditionalFilter("RetrievData", true, null, null, "Equal", true, false, false, "string", this.filtersInputs['ReferentUserId'].length == 0);
        
        return apiQueryFilters;
    }
}