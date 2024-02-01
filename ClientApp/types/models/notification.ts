import { Item } from "./item"
import { User } from "./user"

export type Notification = {
    notificationId: number,
    notificationContent: string,
    notificationDate: string,

    userId: number,
    user?: User,

    itemId: number,
    item?: Item
}