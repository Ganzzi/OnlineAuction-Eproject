import { Item } from "./item"
import { User } from "./user"

export type Rating = {
    ratingId: number,
    rate: number,
    ratingDate: Date,

    userId: number,
    user?: User,

    itemId: number,
    item?: Item
}