import { inject, Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { environment } from "@environments";
import { Game } from "@models";

@Injectable({
  providedIn: 'root'
})
export class GameService {
  
    private http = inject(HttpClient);
    private apiUrl = environment.apiUrl;
    private gameUrl = `${this.apiUrl}/games`;

    getGame(gameId:number) {
        return this.http.get<Game>(`${this.gameUrl}/${gameId}`);
    }
}
