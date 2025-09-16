import { Component, inject } from '@angular/core';
import { NgbToast } from "@ng-bootstrap/ng-bootstrap";
import { ToastService } from "@services";

@Component({
  selector: 'app-toasts',
  standalone: true,
    imports: [
        NgbToast
    ],
  templateUrl: './toasts.component.html',
  styleUrl: './toasts.component.scss'
})
export class ToastsComponent {
    toastService = inject(ToastService);
} 
