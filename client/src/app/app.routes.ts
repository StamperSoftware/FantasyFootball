import { Routes } from '@angular/router';
import { HomeComponent } from "./features/home/home.component";
import { LeagueDetailComponent } from "./features/league/detail/detail.component";
import { UserListComponent } from "./features/user/list/list.component";
import { UserDetailComponent } from "./features/user/detail/detail.component";
import { PlayerListComponent } from "./features/player/list/list.component";
import { PlayerDetailComponent } from "./features/player/detail/detail.component";
import { LeagueListComponent } from "./features/league/list/list.component";

export const routes: Routes = [
    {path:"", component:HomeComponent},
    {path:"leagues", component:LeagueListComponent},
    {path:"leagues/:id", component:LeagueDetailComponent},
    {path:"users", component:UserListComponent},
    {path:"users/:id", component:UserDetailComponent},
    {path:"players", component:PlayerListComponent},
    {path:"players/:id", component:PlayerDetailComponent},
];
