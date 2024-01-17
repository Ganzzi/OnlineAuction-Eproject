import { Bid } from "./bid"
import { CategoryItem } from "./categoryItem"
import { User } from "./user"

export type Item = {
    itemId: number,
    title: string,
    description: string,
    image: string,

    startingPrice: number,
    increasingAmount: number,
    minSellingPrice?: number,
    reservePrice?: number,

    sellerId: number,
    seller?: User,
    

    categoryItems?: CategoryItem[]
    bids?: Bid[]
}