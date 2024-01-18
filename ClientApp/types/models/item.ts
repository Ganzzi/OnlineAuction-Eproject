import { AuctionHistory } from "./auctionHistory"
import { Bid } from "./bid"
import { CategoryItem } from "./categoryItem"
import { User } from "./user"

export type Item = {
    itemId: number,
    title: string,
    description: string,
    image: string,
    imageFile?: File,

    startingPrice: number,
    increasingAmount: number,
    reservePrice?: number,

    startDate: Date,
    endDate: Date,

    sellerId: number,
    seller?: User,

    categoryItems?: CategoryItem[]
    bids?: Bid[]

    auctionHistory?: AuctionHistory
}