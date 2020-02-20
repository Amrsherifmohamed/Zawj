import {Injectable } from "@angular/core"
import { User } from "../_models/user"
import { Resolve, Router, ActivatedRouteSnapshot } from "@angular/router"
import { UserService } from "../_services/user.service"
import { AlertifyService } from "../_services/alertify.service"
import { Observable ,of} from "rxjs"
import { catchError } from "rxjs/operators"
@Injectable()
export class MemeberListResolver implements Resolve<User[]>{
    pageNumber=1;
    pageSize=6;
    constructor(private userserbvice:UserService,private router:Router,private alirtefy:AlertifyService) {}
    resolve(route:ActivatedRouteSnapshot):Observable<User[]>{
        return this.userserbvice.getusers(this.pageNumber,this.pageSize).pipe(
            catchError(error=>{
                this.alirtefy.error('يوجد مشكله فى عرض البيانات ');
                this.router.navigate(['/member']);
                return of(null);
            })
        )
    }
}
