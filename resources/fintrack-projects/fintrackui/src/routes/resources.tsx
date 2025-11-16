import { ThemedLayout, ThemedSider } from "@refinedev/antd";
import {
  Authenticated,
  CanAccess,
  ErrorComponent,
  ResourceProps,
} from "@refinedev/core";
import { CatchAllNavigate, NavigateToResource } from "@refinedev/react-router";
import { Result } from "antd";
import { Routes, Route, Link, Outlet } from "react-router";
import { Header } from "../components";
import { CourseCategoryList } from "../pages/education/course-categories";
import {
  ListCourse,
  EditCourse,
  ShowCourse,
  WatchCourse,
} from "../pages/education/courses";
import { Login } from "../pages/login";
import { AreaChartOutlined, BookOutlined, DashboardOutlined, DollarOutlined } from "@ant-design/icons";
import FinanceDashboard from "../pages/dashboard";

export const appResources: ResourceProps[] = [
  {
    name: "dashboard",
    list: "/dashboard",
    meta: {
      icon: <DashboardOutlined />,
      canDelete: false,
    },
  },
  {
    name: "transactions",
    list: "/transactions",
    meta: {
      icon: <DollarOutlined />,
    },
  },
  {
    name: "investiments",
    list: "/investiments",
    meta: {
      icon: <AreaChartOutlined />,
    }
  },
  {
    name: "course",
    list: "/course",
    show: "/course/show/:id",
    create: "/course/create",
    edit: "/course/edit/:id",
    meta: {
      icon: <BookOutlined />,
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
      label: "Course categories",
    },
  },
];

export const AppRoutes = () => {
  return (
    <Routes>
      <Route
        element={
          <Authenticated
            key="authenticated-inner"
            fallback={<CatchAllNavigate to="/login" />}
          >
            <ThemedLayout
              initialSiderCollapsed
              Header={Header}
              Sider={(props) => <ThemedSider {...props} fixed />}
            >
              <CanAccess
                fallback={
                  <Result
                    status="403"
                    title="403"
                    subTitle="Sorry, you are not authorized to access this page."
                    extra={<Link to="/">Back Home</Link>}
                  />
                }
              >
                <Outlet />
              </CanAccess>
            </ThemedLayout>
          </Authenticated>
        }
      >
        <Route index element={<FinanceDashboard />} />
        <Route path="/dashboard" element={<FinanceDashboard />} />
        <Route path="/course">
          <Route index element={<ListCourse />} />
          <Route path="edit/:id" element={<EditCourse />} />
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
          <Authenticated key="authenticated-outer" fallback={<Outlet />}>
            <NavigateToResource />
          </Authenticated>
        }
      >
        <Route path="/login" element={<Login />} />
      </Route>
    </Routes>
  );
};
