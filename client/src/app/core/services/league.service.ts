import { inject, Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../environments/environment";
import { League, Pagination } from "../../models";
import { CreateLeagueDto } from "../../models/league";

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
}
