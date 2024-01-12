import { CategoryItem } from "./categoryItem"

export type Category = {
    categoryId: number,
    categorName: string,
    description: string,

    categoryItems?: CategoryItem[]
}