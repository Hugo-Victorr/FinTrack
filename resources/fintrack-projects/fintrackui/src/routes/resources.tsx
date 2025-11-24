import {
  ResourceProps,
} from "@refinedev/core";
import { BookOutlined, DashboardOutlined, DollarOutlined, TagOutlined, WalletOutlined } from "@ant-design/icons";

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
    name: "expensecategory",
    list: "/expense-category",
    create: "/expense-category/create",
    edit: "/expense-category/edit/:id",
    meta: {
      canDelete: true,
      label: "Expense Categories",
      icon: <TagOutlined />,
    },
  },
  {
    name: "expense",
    list: "/expense",
    create: "/expense/create",
    edit: "/expense/edit/:id",
    meta: {
      canDelete: true,
      label: "Expenses",
      icon: <DollarOutlined />,
    },
  },
  {
    name: "wallet",
    list: "/wallet",
    create: "/wallet/create",
    edit: "/wallet/edit/:id",
    meta: {
      canDelete: true,
      label: "Wallets",
      icon: <WalletOutlined />,
    },
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
    show: "/coursecategory/show/:id",
    create: "/coursecategory/create",
    edit: "/coursecategory/edit/:id",
    meta: {
      canDelete: true,
      label: "Course categories",
    },
  },
];
