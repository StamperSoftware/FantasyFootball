import { Player, Roster } from ".";

export type UserTeam = {
    player : Player
    id:number
    name?:string
    roster:Roster
    wins:number
    losses:number
    ties:number
    isOnline?:boolean
}