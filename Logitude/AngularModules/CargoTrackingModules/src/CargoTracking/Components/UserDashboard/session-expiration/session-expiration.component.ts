import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-session-expiration',
  templateUrl: './session-expiration.component.html',
  styleUrls: ['./session-expiration.component.scss']
})
export class SessionExpirationComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<SessionExpirationComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit() {
  }
  onClick(): void {
    this.dialogRef.close(true);
  }
}
