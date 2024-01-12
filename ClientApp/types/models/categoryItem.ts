import { Category } from "./category"
import { Item } from "./item"

export type CategoryItem = {
    categoryId: number,
    category?: Category,

    itemId: number,
    item: Item,
}