import { Role } from ".";

export type AppUser = {
    id : string
    email:string
    userName:string
    roles:Role[] | string
}