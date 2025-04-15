import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { EmployeeProjectsService } from '../../services/employee-projects.service';

@Component({
  standalone: false,
  selector: 'app-employee-projects',
  templateUrl: './employee-projects.component.html',
  styleUrls: ['./employee-projects.component.scss']
})
export class EmployeeProjectsComponent implements OnInit {
  displayedColumns: string[] = ['empID1', 'empID2', 'totalDaysWorkedTogether', 'projects'];
  dataSource = new MatTableDataSource<any>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private service: EmployeeProjectsService) { }

  ngOnInit(): void {
    this.service.getProjects().subscribe(data => {
      this.dataSource.data = [data];
    });
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  applyFilter(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  getProjectIdsAsString(projects: any[]): string {
    return projects?.map(p => p.projectID).join(', ') || '';
  }
}
