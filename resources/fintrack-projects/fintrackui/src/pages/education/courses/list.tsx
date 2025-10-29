import { PlayCircleOutlined } from "@ant-design/icons";
import { DeleteButton, EditButton, List, ShowButton, useTable } from "@refinedev/antd";
import { BaseRecord } from "@refinedev/core";
import { Button, Row, Space, Table, Tabs } from "antd";
import { useState } from "react";

export const ListCourse = () => {
  const [activeKey, setActiveKey] = useState("1");

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

  return (
    <List>
      <Row>
        <Tabs tabPosition="left" activeKey={activeKey} onChange={setActiveKey}>
          <Tabs.TabPane key="1" tab="My courses" />
          <Tabs.TabPane key="2" tab="Available courses" />
          <Tabs.TabPane key="3" tab="Learning Plans" />
        </Tabs>
        <Table {...tableProps} rowKey="id" style={{ flex: 1 }}>
          <Table.Column dataIndex="title" title="Title" />
          <Table.Column dataIndex="description" title="Description" />
          <Table.Column dataIndex={["category", "name"]} title="Category" />
          <Table.Column
            title={"Actions"}
            dataIndex="actions"
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
      </Row>
    </List>
  );

}