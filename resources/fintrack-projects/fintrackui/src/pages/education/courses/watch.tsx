import React, { useEffect, useState, useRef, useCallback } from "react";
import {
  Row,
  Col,
  Card,
  Tree,
  Descriptions,
  Typography,
  Skeleton,
  message,
  Spin,
} from "antd";
import { ListButton, Show } from "@refinedev/antd";
import { useApiUrl, useCustom, useGo, useOne, useParsed, useShow } from "@refinedev/core";
import ReactPlayer from "react-player";
import { ICourse, ICourseLesson } from "./types";

const { Title } = Typography;
const { DirectoryTree } = Tree;

export const WatchCourse: React.FC = () => {
  const { id } = useParsed();
  const go = useGo();
  const apiUrl = useApiUrl();

  // === State management ===
  const [selectedLesson, setSelectedLesson] = useState<ICourseLesson>();
  const [loadingVideo, setLoadingVideo] = useState(false);
  const [loadingLessonTree, setLoadingLessonTree] = useState(true);
  const [expandedKeys, setExpandedKeys] = useState<string[]>([]);

  // === Load course and lessons ===
  const { query: { isLoading: courseIsLoading, data } } = useShow<ICourse>({
    resource: "course",
    id,
    meta: { headers: { "x-include-lessons": "true" } },
  });

  const course = data?.data;
  const lessons = course?.modules ?? [];

  const { query: { data: progData, isLoading: progIsLoading } } = useCustom({
    method: "get",
    url: `${apiUrl}/progress/${course?.id}/lessons`,
    queryOptions: {
      enabled: !!course?.id,
    }
  });

  const progress: any = progData?.data;

  useEffect(() => {
    // wait until lessons are loaded and progress is defined
    if (!lessons.length || progress === undefined) return;

    // only run this logic when there is no progress yet
    if (progress.length === 0) {
      const firstSection = lessons[0];
      const firstLesson = firstSection?.children?.[0];

      if (!firstSection || !firstLesson) return;

      const newExpandedKeys = [firstSection.id];
      setExpandedKeys(newExpandedKeys);
      setSelectedLesson(firstLesson);
    } else {
      
    }
    
    setLoadingLessonTree(false);
  }, [progress]);


  // === Handle user lesson selection from the tree ===
  const onSelect = (_keys: React.Key[], info: any) => {
    if (info.node.videoUrl && info.node.key !== selectedLesson!.id) {
      setSelectedLesson(info.node);
      setLoadingVideo(true);
    }
  };

  const handleVideoReady = () => setTimeout(() => {setLoadingVideo(false)}, 500);

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
                  treeData={lessons.map(section => ({
                    title: `${section.order}. ${section.title}`,
                    key: section.id,
                    icon: "@",
                    children:
                      section.children?.map(lesson => ({
                        title: `${section.order}.${lesson.order} - ${lesson.title}`,
                        key: lesson.id,
                        videoUrl: lesson.videoUrl,
                      })) ?? [],
                  }))}
                  onSelect={onSelect}
                  defaultExpandedKeys={expandedKeys}
                  selectedKeys={selectedLesson ? [selectedLesson.id] : []}
                />
            }
          </Card>
        </Col>

        {/* Main Content: Video player + Course Info */}
        <Col xs={24} md={18}>
          <Card title={<Title level={4}>{selectedLesson?.title}</Title>}>
            <div
              style={{
                position: "relative",
                width: "100%",
                aspectRatio: "16 / 9", // replaces paddingTop trick
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
              <ReactPlayer
                src={selectedLesson?.videoUrl}
                controls
                width="100%"
                height="100%"
                onReady={handleVideoReady}
                style={{
                  position: "absolute",
                  inset: 0,
                  opacity: loadingVideo ? 0 : 1,
                  transition: "opacity 0.3s ease",
                }}
              />
            </div>

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
