export class  Message {
    Id:number;
    ChatId:number;
    Date: string;
    SenderId:number;
    Text:string;
    constructor(id:number, chatId:number, date:string, senderId:number, text:string) {
        this.Id=id;
        this.ChatId=chatId;
        this.Date=date;
        this.SenderId=senderId;
        this.Text=text
    }
}