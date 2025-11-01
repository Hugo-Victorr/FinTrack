import { PlayCircleOutlined } from "@ant-design/icons";
import { DeleteButton, EditButton, List, ShowButton, useDrawerForm, useTable } from "@refinedev/antd";
import { BaseRecord, HttpError } from "@refinedev/core";
import { Button, Col, Row, Space, Table, Tabs } from "antd";
import { useState } from "react";
import { CreateCourse } from "./create";

export const ListCourse = () => {
  const [activeKey, setActiveKey] = useState("1");

  const { 
      formProps: createFormProps, 
      drawerProps: createDrawerProps, 
      show: createShow, 
      saveButtonProps: createSaveButtonProps 
    } = useDrawerForm<HttpError>({
    action: "create",
  });

  const { tableProps } = useTable({
    filters: {
      permanent: [
        activeKey === "1"
          ? { field: "ownedByUser", operator: "eq", value: true }
          : activeKey === "2"
            ? { field: "available", operator: "eq", value: true }
            : { field: "type", operator: "eq", value: "learningPlan" },
      ],
    },
  });

  const CourseTable = () => (
    <Table {...tableProps} rowKey="id" style={{ width: "100%" }}>
      <Table.Column dataIndex="title" title="Title" />
      <Table.Column dataIndex="description" title="Description" />
      <Table.Column dataIndex={["category", "name"]} title="Category" />
      <Table.Column
        title="Actions"
        render={(_, record: BaseRecord) => (
          <Space>
            <ShowButton hideText size="small" recordItemId={record.id} />
            <EditButton hideText size="small" recordItemId={record.id} />
            <DeleteButton hideText size="small" recordItemId={record.id} />
            <Button icon={<PlayCircleOutlined />} />
          </Space>
        )}
      />
    </Table>
  )

  return (
    <div>
      <List
        canCreate
        createButtonProps={{
          onClick: () => {
            createShow();
          },
        }}
      >
        <Tabs
          tabPosition="left"
          activeKey={activeKey}
          onChange={setActiveKey}
          style={{ height: "100%" }}
          items={[{
            label: "Available courses",
            key: "2",
            children: <CourseTable />
          },
          {
            label: "My courses",
            key: "1",
          }]}
        />
        <CreateCourse drawerProps={createDrawerProps} formProps={createFormProps} saveButtonProps={createSaveButtonProps}/>
      </List>
    </div>
  );
}