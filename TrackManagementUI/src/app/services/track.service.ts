import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Track, DSP } from '../models/track.model';

@Injectable({
  providedIn: 'root'
})
export class TrackService {
  private apiUrl = 'http://localhost:5110/api';
  constructor(private http: HttpClient) { }

  getTracks(): Observable<Track[]> {
    return this.http.get<Track[]>(`${this.apiUrl}/tracks`);
  }

  getTrackById(id: number): Observable<Track> {
    return this.http.get<Track>(`${this.apiUrl}/tracks/${id}`);
  }

  getDsps(): Observable<DSP[]> {
    return this.http.get<DSP[]>(`${this.apiUrl}/dsps`);
  }

  distributeTrack(trackId: number, dspId: number): Observable<any> {
    const body = {
      dspIds: [Number(dspId)]
    };

    return this.http.post(`${this.apiUrl}/tracks/${trackId}/distribute`, body);
  }
}
