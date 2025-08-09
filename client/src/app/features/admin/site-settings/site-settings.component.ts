import { Component, inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule } from "@angular/forms";
import { SiteSettingsService } from "../../../core/services/site-settings.service";
import { SiteSettings } from "@models";

@Component({
  selector: 'app-site-settings',
  standalone: true,
    imports: [
        FormsModule,
        ReactiveFormsModule
    ],
  templateUrl: './site-settings.component.html',
  styleUrl: './site-settings.component.scss'
})
export class SiteSettingsComponent implements OnInit {
    
    ngOnInit(): void {
        this.getSettings();
    }

    private siteSettingsService = inject(SiteSettingsService);
    
    currentWeek = new FormControl();
    currentSeason = new FormControl();
    
    getSettings(){
      this.siteSettingsService.getSettings().subscribe({
          next: settings => {
              console.log(settings.currentWeek)
              this.currentWeek.setValue(settings.currentWeek);
              this.currentSeason.setValue(settings.currentSeason);
          },
      });
    }
  
    updateSettings(){
      this.siteSettingsService.updateSettings({currentSeason:+this.currentSeason.value, currentWeek:+this.currentWeek.value}).subscribe();
    }
  
}
