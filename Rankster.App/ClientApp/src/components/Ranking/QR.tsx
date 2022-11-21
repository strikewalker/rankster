import * as React from "react";
//import qrCodeConfig from "./qrCodeConfig";
import styles from "./QRCode.module.css";
import QRCodeReact from 'qrcode.react';
import lightning from '../../images/lightning.svg';
import { Box, Link, Grid } from "@mui/material";

const QRCode: React.FC<{ data: string, color: string, expired?: boolean, animationDuration: number, onClick?: (a: any) => any }> = ({
    data,
    color,
    expired,
    animationDuration,
    onClick,
}) => {
    const size = 280;

    const imageSettings = {
        src: lightning,
        height: 48,
        width: 48,
        excavate: true,
    };
    return (
        <Box position="relative" width={280}>
            <Box position="absolute">
                <a href={`lightning:${data}`}>
                    <svg width={size} height={size} viewBox={`0 0 ${size} ${size}`}>
                        <rect
                            x="2"
                            y="2"
                            width={size - 4}
                            height={size - 4}
                            fill="none"
                            stroke={color}
                            strokeWidth="4"
                            rx="28"
                        />
                    </svg>
                    <Box position="absolute" top={0} left={0}>
                        <svg width={size} height={size} viewBox={`0 0 ${size} ${size}`}>
                            <rect
                                className={styles.rect}
                                x="2"
                                y="2"
                                width={size - 4}
                                height={size - 4}
                                fill="none"
                                stroke="#1A1A1A"
                                strokeWidth="6"
                                strokeDashoffset={size * 4}
                                strokeDasharray={size * 4}
                                rx="28"
                                style={{
                                    animationDuration: `${animationDuration}s`,
                                }}
                            />
                        </svg>
                    </Box>
                </a>
            </Box>

            <Grid container justifyContent="center"
                sx={{ background: "#1a1a1a", borderRadius: "30px" }}
                width={size}
                height={size}
                alignItems="center" spacing={0}>
                <Box sx={{background:"white"}} borderRadius={8} boxSizing="border-box" className={(expired ? styles.expired : undefined)} padding={2}>
                    <QRCodeReact value={data} size={208} imageSettings={imageSettings} renderAs="svg" />
                </Box>
            </Grid>
        </Box>
    );
}
export default QRCode;