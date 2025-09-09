import { Component, ElementRef, inject, OnInit, ViewChild } from '@angular/core';
import { SiteSettingsService, StatisticsService } from "@services";

@Component({
  selector: 'app-upload-stats',
  standalone: true,
  imports: [],
  templateUrl: './upload-stats.component.html',
  styleUrl: './upload-stats.component.scss'
})
export class UploadStatsComponent {
  
  
  private statisticsService = inject(StatisticsService);
  siteSettingsService = inject(SiteSettingsService);
  
  @ViewChild("inputUploadStats") inputFileRef!:ElementRef;

  handleFileUpload(e:Event){
    const inputEl = e.target as HTMLInputElement
    const file = inputEl.files?.[0];
    if (!file) return;
    this.statisticsService.uploadStats(file).subscribe({
      next:response => {
        this.inputFileRef.nativeElement.value = "";
      }
    });
  }
}
