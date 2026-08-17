export interface Track {
  id: number;
  title: string;
  artistId: number;
  artistName: string;
  isrc: string;
  releaseDate: string;
  genre: string;
  status: string;
  distributions?: TrackDistribution[];
}

export interface TrackDistribution {
  id: number;
  dspId: number;
  dspName: string;
  submittedAt: string;
  status: string;
}

export interface DSP {
  id: number;
  name: string;
}
