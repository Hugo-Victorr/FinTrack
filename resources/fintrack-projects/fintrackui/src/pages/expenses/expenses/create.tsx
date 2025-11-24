import React from "react";
import { Create, SaveButtonProps } from "@refinedev/antd";
import { Drawer, DrawerProps, Form, FormProps, Input, Select, DatePicker, InputNumber } from "antd";
import { useSelect } from "@refinedev/antd";
import dayjs from "dayjs";

interface ExpenseFormProps {
  drawerProps: DrawerProps;
  formProps: FormProps;
  saveButtonProps: SaveButtonProps;
}

export const CreateExpense: React.FC<ExpenseFormProps> = ({
  drawerProps,
  formProps,
  saveButtonProps
}) => {
  const { selectProps: categorySelectProps } = useSelect({
    resource: "ExpenseCategory",
    optionLabel: "description",
    optionValue: "id",
  });

  const { selectProps: walletSelectProps } = useSelect({
    resource: "Wallet",
    optionLabel: "name",
    optionValue: "id",
  });

  return (
    <Drawer {...drawerProps}>
      <Create saveButtonProps={saveButtonProps} resource="Expense">
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
            label="Amount"
            name="amount"
            rules={[
              {
                required: true,
                message: "Amount is required",
              },
            ]}
          >
            <InputNumber
              style={{ width: "100%" }}
              step={0.01}
              prefix="R$"
            />
          </Form.Item>
          <Form.Item
            label="Expense Date"
            name="expenseDate"
            rules={[
              {
                required: true,
                message: "Expense date is required",
              },
            ]}
            getValueFromEvent={(date) => date ? date.toISOString() : undefined}
            getValueProps={(value) => ({
              value: value ? dayjs(value) : undefined,
            })}
          >
            <DatePicker style={{ width: "100%" }} />
          </Form.Item>
          <Form.Item
            label="Category"
            name="expenseCategoryId"
            rules={[
              {
                required: true,
                message: "Category is required",
              },
            ]}
          >
            <Select {...categorySelectProps} />
          </Form.Item>
          <Form.Item
            label="Wallet"
            name="walletId"
            rules={[
              {
                required: true,
                message: "Wallet is required",
              },
            ]}
          >
            <Select {...walletSelectProps} />
          </Form.Item>
        </Form>
      </Create>
    </Drawer>
  );
};

