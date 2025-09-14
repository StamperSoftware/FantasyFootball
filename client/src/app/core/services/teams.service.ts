import { inject, Injectable } from '@angular/core';
import { environment } from "@environments";
import { HttpClient } from "@angular/common/http";
import { Team } from "@models";

@Injectable({
  providedIn: 'root'
})
export class TeamsService {
  private url = `${environment.apiUrl}/teams`
  private http = inject(HttpClient);
  
  getTeams(){
    return this.http.get<Team[]>(this.url);
  }
}
