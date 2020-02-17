import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { User } from 'src/app/_models/user';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { NgForm } from '@angular/forms';
import { UserService } from 'src/app/_services/user.service';
import { AuthService } from 'src/app/_services/auth.service';
import { nextTick } from 'q';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editform:NgForm
  @HostListener('window:beforeunload',['$event'])
  unLoadNotification($event:any){
    if(this.editform.dirty){
      $event.returnValue=true;  
    }
  }

user:User;
photoUrl:string;
  constructor(private rout:ActivatedRoute,private alertify:AlertifyService,private userservice:UserService,
    private authservice:AuthService) { }

  ngOnInit() {
    this.rout.data.subscribe(data=>{
      this.user=data['user'];
    });
    this.authservice.currentphotoUrl.subscribe(
     photoUrl=>  this.photoUrl=photoUrl
    );
  }
  updateuser(){
    this.userservice.updateuser(this.authservice.decodedToken.nameid,this.user).subscribe(
      next=>{ 
        this.alertify.success("تم تعديلالبيانات بنجاح");
        this.editform.reset(this.user);},
        error=>this.alertify.error('error'));
  }

}
