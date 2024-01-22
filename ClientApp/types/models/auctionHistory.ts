import { Item } from "./item"
import { User } from "./user"

export type AuctionHistory = {
    autionHistoryId: number,
    winningBid: number, // bid amount
    endDate: Date,

    winnerId: number,
    winner?: User,

    itemId: number,
    item?: Item,
}