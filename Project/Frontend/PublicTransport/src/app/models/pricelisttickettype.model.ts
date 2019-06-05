
export class PriceListTicketType{

    constructor(id: number, name: string, price: number, userType: string){
        this.ticketId = id;
        this.name = name;
        this.price = price;
        this.userType = userType;
    }

    ticketId: number;
    name: string;
    price: number;
    userType: string;
}