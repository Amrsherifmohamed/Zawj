import { Injectable } from '@angular/core';
declare let alertify:any;
@Injectable({
  providedIn: 'root'
})
export class AlertifyService {

constructor() { }
confirm(massage:string,oKCallback:()=>any){
  alertify.confirm(massage,function(e){
    if(e){oKCallback()}else{}
  });
}
success(massage:string){
  alertify.success(massage);
}
error(massage:string)
{
  alertify.error(massage);
}
warning(massage:string){
  alertify.warning(massage);
}
massage(massage:string){
  alertify.massage(massage);
}
}
