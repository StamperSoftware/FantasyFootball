import { Athlete } from "./athlete";

export type TradeRequest = {
    id:string
    initiatingTeamId : number
    initiatingAthletes : Athlete[]
    receivingTeamId : number
    receivingAthletes : Athlete[]
}

export type TradeRequestTeamDto = {
    myPlayers:Athlete[]
    theirPlayers:Athlete[]
    id:string
}