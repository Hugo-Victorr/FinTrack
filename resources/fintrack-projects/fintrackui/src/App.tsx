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
import { dataProvider } from "./providers/rest-data-provider";
import { App as AntdApp } from "antd";
import { BrowserRouter, Outlet, Route, Routes } from "react-router";
import { Header } from "./components/header";
import { ColorModeContextProvider } from "./contexts/color-mode";
import { Login } from "./pages/login";
import { CourseCategoryList } from "./pages/education/course-categories";
import { ListCourse, ShowCourse, WatchCourse } from "./pages/education/courses";
import { FintrackLogo } from "./components/icons/fintrackLogo";
import { authProvider } from "./providers/auth-provider";

function App() {
  const { initialized } = useKeycloak();

  if (!initialized) {
     return <div>Loading...</div>;
  }

  return (
    <BrowserRouter>
      <RefineKbarProvider>
        <ColorModeContextProvider>
          <AntdApp>
            <Refine
              dataProvider={dataProvider("http://localhost:5166/api")}
              notificationProvider={useNotificationProvider}
              routerProvider={routerProvider}
              authProvider={authProvider}
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
