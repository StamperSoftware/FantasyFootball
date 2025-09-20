import { Roster } from ".";

export type UserTeam = {
    userId : string
    id:number
    name?:string
    roster:Roster
    wins:number
    losses:number
    ties:number
    isOnline?:boolean
    leagueId:number
}