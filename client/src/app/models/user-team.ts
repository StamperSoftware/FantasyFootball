import { Player, Athlete, Roster } from ".";

export type UserTeam = {
    player : Player
    id:number
    name?:string
    athletes : Athlete[]
    rosterId:string
    roster:Roster
    wins:number
    losses:number
    ties:number
}