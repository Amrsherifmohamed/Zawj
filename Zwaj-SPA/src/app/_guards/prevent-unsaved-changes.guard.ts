import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, CanDeactivate } from '@angular/router';
import { Observable } from 'rxjs';
import { MemberEditComponent } from '../member/member-edit/member-edit.component';
import { AlertifyService, ConfirmResult } from '../_services/alertify.service';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard implements CanDeactivate<MemberEditComponent> {
  x =false;
  constructor(private alertify:AlertifyService) {}
 async canDeactivate(component: MemberEditComponent){
    if(component.editform.dirty){
      const confirm=await this.alertify.promisifyConfirm("انتبه","لم يتم حفظ البيانات هل تود الاستمرار دون حفظ");
      if(confirm==ConfirmResult.Ok){this.x=true}else{
        this.x=false;
      }
     // console.log(this.x)
      return this.x;
      }
  return true;   
  }
  
}
