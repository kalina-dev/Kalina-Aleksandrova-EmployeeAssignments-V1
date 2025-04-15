import { Component } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

@Component({
  standalone: false,
  selector: 'app-csv-import',
  templateUrl: './csv-import.component.html',
  styleUrls: ['./csv-import.component.scss']
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

    this.http.post('/import', formData).subscribe({
      next: () => {
        this.success = 'File uploaded successfully.';
        this.error = '';
      },
      error: (err: HttpErrorResponse) => {
        this.error = `Upload failed: ${err.message}`;
        this.success = '';
      }
    });
  }
}
