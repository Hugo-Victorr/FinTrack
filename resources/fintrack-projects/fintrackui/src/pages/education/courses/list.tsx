import { List, Tab, Tabs } from "@mui/material"
import { useState } from "react";
import { VerticalTabPanel } from "../../../components/tabpanel/tabpanel";

export const ListCourses = () => {

    const [value, setValue] = useState(0);

    const handleChange = (event: React.SyntheticEvent, newValue: number) => {
        setValue(newValue);
    };

    return (

        <Tabs value={value} onChange={handleChange} orientation="vertical" centered>
            <Tab label="Learning Plans" />
            <Tab label="Available courses" />
            <Tab label="My training" />
            <List>
                
            </List>
        </Tabs>
    );

}