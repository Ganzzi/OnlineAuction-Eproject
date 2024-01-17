import { Bid } from "./bid"
import { Item } from "./item"
import { Rating } from "./rating"

export type User = {
    userId: number,
    name: string,
    email?: string,
    password?: string
    role?: string,
    refreshToken?: string,
    locked?: boolean,
    resetPasswordToken?: string,

    bids?: Bid[],
    soldItems?: Item[]
    ratings?: Rating[],
    beingRateds?: Rating[],
    averageBeingRated?: number
}