import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../_services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-Home',
  templateUrl: './Home.component.html',
  styleUrls: ['./Home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(private http:HttpClient,private authservice:AuthService,private router:Router) { }
registermode:boolean =false;
// values:any;
  ngOnInit() {
    // this.getValues();
    if(this.authservice.logedin){
      this.router.navigate(['/member']);
    }
  }
  
  registerToggle(){
    this.registermode=!this.registermode
  }
  
  cancelRegister(mode:boolean){
    this.registermode=mode;
  }

}
