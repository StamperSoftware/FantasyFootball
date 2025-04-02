import { Routes } from '@angular/router';
import { HomeComponent } from "./features/home/home.component";
import { LeaguesComponent } from "./features/leagues/leagues.component";
import { LeagueDetailComponent } from "./features/leagues/detail/detail.component";

export const routes: Routes = [
    {path:"", component:HomeComponent},
    {path:"leagues", component:LeaguesComponent},
    {path:"leagues/:id", component:LeagueDetailComponent},
];
