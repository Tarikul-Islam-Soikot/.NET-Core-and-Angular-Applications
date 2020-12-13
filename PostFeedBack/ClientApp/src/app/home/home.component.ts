import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { BsModalService } from 'ngx-bootstrap/modal';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { PostDto } from './Response'
import { Subscription } from 'rxjs';
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

  public postList: PostDto[];

  constructor(public apiService: ApiService,
    public notificationService: NotificationService) {

  }

  ngOnInit() {

  }

  public search(): void {

    this.subscribers.getAllPost
      = this.apiService.httpGet<PostDto[]>('Post/GetAllPost')
        .subscribe(
          (x) => { this.postList = x; },
          (error) => {
            this.notificationService.showError(error);
            console.log(error);
          },
          () => {
            if (this.postList !== null && this.postList !== undefined)
              this.loadGrid();
          });
  }

  public loadGrid(): void {
    this.gridView = {
      data: this.postList.slice(this.skip, this.skip + this.pageSize),
      total: this.postList.length
    };
  }

  public pageChange(event: PageChangeEvent): void {
    this.skip = event.skip;
    this.loadGrid();
  }

}

