import { AppUser } from ".";

export type Player = {
    userId : string
    appUser : AppUser
    user? : AppUser
    id : number
    firstName : string
    lastName : string
}