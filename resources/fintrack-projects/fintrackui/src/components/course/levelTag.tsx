import { Tag } from "antd";

interface CourseLevelProps {
  level: number | undefined;
}

export const CourseLevel = (props: CourseLevelProps) => {
  let color;
  let text;

  switch (props.level) {
    case 0:
      color = "green";
      text = "Beginner";
      break;
    case 1:
      color = "yellow";
      text = "Intermediate";
      break;
    case 2:
      color = "red";
      text = "Advanced";
      break;
  }

  return <Tag color={color}>{text}</Tag>;
}
