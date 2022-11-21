import * as React from 'react';
import { Grid, Button, Stack, Divider } from "@mui/material";
export const orangeColor = "#fdaa26";
export const isTest = !!["localhost", "test"].find(f => window.location.host.indexOf(f) > -1);

export const RanksterFooter = () => (
    <Grid container justifyContent="center" spacing={0}>
        <Stack direction="row" spacing={1}>
            <Button sx={{color:"text.disabled"}}
                href="https://strike.me/en/legal/privacy"
            >
                Privacy Notice
            </Button>
            <Button sx={{color:"text.disabled"}}
                href="https://strike.me/en/legal/tos"
            >
                Terms of Service
            </Button>
            <Button sx={{color:"text.disabled"}}
                href="mailto:rankster@reacher.me"
            >
                Support
            </Button>
            <Button sx={{color:"text.disabled"}}
                href="https://github.com/strikewalker/rankster"
            >
                GitHub
            </Button>
        </Stack>
    </Grid>
);

