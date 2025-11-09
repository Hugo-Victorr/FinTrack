import { Create, SaveButtonProps, useSelect } from "@refinedev/antd";
import { Drawer, DrawerProps, Form, FormProps, Input, Select } from "antd";
import { CourseLevelSelect } from "../../../components/course/levelSelect";

interface CourseFormProps {
  drawerProps: DrawerProps;
  formProps: FormProps;
  saveButtonProps: SaveButtonProps;
}

export const CreateCourse: React.FC<CourseFormProps> = ({
  drawerProps,
  formProps,
  saveButtonProps
}) => {

  const { selectProps } = useSelect({
    resource: "coursecategory",
    optionLabel: "name"
  });

  return (
    <Drawer {...drawerProps}>
      <Create saveButtonProps={saveButtonProps}>
        <Form {...formProps} layout="vertical">
          <Form.Item
            label="Title"
            name="title"
            rules={[
              {
                required: true,
              },
            ]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Category"

            name={"categoryId"}
            rules={[
              {
                required: true,
              },
            ]}
          >
            <Select
              placeholder="Select a category"
              {...selectProps}
            />
          </Form.Item>
          <Form.Item
            label="Description"
            name="description"
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Aims"
            name="aims"
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Instructor"
            name="instructor"
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Level"
            name="level"
          >
            <CourseLevelSelect />
          </Form.Item>
        </Form>
      </Create>
    </Drawer>
  );
};
