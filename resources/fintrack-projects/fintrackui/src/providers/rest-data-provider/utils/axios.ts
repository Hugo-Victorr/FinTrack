import axios from "axios";
import type { HttpError } from "@refinedev/core";
import { keycloak } from "../../auth-provider/utils/keycloak";

const axiosInstance = axios.create();

axiosInstance.interceptors.request.use(async (config) => {
  if (keycloak?.tokenParsed) {
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
    const errors = error.response?.data?.errors;
    
    const normalizedErrors =2
      errors && typeof errors === "object"
        ? Object.fromEntries(
            Object.entries(errors).map(([key, value]) => [
              key.charAt(0).toLowerCase() + key.slice(1),
              value,
            ])
          )
        : errors;

    const customError: HttpError = {
      ...error,
      message: error.response?.data?.message,
      statusCode: error.response?.status,
      errors: normalizedErrors
    };

    return Promise.reject(customError);
  }
);

export { axiosInstance };
