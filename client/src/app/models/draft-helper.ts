import { Athlete, UserTeam } from "./";

export type DraftHelper = {
    teams:UserTeam[]
    currentPick:UserTeam
    index:number
    availableAthletes:Athlete[]
    rounds:number
    draftOrder:DraftSlot[]
    status:DraftStatus
}


type DraftSlot = {
    position : number
    teamId : number
    athlete? : Athlete
    displayOrder:number
}

export enum DraftStatus {
    predraft, inprogress, completed
}