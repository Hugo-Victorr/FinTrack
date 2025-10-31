import axios from "axios";
import type { HttpError } from "@refinedev/core";
import { keycloak } from "../../auth-provider/utils/keycloak";

const axiosInstance = axios.create();

axiosInstance.interceptors.request.use(async (config) => {
  if (keycloak?.tokenParsed) {
    console.log(keycloak?.tokenParsed)
    config.headers.Authorization = `Bearer ${keycloak.token}`;

    // Validation only
    config.headers["X-User-Id"] = keycloak?.tokenParsed.sub
    config.headers["X-User-Roles"] = ["admin"]
  }
  return config;
});

axiosInstance.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
    const customError: HttpError = {
      ...error,
      message: error.response?.data?.message,
      statusCode: error.response?.status,
    };

    return Promise.reject(customError);
  }
);

export { axiosInstance };
