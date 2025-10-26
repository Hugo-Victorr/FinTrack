import { Box, Card, CardContent, Typography } from "@mui/material"
import { Show } from "@refinedev/mui"

export const ShowCourse = () => {
    <Show>
        {/* Title | Duration | Category  */}
        
        <Card sx={{ minWidth: 275 }}>
            <CardContent>
                <Typography variant="h5" component="div">
                    Description
                </Typography>

                {/* text */}
            </CardContent>
        </Card>

        <Card sx={{ minWidth: 275 }}>
            <CardContent>
                <Typography variant="h5" component="div">
                    Objectives
                </Typography>

                {/* text */}
            </CardContent>

            <CardContent>
                <Typography variant="h5" component="div">
                    Requirements
                </Typography>

                {/* text */}
            </CardContent>
        </Card>
    </Show>
}