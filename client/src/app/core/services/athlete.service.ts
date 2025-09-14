import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from "@angular/common/http";
import { environment } from "@environments";
import { Athlete, Position } from "@models";

@Injectable({
  providedIn: 'root'
})
export class AthleteService {
  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl;
  private athleteUrl = `${this.baseUrl}/athletes`;
  
  
  getAthletes(searchParams?:AthleteSearchParams) {
    
    let params = new HttpParams();
    
    if (searchParams?.position && searchParams.position != 'all') params = params.append("position", searchParams.position);
    if (searchParams?.teamId && searchParams.teamId != 'all') params = params.append("teamId", searchParams.teamId);
    if (searchParams?.search) params = params.append('search', searchParams.search);
    
    return this.http.get<Athlete[]>(this.athleteUrl, {params});
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

  generateWeeklyStats(){
      return this.http.post(`${this.athleteUrl}/generate-weekly-stats`, {});
  }
  
}

type AthleteSearchParams = {
  teamId?:number|'all'
  position?:Position | "all"
  search?:string
}