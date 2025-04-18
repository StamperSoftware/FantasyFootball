import { inject, Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../environments/environment";
import { Athlete } from "../../models";

@Injectable({
  providedIn: 'root'
})
export class AthleteService {
  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl;
  private athleteUrl = `${this.baseUrl}/athlete`;
  
  
  getAthletes() {
    return this.http.get<Athlete[]>(this.athleteUrl);
  }
  
  getAthlete(id:number){
    return this.http.get<Athlete>(`${this.athleteUrl}/${id}`);
  }
  
}
