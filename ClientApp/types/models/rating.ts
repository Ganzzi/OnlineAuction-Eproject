import { Item } from "./item"
import { User } from "./user"

export type Rating = {
    ratingId: number,
    rate: number,
    ratingDate: Date,

    raterId: number,
    rater?: User,

    ratedUserId: number,
    ratedUser?: User,

    itemId: number,
    item?: Item
}