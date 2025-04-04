import { UserTeam } from "./user-team";

export type League = {
    id : string
    name : string
    teams : UserTeam[]
}

export type CreateLeagueDto = {
    name : string
}

export type AddPlayerToLeagueDto = {
    playerId:number
    leagueId:number
}