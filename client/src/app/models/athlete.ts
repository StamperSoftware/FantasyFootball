import { Team } from "./team";
import { Position } from "./position";

export type Athlete = {
    firstName : string
    lastName : string
    id : number
    team : Team
    position: Position
}