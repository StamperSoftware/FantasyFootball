import { inject, Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../environments/environment";
import { AddPlayerToLeagueDto, League, Pagination, CreateLeagueDto } from "../../models";

@Injectable({
  providedIn: 'root'
})
export class LeagueService {
  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl;
  private leagueUrl = `${this.baseUrl}/leagues`;
  
  getLeagues(){
    return this.http.get<Pagination<League>>(this.leagueUrl);
  }
  
  createLeague(league:CreateLeagueDto) {
    return this.http.post<League>(this.leagueUrl, league);
  }
  
  
  getLeague(id:number) {
    return this.http.get<League>(`${this.leagueUrl}/${id}`);
  }
  
  addPlayer(leagueId:number, playerId:number){
    return this.http.post(`${this.leagueUrl}/${leagueId}/players/${playerId}`, {});
  }
  
  submitDraft(results:Map<number,number[]>) {
    let request:any= [];
    results.forEach((athletes:number[],teamId:number)=> request.push({teamId, athletes}));
    return this.http.post(`${this.leagueUrl}/draft`, {results:request})
  }
}
