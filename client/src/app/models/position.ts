export enum Position {
    QB = 1,
    RB,
    WR,
    TE,
    D
}

export const getPositionName = (position:Position) : string => {
    return Position[position];   
}