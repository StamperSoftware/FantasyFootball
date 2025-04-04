import { Role } from ".";

export type AppUser = {
    firstName:string
    lastName:string
    email:string
    userName:string
    roles:Role[] | string
}