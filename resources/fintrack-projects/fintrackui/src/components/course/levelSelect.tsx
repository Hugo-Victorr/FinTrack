import { Select } from "antd"

export const CourseLevelSelect = () => {
  return (
    <Select options={[
      {
        value: 0,
        label: "Beginner"
      },
      {
        value: 1,
        label: "Intermediate"
      },
      {
        value: 2,
        label: "Advanced"
      },
    ]} />
  )
}