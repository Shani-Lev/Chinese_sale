import { Gift } from "./gift"

export class UserTicket {
    isWin?: boolean
    isInBasket? : boolean
    amount? : number
    gift : Gift = new Gift("", 0, [],[],1)
}
