import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'CheckBox',
  templateUrl: './CheckBoxComponent.html',
  styleUrls: ['./CheckBoxComponent.css']
})
export class CheckBoxComponent implements OnInit {

  // @Input() public DataContext;
  @Input() public Bind: string;
  @Input() public Text: string = "";
  @Input() public IsChecked: boolean = false;

  constructor() { }

  ngOnInit(): void {
  }

  private _DataContext : string;
  @Input() 
  public get DataContext() : string {
    return this._DataContext;
  }
  public set DataContext(v : string) {
    this._DataContext = v;
    setTimeout(() => {
      this._Value = this.DataContext[this.Bind];
    }, 50);
  }
  
  
  private _Value : string;
  @Input() 
  public get Value() : string {
    return this._Value;
  }
  public set Value(v : string) {
    this._Value = v;
    if(this.DataContext)
      this.DataContext[this.Bind] = v;
  }
  

}
