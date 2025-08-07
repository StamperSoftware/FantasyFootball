import { Team, Position, WeeklyStats, SeasonStats } from ".";

export type Athlete = {
    firstName : string
    lastName : string
    id : number
    team : Team
    position: Position
    weeklyStats : WeeklyStats[]
    seasonStats : SeasonStats
}