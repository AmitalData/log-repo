import { Injectable } from '@angular/core';

const FOOT = 35.315;
const INCH = 61024;
const METER = 1;
const TON = 1000;
const POUND = 0.45359237;

@Injectable()
export class NewQuoteUnitsService {

  constructor() { }
  
   getWeightUnit(unitCode: string, type: string): number {
    let unit: number = 0;

    switch (unitCode) {
      case 'KG':
        unit = 1;
        break;

      case 'MT':
        unit = TON;
        break;

      case 'LB':
        unit = POUND;
        break;

      default:
        throw type + ' ' + unitCode + ' not exist';
    }
    return unit;
  }

   getDimensionsUnit(unitCode: string): number {
    let unit: number = 0;

    switch (unitCode) {
      case 'Cm':
        unit = 1;
        break;

      case 'Ft':
        unit = FOOT;
        break;

      case 'Inch':
        unit = INCH;
        break;

      default:
        throw 'DimensionsUnitCode ' + unitCode + ' not exist';
    }
    return unit;
  }

   getVolumeUnit(unitCode: string): number {
    let unit: number = 0;

    switch (unitCode) {
      case 'CBM':
        unit = METER;
        break;

      case 'CBF':
        unit = FOOT;
        break;

      case 'CBI':
        unit = INCH;
        break;

      default:
        throw 'VolumeUnitCode ' + unitCode + ' not exist';
    }
    return unit;
  }
}
