import { inject, Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { environment } from "@environments";

@Injectable({
  providedIn: 'root'
})
export class RosterService {
  private http = inject(HttpClient);
  private url = `${environment.apiUrl}/rosters`
  
  
  getRoster(id:string) {
    return this.http.get(`${this.url}/${id}`);
  }
  
}
