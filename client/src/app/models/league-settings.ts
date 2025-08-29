export type LeagueSettings = {
    leagueId:number
    numberOfGames:number
    numberOfTeams:number
    rosterLimit:number
    startingQuarterBackLimit:number
    startingRunningBackLimit:number
    startingWideReceiverLimit:number
    startingTightEndLimit:number
    flexLimit:number

    receptionScore:number
    receivingYardsScore:number
    receivingTouchdownsScore:number
    passingYardsScore:number
    passingTouchdownsScore:number
    rushingYardsScore:number
    rushingTouchdownsScore:number
    draftOrder:number[]
}