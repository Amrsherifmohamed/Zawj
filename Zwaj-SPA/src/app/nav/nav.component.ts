import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model:any={};
  constructor(public authservic:AuthService,private alertify:AlertifyService) { }

  ngOnInit() {
  }
  login(){
    this.authservic.login(this.model).subscribe(
      next=>{this.alertify.success("تم تسجيل الدخول بنجاح ")},
      error=>{this.alertify.error(error)}
    )
  }
  logedin(){
    
    return this.authservic.logedin();
  }
  logedout(){
    localStorage.removeItem('token');
    this.alertify.success("تم تسجيل خروجك");
  }
}
