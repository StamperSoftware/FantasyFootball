import { inject, Injectable } from '@angular/core';
import { environment } from "@environments";
import { HttpClient } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {

  private url = `${environment.apiUrl}/statistics`
  private http = inject(HttpClient);
  
  uploadStats(file:File){
    console.log(file);
    let formData = new FormData();
    formData.append("file", file, file.name);
    return this.http.post(`${this.url}`, formData) 
  }
}
