import { Component, inject, OnInit } from '@angular/core';
import { LeagueSettingsService } from "../../../core/services/league-settings.service";
import { ActivatedRoute } from "@angular/router";
import { LeagueSettings } from "@models";
import { FloatingInputComponent } from "../../../components/floating-input/floating-input.component";
import { FormsModule } from "@angular/forms";

@Component({
  selector: 'app-league-settings',
  standalone: true,
  imports: [
    FloatingInputComponent,
    FormsModule
  ],
  templateUrl: './league-settings.component.html',
  styleUrl: './league-settings.component.scss'
})
export class LeagueSettingsComponent implements OnInit {
  
  private leagueSettingsService = inject(LeagueSettingsService);
  private activatedRoute = inject(ActivatedRoute);
  
  leagueId = +this.activatedRoute.snapshot.paramMap.get("league-id")!;
  settings!:LeagueSettings;
  ngOnInit (): void {
    this.getLeagueSettings();
  }
    
  getLeagueSettings(){
    this.leagueSettingsService.getLeagueSettings(this.leagueId).subscribe({
      next: settings => this.settings = settings,
    })
  }
  
  updateSettings(){
    if (!this.settings) return;
    this.leagueSettingsService.updateLeagueSettings(this.settings).subscribe({});
  }
  
}

