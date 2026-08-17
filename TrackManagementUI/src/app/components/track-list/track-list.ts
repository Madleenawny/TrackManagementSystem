import { Component, OnInit, inject, signal } from '@angular/core'; // 👈 أضيفي signal
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { TrackService } from '../../services/track.service';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-track-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './track-list.html',
  styleUrl: './track-list.css'
})
export class TrackListComponent implements OnInit {
  private trackService = inject(TrackService);

  tracks = signal<any[]>([]);

  private authService = inject(AuthService);
  private router = inject(Router);


  ngOnInit(): void {
    this.loadTracks();
  }

  loadTracks(): void {
    this.trackService.getTracks().subscribe({
      next: (data: any) => {
        console.log('=== DATA RECEIVED FROM API ===', data);
        this.tracks.set(data); // 👈 تحديث الـ Signal
      },
      error: (err) => console.error('API Error:', err)
    });
  }

  onCreateTrack(): void {
    // Logic for create
  }
  onLogout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}

