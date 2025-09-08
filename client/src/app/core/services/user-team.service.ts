import { inject, Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { environment } from "@environments";
import { TradeRequestTeamDto, UserTeam } from "@models";

@Injectable({
  providedIn: 'root'
})
export class UserTeamService {
  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl;
  private teamUrl = `${this.baseUrl}/user-teams`;
  
  getTeam(teamId:number){
    return this.http.get<UserTeam>(`${this.teamUrl}/${teamId}`)
  }
  
  getUserTeams(userId:string) {
    return this.http.get<UserTeam[]>(`${this.teamUrl}/user/${userId}`);
  }
  getTeams(){
    return this.http.get<UserTeam[]>(this.teamUrl);
  }
  
  updateTeamName(name:string, id:number){
    return this.http.put(this.teamUrl, {name, id});
  }
  
  dropAthlete(athleteId:number, teamId:number){
    return this.http.delete(`${this.teamUrl}/${teamId}/athletes/${athleteId}`);
  }
  
  moveToBench(athleteId:number, teamId:number){
    return this.http.put(`${this.teamUrl}/${teamId}/bench/${athleteId}`, {});
  }
  
  moveToStarters(athleteId:number, teamId:number){
    return this.http.put(`${this.teamUrl}/${teamId}/starters/${athleteId}`, {});
  }
  
  tradeAthletes(teamOneId:number, teamTwoId:number, teamOneAthleteIds:number[], teamTwoAthleteIds:number[]){
    return this.http.put(`${this.teamUrl}/trade-athletes`, {teamOneId, teamTwoId, teamOneAthleteIds, teamTwoAthleteIds})
  }
  
  createTradeRequest(teamOneId:number, teamTwoId:number, teamOneAthleteIds:number[], teamTwoAthleteIds:number[]){
    return this.http.post(`${this.teamUrl}/trade-request`, {teamOneId, teamTwoId, teamOneAthleteIds, teamTwoAthleteIds})
  }
  
  getReceivedTradeRequests(teamId:number){
    return this.http.get<TradeRequestTeamDto[]>(`${this.teamUrl}/${teamId}/received-trade-requests`)
  }
  
  getInitiatedTradeRequests(teamId:number){
    return this.http.get<TradeRequestTeamDto[]>(`${this.teamUrl}/${teamId}/initiated-trade-requests`)
  }
  
  confirmTradeRequest(requestId:string){
    return this.http.put(`${this.teamUrl}/trade-requests/${requestId}/confirm`, {});
  }
  
  declineTradeRequest(requestId:string){
    return this.http.put(`${this.teamUrl}/trade-requests/${requestId}/decline`, {});
  }
  
}
