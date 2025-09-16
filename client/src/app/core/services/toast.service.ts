import { Injectable, signal } from '@angular/core';
import { Toast } from "@models";

@Injectable({
  providedIn: 'root'
})
export class ToastService {
  
    private readonly _toasts = signal<Toast[]>([]);
    readonly toasts = this._toasts.asReadonly();
    
    addToast(toast:Toast){
      this._toasts.update(toasts => [...toasts, toast]);
    }
}
