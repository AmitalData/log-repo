import {Pipe, PipeTransform} from '@angular/core';
@Pipe({ name: 'groupBy' })
export class GroupByPipe implements PipeTransform {
    transform(value: Array<any>, field: string): Array<any> {
        const groupedObj = value.reduce((prev, cur) => {
            //if(prev)
            if (!prev[cur[field]]) {
                prev[cur[field]] = [cur];
            } else {
                prev[cur[field]].push(cur);
            }
            return prev;
        }, {});
        return Object.keys(groupedObj).map(key => {return { key, value: groupedObj[key] }});
    }

    transformMultiFields(value: Array<any>, field: string, field2: string): Array<any> {
        const groupedObj = value.reduce((prev, cur) => {
            //if(prev)
            var temp = prev[cur[field]];
            var temp1 = cur[field];
            var temp2 = [cur];
            if ((!prev[cur[field]]) && (!prev[cur[field2]])) {
                prev[cur[field] + ',' + cur[field2]] = [cur];
              //  prev[cur[field2]] = [cur];

            } else {
                prev[cur[field]].push(cur);
               // prev[cur[field2]].push(cur);

            }
            return prev;
        }, {});
        return Object.keys(groupedObj).map(key => { return { key, value: groupedObj[key] } });
    }


    ShapeGrouping(value: Array<any>, field: string): Array<any> {
        const groupedObj = value.reduce((prev, cur) => {
            //if(prev)
            if (!prev[cur[field]]) {
                prev[cur[field]] = [cur];
            } else {
                prev[cur[field]].push(cur);
            }
            return prev;
        }, {});
        var temp = Object.keys(groupedObj).map(key => { return { key, value: groupedObj[key] } });
        var finalarray = [];
        var i = 0;
        temp.forEach((item, key) => {
            finalarray.push({ Type: 'Head', Data: item.key, Index: i, ChildrenFirstIndex: i + 1, Count: item.value.length }); 
            i++;
            item.value.forEach((inneritem, key) => {
                finalarray.push({ Type: 'Body', Data: inneritem, Index: i }); 
                i++;
            }); 
        }); 
        return finalarray;
    }

    ShapeList(value: Array<any>): Array<any> {
        var finalarray = [];
        var i = 0;
        value.forEach((item, key) => {
            finalarray.push({ Type: 'Body', Data: item, Index: i }); 
            i++;
            
        });
        return finalarray;
    }
}