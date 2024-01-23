import { Item } from "./item"
import { User } from "./user"

export type AuctionHistory = {
    auctionHistoryId: number,
    winningBid: number, // bid amount
    endDate: string,

    winnerId: number,
    winner?: User,

    itemId: number,
    item?: Item,
}