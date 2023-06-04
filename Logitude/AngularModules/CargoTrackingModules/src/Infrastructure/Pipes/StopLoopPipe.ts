import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'StopLoopPipe'
})
export class StopLoopPipe implements PipeTransform {
  transform(array: any[], stopCondition: boolean,Count:number): any[] {
    
    if (!stopCondition) {
      return array.slice(0, Count); // return first 5 items
    } else {
      return array;
    }
  }
}
