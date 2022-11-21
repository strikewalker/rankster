import * as React from "react";
import bitcoin from '../../images/bitcoin.svg';
import QRCode from "./QR";
import { formatCurrency } from "./utils";
import { Stack, Typography, Box, Link, Button, Grid } from "@mui/material";
import Image from 'material-ui-image'
import connect from '../../util/Connector';
import { createLnInvoice } from './rankingRepo'
import { VoteType } from '../../enums/Rankster'

const userColor = "#fdaa26";

interface InvoiceProps {
    onClose(paid?: boolean): any;
    rankingItemId: string;
    sessionId: string;
    voteType: VoteType;
}

const InvoiceComponent: React.FC<InvoiceProps> = ({ onClose, rankingItemId, sessionId, voteType }) => {
    const [error, setError] = React.useState<boolean>();
    const [isExpired, setExpired] = React.useState<boolean>();
    const [copied, setCopied] = React.useState<boolean>();
    const [lnInvoice, setLnInvoice] = React.useState<LightningInvoiceModel>();

    const setPaid = () => {
        onClose(true);
    }

    React.useEffect(() => {
        let callback: Function;
        (async () => {
            const conn = await connect();
            const result = await conn.subscribeToVote(rankingItemId, sessionId, voteType, setPaid)
            if (result) {
                setLnInvoice(result.lnInvoice);
                callback = result.unload;
            }
        })();
        return () => callback && callback();
    }, []);

    const refreshLnInvoice = async () => {
        const result = await createLnInvoice(rankingItemId, sessionId);
        setExpired(false);
        result.expirationInSeconds -= 10;
        setLnInvoice(result);
        setTimeout(() => setExpired(true), (result.expirationInSeconds) * 1000)
    }

    if (error) {
        return <Typography variant="h5" mb={4}>
            Error occured. Please try again later.
        </Typography>
    }
    else if (!lnInvoice) {
        return <Typography mb={4} sx={{ textAlign: "center" }}>
            Loading details. Please wait...
        </Typography>
    }
    else {
        return <Grid container justifyContent="center" spacing={0}>
            <Stack spacing={2} alignItems="center">
                <Typography variant="caption" color="text.secondary" display="block">
                    Just <b>scan</b> or <b>click</b> the QR code below
                </Typography>
                <Typography variant="caption" display="block">
                    Amount: <Typography component="span" color="orange">{formatCurrency({ amount: lnInvoice.costUsd })}</Typography>
                </Typography>
                <Box mt={[3, 0]}>
                    <a href={`lightning:${lnInvoice.lnInvoiceId}`}>
                        {lnInvoice.expirationInSeconds && (
                            <QRCode key={lnInvoice.lnInvoiceId}
                                color={userColor}
                                expired={isExpired}
                                data={lnInvoice.lnInvoiceId}
                                animationDuration={lnInvoice.expirationInSeconds}
                            />
                        )}
                    </a>
                </Box>
                <Typography variant='caption' color="text.secondary" display="block">
                    or paste it in your <Image src={bitcoin} height="1.3rem" style={{ display: "inline" }} /> Bitcoin lightning wallet
                </Typography>
                <Box>
                    {isExpired ? (
                        <Button key="refresh" onClick={refreshLnInvoice}>
                            Refresh
                        </Button>
                    ) : (
                        <Button key="copy"
                            onClick={() => {
                                navigator.clipboard.writeText(lnInvoice.lnInvoiceId);
                                setCopied(true);
                                setTimeout(() => setCopied(false), 2000);
                            }}
                        >
                            {copied ? "Copied" : "Copy To Clipboard"}
                        </Button>
                    )}
                </Box>
                <Typography variant="caption" sx={{ textAlign: "center" }}>
                    New to <Image src={bitcoin} height="1.3rem" style={{ display: "inline" }} /> Bitcoin?{" "}
                    <Link
                        href="https://invite.strike.me/5AL8KE"
                        sx={{ color: userColor }} target="_blank"
                    >
                        Click here
                    </Link>{" "}
                    to get started with <b>Strike</b>.
                </Typography>
            </Stack>
        </Grid>
    }
}
export default InvoiceComponent;