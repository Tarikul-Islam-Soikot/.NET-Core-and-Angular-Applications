import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, Subject, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';


@Injectable({
  providedIn: 'root',
})


@Injectable()
export class ApiService {

  public baseUri = 'https://localhost:44392/api/';

  constructor(public httpClient: HttpClient) {
  }

  public httpGet<T>(url: string): Observable<T> {
    return this.httpClient.get<T>(this.baseUri + url)
      .pipe(
        catchError(this.handleClientError)
      );
  }

  public handleClientError(error: HttpErrorResponse) {
    return throwError(error.message || 'An error has been occurred. Please try again or contact to system administrator');
  }
}


