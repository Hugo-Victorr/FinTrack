import { ThemedLayout, ThemedSider } from "@refinedev/antd";
import { Authenticated, CanAccess, ErrorComponent } from "@refinedev/core";
import { CatchAllNavigate, NavigateToResource } from "@refinedev/react-router";
import { Result } from "antd";
import { Routes, Route, Link, Outlet, Navigate } from "react-router";
import { Header } from "../components";
import FinanceDashboard from "../pages/dashboard";
import { CourseCategoryList } from "../pages/education/course-categories";
import { ListCourse, EditCourse, ShowCourse, WatchCourse } from "../pages/education/courses";
import { ExpenseCategoryList } from "../pages/expenses/expense-categories";
import { ExpenseList } from "../pages/expenses/expenses";
import { WalletList } from "../pages/expenses/wallets";
import { Login } from "../pages/login";

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
        <Route index element={<Navigate to="/dashboard" replace />} />
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
        <Route path="/expense-category">
          <Route index element={<ExpenseCategoryList />} />
        </Route>
        <Route path="/expense">
          <Route index element={<ExpenseList />} />
        </Route>
        <Route path="/wallet">
          <Route index element={<WalletList />} />
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
