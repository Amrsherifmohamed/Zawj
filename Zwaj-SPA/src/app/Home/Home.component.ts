import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-Home',
  templateUrl: './Home.component.html',
  styleUrls: ['./Home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(private http:HttpClient) { }
registermode:boolean =false;
// values:any;
  ngOnInit() {
    // this.getValues();
  }
  
  registerToggle(){
    this.registermode=!this.registermode
  }
  
  cancelRegister(mode:boolean){
    this.registermode=mode;
  }

}
