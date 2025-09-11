import { inject, Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { environment } from "@environments";
import { AddPlayerToLeagueDto, League, Pagination, CreateLeagueDto, Athlete, Player } from "@models";

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

  addAthleteToTeam(leagueId:number, athleteId:number, teamId:number){
    return this.http.put(`${this.leagueUrl}/${leagueId}/team/${teamId}/athletes/${athleteId}`, {})
  }
  
  submitDraft(leagueId:number, results:Map<number,number[]>) {
    let request:any= [];
    results.forEach((athletes:number[],teamId:number)=> request.push({teamId, athletes}));
    return this.http.post(`${this.leagueUrl}/${leagueId}/draft`, {results:request})
  }
  
  createSchedule(leagueId:number) {
    return this.http.post(`${this.leagueUrl}/${leagueId}/schedule`, {})
  }
  
  getAvailableAthletes(leagueId:number){
    return this.http.get<Athlete[]>(`${this.leagueUrl}/${leagueId}/available-athletes`);
  }
  
  deleteLeague(leagueId:number){
    return this.http.delete(`${this.leagueUrl}/${leagueId}`)
  }
  
  checkIfUserIsInLeague(userId:string, leagueId:number){
    return this.http.get<boolean>(`${this.leagueUrl}/${leagueId}/user/${userId}`)
  }
  getPlayersNotInLeague(leagueId:number){
    return this.http.get<Player[]>(`${this.leagueUrl}/${leagueId}/players-not-in-league`)
  }
}
