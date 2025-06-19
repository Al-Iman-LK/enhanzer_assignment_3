import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { DatabaseService, PurchaseBillItem, PurchaseBillSummary } from '../services/database.service';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-purchase-bill',
  imports: [CommonModule, FormsModule],
  templateUrl: './purchase-bill.component.html',
  styleUrl: './purchase-bill.component.css'
})
export class PurchaseBillComponent implements OnInit {
  // Tab management
  activeTab: string = 'details';
  
  // Form data
  selectedItem: string = '';
  selectedBatch: string = '';
  standardCost: number = 0;
  standardPrice: number = 0;
  quantity: number = 1;
  discount: number = 0;

  // Available options
  availableItems: string[] = ['Mango', 'Apple', 'Banana', 'Orange', 'Grapes', 'Kiwi', 'Strawberry'];
  availableBatches: any[] = [];

  // Data lists
  purchaseItems: PurchaseBillItem[] = [];
  summary: PurchaseBillSummary = {
    TotalItems: 0,
    TotalQuantity: 0,
    GrossTotal: 0,
    TotalDiscount: 0,
    NetTotal: 0
  };

  constructor(
    private databaseService: DatabaseService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Check if user is authenticated
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login']);
      return;
    }

    this.loadBatches();
    this.loadPurchaseItems();
    this.loadSummary();
  }
  loadBatches(): void {
    this.databaseService.getLocations().subscribe({
      next: (locations) => {
        console.log('Loaded locations:', locations);
        this.availableBatches = locations;
      },
      error: (error) => {
        console.error('Error loading batches:', error);
      }
    });
  }

  loadPurchaseItems(): void {
    this.databaseService.getPurchaseBillItems().subscribe({
      next: (items) => {
        this.purchaseItems = items;
      },
      error: (error) => {
        console.error('Error loading purchase items:', error);
      }
    });
  }

  loadSummary(): void {
    this.databaseService.getPurchaseBillSummary().subscribe({
      next: (summary) => {
        this.summary = summary;
      },
      error: (error) => {
        console.error('Error loading summary:', error);
      }
    });
  }

  calculateTotals(): void {
    if (this.standardCost && this.quantity) {
      // Total Cost = (Standard Cost × Quantity) – Discount%
      const grossCost = this.standardCost * this.quantity;
      const discountAmount = grossCost * (this.discount / 100);
      const totalCost = grossCost - discountAmount;
      
      // Total Selling = Standard Price × Quantity
      const totalSelling = this.standardPrice * this.quantity;
      
      // Update display (these would be shown in the UI)
      console.log('Total Cost:', totalCost);
      console.log('Total Selling:', totalSelling);
    }
  }

  onStandardCostChange(): void {
    this.calculateTotals();
  }

  onStandardPriceChange(): void {
    this.calculateTotals();
  }

  onQuantityChange(): void {
    this.calculateTotals();
  }

  onDiscountChange(): void {
    this.calculateTotals();
  }  addItem(): void {
    console.log('Selected batch:', this.selectedBatch);
    console.log('Available batches:', this.availableBatches);
    
    if (!this.selectedItem || !this.selectedBatch) {
      alert('Please select both Item and Batch');
      return;
    }
    
    if (this.standardCost <= 0) {
      alert('Standard Cost must be greater than 0');
      return;
    }
    
    if (this.standardPrice <= 0) {
      alert('Standard Price must be greater than 0');
      return;
    }
    
    if (this.quantity <= 0) {
      alert('Quantity must be greater than 0');
      return;
    }

    // Calculate totals
    const totalCost = (this.standardCost * this.quantity) * (1 - this.discount / 100);
    const totalSelling = this.standardPrice * this.quantity;    const item: PurchaseBillItem = {
      Item: this.selectedItem,
      Batch: this.selectedBatch,
      StandardCost: this.standardCost,
      StandardPrice: this.standardPrice,
      Quantity: this.quantity,
      Discount: this.discount,
      TotalCost: totalCost,
      TotalSelling: totalSelling
    };

    console.log('Adding item:', item);

    this.databaseService.addPurchaseBillItem(item).subscribe({
      next: (response) => {
        console.log('Item added successfully:', response);
        this.resetForm();
        this.loadPurchaseItems();
        this.loadSummary();
      },
      error: (error) => {
        console.error('Error adding item:', error);
        alert('Error adding item. Please try again.');
      }
    });
  }

  resetForm(): void {
    this.selectedItem = '';
    this.selectedBatch = '';
    this.standardCost = 0;
    this.standardPrice = 0;
    this.quantity = 1;
    this.discount = 0;
  }

  clearAllItems(): void {
    if (confirm('Are you sure you want to clear all items?')) {
      this.databaseService.clearPurchaseBillItems().subscribe({
        next: () => {
          this.loadPurchaseItems();
          this.loadSummary();
        },
        error: (error) => {
          console.error('Error clearing items:', error);
        }
      });
    }
  }
  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
  setActiveTab(tab: string): void {
    this.activeTab = tab;
  }

  getCurrentDate(): string {
    return new Date().toISOString().split('T')[0];
  }
}
