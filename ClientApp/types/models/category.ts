import { CategoryItem } from "./categoryItem"

export type Category = {
    categoryId: number,
    categoryName: string,
    description: string,

    categoryItems?: CategoryItem[]
}