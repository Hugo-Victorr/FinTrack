import React, { useEffect, useState, useRef } from "react";
import {
  Row,
  Col,
  Card,
  Tree,
  Descriptions,
  Typography,
  Skeleton,
  Spin,
} from "antd";
import { ListButton, Show } from "@refinedev/antd";
import { useApiUrl, useCustom, useGo, useParsed, useShow } from "@refinedev/core";
import ReactPlayer from "react-player";
import axios from "axios";
import { ICourse, ICourseLesson } from "./types";
import type { DataNode } from "antd/es/tree";
import { axiosInstance } from "../../../providers/rest-data-provider/utils";

const { Title } = Typography;
const { DirectoryTree } = Tree;

interface ProgressData {
  lessonId: string;
  currentTimestamp: number;
  progressPercentage: number;
}

export const WatchCourse: React.FC = () => {
  const { id } = useParsed();
  const go = useGo();
  const apiUrl = useApiUrl();
  const progressUpdateTimeoutRef = useRef<NodeJS.Timeout | null>(null);

  // === State management ===
  const [selectedLesson, setSelectedLesson] = useState<ICourseLesson>();
  const [loadingVideo, setLoadingVideo] = useState(false);
  const [loadingLessonTree, setLoadingLessonTree] = useState(true);
  const [expandedKeys, setExpandedKeys] = useState<React.Key[]>([]);
  const [presignedUrl, setPresignedUrl] = useState<string | null>(null);

  // === Load course and lessons ===
  const { query: { isLoading: courseIsLoading, data } } = useShow<ICourse>({
    resource: "course",
    id,
    meta: { headers: { "x-include-lessons": "true" } },
  });

  const course = data?.data;
  const modules = React.useMemo(() => course?.modules ?? [], [course?.modules]);

  const { query: { data: progData, isLoading: progIsLoading } } = useCustom<ProgressData[]>({
    method: "get",
    url: `${apiUrl}/progress/${course?.id}/lessons`,
    queryOptions: {
      enabled: !!course?.id,
    }
  });

  const progress: ProgressData[] = React.useMemo(() => 
    Array.isArray(progData?.data) ? progData.data : [], 
    [progData?.data]
  );

  // === Fetch presigned URL for video playback ===
  const fetchPresignedUrl = React.useCallback(async (videoKey: string): Promise<string | null> => {
    if (!videoKey) return null;
    
    try {
      const response = await axiosInstance.get<{ url: string }>(
        `${apiUrl}/course/play?key=${encodeURIComponent(videoKey)}`,
        {
          headers: {
            Authorization: axiosInstance.defaults.headers.common.Authorization,
          },
        }
      );
      
      return response.data?.url || null;
    } catch (error) {
      console.error("Failed to fetch presigned URL:", error);
      return null;
    }
  }, [apiUrl]);

  // === Initialize lesson selection based on progress ===
  useEffect(() => {
    if (!modules.length || progData === undefined) return;

    setLoadingLessonTree(false);

    const initializeLesson = async () => {
      // If there's progress, find the last watched lesson or next unwatched lesson
      if (progress.length > 0) {
        // Find the last lesson with progress
        const lastProgress = progress.reduce((prev, curr) => 
          curr.progressPercentage > prev.progressPercentage ? curr : prev
        );
        
        // Find the lesson in the modules
        for (const module of modules) {
          const lesson = module.lessons?.find(l => l.id === lastProgress.lessonId);
          if (lesson) {
            setSelectedLesson(lesson);
            setExpandedKeys([module.id]);
            if (lesson.videoUrl) {
              const url = await fetchPresignedUrl(lesson.videoUrl);
              setPresignedUrl(url);
            }
            return;
          }
        }
      }

      // No progress - select first lesson
      const firstModule = modules[0];
      const firstLesson = firstModule?.lessons?.[0];

      if (firstModule && firstLesson) {
        setSelectedLesson(firstLesson);
        setExpandedKeys([firstModule.id]);
        if (firstLesson.videoUrl) {
          const url = await fetchPresignedUrl(firstLesson.videoUrl);
          setPresignedUrl(url);
        }
      }
    };

    initializeLesson();
  }, [modules, progData, progress, fetchPresignedUrl]);

  // === Handle user lesson selection from the tree ===
  const onSelect = async (_keys: React.Key[], info: { node: DataNode }) => {
    const node = info.node as DataNode & { videoUrl?: string };
    
    // Only handle lesson selection (lessons have videoUrl)
    if (node.videoUrl && node.key !== selectedLesson?.id) {
      setLoadingVideo(true);
      
      // Find the module and lesson
      const module = modules.find(m => 
        m.lessons?.some(l => l.id === node.key)
      );
      const lesson = module?.lessons?.find(l => l.id === node.key);
      
      if (lesson) {
        setSelectedLesson(lesson);
        
        // Fetch presigned URL
        const url = await fetchPresignedUrl(lesson.videoUrl!);
        setPresignedUrl(url);
      }
    }
  };

  // === Handle video progress updates ===
  const handleProgress = React.useCallback((state: { playedSeconds: number; played: number }) => {
    if (!selectedLesson) return;
    
    const currentTimestamp = Math.floor(state.playedSeconds);
    
    // Debounce progress updates - only update every 5 seconds
    if (progressUpdateTimeoutRef.current) {
      clearTimeout(progressUpdateTimeoutRef.current);
    }
    
    progressUpdateTimeoutRef.current = setTimeout(async () => {
      try {
        await axios.post(
          `${apiUrl}/progress`,
          {
            lessonId: selectedLesson.id,
            currentTimestamp: currentTimestamp,
          },
          {
            headers: {
              Authorization: axios.defaults.headers.common.Authorization,
            }
          }
        );
      } catch (error) {
        console.error("Failed to update progress:", error);
      }
    }, 5000);
  }, [selectedLesson, apiUrl]);

  const handleVideoReady = () => {
    setTimeout(() => {
      setLoadingVideo(false);
    }, 500);
  };

  // Cleanup timeout on unmount
  useEffect(() => {
    return () => {
      if (progressUpdateTimeoutRef.current) {
        clearTimeout(progressUpdateTimeoutRef.current);
      }
    };
  }, []);

  return (
    <Show
      isLoading={courseIsLoading || progIsLoading}
      title={course?.title}
      headerButtons={({ listButtonProps }) => (
        <>
          {listButtonProps && <ListButton type="primary" {...listButtonProps} />}
        </>
      )}
      headerProps={{
        onBack: () => go({ to: { resource: "course", action: "list" } }),
      }}
    >
      <Row gutter={[16, 16]}>
        {/* Sidebar: Lessons list */}
        <Col xs={24} md={6}>
          <Card title="Lessons">
            {
              loadingLessonTree ?
                <Spin/>
                :
                <DirectoryTree
                  showIcon={false}
                  treeData={modules.map(module => ({
                    title: `${module.order}. ${module.title}`,
                    key: module.id,
                    children:
                      module.lessons?.map(lesson => ({
                        title: `${module.order}.${lesson.order} - ${lesson.title}`,
                        key: lesson.id,
                        videoUrl: lesson.videoUrl,
                        isLeaf: true,
                      })) ?? [],
                  }))}
                  onSelect={onSelect}
                  expandedKeys={expandedKeys}
                  onExpand={(keys) => setExpandedKeys(keys)}
                  selectedKeys={selectedLesson ? [selectedLesson.id] : []}
                />
            }
          </Card>
        </Col>

        {/* Main Content: Video player + Course Info */}
        <Col xs={24} md={18}>
          <Card title={<Title level={4}>{selectedLesson?.title || "Select a lesson"}</Title>}>
            {selectedLesson && presignedUrl ? (
              <div
                style={{
                  position: "relative",
                  width: "100%",
                  aspectRatio: "16 / 9",
                  overflow: "hidden",
                }}
              >
                {loadingVideo && (
                  <Skeleton.Node
                    active
                    style={{
                      width: "100%",
                      height: "100%",
                      position: "absolute",
                      inset: 0,
                    }}
                  />
                )}
                {/* Note: ReactPlayer type definitions may show errors but the component works correctly at runtime */}
                <ReactPlayer
                  url={presignedUrl}
                  controls
                  width="100%"
                  height="100%"
                  onReady={handleVideoReady}
                  onProgress={handleProgress as unknown as (state: { playedSeconds: number; played: number }) => void}
                  progressInterval={5000}
                  playing={false}
                  style={{
                    position: "absolute",
                    inset: 0,
                    opacity: loadingVideo ? 0 : 1,
                    transition: "opacity 0.3s ease",
                  }}
                />
              </div>
            ) : selectedLesson ? (
              <Spin tip="Loading video..." style={{ width: "100%", padding: "40px" }} />
            ) : (
              <div style={{ padding: "40px", textAlign: "center" }}>
                <Typography.Text type="secondary">Please select a lesson to start watching</Typography.Text>
              </div>
            )}
          </Card>

          <Card style={{ marginTop: 16 }} title="Information">
            <Descriptions bordered column={1} size="small">
              <Descriptions.Item label="Instructor">
                {course?.instructor}
              </Descriptions.Item>
              <Descriptions.Item label="Level">
                {course?.level}
              </Descriptions.Item>
              <Descriptions.Item label="Duration (min)">
                {course?.durationMinutes}
              </Descriptions.Item>
              <Descriptions.Item label="Description">
                {course?.description}
              </Descriptions.Item>
            </Descriptions>
          </Card>
        </Col>
      </Row>
    </Show>
  );
};
