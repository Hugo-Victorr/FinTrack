import React, { useEffect } from "react";
import { Create, SaveButtonProps } from "@refinedev/antd";
import { Drawer, DrawerProps, Form, FormProps, Input, ColorPicker, Select } from "antd";
import { OperationType } from "../expenses";

interface ExpenseCategoryFormProps {
  drawerProps: DrawerProps;
  formProps: FormProps;
  saveButtonProps: SaveButtonProps;
}

const generateRandomLightColor = (): string => {
  const R = Math.floor(Math.random() * 127 + 127);
  const G = Math.floor(Math.random() * 127 + 127);
  const B = Math.floor(Math.random() * 127 + 127);

  const rgb = (R << 16) + (G << 8) + B;
  return `#${rgb.toString(16)}`;
};

export const CreateExpenseCategory: React.FC<ExpenseCategoryFormProps> = ({
  drawerProps,
  formProps,
  saveButtonProps
}) => {
  // Generate random light color when drawer opens
  useEffect(() => {
    if (drawerProps.open && formProps.form) {
      const randomColor = generateRandomLightColor();
      formProps.form.setFieldValue('color', randomColor);
    }
  }, [drawerProps.open, formProps.form]);

  return (
    <Drawer {...drawerProps}>
      <Create saveButtonProps={saveButtonProps} resource="ExpenseCategory">
        <Form {...formProps} layout="vertical" autoComplete="off">
          <Form.Item label="ID" name="id">
            <Input disabled />
          </Form.Item>
          <Form.Item
            label="Description"
            name="description"
            rules={[
              {
                required: true,
                message: "Description is required",
              },
            ]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Color"
            name="color"
            rules={[
              {
                required: true,
                message: "Color is required",
              },
            ]}
            getValueFromEvent={(color) => color?.toHexString() || generateRandomLightColor()}
            initialValue={generateRandomLightColor()}
          >
            <ColorPicker showText disabledAlpha />
          </Form.Item>
          <Form.Item
            label="Operation Type"
            name="operationType"
            rules={[
              {
                required: true,
                message: "Operation type is required",
              },
            ]}
          >
            <Select>
              <Select.Option value={OperationType.Expense}>Expense</Select.Option>
              <Select.Option value={OperationType.Income}>Income</Select.Option>
            </Select>
          </Form.Item>
        </Form>
      </Create>
    </Drawer>
  );
};

