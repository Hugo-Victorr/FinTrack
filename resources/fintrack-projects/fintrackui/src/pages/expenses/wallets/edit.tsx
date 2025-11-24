import React from "react";
import { Edit, SaveButtonProps } from "@refinedev/antd";
import { BaseKey } from "@refinedev/core";
import { Drawer, DrawerProps, Form, FormProps, Input, Select, InputNumber } from "antd";
import { CurrencyType, CurrencyLabels, WalletType, WalletTypeLabels } from "./types";

interface WalletFormProps {
  drawerProps: DrawerProps;
  formProps: FormProps;
  saveButtonProps: SaveButtonProps;
  id: BaseKey | undefined
}

export const EditWallet: React.FC<WalletFormProps> = ({
  drawerProps,
  formProps,
  saveButtonProps,
  id
}) => {
  const currencyOptions = Object.entries(CurrencyLabels).map(([value, label]) => ({
    label,
    value: parseInt(value),
  }));

  const walletTypeOptions = Object.entries(WalletTypeLabels).map(([value, label]) => ({
    label,
    value: parseInt(value),
  }));

  return (
    <Drawer {...drawerProps}>
      <Edit saveButtonProps={saveButtonProps} recordItemId={id} resource="Wallet">
        <Form {...formProps} layout="vertical" autoComplete="off">
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
                message: "Name is required",
              },
            ]}
          >
            <Input />
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
            <Input.TextArea rows={3} />
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
              min={0}
              step={0.01}
            />
          </Form.Item>
          <Form.Item
            label="Currency"
            name="currency"
            rules={[
              {
                required: true,
                message: "Currency is required",
              },
            ]}
          >
            <Select options={currencyOptions} />
          </Form.Item>
          <Form.Item
            label="Wallet Type"
            name="walletCategory"
            rules={[
              {
                required: true,
                message: "Wallet type is required",
              },
            ]}
          >
            <Select options={walletTypeOptions} />
          </Form.Item>
        </Form>
      </Edit>
    </Drawer>
  );
};







