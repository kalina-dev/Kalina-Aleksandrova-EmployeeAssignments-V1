import { Component, OnInit } from '@angular/core';
import { EmployeeProjectsService } from '../../services/employee-projects.service';
import { Moment } from 'moment';  // If you want moment.js for date formatting

@Component({
  selector: 'app-employee-projects',
  templateUrl: './employee-projects.component.html',
  styleUrls: ['./employee-projects.component.css']
})
export class EmployeeProjectsComponent implements OnInit {
  employeeProjects: any[] = [];

  constructor(private employeeProjectsService: EmployeeProjectsService) {}

  ngOnInit(): void {
    this.fetchEmployeeProjects();
  }

  fetchEmployeeProjects(): void {
    this.employeeProjectsService.getEmployeeProjects().subscribe(
      (data) => {
        this.employeeProjects = data;
      },
      (error) => {
        console.error('Error fetching employee projects:', error);
      }
    );
  }

  formatDate(date: string): string {
    // Format the date using moment.js (you can use date-fns too if you prefer)
    return new Date(date).toLocaleDateString(); // Simple JavaScript date formatting
  }
}
