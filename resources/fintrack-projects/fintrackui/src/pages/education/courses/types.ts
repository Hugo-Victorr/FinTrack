export interface ICourseCategory {
  id: string;
  name: string;
}

export interface ICourse {
  id: string;
  title: string;
  description?: string;
  aims?: string;
  thumbnailUrl?: string;
  instructor: string;
  level: number;
  durationMinutes: number;
  category: ICourseCategory;
}
