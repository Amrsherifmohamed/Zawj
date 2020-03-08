import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/_models/user';
import { AdminService } from 'src/app/_services/admin.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { RolesModelComponent } from '../roles-model/roles-model.component';

@Component({
  selector: 'app-user-managment',
  templateUrl: './user-managment.component.html',
  styleUrls: ['./user-managment.component.css']
})
export class UserManagmentComponent implements OnInit {
users:User[];
bsModelRef:BsModalRef;
  constructor(private adminservice:AdminService,private aletrfy:AlertifyService,
               private modelService:BsModalService) { }

  ngOnInit() {
    this.getuserwithroles();
  }
  getuserwithroles(){
    return this.adminservice.getuserwithroles().subscribe(
      (users:User[])=>{this.users=users},//->from subscribe},
      error=>{this.aletrfy.error(error)}
    );  
  }
  editRolesModal(user:User){
    const initialState = {
     user,
     roles : this.getRolesArray(user)
    };
    this.bsModelRef = this.modelService.show(RolesModelComponent, {initialState});
    this.bsModelRef.content.updateSelectedRoles.subscribe((values)=>{
      const rolesToUpdate = {
        roleNames : [...values.filter(el=>el.checked===true).map(el=>el.value)]
      };
     if(rolesToUpdate){
       this.adminservice.updateUserRoles(user,rolesToUpdate).subscribe(
         ()=>{
           user.roles = [...rolesToUpdate.roleNames];
         },error=>this.aletrfy.error(error)
       );
     }
    })
  }
  private getRolesArray(user) {
    const roles = [];
    const userRoles = user.roles as any[];
    const availableRoles: any[] = [
      {name: 'مدير النظام', value: 'Admin'},
      {name: 'مشرف', value: 'Moderator'},
      {name: 'عضو', value: 'Member'},
      {name: 'مشترك', value: 'VIP'},
    ];

    availableRoles.forEach(aRole=>{
      let isMatch =false;
      userRoles.forEach(uRole=>{
        if(aRole.value===uRole){
          isMatch=true;
          aRole.checked = true;
          roles.push(aRole);
          return;
         }
      })
      if(!isMatch){
        aRole.checked=false;
        roles.push(aRole);
      }
    })
    return roles;
  }

}
