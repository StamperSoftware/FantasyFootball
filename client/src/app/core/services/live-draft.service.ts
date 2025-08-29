
import { Injectable, signal } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from "@environments";
import { DraftHelper } from "@models"

@Injectable({
  providedIn: 'root'
})
export class LiveDraftService {
    
    private hubConnection : signalR.HubConnection;
    private url = `${environment.apiUrl}/live-draft`
    isConnected = signal(false);
    
    constructor(){
        this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(`${this.url}`)
            .build();    
    }
    
    async start () {
        await this.hubConnection.start();
        if (this.hubConnection.state == "Connected") this.isConnected.set(true);
    }
    
    async stop () {
        await this.hubConnection.stop();
        if (this.hubConnection.state == "Disconnected") this.isConnected.set(false);
    }
    
    async joinGroup(leagueId:number) {
        return await this.hubConnection.invoke<DraftHelper>("JoinGroup", leagueId);
    }
    
    async leaveGroup(leagueId:number) {
        return await this.hubConnection.invoke<DraftHelper>("LeaveGroup", leagueId);
    }
    
    async draftPlayer(leagueId:number, teamId:number, athleteId:number) {
        return await this.hubConnection.invoke<DraftHelper>("DraftPlayer", leagueId, teamId, athleteId);
    }
    
    addEventListener(event:LiveDraftEvent, handleCallback:(response?:any)=>any){
        this.hubConnection.on(event, handleCallback);
    }
}

type LiveDraftEvent = "DraftedPlayer" | "JoinedGroup" | "LeftGroup";