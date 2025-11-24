import { Edit, useForm, useSelect } from "@refinedev/antd";
import {
  Button,
  Card,
  Col,
  Form,
  Input,
  Row,
  Select,
  Space,
  Switch,
  Tabs,
} from "antd";
import { CourseLevelSelect } from "../../../components/course/levelSelect";
import { useApiUrl, useParsed } from "@refinedev/core";

import { useState, useEffect } from "react";
import {
  ArrowDownOutlined,
  ArrowUpOutlined,
  CheckOutlined,
  CloseOutlined,
  InboxOutlined,
  UploadOutlined,
} from "@ant-design/icons";
import type { UploadFile } from "antd";
import { message, Upload } from "antd";
import { axiosInstance } from "../../../providers/rest-data-provider/utils";

const { Dragger } = Upload;

export const EditCourse = () => {
  const [fileList, setFileList] = useState<UploadFile[]>([]);
  const apiUrl = useApiUrl();
  const { id, resource } = useParsed();

  const [thumbnailUrl, setThumbnailUrl] = useState<string | undefined>();

  const {
    form,
    formProps,
    saveButtonProps,
    query,
  } = useForm({
    autoSave: {
      enabled: true,
      debounce: 2000,
    },
    meta: {
      headers: {
        "x-include-lessons": "true",
      },
    },
  });

  const obtainDownloadUrl = async (key: string) => {
    try {
      const res = await axiosInstance.get(`${apiUrl}/course/play?key=${encodeURIComponent(key)}`).then((response) => response.data?.url);
      return res;
    } catch (error) {
      console.error("Failed to obtain download URL:", error);
      return undefined;
    }
  };

  const obtainUploadUrl = async (key: string) => {
    const res = await axiosInstance.post(`${apiUrl}/${resource?.name}/upload`, {
      key: key,
    }).then((response) => response.data.url);

    return res;
  };

  const { selectProps, query: { isLoading: catIsLoading } } = useSelect({
    resource: "coursecategory",
    optionLabel: "name",
  });

  const courseIsLoading = query?.isLoading ?? false;
  const courseData = query?.data;
  const cat = courseData?.data?.category;
  const thumbnailKey = `${id}/thumbnail`;

  // Generic upload handler for both thumbnails and videos
  const handleUpload = async (
    file: File,
    key: string,
    options?: {
      onSuccess?: (downloadUrl: string, key: string) => void;
      onError?: (error: Error) => void;
      successMessage?: string;
      errorMessage?: string;
    }
  ): Promise<boolean> => {
    try {
      // Get presigned upload URL
      const uploadUrl = await obtainUploadUrl(key);
      
      // Upload file to S3
      await fetch(uploadUrl, {
        method: "PUT",
        headers: {
          "Content-Type": file.type,
        },
        body: file,
      });

      // Get presigned download URL
      const downloadUrl = await obtainDownloadUrl(key);
      
      if (!downloadUrl) {
        throw new Error("Failed to get download URL");
      }

      // Call success callback if provided
      if (options?.onSuccess) {
        options.onSuccess(downloadUrl, key);
      }

      // Show success message
      message.success(options?.successMessage || "File uploaded successfully");
      return false; // Prevent default upload behavior
    } catch (error) {
      console.error("Upload error:", error);
      const errorMsg = error instanceof Error ? error : new Error("Upload failed");
      
      // Call error callback if provided
      if (options?.onError) {
        options.onError(errorMsg);
      }

      // Show error message
      message.error(options?.errorMessage || "Failed to upload file");
      return false;
    }
  };

  // Thumbnail upload handler
  const handleThumbnailUpload = async (file: File) => {
    return handleUpload(file, thumbnailKey, {
      onSuccess: (downloadUrl) => {
        setThumbnailUrl(downloadUrl);
        setFileList([{ uid: "thumbnail", url: downloadUrl, name: thumbnailKey, status: "done" }]);
      },
      successMessage: "Thumbnail uploaded successfully",
      errorMessage: "Failed to upload thumbnail",
    });
  };

  // Video upload handler - creates a handler for a specific lesson
  // Automatically generates a unique key in frontend and stores only the key (not URL) in the form
  // Signed URLs are fetched on-demand when needed for playback (in watch.tsx)
  const createVideoUploadHandler = (moduleIndex: number, lessonIndex: number) => {
    return async (file: File) => {
      // Get file extension
      const fileExtension = file.name.split('.').pop() || 'mp4';

      console.log(moduleIndex, lessonIndex);
      
      const videoKey = `${id}/videos/${moduleIndex}/${lessonIndex}.${fileExtension}`;
      
      // Upload file to S3 using the generated unique key
      return handleUpload(file, videoKey, {
        onSuccess: (_downloadUrl, key) => {
          // Store only the key in the form field (not the signed download URL)
          // The backend will store this key, and signed URLs are fetched on-demand
          // when needed for video playback (see watch.tsx)
          const modules = form?.getFieldValue("modules") || [];
          if (modules[moduleIndex]?.lessons?.[lessonIndex]) {
            modules[moduleIndex].lessons[lessonIndex].videoUrl = key;
            form?.setFieldsValue({ modules });
          }
        },
        successMessage: "Video uploaded successfully",
        errorMessage: "Failed to upload video",
      });
    };
  };

  // Fetch thumbnail on mount
  useEffect(() => {
    if (!id) return;
    
    const fetchThumbnailUrl = async () => {
      const downloadUrl = await obtainDownloadUrl(thumbnailKey);
      if (downloadUrl) {
        setThumbnailUrl(downloadUrl);
        setFileList([{ uid: "thumbnail", url: downloadUrl, name: thumbnailKey, status: "done" }]);
      }
    };
    
    fetchThumbnailUrl();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [id]);

  return (
    <Edit
      saveButtonProps={saveButtonProps}
      isLoading={courseIsLoading || catIsLoading}
    >
      <Form {...formProps} layout="vertical" autoComplete="off">
        <Tabs
          items={[
            {
              key: "1",
              label: "Metadata",
              children: (
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
                      initialValue={cat?.id}
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
                    <Form.Item label="Description" name="description">
                      <Input />
                    </Form.Item>
                    <Form.Item label="Aims" name="aims">
                      <Input />
                    </Form.Item>
                    <Form.Item label="Instructor" name="instructor">
                      <Input />
                    </Form.Item>
                    <Form.Item label="Level" name="level">
                      <CourseLevelSelect initialValue={courseData?.data?.level ?? 0} />
                    </Form.Item>
                  </Col>
                  <Col xs={24} sm={24} md={12}>
                    <Form.Item
                      label="Thumbnail"                      
                    >
                      <Dragger
                        name="file"
                        multiple={false}
                        maxCount={1}
                        fileList={fileList}
                        beforeUpload={handleThumbnailUpload}
                        onRemove={() => {
                          setFileList([]);
                          setThumbnailUrl(undefined);
                          return true;
                        }}
                        onChange={(info) => {
                          setFileList(info.fileList);
                          if (info.file.status === "error") {
                            message.error(
                              `${info.file.name} file upload failed.`
                            );
                          }
                        }}
                        accept="image/*"
                      >
                        {fileList.length > 0 ? (
                          <img src={thumbnailUrl} alt="Thumbnail" style={{ maxWidth: 100, height: "auto", objectFit: "contain" }} />
                        ) : (
                        <>
                          <p className="ant-upload-drag-icon">
                            <InboxOutlined />
                          </p>
                          <p className="ant-upload-text">
                            Click or drag file to this area to upload
                          </p>
                          <p className="ant-upload-hint">
                            Support for a single image upload
                          </p>
                        </>
                        )}
                      </Dragger>
                    </Form.Item>
                    <Form.Item label="Is Published?" name="isPublished">
                      <Switch
                        checkedChildren={<CheckOutlined />}
                        unCheckedChildren={<CloseOutlined />}
                        defaultChecked
                      />
                    </Form.Item>
                  </Col>
                </Row>
              ),
            },
            {
              key: "2",
              label: "Lessons",
              children: (
                <Form.List name="modules">
                  {(fields, { add, remove }) => (
                    <div
                      style={{
                        display: "flex",
                        rowGap: 16,
                        flexDirection: "column",
                      }}
                    >
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
                          <Form.Item label="Name" name={[field.name, "title"]}>
                            <Input />
                          </Form.Item>

                          {/* Nest Form.List */}
                          <Form.Item label="Lessons">
                            <Form.List name={[field.name, "lessons"]}>
                              {(subFields, subOpt) => (
                                <div
                                  style={{
                                    display: "flex",
                                    flexDirection: "column",
                                    rowGap: 16,
                                  }}
                                >
                                  {subFields.map((subField) => {
                                    // Get current lesson data to check if video is uploaded
                                    const hasVideo = false;
                                    
                                    return (
                                      <Space key={subField.key} style={{ width: "100%" }} wrap>
                                        <Form.Item
                                          noStyle
                                          name={[subField.name, "title"]}
                                        >
                                          <Input placeholder="Lesson Title" style={{ minWidth: 200 }} />
                                        </Form.Item>
                                        <Upload 
                                          multiple={false}
                                          maxCount={1}
                                          name="file"
                                          listType="text"
                                          accept="video/*"
                                          showUploadList={false}
                                          beforeUpload={createVideoUploadHandler(field.key, subField.key)}
                                        >
                                          <Button 
                                            icon={<UploadOutlined />}
                                            type={hasVideo ? "default" : "primary"}
                                          >
                                            {hasVideo ? "Change Video" : "Upload Video"}
                                          </Button>
                                        </Upload>
                                        {hasVideo && (
                                          <span style={{ color: "#52c41a", fontSize: "12px" }}>
                                            ✓ Video uploaded
                                          </span>
                                        )}
                                        <ArrowUpOutlined />
                                        <ArrowDownOutlined />
                                        <CloseOutlined
                                          onClick={() => {
                                            subOpt.remove(subField.name);
                                          }}
                                        />
                                      </Space>
                                    );
                                  })}
                                  <Button
                                    type="dashed"
                                    onClick={() => subOpt.add()}
                                    block
                                  >
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
              ),
            },
          ]}
        />
      </Form>
    </Edit>
  );
};
