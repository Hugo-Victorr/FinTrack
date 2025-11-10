import { Edit, useForm, useSelect } from "@refinedev/antd";
import { Button, Card, Col, Form, Input, Row, Select, Space, Switch, Tabs } from "antd";
import { CourseLevelSelect } from "../../../components/course/levelSelect";
import { useOne } from "@refinedev/core";

import React, { useEffect, useState } from 'react';
import { ArrowDownOutlined, ArrowUpOutlined, CheckOutlined, CloseOutlined, InboxOutlined, UploadOutlined } from '@ant-design/icons';
import type { UploadProps } from 'antd';
import { message, Upload } from 'antd';

const { Dragger } = Upload;

const uploadProps: UploadProps = {
  name: 'file',
  multiple: true,
  action: 'https://660d2bd96ddfa2943b33731c.mockapi.io/api/upload',
  onChange(info) {
    const { status } = info.file;
    if (status !== 'uploading') {
      console.log(info.file, info.fileList);
    }
    if (status === 'done') {
      message.success(`${info.file.name} file uploaded successfully.`);
    } else if (status === 'error') {
      message.error(`${info.file.name} file upload failed.`);
    }
  },
  onDrop(e) {
    console.log('Dropped files', e.dataTransfer.files);
  },
};


export const EditCourse = () => {
  const [initialCategoryId, setInitialCategoryId] = useState<any>();

  const {
    formProps,
    saveButtonProps,
    query: { isLoading: courseIsLoading, data },
  } = useForm({
    meta: {
      headers: {
        "x-include-lessons": "true"
      }
    }
  });

  const {
    category,
    query: { isLoading: catIsLoading, data: catData }
  } = useOne({
    resource: "coursecategory",
    id: data?.data?.category?.id,
  });

  const { selectProps } = useSelect({
    resource: "coursecategory",
    optionLabel: "name"
  });

  return (
    <Edit saveButtonProps={saveButtonProps} isLoading={courseIsLoading || catIsLoading}>
      <Form
        {...formProps}
        layout="vertical"
        autoComplete="off"
      >
        <Tabs
          items={[
            {
              key: "1",
              label: "Metadata",
              children: <CourseMetadata />
            },
            {
              key: "2",
              label: "Lessons",
              children: <CourseLessons />
            }
          ]}
        />
      </Form>
    </Edit>
  );
}

const CourseMetadata = ({selectProps, catData}) => {
    return (
      <Row gutter={[32, 24]}>
        <Col xs={24} sm={24} md={12}>
          <Form.Item
            label="Title"
            name="title"
            rules={[
              {
                required: true,
              },
            ]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Category"
            initialValue={catData?.data.id}
            name={"categoryId"}
            rules={[
              {
                required: true,
              },
            ]}
          >
            <Select
              placeholder="Select a category"
              {...selectProps}
            />
          </Form.Item>
          <Form.Item
            label="Description"
            name="description"
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Aims"
            name="aims"
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Instructor"
            name="instructor"
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Level"
            name="level"
          >
            <CourseLevelSelect />
          </Form.Item>
        </Col>
        <Col xs={24} sm={24} md={12}>
          <Form.Item
            label="Thumbnail"
          >
            <Dragger {...uploadProps}>
              <p className="ant-upload-drag-icon">
                <InboxOutlined />
              </p>
              <p className="ant-upload-text">Click or drag file to this area to upload</p>
              {/* <p className="ant-upload-hint">
                  Support for a single or bulk upload. Strictly prohibited from uploading company data or other
                  banned files.
                </p> */}
            </Dragger>
          </Form.Item>
          <Form.Item
            label="Is Published?"
            name="isPublished"
          >
            <Switch
              checkedChildren={<CheckOutlined />}
              unCheckedChildren={<CloseOutlined />}
              defaultChecked
            />
          </Form.Item>
        </Col>
      </Row>
    );
  };

  const CourseLessons = () => {
    const props: UploadProps = {
      name: 'file',
      action: 'https://660d2bd96ddfa2943b33731c.mockapi.io/api/upload',
      headers: {
        authorization: 'authorization-text',
      },
      onChange(info) {
        if (info.file.status !== 'uploading') {
          console.log(info.file, info.fileList);
        }
        if (info.file.status === 'done') {
          message.success(`${info.file.name} file uploaded successfully`);
        } else if (info.file.status === 'error') {
          message.error(`${info.file.name} file upload failed.`);
        }
      },
    };

    return (
      <Form.List name="modules">
        {(fields, { add, remove }) => (
          <div style={{ display: 'flex', rowGap: 16, flexDirection: 'column' }}>
            {fields.map((field) => (
              <Card
                size="small"
                title={`Module ${field.name + 1}`}
                key={field.key}
                extra={
                  <Space>
                    <ArrowUpOutlined />
                    <ArrowDownOutlined />
                    <CloseOutlined
                      onClick={() => {
                        remove(field.name);
                      }}
                    />
                  </Space>
                }
              >
                <Form.Item label="Name" name={[field.name, 'title']}>
                  <Input />
                </Form.Item>

                {/* Nest Form.List */}
                <Form.Item label="Lessons">
                  <Form.List name={[field.name, 'lessons']}>
                    {(subFields, subOpt) => (
                      <div style={{ display: 'flex', flexDirection: 'column', rowGap: 16 }}>
                        {subFields.map((subField) => (
                          <Space key={subField.key}>
                            <Form.Item noStyle name={[subField.name, 'title']}>
                              <Input placeholder="Title" />
                            </Form.Item>
                            <Upload {...props}>
                              <Button icon={<UploadOutlined />}>Click to Upload</Button>
                            </Upload>
                            <ArrowUpOutlined />
                            <ArrowDownOutlined />
                            <CloseOutlined
                              onClick={() => {
                                subOpt.remove(subField.name);
                              }}
                            />
                          </Space>
                        ))}
                        <Button type="dashed" onClick={() => subOpt.add()} block>
                          + Add Lesson
                        </Button>
                      </div>
                    )}
                  </Form.List>
                </Form.Item>
              </Card>
            ))}

            <Button type="dashed" onClick={() => add()} block>
              + Add Module
            </Button>
          </div>
        )}
      </Form.List>
    );
  };