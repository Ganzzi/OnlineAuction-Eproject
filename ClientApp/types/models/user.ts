import { Bid } from "./bid"
import { Item } from "./item"

export type User = {
    userId: number,
    username: string,
    email?: string,
    password?: string
    role?: string,
    refreshToken?: string,
    locked?: boolean,
    resetPasswordToken?: string,

    bids?: Bid[],
    soldItems?: Item[]
}