import { Athlete } from ".";

export type Roster = {
    id:string
    starters:Athlete[]
    bench:Athlete[]
}