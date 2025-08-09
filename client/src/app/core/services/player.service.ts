import { inject, Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { environment } from "@environments";
import { Player } from "@models";

@Injectable({
  providedIn: 'root'
})
export class PlayerService {
    
    private http = inject(HttpClient);
    private baseUrl = environment.apiUrl;
    private playerUrl = `${this.baseUrl}/players`;
    
    getPlayer(id:number) {
        return this.http.get<Player>(`${this.playerUrl}/${id}`);
    }
    
    getPlayers(){
      return this.http.get<Player[]>(this.playerUrl);
    }
    
}
