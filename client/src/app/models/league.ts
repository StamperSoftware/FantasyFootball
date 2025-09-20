import { UserTeam, Game,LeagueSettings } from ".";

export type League = {
    id : number
    name : string
    teams : UserTeam[]
    schedule : Game[]
    season : number
    settings:LeagueSettings
    adminId : string
}

export type CreateLeagueDto = {
    name : string
}
