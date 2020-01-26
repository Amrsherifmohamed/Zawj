import {Routes} from '@angular/router'
import { HomeComponent } from './Home/Home.component'
import { ListsComponent } from './lists/lists.component'
import { MassegasComponent } from './messages/massegas.component';
import { MemberListComponent } from './member/member-list/member-list.component';
import { MemberDetailComponent } from './member/member-detail/member-detail.component';

import { AuthGuard } from './_guards/auth.guard';
import { MemeberDetailResolver } from './_resorvers/member-detail.resolver';
import { MemeberListResolver } from './_resorvers/member-list.resolver';
export const appRoutes:Routes=[
{path:'',component:HomeComponent},
{path:'lists',component:ListsComponent,canActivate:[AuthGuard]},
{path:'member',component:MemberListComponent,canActivate:[AuthGuard],resolve:{
   users:MemeberListResolver
}},
{path:'member/:id',component:MemberDetailComponent,canActivate:[AuthGuard],resolve:{
    user:MemeberDetailResolver
}},
{path:'Messages',component:MassegasComponent,canActivate:[AuthGuard]},
{path:'**',redirectTo:'',pathMatch:'full'}


]