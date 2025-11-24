import { BaseRecord, HttpError } from "@refinedev/core";
import React from "react";

import { DeleteButton, EditButton, List, useDrawerForm, useTable } from "@refinedev/antd";
import { Table, Space } from "antd";
import { CreateWallet } from "./create";
import { EditWallet } from "./edit";
import { CurrencyLabels, WalletTypeLabels } from "./types";


export const WalletList: React.FC = () => {
  const { tableProps } = useTable<HttpError>({
    resource: "Wallet",
  });
  
  const { formProps: createFormProps, drawerProps: createDrawerProps, show: createShow, saveButtonProps: createSaveButtonProps } = useDrawerForm<HttpError>({
    action: "create",
    resource: "Wallet",
  });

  const { formProps: editFormProps, drawerProps: editDrawerProps, show: editShow, saveButtonProps: editSaveButtonProps, id } = useDrawerForm<HttpError>({
    action: "edit",
    resource: "Wallet",
  });

  return (
    <>
      <List
        canCreate
        createButtonProps={{
          onClick: () => {
            createShow();
          },
        }}
      >
        <Table {...tableProps} rowKey="id">
          <Table.Column dataIndex="name" title="Name" />
          <Table.Column dataIndex="description" title="Description" />
          <Table.Column 
            dataIndex="amount" 
            title="Amount" 
            render={(amount, record: BaseRecord) => {
              const currency = CurrencyLabels[record.currency as number] || "USD";
              const symbol = currency === "USD" ? "$" : currency === "EUR" ? "€" : currency === "GBP" ? "£" : currency === "JPY" ? "¥" : "R$";
              return amount ? `${symbol} ${amount.toFixed(2)}` : `${symbol} 0.00`;
            }}
          />
          <Table.Column 
            dataIndex="currency" 
            title="Currency" 
            render={(currency) => CurrencyLabels[currency as number] || "USD"}
          />
          <Table.Column 
            dataIndex="walletCategory" 
            title="Type" 
            render={(category) => WalletTypeLabels[category as number] || "Cash"}
          />
          <Table.Column 
            dataIndex="createdAt" 
            title="Created at" 
            render={(text) => text ? new Date(text).toLocaleString() : ""}
          />
          <Table.Column
            title={"Actions"}
            dataIndex="actions"
            render={(_, record: BaseRecord) => (
              <Space>
                <EditButton hideText size="small" recordItemId={record.id} onClick={() => editShow(record.id)} />
                <DeleteButton hideText size="small" recordItemId={record.id} resource="Wallet" />
              </Space>
            )}
          />
        </Table>
      </List>
      <CreateWallet drawerProps={createDrawerProps} formProps={createFormProps} saveButtonProps={createSaveButtonProps} />
      <EditWallet drawerProps={editDrawerProps} formProps={editFormProps} saveButtonProps={editSaveButtonProps} id={id} />
    </>
  );
};







