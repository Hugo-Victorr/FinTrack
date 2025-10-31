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
  courseContent: ICourseLesson[];
}

export interface ICourseLesson {
  title: string;
  key: string; // unique string key for React & tracking
  videoUrl: string;
  children?: ICourseLesson[];
}

