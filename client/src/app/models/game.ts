import { UserTeam } from "./user-team";
import { WeeklyStats } from "./weekly-stats";

export type Game = {
    id : number
    home : UserTeam
    away : UserTeam
    week : number
    
    awayScore : number
    homeScore : number
    
    weeklyStats : WeeklyStats[]
    
}