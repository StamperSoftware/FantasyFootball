import { Routes } from '@angular/router';
import { HomeComponent } from "./features/home/home.component";
import { LeagueDetailComponent } from "./features/league/detail/detail.component";
import { UserListComponent } from "./features/user/list/list.component";
import { UserDetailComponent } from "./features/user/detail/detail.component";
import { PlayerListComponent } from "./features/player/list/list.component";
import { PlayerDetailComponent } from "./features/player/detail/detail.component";
import { LeagueListComponent } from "./features/league/list/list.component";
import { UserTeamListComponent } from "./features/user-team/list/list.component";
import { UserTeamDetailComponent } from "./features/user-team/detail/detail.component";
import { AthleteListComponent } from "./features/athlete/list/list.component";
import { AthleteDetailComponent } from "./features/athlete/detail/detail.component";
import { TradeComponent } from "./features/league/trade/trade.component";
import { DraftComponent } from "./features/league/draft/draft.component";

export const routes: Routes = [
    {path:"", component:HomeComponent},
    {path:"leagues", component:LeagueListComponent},
    {path:"leagues/:id", component:LeagueDetailComponent},
    {path:"leagues/:league-id/trade", pathMatch:"full", component:TradeComponent},
    {path:"leagues/:league-id/draft", pathMatch:"full", component:DraftComponent},
    {path:"leagues/:league-id/user-teams", pathMatch:"full", component:UserTeamListComponent},
    {path:"leagues/:league-id/user-teams/:id", pathMatch:"full", component:UserTeamDetailComponent},
    {path:"users", component:UserListComponent},
    {path:"users/:id", component:UserDetailComponent},
    {path:"players", component:PlayerListComponent},
    {path:"players/:id", component:PlayerDetailComponent},
    {path:"athletes", component:AthleteListComponent},
    {path:"athletes/:id", component:AthleteDetailComponent},
];
