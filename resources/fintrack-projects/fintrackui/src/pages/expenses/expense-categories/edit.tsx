import React from "react";
import { Edit, SaveButtonProps } from "@refinedev/antd";
import { BaseKey } from "@refinedev/core";
import { Drawer, DrawerProps, Form, FormProps, Input, ColorPicker, Select } from "antd";
import { OperationType } from "../expenses/types";

interface ExpenseCategoryFormProps {
  drawerProps: DrawerProps;
  formProps: FormProps;
  saveButtonProps: SaveButtonProps;
  id: BaseKey | undefined
}

export const EditExpenseCategory: React.FC<ExpenseCategoryFormProps> = ({
  drawerProps,
  formProps,
  saveButtonProps,
  id
}) => {
  return (
    <Drawer {...drawerProps}>
      <Edit saveButtonProps={saveButtonProps} recordItemId={id} resource="ExpenseCategory">
        <Form {...formProps} layout="vertical" autoComplete="off">
          <Form.Item
            label="ID"
            name="id"
          >
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
            getValueFromEvent={(color) => color?.toHexString() || "#FFFFFF"}
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
      </Edit>
    </Drawer>
  );
};

