import React from "react";
import { type HttpError, useMany } from "@refinedev/core";
import { List, useDataGrid, DateField, ShowButton, EditButton, DeleteButton } from "@refinedev/mui";
import { DataGrid, GridColDef } from "@mui/x-data-grid";
import { Button, Stack } from "@mui/material";
import { CreateCourseCategoryDrawer } from "./create";
import { useModalForm } from "@refinedev/react-hook-form";

export const CourseCategoryList = () => {
    const { dataGridProps } = useDataGrid();

    const createDrawerFormProps = useModalForm <HttpError>(
        {
            refineCoreProps: { action: "create" },
            syncWithLocation: true,
        },
    );
    const {
        modal: { show: showCreateDrawer },
    } = createDrawerFormProps;

    const columns = React.useMemo<GridColDef<any>[]>(
        () => [
            {
                field: "id",
                headerName: "Id",
                type: "number",
                minWidth: 50,
            },
            {
                field: "name",
                headerName: "Name",
                minWidth: 200,
            },
            {
                field: "description",
                headerName: "description",
                minWidth: 200,
            },
            {
                field: "createdAt",
                headerName: "Created At",
                minWidth: 250,
                display: "flex",
                renderCell: function render({ value }) {
                    return <DateField value={value} />;
                },
            },
            {
                    field: "actions",
                    filterable: false,
                    hideable: false,
                    headerName: "Actions",
                    align: "right",
                    headerAlign: "right",
                    minWidth: 120,
                    sortable: false,
                    display: "flex",
                    renderCell: function render({ row }) {
                      return (
                        <>
                          <EditButton hideText recordItemId={row.id} />
                          <DeleteButton hideText recordItemId={row.id} />
                        </>
                      );
                    },
                  },
        ], []
    );

    return (
        <List createButtonProps={{ onClick: () => showCreateDrawer() }} >
            <DataGrid {...dataGridProps} columns={columns} />
            <CreateCourseCategoryDrawer {...createDrawerFormProps} />
        </List>
    );
};

