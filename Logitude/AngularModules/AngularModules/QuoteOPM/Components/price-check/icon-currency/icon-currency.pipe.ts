import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'iconCurrency'
})
export class IconCurrencyPipe implements PipeTransform {

  transform(value: unknown): string {
    let icon: string = '';
    switch (value) {
      case 'USD':
        icon = '$';
        break;
    
      default:
        break;
    }

    return icon;
    
  }

}
