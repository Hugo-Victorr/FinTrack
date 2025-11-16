import { PlayCircleOutlined } from "@ant-design/icons";
import {
  DeleteButton,
  EditButton,
  List,
  useDrawerForm,
  useTable,
} from "@refinedev/antd";
import {
  BaseKey,
  BaseRecord,
  HttpError,
  useGo,
  useParsed,
} from "@refinedev/core";
import { Button, Space, Table, Tabs } from "antd";
import { useState } from "react";
import { CreateCourse } from "./create";

export const ListCourse = () => {
  const [activeKey, setActiveKey] = useState("2");

  const {
    formProps: createFormProps,
    drawerProps: createDrawerProps,
    show: createShow,
    saveButtonProps: createSaveButtonProps,
  } = useDrawerForm<HttpError>({
    action: "create",
  });

  const go = useGo();
  const { resource } = useParsed();

  const handleStartOrContinue = (id?: BaseKey) => {
    go({
      to: `/${resource!.name}/watch/${id}`,
      type: "push",
    });
  };

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
    <Table
      {...tableProps}
      rowKey="id"
      style={{ width: "100%" }}
      // onRow={(record) => {
      //   return {
      //     style: { cursor: "pointer" },
      //     onClick: () => {
      //       go({
      //         to: {
      //           action: "show",
      //           resource: resource!.name,
      //           id: record.id!,
      //         },
      //       });
      //     },
      //   };
      // }}
    >
      <Table.Column
        dataIndex="thumbnailUrl"
        width={100}
        render={(value) => {
          return (
            <img
              alt="Thumbnail"
              src={value}
              style={{
                maxWidth: 100,
                height: "auto",
                objectFit: "contain",
              }}
            />
          );
        }}
      />
      <Table.Column dataIndex="title" title="Title" />
      <Table.Column dataIndex="description" title="Description" />
      <Table.Column dataIndex={["category", "name"]} title="Category" />
      <Table.Column
        title="Actions"
        render={(_, record: BaseRecord) => (
          <Space>
            {/* <ShowButton hideText size="small" recordItemId={record.id} /> */}
            <EditButton
              hideText
              size="small"
              recordItemId={record.id}
            />
            <DeleteButton
              hideText
              size="small"
              recordItemId={record.id}
            />
            <Button
              type="link"
              icon={<PlayCircleOutlined style={{ verticalAlign: "middle" }} />}
              shape="circle"
              size="large"
              onClick={() => {
                handleStartOrContinue(record.id);
              }}
            />
          </Space>
        )}
      />
    </Table>
  );

  return (
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
        items={[
          {
            label: "Available courses",
            key: "2",
            children: <CourseTable />,
          },
          {
            label: "My courses",
            key: "1",
          },
        ]}
      />
      <CreateCourse
        drawerProps={createDrawerProps}
        formProps={createFormProps}
        saveButtonProps={createSaveButtonProps}
      />
    </List>
  );
};
