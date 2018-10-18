
import {Injectable, OnInit} from 'angular2/core';
import {Http} from 'angular2/http';

export class ObjectField {
    constructor(
        public Id: string,
        public Tenant: number,
        public FieldName: string,
        public MaxLength: number,
        public IsRequiered: boolean,
        public ObjectTableId: string,
        public MinLength: number,
        public DataTypeCode: string
    ) { }
}

@Injectable()
export class GeneralService {

    //constructor(private _http: Http) { }

    //ngOnInit() {
    //    //this._http.get('http://localhost:9996/api/ngMetaData?tenant=1&objectTableName=Shipment&inActive=false')
    //    //    // Call map on the response observable to get the parsed people object
    //    //    //.map(res => res.json())
    //    //    // Subscribe to the observable to get the parsed people object and attach it to the
    //    //    // component
    //    //    .subscribe(objectfields => this.mapConvert(objectfields)); //this.doSomething(userData));
    //    Promise.resolve(objectFields);
    //}

    mapConvert(objectfields) {
        //console.log(objectfields.json());
        var jsoned = objectfields.json();
        objectFields = jsoned;
        var filtered = jsoned.filter(d => d.ObjectTableId === "1-4");
        var filtered = jsoned.filter(d => d.Id === "1-80")[0];
        //this.notesObjectField = filtered;
        console.log(filtered);
    }

    getNotesObjectField() {
        //return this._http.get('http://localhost:9996/api/ngMetaData?tenant=1&objectTableName=Shipment&inActive=false')
        //    //.toRx()
        //    //.map(res => res.json())
        //    .subscribe(objectFields => this.mapConvert(objectFields));
        //return window.ObjectFields.filter(d => d.Id === "1-80")[0];
        console.log(objectFieldsPromise.then(objectfield => objectfield.filter(c => c.Id === "1-80")[0]));
        return objectFieldsPromise.then(objectfield => objectfield.filter(c => c.Id === "1-80")[0]);
    }

    getObjectFields() {
        //return objectFieldsPromise;
        //return this._http.get('http://localhost:9996/api/ngMetaData?tenant=1&objectTableName=Shipment&inActive=false')
        //    // Call map on the response observable to get the parsed people object
        //    //.map(res => res.json())
        //    // Subscribe to the observable to get the parsed people object and attach it to the
        //    // component
        //    .subscribe(objectfields => objectfields.filter(h => h.ObjectTableId === +"1-4")[0]);//this.mapConvert(objectfields)); //this.doSomething(userData));
        //http.get('./people.json').toRx().map(res => res.json()).subscribe(people => this.people = people);
    }

    getObjectField(id: number | string) {
        return objectFieldsPromise
            .then(objectFields => objectFields.filter(c => c.Id === +id)[0]);
    }

    getAll() {
        return Promise.resolve(objectFields);
    }
    // See the "Take it slow" appendix
    getHeroesSlowly() {
        return new Promise(resolve =>
            setTimeout(() => resolve(objectFields), 2000) // 2 seconds
        );
    }

    static nextCrisisId = 100;

    //addCrisis(name: string) {
    //    name = name.trim();
    //    if (name) {
    //        let objectField = new ObjectField(GeneralService.nextCrisisId++, name);
    //        objectFieldsPromise.then(objectFields => objectFields.push(objectField));
    //    }
    //}
}

var objectFields = window.ObjectFields;

var objectFieldsPromise = Promise.resolve(objectFields);
