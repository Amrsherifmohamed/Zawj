import {Routes} from '@angular/router'
import { HomeComponent } from './Home/Home.component'
import { ListsComponent } from './lists/lists.component'
import { MassegasComponent } from './messages/massegas.component';
import { MemberListComponent } from './member-list/member-list.component';
import { AuthGuard } from './_guards/auth.guard';
export const appRoutes:Routes=[
{path:'',component:HomeComponent},
{path:'',component:HomeComponent},
{path:'lists',component:ListsComponent,canActivate:[AuthGuard]},
{path:'member',component:MemberListComponent,canActivate:[AuthGuard]},
{path:'Messages',component:MassegasComponent,canActivate:[AuthGuard]},
{path:'**',redirectTo:'',pathMatch:'full'}


]