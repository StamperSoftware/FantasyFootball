import { Component, inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule } from "@angular/forms";
import { SiteSettingsService } from "../../../core/services/site-settings.service";
import { SiteSettings } from "@models";
import { GameService } from "../../../core/services/game.service";
import { AthleteService } from "../../../core/services/athlete.service";

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
export class SiteSettingsComponent {
    
    public siteSettingsService = inject(SiteSettingsService);
    private gameService = inject(GameService);
    private athleteService = inject(AthleteService);
    
    finalizeWeeklyGames(){
        this.gameService.finalizeGames().subscribe();
    }

    generateWeeklyStats(){
        this.athleteService.generateWeeklyStats().subscribe();
    }

    completeSeason(){
        
    }
    
    advanceWeek(){
        this.siteSettingsService.advanceWeek().subscribe();
    }
    
}
