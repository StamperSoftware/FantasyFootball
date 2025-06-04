import { Team } from "./team";
import { Position } from "./position";
import { WeeklyStats } from "./weekly-stats";
import { SeasonStats } from "./season-stats";

export type Athlete = {
    firstName : string
    lastName : string
    id : number
    team : Team
    position: Position
    weeklyStats : WeeklyStats[]
    seasonStats : SeasonStats
}