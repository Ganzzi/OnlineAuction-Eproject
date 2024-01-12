import { Item } from "./item"
import { User } from "./user"

export type Notification = {
    notificationId: number,
    notificationContent: number,
    notificationDate: Date,

    userId: number,
    user?: User,

    itemId: number,
    item?: Item
}