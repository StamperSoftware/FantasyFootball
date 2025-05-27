import { UserTeam } from "./user-team";
import { Schedule } from "./schedule";

export type League = {
    id : number
    name : string
    teams : UserTeam[]
    schedule : Schedule
}

export type CreateLeagueDto = {
    name : string
}

export type AddPlayerToLeagueDto = {
    playerId:number
    leagueId:number
}