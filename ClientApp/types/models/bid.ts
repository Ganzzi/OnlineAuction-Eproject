import { Item } from "./item"
import { User } from "./user"

export type Bid = {
    bidId: number,
    bidAmount: number,
    bidDate: Date,

    userId: number,
    user?: User,

    itemId: number,
    item?: Item
}