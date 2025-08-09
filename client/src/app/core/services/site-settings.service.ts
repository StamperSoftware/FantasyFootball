import { inject, Injectable, OnInit } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { environment } from "@environments";
import { SiteSettings, UpdateSiteSettingsDto } from "@models";

@Injectable({
  providedIn: 'root'
})
export class SiteSettingsService implements OnInit {
  
  ngOnInit(): void {
    this.getSettings();
  }
  
  private http = inject(HttpClient);
  private url = `${environment.apiUrl}/site-settings`;
  
  getSettings(){
    return this.http.get<SiteSettings>(this.url)
  }
  
  updateSettings(settingsDto :UpdateSiteSettingsDto){
    return this.http.put(this.url, settingsDto);
  }
  
}
