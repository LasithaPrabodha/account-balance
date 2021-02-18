import { environment } from './../../environments/environment';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class EnvironmentService {
  public urlAddress: string = environment.urlAddress;
}
