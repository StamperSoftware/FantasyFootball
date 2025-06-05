import { UserTeam } from "./user-team";
import { Game } from "./game";

export type League = {
    id : number
    name : string
    teams : UserTeam[]
    schedule : Game[]
    season : number
}

export type CreateLeagueDto = {
    name : string
}

export type AddPlayerToLeagueDto = {
    playerId:number
    leagueId:number
}