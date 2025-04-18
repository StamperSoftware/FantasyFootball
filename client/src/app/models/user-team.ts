import { Player } from "./player";
import { Athlete } from "./athlete";

export type UserTeam = {
    player : Player
    id:number
    name?:string
    athletes : Athlete[]
}