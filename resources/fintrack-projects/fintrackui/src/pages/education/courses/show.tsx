import React, { useMemo } from "react";
import { Card, Row, Col, Button, Typography, Descriptions, Progress, Space } from "antd";
import { PlayCircleOutlined, ArrowRightOutlined } from "@ant-design/icons";
import { useOne, useShow } from "@refinedev/core";
// import { useNavigate } from "react-router-dom";

const { Title, Paragraph } = Typography;

const course = {
  id: "java-backend",
  title: "Advanced Java Backend Development",
  thumbnail:
    "https://images.unsplash.com/photo-1553484771-371a605b060b?auto=format&fit=crop&w=1000&q=80",
  description:
    "Learn how to build scalable backends using Spring Boot, Kafka, and Redis. This course covers design patterns, event-driven architectures, and distributed systems best practices.",
  instructor: "Luiz Silva",
  duration: "8h 25m",
  level: "Intermediate",
  lessons: [
    {
      title: "Introduction",
      key: "0",
      children: [
        { title: "Welcome to the course", key: "0-0" },
        { title: "Setting up your environment", key: "0-1" },
      ],
    },
    {
      title: "Core Concepts",
      key: "1",
      children: [
        { title: "Dependency Injection", key: "1-0" },
        { title: "RESTful APIs", key: "1-1" },
      ],
    },
  ],
};

const flattenLessons = (sections: any[]) =>
  sections.flatMap(section => section.children.map((l: any) => ({ ...l, section: section.title })));

export const ShowCourse: React.FC = () => {
  // const navigate = useNavigate();
  const lessons = useMemo(() => flattenLessons(course.lessons), []);

  const { query } = useShow({});

  const { data, isLoading } = query;

  const record = data?.data;

  const {
    result: category,
    query: { isLoading: categoryIsLoading },
  } = useOne({
    resource: "categories",
    id: record?.category?.id || "",
    queryOptions: {
      enabled: !!record,
    },
  });

  // Retrieve progress data from localStorage
  const completed = JSON.parse(localStorage.getItem(`completed_${course.id}`) || "[]");
  const progress = Math.round((completed.length / lessons.length) * 100);
  const hasProgress = progress > 0;
  const lastLessonKey = localStorage.getItem(`lastLesson_${course.id}`);

  const handleStartOrContinue = () => {
    // navigate(`/courses/${course.id}/watch`);
  };

  return (
    <div style={{ padding: 24 }}>
      <Row gutter={[32, 32]} align="middle">
        {/* Left side: Thumbnail */}
        <Col xs={24} md={10}>
          <Card
            cover={<img alt={course.title}
              src={course.thumbnail}
              style={{ objectFit: "cover" }} />}
          />
        </Col>

        {/* Right side: Details */}
        <Col xs={24} md={14}>
          <Space direction="vertical" size="middle" style={{ width: "100%" }}>
            <Title level={2}>{course.title}</Title>
            <Paragraph>{course.description}</Paragraph>

            <Descriptions column={1} bordered size="small">
              <Descriptions.Item label="Instructor">{course.instructor}</Descriptions.Item>
              <Descriptions.Item label="Level">{course.level}</Descriptions.Item>
              <Descriptions.Item label="Duration">{course.duration}</Descriptions.Item>
              <Descriptions.Item label="Lessons">{lessons.length}</Descriptions.Item>
            </Descriptions>

            {hasProgress && (
              <Progress
                percent={progress}
                status={progress === 100 ? "success" : "active"}
                size="default"
              />
            )}

            <Button
              type="primary"
              size="large"
              icon={hasProgress ? <ArrowRightOutlined /> : <PlayCircleOutlined />}
              onClick={handleStartOrContinue}
            >
              {hasProgress
                ? `Continue ${progress === 100 ? "Reviewing" : "Watching"}`
                : "Start Course"}
            </Button>

            {lastLessonKey && (
              <Paragraph type="secondary" style={{ marginTop: 8 }}>
                Last lesson watched: <b>{lessons.find(l => l.key === lastLessonKey)?.title}</b>
              </Paragraph>
            )}
          </Space>
        </Col>
      </Row>
    </div>
  );
};
