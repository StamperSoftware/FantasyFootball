import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from "@angular/common/http";
import { environment } from "@environments";
import { UserTeam } from "@models";

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
}
