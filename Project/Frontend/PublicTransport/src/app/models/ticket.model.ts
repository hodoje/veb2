
export class TicketDto{
    constructor(ticketTypeid: number, email: string){
        this.ticketTypeId = ticketTypeid;
        this.email = email;
    }

    ticketTypeId: number;
    email: string;
}