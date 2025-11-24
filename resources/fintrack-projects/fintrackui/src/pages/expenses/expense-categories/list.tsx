import { BaseRecord, HttpError } from "@refinedev/core";
import React from "react";

import { DeleteButton, EditButton, List, useDrawerForm, useTable } from "@refinedev/antd";
import { Space, Table, Tag } from "antd";
import { CreateExpenseCategory } from "./create";
import { EditExpenseCategory } from "./edit";
import { OperationType } from "../expenses/types";


export const ExpenseCategoryList: React.FC = () => {
  const { tableProps } = useTable<HttpError>({
    resource: "ExpenseCategory",
  });
  
  const { formProps: createFormProps, drawerProps: createDrawerProps, show: createShow, saveButtonProps: createSaveButtonProps } = useDrawerForm<HttpError>({
    action: "create",
    resource: "ExpenseCategory",
  });

  const { formProps: editFormProps, drawerProps: editDrawerProps, show: editShow, saveButtonProps: editSaveButtonProps, id } = useDrawerForm<HttpError>({
    action: "edit",
    resource: "ExpenseCategory",
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
          <Table.Column
            dataIndex="description"
            title="Description"
            render={(_, record: BaseRecord) => (
              <Tag color={record.color} style={{ transform: "scale(1.1)" }}>{record.description}</Tag>
            )}
          />
          <Table.Column
            dataIndex="operationType"
            title="Operation Type"
            render={(value: number) => (
              <Tag color={value === OperationType.Expense ? "red" : "green"}
                style={{ transform: "scale(1.1)" }}
              >
                {value === OperationType.Expense ? "Expense" : "Income"}
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
                  resource="ExpenseCategory"
                />
              </Space>
            )}
          />
        </Table>
      </List>
      <CreateExpenseCategory
        drawerProps={createDrawerProps}
        formProps={createFormProps}
        saveButtonProps={createSaveButtonProps}
      />
      <EditExpenseCategory
        drawerProps={editDrawerProps}
        formProps={editFormProps}
        saveButtonProps={editSaveButtonProps}
        id={id}
      />
    </>
  );
};







