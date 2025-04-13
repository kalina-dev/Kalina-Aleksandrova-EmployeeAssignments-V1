import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { EmployeeProjectsService } from '../../services/employee-projects.service';

@Component({
  selector: 'app-employee-projects',
  templateUrl: './employee-projects.component.html'
})
export class EmployeeProjectsComponent implements OnInit {
  displayedColumns: string[] = ['EmpID', 'ProjectID', 'DateFrom', 'DateTo'];
  dataSource = new MatTableDataSource<any>();
  error = '';
  startDate?: Date;
  endDate?: Date;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private service: EmployeeProjectsService) {}

  ngOnInit(): void {
    this.loadProjects();
  }

  loadProjects(): void {
    this.service.getProjects().subscribe({
      next: data => {
        this.dataSource = new MatTableDataSource(data);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      },
      error: err => {
        this.error = 'Failed to load data. ' + err.message;
      }
    });
  }

  applyDateFilter(): void {
    if (this.startDate && this.endDate && this.startDate > this.endDate) {
      this.error = 'Start date must be before end date.';
      return;
    }
    // Optionally implement filter using API or local filtering
  }
}
