//import { Component } from '@angular/core';

//@Component({
//  selector: 'app-home',
//  templateUrl: './home.component.html',
//})
//export class HomeComponent {
//}


import { Component, OnInit, TemplateRef, Inject } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators, NgForm, FormControl } from '@angular/forms';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { RowArgs, GridDataResult, PageChangeEvent, SelectableSettings } from '@progress/kendo-angular-grid';
import { PostDto, CommentsDto } from './Response'
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, Subscription } from 'rxjs';
import { ApiService } from '../app.api.service';
import { NotificationService } from '../app.notification.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styles: ['']
})

export class HomeComponent implements OnInit {

  public _getAllPostSubscriber$: Subscription;
  subscribers: any = {};
  public gridView: GridDataResult;
  public pageSize = 10;
  public skip = 0;

  public dataList: PostDto[];
  selectableSettings: SelectableSettings;

  constructor(public router: Router,
    public apiService: ApiService,
    public notificationService: NotificationService,
    public modalService: BsModalService,
    public route: ActivatedRoute,
    public formBuilder: FormBuilder,
    http: HttpClient) {

  }

  ngOnInit() {

  }

  public setSelectableSettings(): void {
    this.selectableSettings = {
      checkboxOnly: true,
      mode: 'single'
    };
  }


  public search(): void {

    this.subscribers.getAllPost
      = this.apiService.httpGet<PostDto[]>('Post/GetAllPost')
        .subscribe(
          (x) => { this.dataList = x; },
          (error) => {
            this.notificationService.showError(error);
            console.log(error);
          },
          () => {
            if (this.dataList !== null && this.dataList !== undefined)
              this.loadGrid();
          });
  }

  public loadGrid(): void {
    this.gridView = {
      data: this.dataList.slice(this.skip, this.skip + this.pageSize),
      total: this.dataList.length
    };
  }

  public pageChange(event: PageChangeEvent): void {
    this.skip = event.skip;
    this.loadGrid();
  }

}

