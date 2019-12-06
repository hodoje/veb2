
export class PriceListTicketType{

    constructor(id: number, name: string, price: number, userType: string){
        this.ticketTypeId = id;
        this.name = name;
        this.price = price;
        this.userType = userType;
    }

    ticketTypeId: number;
    name: string;
    price: number;
    userType: string;
}