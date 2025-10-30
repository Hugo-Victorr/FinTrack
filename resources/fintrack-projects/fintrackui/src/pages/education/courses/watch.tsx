import React, { useEffect, useState } from "react";
import {
  Row,
  Col,
  Card,
  Tree,
  Descriptions,
  Typography,
  Skeleton,
  message,
} from "antd";
import { ListButton, Show } from "@refinedev/antd";
import { useGo, useParsed, useShow } from "@refinedev/core";
import ReactPlayer from "react-player";

const { Title } = Typography;

interface ICourseLesson {
  title: string;
  key: string; // use string for React keys (more consistent)
  videoUrl: string;
  children?: ICourseLesson[];
}

interface ICourse {
  id: string;
  title: string;
  description?: string;
  aims?: string;
  thumbnailUrl?: string;
  instructor: string;
  level: number;
  durationMinutes: number;
  courseContent: ICourseLesson[];
}

export const WatchCourse: React.FC = () => {
  const { id } = useParsed();
  const go = useGo();

  const { query: queryResult } = useShow<ICourse>({
    resource: "course",
    id,
    meta: { headers: { "x-include-headers": "true" } },
  });

  const { isLoading, data } = queryResult;
  const course = data?.data;

  const lessons = course?.courseContent ?? [];

  // Restore last lesson
  const [selectedLesson, setSelectedLesson] = useState<ICourseLesson | null>(null);
  const [loadingVideo, setLoadingVideo] = useState(false);

  useEffect(() => {
    if (!course) return;
    const savedKey = localStorage.getItem(`lastLesson_${course.id}`);
    const firstLesson = lessons.flatMap(l => [l, ...(l.children ?? [])])[0];

    if (savedKey) {
      const found = lessons
        .flatMap(section => [section, ...(section.children ?? [])])
        .find(lesson => lesson.key === savedKey);
      setSelectedLesson(found || firstLesson);
    } else {
      setSelectedLesson(firstLesson);
    }
  }, [course]);

  const onSelect = (_keys: React.Key[], info: any) => {
    console.log(info)
    console.log(_keys)
    
    if (info.node.videoUrl) {
      setSelectedLesson(info.node);
      localStorage.setItem(`lastLesson_${course?.id}`, info.node.key);
      setLoadingVideo(true);
    }
  };

  const handleVideoReady = () => setLoadingVideo(false);

  // const handleVideoEnd = () => {
  //   message.success(`Lesson "${selectedLesson?.title}" completed!`);
  // };

  return (
    <Show
      isLoading={isLoading}
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
        {/* Sidebar: Lessons */}
        <Col xs={24} md={6}>
          <Card title="Lessons" bordered>
            <Tree
              treeData={lessons.map(section => ({
                title: section.title,
                key: section.key,
                children:
                  section.children?.map(lesson => ({
                    title: lesson.title,
                    key: lesson.key,
                    videoUrl: lesson.videoUrl,
                  })) ?? [],
              }))}
              defaultExpandAll
              onSelect={onSelect}
              selectedKeys={selectedLesson ? [selectedLesson.key] : []}
            />
          </Card>
        </Col>

        {/* Main content: Video + Metadata */}
        <Col xs={24} md={18}>
          <Card title={<Title level={4}>{selectedLesson?.title}</Title>}>
            <div style={{ position: "relative", paddingTop: "56.25%" }}>
              {loadingVideo && (
                <Skeleton.Node
                  active
                  style={{
                    width: "100%",
                    height: "100%",
                    position: "absolute",
                    top: 0,
                    left: 0,
                  }}
                />
              )}
              <ReactPlayer
                src={selectedLesson?.videoUrl}
                controls
                width="100%"
                height="100%"
                playing
                onReady={handleVideoReady}
                // onEnded={handleVideoEnd}
                style={{
                  position: "absolute",
                  top: 0,
                  left: 0,
                  opacity: loadingVideo ? 0 : 1,
                  transition: "opacity 0.3s",
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
