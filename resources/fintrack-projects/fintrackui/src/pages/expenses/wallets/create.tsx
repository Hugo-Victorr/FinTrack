import React from "react";
import { Create, SaveButtonProps } from "@refinedev/antd";
import { Drawer, DrawerProps, Form, FormProps, Input, Select, InputNumber } from "antd";
import { CurrencyType, CurrencyLabels, WalletType, WalletTypeLabels } from "./types";

interface WalletFormProps {
  drawerProps: DrawerProps;
  formProps: FormProps;
  saveButtonProps: SaveButtonProps;
}

export const CreateWallet: React.FC<WalletFormProps> = ({
  drawerProps,
  formProps,
  saveButtonProps
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
      <Create saveButtonProps={saveButtonProps} resource="Wallet">
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
            initialValue={0}
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
            initialValue={CurrencyType.BRL}
          >
            <Select disabled options={currencyOptions} />
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
            initialValue={WalletType.Cash}
          >
            <Select options={walletTypeOptions} />
          </Form.Item>
        </Form>
      </Create>
    </Drawer>
  );
};







