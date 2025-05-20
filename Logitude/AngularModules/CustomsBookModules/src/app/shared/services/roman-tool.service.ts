import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class RomanToolService {

  constructor() { }

  sortArry(arr: any[], propName: string): any[] {    
    return arr.sort((a, b) => this.comparetorObject(a, b, propName));
  }

  comparetorObject = (a, b, propName) => this.romanToInt(a[propName]) - this.romanToInt(b[propName]);

  comparetor = (a, b) => this.romanToInt(a) - this.romanToInt(b);

  romanToInt(roman: string): number {
    const romanMap: { [key: string]: number } = { I: 1, V: 5, X: 10, L: 50, C: 100, D: 500, M: 1000 };
    return roman.split('').reduce((num, char, i, arr) =>
      num + (romanMap[char] < romanMap[arr[i + 1]] ? -romanMap[char] : romanMap[char]), 0);
  }

}
