import { Item } from "./item"
import { User } from "./user"

export type Bid = {
    bidId: number,
    bidAmount: number,
    bidDate: string,

    userId: number,
    user?: User,

    itemId: number,
    item?: Item
}