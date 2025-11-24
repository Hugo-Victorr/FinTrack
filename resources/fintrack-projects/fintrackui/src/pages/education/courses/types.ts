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
  modules?: ICourseModule[];
}

export interface ICourseModule {
  title: string;
  order: number;
  id: string;
  lessons?: ICourseLesson[];
}

export interface ICourseLesson {
  title: string;
  order: number;
  id: string; // unique string key for React & tracking
  videoUrl?: string;
}

