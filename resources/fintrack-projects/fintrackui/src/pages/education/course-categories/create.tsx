import { Create, useAutocomplete } from "@refinedev/mui";
import Autocomplete from "@mui/material/Autocomplete";
import Box from "@mui/material/Box";
import Drawer from "@mui/material/Drawer";
import IconButton from "@mui/material/IconButton";
import TextField from "@mui/material/TextField";
import type { UseModalFormReturnType } from "@refinedev/react-hook-form";
import { Controller } from "react-hook-form";
import CloseOutlined from "@mui/icons-material/CloseOutlined";
import type { HttpError } from "@refinedev/core";


export const CreateCourseCategoryDrawer: React.FC<
    UseModalFormReturnType<HttpError>
> = ({
    saveButtonProps,
    modal: { visible, close },
    register,
    formState: { errors },
}) => {
        return (
            <Drawer
                open={visible}
                onClose={close}
                anchor="right"
                PaperProps={{ sx: { width: { sm: "100%", md: 500 } } }}
            >
                <Create
                    saveButtonProps={saveButtonProps}
                    headerProps={{
                        action: (
                            <IconButton
                                onClick={() => close()}
                                sx={{ width: "30px", height: "30px" }}
                            >
                                <CloseOutlined />
                            </IconButton>
                        ),
                        avatar: null,
                    }}
                >
                    <Box
                        component="form"
                        autoComplete="off"
                        sx={{ display: "flex", flexDirection: "column" }}
                    >
                        <TextField
                            id="name"
                            {...register("Name", {
                                required: "This field is required",
                            })}
                            error={!!errors.title}
                            // helperText={errors.title?.message}
                            margin="normal"
                            fullWidth
                            label="Name"
                            name="name"
                        />
                        
                        <TextField
                            id="description"
                            {...register("description", {
                                required: "This field is required",
                            })}
                            error={!!errors.content}
                            // helperText={errors.content?.message}
                            margin="normal"
                            label="Description"
                            multiline
                            rows={4}
                        />
                    </Box>
                </Create>
            </Drawer>
        );
    };
