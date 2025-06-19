import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface PurchaseBillItem {
  Id?: number;
  Item: string;
  Batch: string;
  StandardCost: number;
  StandardPrice: number;
  Quantity: number;
  Discount: number;
  TotalCost?: number;
  TotalSelling?: number;
}

export interface PurchaseBillSummary {
  TotalItems: number;
  TotalQuantity: number;
  GrossTotal: number;
  TotalDiscount: number;
  NetTotal: number;
}

@Injectable({
  providedIn: 'root'
})
export class DatabaseService {
  private apiUrl = 'http://localhost:5045/api';

  constructor(private http: HttpClient) { }

  addPurchaseBillItem(item: PurchaseBillItem): Observable<any> {
    return this.http.post(`${this.apiUrl}/purchasebill/add-item`, item);
  }

  getPurchaseBillItems(): Observable<PurchaseBillItem[]> {
    return this.http.get<PurchaseBillItem[]>(`${this.apiUrl}/purchasebill/items`);
  }

  getPurchaseBillSummary(): Observable<PurchaseBillSummary> {
    return this.http.get<PurchaseBillSummary>(`${this.apiUrl}/purchasebill/summary`);
  }

  clearPurchaseBillItems(): Observable<any> {
    return this.http.delete(`${this.apiUrl}/purchasebill/clear`);
  }

  getLocations(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/auth/locations`);
  }
}
