import { inject, Injectable, OnInit, signal } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { environment } from "@environments";
import { SiteSettings, UpdateSiteSettingsDto } from "@models";
import { map } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class SiteSettingsService implements OnInit {
  
  ngOnInit(): void {
    this.getSettings();
  }
  
  private http = inject(HttpClient);
  private url = `${environment.apiUrl}/site-settings`;
  
  currentSeason = signal<number|null>(null);
  currentWeek = signal<number|null>(null);
  
  getSettings(){
    return this.http.get<SiteSettings>(this.url).pipe(
        map(siteSettings => {
            this.currentSeason.set(siteSettings.currentSeason);  
            this.currentWeek.set(siteSettings.currentWeek);  
            return siteSettings;
        })
    )
  }
  
  updateSettings(settingsDto :UpdateSiteSettingsDto){
    return this.http.put(this.url, settingsDto);
  }
}
