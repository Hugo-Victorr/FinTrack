import { BaseRecord, HttpError } from "@refinedev/core";
import React from "react";

import { Create, DeleteButton, EditButton, List, useDrawerForm, useTable } from "@refinedev/antd";
import { Drawer, Form, Input, Select, Space, Table } from "antd";
import { CreateCourseCategory } from "./create";
import { EditCourseCategory } from "./edit";


export const CourseCategoryList: React.FC = () => {
  const { tableProps } = useTable<HttpError>();

  const { formProps: createFormProps, drawerProps: createDrawerProps, show: createShow, saveButtonProps: createSaveButtonProps } = useDrawerForm<HttpError>({
    action: "create",
  });

  const { formProps: editFormProps, drawerProps: editDrawerProps, show: editShow, saveButtonProps: editSaveButtonProps, id } = useDrawerForm<HttpError>({
    action: "edit",
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
          <Table.Column dataIndex="createdAt" title="Created at" />
          <Table.Column
            title={"Actions"}
            dataIndex="actions"
            render={(_, record: BaseRecord) => (
              <Space>
                <EditButton hideText size="small" recordItemId={record.id} onClick={() => editShow(record.id)} />
                <DeleteButton hideText size="small" recordItemId={record.id} />
              </Space>
            )}
          />
        </Table>
      </List>
      <CreateCourseCategory drawerProps={createDrawerProps} formProps={createFormProps} saveButtonProps={createSaveButtonProps} />
      <EditCourseCategory drawerProps={editDrawerProps} formProps={editFormProps} saveButtonProps={editSaveButtonProps} id={id} />
    </>
  );
};

