import { User } from "./user"

export type RefreshToken = {
    refreshTokenId: number,
    token: string,
    expiryDate: Date,

    userId: number,
    user?: User,
}