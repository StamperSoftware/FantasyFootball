import { inject, Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { environment } from "@environments";
import { Athlete } from "@models";

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
  
  getAthleteWithStats(id:number){
    return this.http.get<Athlete>(`${this.athleteUrl}/${id}/with-stats`);
  }
  
  updateAthleteStats(athleteId:number, body:any) {
    return this.http.put(`${this.athleteUrl}/${athleteId}/stats`, body)
  }

  generateYearlyStats(){
      return this.http.post(`${this.athleteUrl}/generate-yearly-stats`, {});
  }
  
}
