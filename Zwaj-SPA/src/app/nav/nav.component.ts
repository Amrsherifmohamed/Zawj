import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model:any={};
  constructor(private authservic:AuthService) { }

  ngOnInit() {
  }
  login(){
    this.authservic.login(this.model).subscribe(
      next=>{console.log("welcome")},
      error=>{console.log("faild")}
    )
  }
  logedin(){
    const token=localStorage.getItem("token")
    return !!token
  }
  logedout(){
    localStorage.removeItem('token');
    console.log("تم تسجيل خروجك");
  }
}
