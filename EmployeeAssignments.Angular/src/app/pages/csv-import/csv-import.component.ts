import { Component } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-csv-import',
  templateUrl: './csv-import.component.html'
})
export class CsvImportComponent {
  file: File | null = null;
  error = '';
  success = '';

  constructor(private http: HttpClient) {}

  onFileSelected(event: any): void {
    this.file = event.target.files[0];
    this.error = '';
    this.success = '';
  }

  onUpload(): void {
    if (!this.file) {
      this.error = 'Please select a CSV file.';
      return;
    }

    const formData = new FormData();
    formData.append('file', this.file);

    this.http.post('/api/employeeprojects/import', formData).subscribe({
      next: () => this.success = 'File uploaded successfully.',
      error: (err: HttpErrorResponse) => this.error = `Upload failed: ${err.message}`
    });
  }
}
