import React, { useEffect, useMemo, useState } from "react";
import { Card, Row, Col, Button, Typography, Descriptions, Progress, Space } from "antd";
import { PlayCircleOutlined, ArrowRightOutlined } from "@ant-design/icons";
import { useGo, useOne, useParsed, useShow } from "@refinedev/core";
import { ListButton, Show } from "@refinedev/antd";
import { CourseLevelTag } from "../../../components/course/levelTag";
import { ICourse } from "./types";

const { Title, Paragraph } = Typography;


const flattenLessons = (sections: any[]) =>
  sections.flatMap(section => section.children.map((l: any) => ({ ...l, section: section.title })));

function formatDuration(minutes: number | undefined): string {
  if (!minutes) {
    return "";
  }
  
  if (minutes < 0 || isNaN(minutes)) return "0m";

  const hours = Math.floor(minutes / 60);
  const mins = minutes % 60;

  if (hours === 0) return `${mins}m`;
  if (mins === 0) return `${hours}h`;
  return `${hours}h ${mins}m`;
}


export const ShowCourse: React.FC = () => {
  // const navigate = useNavigate();
  const { query: queryResult } = useShow<ICourse>();
  const { isLoading, data } = queryResult;
  const course = data?.data;
  
  const go = useGo();
  const {
    resource,
    id,
  } = useParsed();

  // const lessons = useMemo(() => flattenLessons(course.lessons), []);

  // Retrieve progress data from localStorage
  // const completed = JSON.parse(localStorage.getItem(`completed_${course.id}`) || "[]");
  // const progress = 0; //Math.round((completed.length / lessons.length) * 100);
  // const hasProgress = progress > 0;
  // const lastLessonKey = localStorage.getItem(`lastLesson_${course.id}`);

  const handleStartOrContinue = () => {
    go({
      to: `/${resource!.name}/watch/${id}`,
      type: "push",
    });
  };


  return (
    <Show 
      headerButtons={({
        listButtonProps,
      }) => (<>{listButtonProps && (<ListButton type="primary" {...listButtonProps} />)}</>)}
      resource="course" 
      isLoading={isLoading} 
      title={course?.title} 
      breadcrumb={null}>

      <div style={{ padding: 24 }}>

        <Row gutter={[32, 32]} align="middle">

          <Col xs={24} md={10}>
            <Card
              cover={<img alt={course?.title}
                src={course?.thumbnailUrl}
                style={{ objectFit: "cover" }} />}
            />
          </Col>

          <Col xs={24} md={14}>
            <Space direction="vertical" size="middle" style={{ width: "100%" }}>
              <Title level={2}>{course?.title}</Title>
              <Paragraph>{course?.description}</Paragraph>

              <Descriptions column={1} bordered size="small">
                {
                  course?.aims && (<Descriptions.Item label="Aims">{course?.aims}</Descriptions.Item>)
                }
                <Descriptions.Item label="Instructor">{course?.instructor}</Descriptions.Item>
                <Descriptions.Item label="Category">{course?.category.name}</Descriptions.Item>
                <Descriptions.Item label="Level">
                  <CourseLevelTag level={course?.level} />
                </Descriptions.Item>
                <Descriptions.Item label="Duration">{formatDuration(course?.durationMinutes)}</Descriptions.Item>
                <Descriptions.Item label="Lessons">{0}</Descriptions.Item>
              </Descriptions>

              {/* {hasProgress && (
                <Progress
                  percent={progress}
                  status={progress === 100 ? "success" : "active"}
                  size="default"
                />
              )} */}

              <Button
                type="primary"
                size="large"
                // icon={hasProgress ? <ArrowRightOutlined /> : <PlayCircleOutlined />}
                onClick={handleStartOrContinue}
              >
                {/* {hasProgress
                  ? `Continue ${progress === 100 ? "Reviewing" : "Watching"}`
                  : "Start Course"} */}
                Start Course
              </Button>

              {/* {lastLessonKey && (
                <Paragraph type="secondary" style={{ marginTop: 8 }}>
                  Last lesson watched: <b>{lessons.find(l => l.key === lastLessonKey)?.title}</b>
                </Paragraph>
              )} */}
            </Space>
          </Col>
        </Row>
      </div>
    </Show>
  );
};
