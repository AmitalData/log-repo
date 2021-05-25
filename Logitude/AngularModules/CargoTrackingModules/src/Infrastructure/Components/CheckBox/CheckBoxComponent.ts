import { Component, Input,Output, OnInit, EventEmitter } from '@angular/core';

@Component({
    selector: 'CheckBox',
    templateUrl: './CheckBoxComponent.html',
    styleUrls: ['./CheckBoxComponent.css']
})
export class CheckBoxComponent implements OnInit
{

    // @Input() public DataContext;
    @Input() public Bind: string;
    @Input() public Text: string = "";
    @Input() public IsChecked: boolean = false;
    @Output() public ValueChanged: EventEmitter<boolean> = new EventEmitter<boolean>();

    constructor() { }

    ngOnInit(): void
    {
    }

    private _DataContext: string;
    @Input()
    public get DataContext(): string
    {
        return this._DataContext;
    }
    public set DataContext(v: string)
    {
        this._DataContext = v;
        setTimeout(() =>
        {
            this._Value = this.DataContext[this.Bind];
        }, 50);
    }


    private _Value: boolean;
    @Input()
    public get Value(): boolean
    {
        return this._Value;
    }
    public set Value(v: boolean)
    {
        this._Value = v;
        this.ValueChanged.emit(v);

        if (this.DataContext){
            this.DataContext[this.Bind] = v;
        }
    }


}
