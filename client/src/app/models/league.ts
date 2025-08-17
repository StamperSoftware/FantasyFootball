import { UserTeam, Game,LeagueSettings } from ".";

export type League = {
    id : number
    name : string
    teams : UserTeam[]
    schedule : Game[]
    season : number
    settings:LeagueSettings
}

export type CreateLeagueDto = {
    name : string
}

export type AddPlayerToLeagueDto = {
    playerId:number
    leagueId:number
}
