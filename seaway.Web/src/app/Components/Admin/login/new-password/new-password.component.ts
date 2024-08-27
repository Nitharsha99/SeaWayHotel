import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-new-password',
  templateUrl: './new-password.component.html',
  styleUrls: ['./new-password.component.css']
})
export class NewPasswordComponent implements OnInit{

  visible1: boolean = false;
  visible2: boolean = false;

  ngOnInit(): void {
    
  }

  password1Visible(){
    this.visible1 = !this.visible1;
  }

  password2Visible(){
    this.visible2 = !this.visible2;
  }
}
