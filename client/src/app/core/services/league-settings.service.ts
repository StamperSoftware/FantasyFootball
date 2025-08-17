import { inject, Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { environment } from "@environments";
import {LeagueSettings} from "@models";

@Injectable({
  providedIn: 'root'
})
export class LeagueSettingsService {

  private http = inject(HttpClient);
  private url = `${environment.apiUrl}/league-settings`
  
  getLeagueSettings(leagueId:number){
    return this.http.get<LeagueSettings>(`${this.url}/${leagueId}`);
  }
  updateLeagueSettings(settings:LeagueSettings){
    return this.http.put(`${this.url}/${settings.leagueId}`, settings)
  }
}
