import { Directive, OnInit, Input, ViewContainerRef, TemplateRef } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Directive({
  selector: '[hasRole]'
})
export class HasroleDirective implements OnInit {
  @Input() hasRole:string[];
  isVisible=false;
 

  constructor(private ViewContainerRef:ViewContainerRef,
              private templetRef:TemplateRef<any>,
               private authservice:AuthService) { }
    ngOnInit( ) {
      const userRoles=this.authservice.decodedToken.role as Array<string>;
      if(!userRoles)
      this.ViewContainerRef.clear();
      if(this.authservice.rolematch(this.hasRole)){
        if(!this.isVisible){
          this.isVisible=true;
          this.ViewContainerRef.createEmbeddedView(this.templetRef);
        }else{
          this.isVisible=false;
          this.ViewContainerRef.clear();
        }
      }
             }

}
