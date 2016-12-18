/* SignalR related interfaces  */

export interface FeedSignalR extends SignalR {
    broadcaster: Proxy
}

export interface Proxy {
    client: Client;
    server: Server;
}

export interface Client {
    setConnectionId: (id: string) => void;
    addChat: (chat: Chat) => void;
    addMessage: (message: Message) => void;
}
//change , don't need in this project
export interface Server {
    subscribe(matchId: number): void;
    unsubscribe(matchId: number): void;
}

export enum SignalRConnectionStatus {
    Connected = 1,
    Disconnected = 2,
    Error = 3
}

/* interfaces for listening*/
export interface Message {
    Id:number;
    ChatId: number;
    CreatedAt: Date;
    SenderId:number;
    Text: string; 
}

export interface Chat {
    Id:number;
    CreatedAt: Date;
    Name: string; 
}