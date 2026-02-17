import { Component, Input} from '@angular/core';
///////////////////////////////////////////////
enum elementType {
    header = 1,
    separator = 2,
    field = 3
}
///////////////////////////////////////////////////
interface IMyElement {
    id: number;
    key: string;
    value: any;
    myElementType: elementType;

}
/////////////////////////////////////////////////
//<span *ngSwitchCase="2" > <hr/></span>
@Component({
    moduleId: module.id,
    selector: 'object-viewer',
    templateUrl: './ObjectViewerComponent.html',
})


export class ObjectViewerComponent {
    _MyObject: any;
    @Input() public get MyObject() {
        return this._MyObject;
    }
    public set MyObject(newValue: any) {
        if (this._MyObject != newValue) {
            this._SearchValue = "";
            this._MyObject = newValue;
            this._MyObjectElements = null;
            this._MyObjectElements = new Array<IMyElement>();
            this.parseObjectToArrayElements(this._MyObject);
           // console.log(this._MyObjectElements);
        }
    }
    @Input()
    MyNgStyleHeight: string ="100px";
    _SearchValue: string = "";
    _MyObjectElements = new Array<IMyElement>();
    
    
    _id: number;
    constructor() {

    }
    SetWindowArgs(myArg: any) {
        ///alert("SetWindowArgs" + myArg);
        this.MyNgStyleHeight = "420px";
        this.MyObject=myArg;
    }

    parseObjectToArrayElements(myStartObj: any) {
        if (myStartObj == null || myStartObj == undefined) return;
        var mytype = typeof myStartObj;

        this._MyObjectElements.push({ id: this._id++, key: mytype, value: null, myElementType: elementType.separator });

        let tst = Object.keys(myStartObj).length;
        let currKey: string;
        for (currKey in myStartObj) {
            if (myStartObj.hasOwnProperty(currKey)) {
                let currPropValue = myStartObj[currKey];
                if (currPropValue instanceof Object) {

                    
                    this._MyObjectElements.push({ id: this._id++, key: currKey, value: null, myElementType: elementType.header });

                    if (currPropValue instanceof Array) {
                        
                        for (let iCounter = 0; iCounter < currPropValue.length; iCounter++) {
                            
                            this.parseObjectToArrayElements(currPropValue[iCounter]);
                        }
                    }
                    else {
                        this.parseObjectToArrayElements(currPropValue);
                    }
                    continue;
                }

                var currKeyValue: IMyElement =
                    {
                        id: this._id++,
                        key: currKey,
                        value: currPropValue,
                        myElementType: elementType.field,
                    }
                this._MyObjectElements.push(currKeyValue);
            }
        }


    }




    toShow(myele: IMyElement) {
        if (this._SearchValue == '') return true;
        var search = myele.id + myele.key + myele.value;
        search = search.toLowerCase();
        if (search.indexOf(this._SearchValue.toLowerCase()) == -1) {
            return false;
        } else {
            return true;
        }

    }
   

}

