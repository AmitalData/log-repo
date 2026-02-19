import { NgIf, AsyncPipe, CommonModule } from '@angular/common';
import { HttpResponse } from '@angular/common/http';
import { Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatSelectModule } from '@angular/material/select';
import { Subject, switchMap, catchError, EMPTY, of, timer, map } from 'rxjs';
import { SessionInfo } from '../../../../core/Infrastructure/Utilities/SessionInfo';
import { CustomsItemsAutocomplate, CustomsClassification, GetFromTypesenseResponse, AllClassification, CustomBookClassification, GroupedCustomsItems } from '../page-top.interface';
import { CacheService } from '../../../../core/Services/cache.service';
import { RomanToolService } from '../../../services/roman-tool.service';
import { HeaderService } from '../../app-header/service/header.service';
import { SearchService } from '../service/top-page.service';
import { API_MainService } from '../../../../core/API_MainService';
import { Pipes } from '../../../../core/Infrastructure/ModuleDeclarations';

@Component({
  selector: 'app-search-customs-item-autocomplate',
  standalone: true,
  imports: [FormsModule, NgIf, MatAutocompleteModule, AsyncPipe, MatSelectModule, CommonModule, MatExpansionModule, Pipes],
  templateUrl: './search-customs-item-autocomplate.component.html',
  styleUrl: './search-customs-item-autocomplate.component.scss'
})
export class SearchCustomsItemAutocomplateComponent {
  @ViewChild("auto") auto: any;
  @Output() optionSelected = new EventEmitter<CustomsItemsAutocomplate>();
  customsItemsAutocomplateList: Subject<CustomsItemsAutocomplate[]> = new Subject<CustomsItemsAutocomplate[]>();
  groupCustomsItemsAutocomplateList: Subject<CustomsClassification[]> = new Subject<CustomsClassification[]>();
  public groupedItems: GroupedCustomsItems = {};

  constructor(
    private searchService: SearchService,
    private headerService: HeaderService,
    private aPI_MainService: API_MainService,
    private cacheService: CacheService,
    private romanTool: RomanToolService,
  ) { }

  ngOnInit() {
    this.getClassifications();
    this.applyAutocomplate();
  }

  onCustomsItemSelected() {
    this.optionSelected.emit();
  }

  clearAutocomplete() {
    this.customsItemsAutocomplateList.next([]);
    this.groupCustomsItemsAutocomplateList.next([]);
  }

  private applyAutocomplate() {
    this.searchService.searchText$.pipe(
      switchMap((value) =>
        timer(value?.length > 3 ? 100 : 200).pipe(map(() => value))
      ),
      switchMap((searchText: string) =>
        !!searchText ?
          this.aPI_MainService.GetFromTypesense(searchText, this.headerService.getSearchState(true), SessionInfo.LoggedUserTenant).pipe(catchError((error) => EMPTY)) :
          of(() => EMPTY)
      )
    ).subscribe(async (res: any) => {
      if (!res.body)
      return this.clearAutocomplete();

      const regex = new RegExp(`(${this.searchService.GetSearchText()})`, 'gi');
      const result: GetFromTypesenseResponse = res.body;
      result.Remarks.forEach((remark) => remark.CustomsItem.BaseCustomsItemID = -1);
      let customsItems = [...result.CustomsItems, ...result.Remarks.map((remark) => { return { ...remark.CustomsItem, remark: remark.Remark.RemarkDescription } })];
      let customsItemsAutocomplateList: CustomsItemsAutocomplate[] = customsItems.map((item) => {
      let text = item.FullClassification + ' | ' + ((<any>item).remark || item.CIH_GoodsDescription);
      text = text.replace(regex, `<mark>$1</mark>`);
      return { FullClassification: item.FullClassification, text: text, BaseCustomsItemID: item.BaseCustomsItemID };
      });

      if (customsItemsAutocomplateList.length < 10) {
      this.customsItemsAutocomplateList.next(customsItemsAutocomplateList);
      this.groupCustomsItemsAutocomplateList.next([]);
      } else {

      const classificationType: AllClassification = await this.getClassifications();
      const customsBookType: string = this.headerService.getSearchState(true);
      const classifications: CustomBookClassification = classificationType[customsBookType];

      const groupedItems: GroupedCustomsItems = {};
      (classifications.sortedClassifications as CustomsClassification[]).forEach((key: CustomsClassification) => groupedItems[key.Classification] = []);

      customsItemsAutocomplateList.forEach((item) => {
        const classification: CustomsClassification = classifications[item.BaseCustomsItemID] as any || "Unrecognised"; // classifications ={["090000000"]:"XV"}
        groupedItems[classification.Classification].push(item);
      });

      this.groupedItems = groupedItems;
      this.groupCustomsItemsAutocomplateList.next(classifications.sortedClassifications as any[]);
      this.customsItemsAutocomplateList.next([]);
      }
    });
  }

  private async getClassifications() {
    return await this.cacheService.getByPromise('classifications', async () => {
      const respnse = await this.aPI_MainService.GetClassifications()?.toPromise();
      const allClassifications: AllClassification = (respnse as HttpResponse<any>).body;
      delete allClassifications["$id"];

      for (const bookType in allClassifications) {
        const classifications = allClassifications[bookType];
        delete classifications["$id"];
        allClassifications[bookType].sortedClassifications = Object.values(classifications).sort((a, b) => this.romanTool.comparetorObject(a, b, 'Classification')) as any[];

        (<any>classifications)[-1] = { Classification: "Remark", Description: "הערות משתמש" };
        (<CustomsClassification[]>allClassifications[bookType].sortedClassifications).unshift(<any>classifications[-1]);
        (<any>classifications)["-"] = { Classification: "Unrecognised", Description: "אחר" };
        (<CustomsClassification[]>allClassifications[bookType].sortedClassifications).push(<any>classifications['-']);
      }

      return allClassifications;
    });
  }
}
