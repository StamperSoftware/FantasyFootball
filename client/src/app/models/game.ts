import { UserTeam } from "./user-team";

export type Game = {
    id : number
    home : UserTeam
    away : UserTeam
}