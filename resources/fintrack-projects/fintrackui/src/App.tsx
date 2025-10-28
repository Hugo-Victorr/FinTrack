import {
  Authenticated,
  AuthProvider,
  Refine,
} from "@refinedev/core";
import { RefineKbar, RefineKbarProvider } from "@refinedev/kbar";

import {
  ErrorComponent,
  ThemedLayout,
  ThemedSider,
  useNotificationProvider,
} from "@refinedev/antd";
import "@refinedev/antd/dist/reset.css";

import { useKeycloak } from "@react-keycloak/web";
import routerProvider, {
  CatchAllNavigate,
  DocumentTitleHandler,
  NavigateToResource,
  UnsavedChangesNotifier,
} from "@refinedev/react-router";
import { dataProvider } from "./rest-data-provider";
import { App as AntdApp } from "antd";
import axios from "axios";
import { BrowserRouter, Outlet, Route, Routes } from "react-router";
import { Header } from "./components/header";
import { ColorModeContextProvider } from "./contexts/color-mode";
import { Login } from "./pages/login";
import { CourseCategoryList } from "./pages/education/course-categories";
import { ListCourse, ShowCourse, WatchCourse } from "./pages/education/courses";
import { FintrackLogo } from "./components/icons/fintrackLogo";

function App() {
  const { keycloak, initialized } = useKeycloak();

  // if (!initialized) {
  //   return <div>Loading...</div>;
  // }

  const authProvider: AuthProvider = {
    login: async () => {
      const urlSearchParams = new URLSearchParams(window.location.search);
      const { to } = Object.fromEntries(urlSearchParams.entries());
      await keycloak.login({
        redirectUri: to ? `${window.location.origin}${to}` : undefined,
      });
      return {
        success: true,
      };
    },
    logout: async () => {
      try {
        await keycloak.logout({
          redirectUri: window.location.origin,
        });
        return {
          success: true,
          redirectTo: "/login",
        };
      } catch (error) {
        return {
          success: false,
          error: new Error("Logout failed"),
        };
      }
    },
    onError: async (error) => {
      console.error(error);
      return { error };
    },
    check: async () => {
      try {
        const { token } = keycloak;
        if (token) {
          axios.defaults.headers.common = {
            Authorization: `Bearer ${token}`,
          };
          return {
            authenticated: true,
          };
        } else {
          return {
            authenticated: false,
            logout: true,
            redirectTo: "/login",
            error: {
              message: "Check failed",
              name: "Token not found",
            },
          };
        }
      } catch (error) {
        return {
          authenticated: false,
          logout: true,
          redirectTo: "/login",
          error: {
            message: "Check failed",
            name: "Token not found",
          },
        };
      }
    },
    getPermissions: async () => null,
    getIdentity: async () => {
      if (keycloak?.tokenParsed) {
        return {
          name: keycloak.tokenParsed.family_name,
        };
      }
      return null;
    },
  };

  return (
    <BrowserRouter>
      <RefineKbarProvider>
        <ColorModeContextProvider>
          <AntdApp>
            <Refine
              dataProvider={dataProvider("http://localhost:5166/api")}
              notificationProvider={useNotificationProvider}
              routerProvider={routerProvider}
              // authProvider={authProvider}
              resources={[
                {
                  name: "course",
                  list: "/course",
                  show: "/course/show/:id",
                  create: "/course/create",
                  edit: "/course/edit/:id",
                  meta: {
                    canDelete: true,
                  },
                },
                {
                  name: "coursecategory",
                  list: "/coursecategory",
                  create: "/coursecategory/create",
                  edit: "/coursecategory/edit/:id",
                  meta: {
                    canDelete: true,
                  },
                },
              ]}
              options={{
                syncWithLocation: true,
                warnWhenUnsavedChanges: true,
                projectId: "G9f8XF-O7GGK6-BspEWd",
                disableTelemetry: true, 
                
                title: {
                  icon: <FintrackLogo/>,
                  text: "FinTrack Project"
                }
              }}
            >
              <Routes>
                <Route
                  element={
                    <Authenticated
                      key="authenticated-inner"
                      fallback={<CatchAllNavigate to="/login" />}
                    >
                      <ThemedLayout
                        Header={Header}
                        Sider={(props) => <ThemedSider {...props} fixed />}
                      >
                        <Outlet />
                      </ThemedLayout>
                    </Authenticated>
                  }
                >
                  <Route path="/course">
                    <Route index element={<ListCourse />} />
                    <Route path="show/:id" element={<ShowCourse />} />
                    <Route path="watch/:id" element={<WatchCourse />} />
                  </Route>
                  <Route path="/coursecategory">
                    <Route index element={<CourseCategoryList />} />
                  </Route>
                  <Route path="*" element={<ErrorComponent />} />
                </Route>
                <Route
                  element={
                    <Authenticated
                      key="authenticated-outer"
                      fallback={<Outlet />}
                    >
                      <NavigateToResource />
                    </Authenticated>
                  }
                >
                  <Route path="/login" element={<Login />} />
                </Route>
              </Routes>
              <RefineKbar />
              <UnsavedChangesNotifier />
              <DocumentTitleHandler />
            </Refine>
          </AntdApp>
        </ColorModeContextProvider>
      </RefineKbarProvider>
    </BrowserRouter>
  );
}

export default App;
