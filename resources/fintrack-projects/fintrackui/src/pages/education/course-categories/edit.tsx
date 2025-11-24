import { Create, Edit, SaveButtonProps } from "@refinedev/antd";
import { BaseKey } from "@refinedev/core";
import { Drawer, DrawerProps, Form, FormProps, Input } from "antd";

interface CourseCategoryFormProps {
  drawerProps: DrawerProps;
  formProps: FormProps;
  saveButtonProps: SaveButtonProps;
  id: BaseKey | undefined
}

export const EditCourseCategory: React.FC<CourseCategoryFormProps> = ({
  drawerProps,
  formProps,
  saveButtonProps,
  id
}) => {
  return (
    <Drawer {...drawerProps}>
      <Edit saveButtonProps={saveButtonProps} recordItemId={id}>
        <Form {...formProps} layout="vertical" autoComplete="off">
          <Form.Item label="ID" name="id">
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
          <Form.Item label="Description" name="description">
            <Input />
          </Form.Item>
        </Form>
      </Edit>
    </Drawer>
  );
};
