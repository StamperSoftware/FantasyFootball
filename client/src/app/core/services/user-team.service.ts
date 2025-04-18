import { inject, Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../environments/environment";
import { UserTeam } from "../../models/user-team";

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
  
  getTeams() {
    return this.http.get<UserTeam[]>(this.teamUrl);
  }
  
  updateTeamName(name:string, id:number){
    return this.http.put(this.teamUrl, {name, id});
  }
  
  addAthleteToTeam(athleteId:number, teamId:number){
    return this.http.put(`${this.teamUrl}/${teamId}/athletes/${athleteId}`, {})
  }
  
}
