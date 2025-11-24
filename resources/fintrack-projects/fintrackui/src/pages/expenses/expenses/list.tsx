import { BaseRecord, HttpError } from "@refinedev/core";
import React from "react";

import {
  DeleteButton,
  EditButton,
  List,
  useDrawerForm,
  useTable,
} from "@refinedev/antd";
import { Space, Table, Tag } from "antd";
import { CreateExpense } from "./create";
import { EditExpense } from "./edit";

export const ExpenseList: React.FC = () => {
  const { tableProps } = useTable<HttpError>({
    resource: "Expense",
  });

  const {
    formProps: createFormProps,
    drawerProps: createDrawerProps,
    show: createShow,
    saveButtonProps: createSaveButtonProps,
  } = useDrawerForm<HttpError>({
    action: "create",
    resource: "Expense",
  });

  const {
    formProps: editFormProps,
    drawerProps: editDrawerProps,
    show: editShow,
    saveButtonProps: editSaveButtonProps,
    id,
  } = useDrawerForm<HttpError>({
    action: "edit",
    resource: "Expense",
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
          <Table.Column dataIndex="description" title="Description" />
          <Table.Column
            dataIndex="amount"
            title="Amount"
            render={(amount) => (
              <Tag color={amount < 0 ? "red" : "green"}>{amount ? `R$ ${amount.toFixed(2)}` : "R$ 0.00"}</Tag>
            )}
          />
          <Table.Column
            dataIndex="expenseDate"
            title="Expense Date"
            render={(text) => (text ? new Date(text).toLocaleDateString() : "")}
          />
          <Table.Column
            dataIndex={["expenseCategory", "description"]}
            title="Category"
            render={(_, record: BaseRecord) => (
              <Tag
                color={record.expenseCategory?.color}
                style={{ transform: "scale(1.1)" }}
              >
                {record.expenseCategory?.description}
              </Tag>
            )}
          />
          <Table.Column
            dataIndex="createdAt"
            title="Created at"
            render={(text) => (text ? new Date(text).toLocaleString() : "")}
          />
          <Table.Column
            title={"Actions"}
            dataIndex="actions"
            render={(_, record: BaseRecord) => (
              <Space>
                <EditButton
                  hideText
                  size="small"
                  recordItemId={record.id}
                  onClick={() => editShow(record.id)}
                />
                <DeleteButton
                  hideText
                  size="small"
                  recordItemId={record.id}
                  resource="Expense"
                />
              </Space>
            )}
          />
        </Table>
      </List>
      <CreateExpense
        drawerProps={createDrawerProps}
        formProps={createFormProps}
        saveButtonProps={createSaveButtonProps}
      />
      <EditExpense
        drawerProps={editDrawerProps}
        formProps={editFormProps}
        saveButtonProps={editSaveButtonProps}
        id={id}
      />
    </>
  );
};
