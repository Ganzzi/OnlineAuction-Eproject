import { Item } from "./item"
import { User } from "./user"

export type AuctionHistory = {
    autionHistoryId: number,
    paymentMethod: string,
    winningBid: number, // bid amount
    startDate: Date,
    endDate: Date,

    winnerId: number,
    winner?: User,

    itemId: number,
    item?: Item,
}