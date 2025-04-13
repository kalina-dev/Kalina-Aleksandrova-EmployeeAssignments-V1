import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EmployeeProjectsService {
  private apiUrl = 'https://localhost:7003/api';  // Adjust with your backend API

  constructor(private http: HttpClient) {}

  // CSV Import endpoint
  importCsv(file: FormData): Observable<any> {
    return this.http.post(`${this.apiUrl}/import`, file, {
      headers: new HttpHeaders().set('Content-Type', 'multipart/form-data')
    });
  }

  // Get Employee-Project Data
  getEmployeeProjects(): Observable<any> {
    return this.http.get(`${this.apiUrl}/employeeprojects`);
  }
}
