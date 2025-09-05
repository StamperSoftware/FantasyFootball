import { UserTeam, WeeklyStats } from ".";

export type Game = {
    id : string
    home : UserTeam
    away : UserTeam
    week : number
    
    awayScore : number
    homeScore : number
    
    weeklyStats : WeeklyStats[]
    
}