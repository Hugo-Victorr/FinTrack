import React, { useEffect, useState } from "react";
import { Row, Col, Card, Tree, Descriptions, Typography, Progress, Skeleton, message } from "antd";
import { CheckCircleFilled } from "@ant-design/icons";
import ReactPlayer from "react-player";

const { Title } = Typography;

const course = {
  id: "java-backend",
  title: "Advanced Java Backend Development",
  description: "Learn how to build scalable backends using Spring Boot, Kafka, and Redis.",
  instructor: "Luiz Silva",
  duration: "8h 25m",
  level: "Intermediate",
  lessons: [
    {
      title: "Introduction",
      key: "0",
      children: [
        { title: "Welcome to the course", key: "0-0", url: "https://www.youtube.com/watch?v=dQw4w9WgXcQ" },
        { title: "Setting up your environment", key: "0-1", url: "https://www.youtube.com/watch?v=ysz5S6PUM-U" },
      ],
    },
    {
      title: "Core Concepts",
      key: "1",
      children: [
        { title: "Dependency Injection", key: "1-0", url: "https://www.youtube.com/watch?v=9bZkp7q19f0" },
        { title: "RESTful APIs", key: "1-1", url: "https://www.youtube.com/watch?v=ysz5S6PUM-U" },
      ],
    },
  ],
};

// Utility — flatten lessons for easier navigation
const flattenLessons = (sections: any[]) =>
  sections.flatMap(section => section.children.map((l: any) => ({ ...l, section: section.title })));

export const WatchCourse: React.FC = () => {
  const lessons = flattenLessons(course.lessons);

  // Restore last lesson from localStorage
  const savedKey = localStorage.getItem(`lastLesson_${course.id}`);
  const [selectedLesson, setSelectedLesson] = useState(
    lessons.find(l => l.key === savedKey) || lessons[0]
  );

  const [completedKeys, setCompletedKeys] = useState<string[]>(
    JSON.parse(localStorage.getItem(`completed_${course.id}`) || "[]")
  );

  const [loadingVideo, setLoadingVideo] = useState(false);

  const onSelect = (_keys: React.Key[], info: any) => {
    if (info.node.url) {
      setSelectedLesson(info.node);
      localStorage.setItem(`lastLesson_${course.id}`, info.node.key);
      setLoadingVideo(true);
    }
  };

  const handleVideoReady = () => {
    setLoadingVideo(false);
  };

  const handleVideoEnd = () => {
    // Mark as completed
    if (!completedKeys.includes(selectedLesson.key)) {
      const updated = [...completedKeys, selectedLesson.key];
      setCompletedKeys(updated);
      localStorage.setItem(`completed_${course.id}`, JSON.stringify(updated));
      message.success(`Lesson "${selectedLesson.title}" completed!`);
    }

    // Auto-play next
    const currentIndex = lessons.findIndex(l => l.key === selectedLesson.key);
    const next = lessons[currentIndex + 1];
    if (next) {
      setSelectedLesson(next);
      localStorage.setItem(`lastLesson_${course.id}`, next.key);
      setLoadingVideo(true);
    } else {
      message.success("🎉 You've completed all lessons!");
    }
  };

  const progress = Math.round((completedKeys.length / lessons.length) * 100);

  return (
    <div style={{ padding: 24 }}>
      <Title level={3} style={{ marginBottom: 16 }}>
        {course.title}
      </Title>

      <Row gutter={[16, 16]}>
        {/* Sidebar: Lesson Tree */}
        <Col xs={24} md={6}>
          <Card
            title={
              <>
                Lessons
                <Progress
                  percent={progress}
                  size="small"
                  status={progress === 100 ? "success" : "active"}
                  style={{ marginTop: 8 }}
                />
              </>
            }
            bordered
          >
            <Tree
              treeData={course.lessons.map(section => ({
                title: section.title,
                key: section.key,
                children: section.children.map(lesson => ({
                  title: (
                    <>
                      {completedKeys.includes(lesson.key) && (
                        <CheckCircleFilled style={{ color: "#52c41a", marginRight: 6 }} />
                      )}
                      {lesson.title}
                    </>
                  ),
                  key: lesson.key,
                  url: lesson.url,
                })),
              }))}
              defaultExpandAll
              onSelect={onSelect}
              selectedKeys={[selectedLesson.key]}
            />
          </Card>
        </Col>

        {/* Main content: Video + Metadata */}
        <Col xs={24} md={18}>
          <Card
            title={<Title level={4}>{selectedLesson.title}</Title>}
            bodyStyle={{ padding: 0, minHeight: 360 }}
          >
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
                url={selectedLesson.url}
                controls
                width="100%"
                height="100%"
                playing
                onReady={handleVideoReady}
                onEnded={handleVideoEnd}
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

          <Card style={{ marginTop: 16 }} title="Course Information">
            <Descriptions bordered column={1} size="small">
              <Descriptions.Item label="Instructor">{course.instructor}</Descriptions.Item>
              <Descriptions.Item label="Level">{course.level}</Descriptions.Item>
              <Descriptions.Item label="Duration">{course.duration}</Descriptions.Item>
              <Descriptions.Item label="Description">{course.description}</Descriptions.Item>
            </Descriptions>
          </Card>
        </Col>
      </Row>
    </div>
  );
};
