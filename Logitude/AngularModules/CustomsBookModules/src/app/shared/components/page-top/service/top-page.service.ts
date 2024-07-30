import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

export enum SearchBy {
  'searchBy_form01' = 'חיפוש פרט מכס... ',
  'pageSearch_form02' = 'חיפוש מילה/ צירוף מילים...',
}

@Injectable({
  providedIn: 'root',
})
export class SearchService {
  private _Default_selectSearchBy: number | string = 'searchBy_form01';
  selectSearchBy = SearchBy[this._Default_selectSearchBy as keyof typeof SearchBy];

  private _searchTextSubject: BehaviorSubject<string> = new BehaviorSubject<string>('');
  public searchText$: Observable<string> = this._searchTextSubject.asObservable();

  constructor() {
    this._searchTextSubject.next('');
  }

  SearchBy(value?: number | string) {
    if (!value) {
      this.selectSearchBy = SearchBy[this._Default_selectSearchBy as keyof typeof SearchBy];
    } else {
      this.selectSearchBy = SearchBy[value as keyof typeof SearchBy];
    }
    return this.selectSearchBy;
  }

  GetDefaultValue() {
    return this._Default_selectSearchBy;
  }

  GetSearchText(): string {
    return this._searchTextSubject.getValue();
  }

  SetSearchText(value: string) {
    this._searchTextSubject.next(value);
  }
}
