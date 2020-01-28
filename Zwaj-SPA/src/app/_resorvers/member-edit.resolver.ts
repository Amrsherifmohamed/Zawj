import {Injectable } from "@angular/core"
import { User } from "../_models/user"
import { Resolve, Router, ActivatedRouteSnapshot } from "@angular/router"
import { UserService } from "../_services/user.service"
import { AlertifyService } from "../_services/alertify.service"
import { Observable ,of} from "rxjs"
import { catchError } from "rxjs/operators"
import { AuthService } from "../_services/auth.service"
@Injectable()
export class MemeberEditResolver implements Resolve<User>{
    constructor(private userserbvice:UserService,private authservice:AuthService
        ,private router:Router,private alirtefy:AlertifyService) {}
    resolve(route:ActivatedRouteSnapshot):Observable<User>{
        return this.userserbvice.getuser(this.authservice.decodedToken.nameid).pipe(
            catchError(error=>{
                this.alirtefy.error('يوجد مشكله فى عرض البيانات ');
                this.router.navigate(['/member']);
                return of(null);
            })
        )
    }
}
