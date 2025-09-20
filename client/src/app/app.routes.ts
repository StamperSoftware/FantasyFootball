import { Routes } from '@angular/router';
import { HomeComponent } from "./features/home/home.component";
import { LeagueDetailComponent } from "./features/league/detail/detail.component";
import { UserListComponent } from "./features/user/list/list.component";
import { UserDetailComponent } from "./features/user/detail/detail.component";
import { LeagueListComponent } from "./features/league/list/list.component";
import { UserTeamListComponent } from "./features/user-team/list/list.component";
import { UserTeamDetailComponent } from "./features/user-team/detail/detail.component";
import { AthleteListComponent } from "./features/athlete/list/list.component";
import { AthleteDetailComponent } from "./features/athlete/detail/detail.component";
import { TradeComponent } from "./features/league/trade/trade.component";
import { DraftComponent } from "./features/league/draft/draft.component";
import { GameDetailComponent } from "./features/game/detail/detail.component";
import { SiteSettingsComponent } from "./features/admin/site-settings/site-settings.component";
import { LeagueSettingsComponent } from "./features/admin/league-settings/league-settings.component";
import { LiveDraftComponent } from "./features/draft/live-draft/live-draft.component";
import { TradeRequestsComponent } from "./features/user-team/trade-requests/trade-requests.component";
import { UploadStatsComponent } from "./features/admin/upload-stats/upload-stats.component";

export const routes: Routes = [
    {path:"", component:HomeComponent},
    
    {
        path:"leagues", 
        children:[
            {path:"", component:LeagueListComponent},
            {
                path:":league-id",
                children:[
                    {path:"", component:LeagueDetailComponent},
                    {path:"trade", component:TradeComponent},
                    {path:"draft", component:DraftComponent},
                    {
                        path:"user-teams",
                        children:[
                            {path: "", component:UserTeamListComponent},
                            {
                                path: ":user-team-id",
                                children: [
                                    {path: "", component: UserTeamDetailComponent},
                                    {path:"trade-requests", component:TradeRequestsComponent}
                                ],
                            }
                        ]
                    },
                    {path:"games/:game-id", component:GameDetailComponent},
                    {path:"live-draft", component:LiveDraftComponent}
                ],
            },
        ],
    },

    {
        path:"users",
        children:[
            {path:"", component:UserListComponent},
            {path:":id", component:UserDetailComponent},
        ]
    },

    {
        path:"athletes",
        children:[
            {path:"", component:AthleteListComponent},
            {path:":id", component:AthleteDetailComponent},
        ]
    },

    {
        path:"admin",
        children:[
            {path:"site-settings", component:SiteSettingsComponent},
            {path:"league-settings/:league-id", component:LeagueSettingsComponent},
            {path:"upload-stats", component: UploadStatsComponent}
        ]
    },
    
    
];
