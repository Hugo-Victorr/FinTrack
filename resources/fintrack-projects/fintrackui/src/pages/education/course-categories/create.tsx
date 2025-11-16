import { Create, SaveButtonProps } from "@refinedev/antd";
import { Drawer, DrawerProps, Form, FormProps, Input } from "antd";

interface CourseCategoryFormProps {
  drawerProps: DrawerProps;
  formProps: FormProps;
  saveButtonProps: SaveButtonProps;
}

export const CreateCourseCategory: React.FC<CourseCategoryFormProps> = ({
  drawerProps,
  formProps,
  saveButtonProps
}) => {
  return (
    <Drawer {...drawerProps}>
      <Create saveButtonProps={saveButtonProps}>
        <Form {...formProps} layout="vertical">
          <Form.Item
            label="ID"
            name="id"
          >
            <Input disabled />
          </Form.Item>
          <Form.Item
            label="Name"
            name="name"
            rules={[
              {
                required: true,
              },
            ]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Description"
            name="description"
          >
            <Input />
          </Form.Item>
        </Form>
      </Create>
    </Drawer>
  );
};
