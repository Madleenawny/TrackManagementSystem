import { Component, OnInit, ChangeDetectorRef, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { TrackService } from '../../services/track.service';
import { Track, DSP } from '../../models/track.model';
import { forkJoin, catchError, of } from 'rxjs';

@Component({
  selector: 'app-track-detail',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './track-detail.html',
  styleUrl: './track-detail.css'
})
export class TrackDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private trackService = inject(TrackService);
  private cdr = inject(ChangeDetectorRef);

  trackId!: number;
  track?: Track;
  dsps: DSP[] = [];
  selectedDspId?: number;

  loading: boolean = true;
  errorMessage: string = '';
  successMessage: string = '';

  ngOnInit(): void {
    this.trackId = Number(this.route.snapshot.paramMap.get('id'));
    this.loadData();
  }

  loadData(): void {
    this.loading = true;
    this.errorMessage = '';

    forkJoin({
      trackData: this.trackService.getTrackById(this.trackId),
      dspsData: this.trackService.getDsps().pipe(
        catchError((err) => {
          console.warn('حدث خطأ أثناء جلب الـ DSPs:', err);
          return of([]);
        })
      )
    }).subscribe({
      next: (res) => {
        console.log('✅ تم جلب البيانات بنجاح:', res);
        this.track = res.trackData;
        this.dsps = res.dspsData;
        this.loading = false;

        // 🚀 إجبار Angular على تحديث الواجهة فوراً!
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error loading data:', err);
        this.errorMessage = 'فشل في تحميل البيانات.';
        this.loading = false;

        this.cdr.detectChanges();
      }
    });
  }

  onDistribute(): void {
    if (!this.selectedDspId) {
      alert('من فضلك اختاري منصة DSP أولاً');
      return;
    }

    this.errorMessage = '';
    this.successMessage = '';

    this.trackService.distributeTrack(this.trackId, Number(this.selectedDspId)).subscribe({
      next: (res) => {
        this.successMessage = 'تم توزيع الأغنية بنجاح! 🎉';
        this.loadData();
      },
      error: (err) => {
        if (err.status === 401) {
          this.errorMessage = '401 Unauthorized: يرجى إدخال JWT Token صحيح للقيام بالتوزيع.';
        } else {
          this.errorMessage = err.error?.message || 'حدث خطأ أثناء التوزيع.';
        }
        this.cdr.detectChanges();
      }
    });
  }
}
