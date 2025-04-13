import { Component } from '@angular/core';
import { EmployeeProjectsService } from '../../services/employee-projects.service';

@Component({
  selector: 'app-csv-import',
  templateUrl: './csv-import.component.html',
  styleUrls: ['./csv-import.component.css']
})
export class CsvImportComponent {
  selectedFile: File | null = null;
  message: string = '';

  constructor(private employeeProjectsService: EmployeeProjectsService) {}

  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0];
  }

  onImport(): void {
    if (!this.selectedFile) {
      this.message = 'Please select a CSV file to upload.';
      return;
    }

    const formData = new FormData();
    formData.append('file', this.selectedFile, this.selectedFile.name);

    this.employeeProjectsService.importCsv(formData).subscribe(
      (response) => {
        this.message = 'CSV Imported successfully!';
      },
      (error) => {
        this.message = 'Error importing CSV!';
      }
    );
  }
}
