import { Component, inject, OnInit } from '@angular/core';
import { LeagueSettingsService } from "@services";
import { ActivatedRoute } from "@angular/router";
import { LeagueSettings } from "@models";
import { FormsModule } from "@angular/forms";
import { CdkDrag, CdkDragDrop, CdkDropList, moveItemInArray } from "@angular/cdk/drag-drop";
import { FloatingInputComponent } from "../../../components/floating-input/floating-input.component";

@Component({
  selector: 'app-league-settings',
  standalone: true,
  imports: [
    FloatingInputComponent,
    FormsModule,
    CdkDropList,
    CdkDrag,
    FloatingInputComponent
  ],
  templateUrl: './league-settings.component.html',
  styleUrl: './league-settings.component.scss'
})
export class LeagueSettingsComponent implements OnInit {

  private leagueSettingsService = inject(LeagueSettingsService);
  private activatedRoute = inject(ActivatedRoute);

  leagueId = +this.activatedRoute.snapshot.paramMap.get("league-id")!;
  settings!: LeagueSettings;

  ngOnInit(){
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

  drop(e: CdkDragDrop<number[]>){
    moveItemInArray(this.settings.draftOrder, e.previousIndex, e.currentIndex);
  }
}
