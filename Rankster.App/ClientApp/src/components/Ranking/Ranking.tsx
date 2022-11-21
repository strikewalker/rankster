import CloseIcon from "@mui/icons-material/Close";
import ThumbDownIcon from "@mui/icons-material/ThumbDown";
import ThumbUpIcon from "@mui/icons-material/ThumbUp";
import {
    Alert, Avatar, Badge, Button, Dialog, DialogContent, DialogTitle, IconButton, ListItemAvatar, useTheme
} from "@mui/material";
import Box from "@mui/material/Box";
import { grey } from '@mui/material/colors';
import Grid from "@mui/material/Grid";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import ListItemText from "@mui/material/ListItemText";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";
import * as React from "react";
import FlipNumbers from "react-flip-numbers";
import { Flipped, Flipper } from "react-flip-toolkit";
import { useParams } from "react-router-dom";
import { RanksterFooter } from "../Common";
import Invoice from "./Invoice";
import { formatCurrency, useInterval } from "./utils";

import connect from '../../util/Connector';
import newGuid from '../../util/newGuid';

import { VoteType } from '../../enums/Rankster'

const sessionId = newGuid();

function randomIntFromInterval(min: number, max: number) {
    // min and max included
    return Math.floor(Math.random() * (max - min + 1) + min);
}
type func = () => any;
const RankingComponent = () => {
    let { id } = useParams<{ id: string }>();
    id = id.toUpperCase();
    const [items, setItems] = React.useState<RankingItemModel[]>();
    const [rankster, setRankster] = React.useState<RanksterModel>();
    const [updates, setUpdates] = React.useState(0);
    const [selected, setSelected] = React.useState<RankingItemModel | null>();
    const [down, setDown] = React.useState(false);
    const [success, setSuccess] = React.useState(false);
    const setAndSortItems = (its: RankingItemModel[]) => {
        its.sort((a, b) => {
            if (a.votesUp - a.votesDown < b.votesUp - b.votesDown) return 1;
            if (a.votesUp - a.votesDown > b.votesUp - b.votesDown) return -1;
            return 0;
        });
        setItems(its);
        setUpdates(updates + 1);
    }
    React.useEffect(() => {
        let callback: Function;
        (async () => {
            const conn = await connect();
            const result = await conn.subscribeToRankster(id, setAndSortItems)
            if (result) {
                setAndSortItems(result.rankster.items);
                setRankster(result.rankster);
                callback = result.unload;
            }
        })();
        return () => callback && callback();
    }, []);
    //useInterval(() => {
    //    const its = items;
    //    its[randomIntFromInterval(0, its.length - 1)].votesUp += 3;
    //    its[randomIntFromInterval(0, its.length - 1)].votesDown += 1;
    //    its.sort((a, b) => {
    //        if (a.votesUp - a.votesDown < b.votesUp - b.votesDown) return 1;
    //        if (a.votesUp - a.votesDown > b.votesUp - b.votesDown) return -1;
    //        return 0;
    //    });
    //    setItems(its);
    //    setUpdates(updates + 1);
    //}, 1000);

    const closeDialog = (paid?: boolean) => {
        setSelected(null);
        if (paid) {
            setSuccess(true);
            setTimeout(() => setSuccess(false), 3000);
        }
    };

    const selectUp = (item: any) => {
        setSelected(item);
        setDown(false);
    };

    const selectDown = (item: any) => {
        setSelected(item);
        setDown(true);
    };

    if (!rankster) {
        return null;
    }

    var flipKey = items!.map((it) => it.id).join("");

    return (
        <>
            <Grid container justifyContent="center" spacing={0}>
                <Box sx={{ flexGrow: 1, maxWidth: 500 }}>
                    <Grid container spacing={2}>
                        <Grid item xs={12}>
                            <Typography
                                textAlign="center"
                                color="black"
                                sx={{ mt: 4, mb: 2, p: 1, background: grey[800], borderRadius: "10px" }}
                                variant="h5"
                                component="div"
                            >
                                rankster.me/<b style={{ fontSize: "1.1em" }}>{id}</b>
                            </Typography>
                            <Typography
                                textAlign="center"
                                sx={{ mt: 4, mb: 2 }}
                                variant="h4"
                                component="h1"
                            >
                                {rankster!.name}
                            </Typography>
                            <Typography
                                variant="h6"
                                sx={{ mt: 1, mb: 3 }}
                                component="div"
                            >
                                {rankster!.description}
                            </Typography>
                            <Typography
                                textAlign="center"
                                sx={{ mt: 1, mb: 2 }}
                                component="p"
                            >
                                Cast a vote for {" "}
                                <span style={{ fontWeight: "bold", color: "orange" }}>
                                    {formatCurrency({ amount: rankster!.costUsd })}
                                </span>{" "}
                                in Bitcoin
                            </Typography>
                            <Box>
                                {success && (
                                    <Alert severity="success">Vote counted successfully</Alert>
                                )}
                                <List>
                                    <Flipper
                                        flipKey={flipKey}
                                        className="staggered-list-content"
                                        spring="gentle"
                                    >
                                        {items!.map((it, index) => (
                                            <Flipped
                                                flipId={`avatar-${it.id}`}
                                                stagger="card-content"
                                                delayUntil={it.id}
                                            >
                                                <ListItem
                                                    key={it.id}
                                                    sx={{ mb: 1, pr: 1, border: "1px dashed gray" }}
                                                >
                                                    <ListItemAvatar>
                                                        <Avatar>{index + 1}</Avatar>
                                                    </ListItemAvatar>
                                                    <ListItemText
                                                        primary={it.name}
                                                        secondary={it.description}
                                                    />
                                                    <Badge
                                                        anchorOrigin={{
                                                            vertical: "bottom",
                                                            horizontal: "right"
                                                        }}
                                                        badgeContent={
                                                            null /*(it.votesUp - it.votesDown).toLocaleString()*/
                                                        }
                                                        max={1000000000}
                                                        color="secondary"
                                                    >
                                                        <Stack direction="row" spacing={0}>
                                                            <Button onClick={() => selectUp(it)}>
                                                                <Grid>
                                                                    <ThumbUpIcon color="success" />
                                                                    <Box>
                                                                        <FlipNumbers
                                                                            color="inherit"
                                                                            background="transparent"
                                                                            width={10}
                                                                            height={15}
                                                                            play
                                                                            numbers={it.votesUp.toLocaleString()}
                                                                        />
                                                                    </Box>
                                                                </Grid>
                                                            </Button>
                                                            <Button onClick={() => selectDown(it)}>
                                                                <Grid>
                                                                    <ThumbDownIcon color="error" />
                                                                    <Box>
                                                                        <FlipNumbers
                                                                            color="inherit"
                                                                            background="transparent"
                                                                            width={10}
                                                                            height={15}
                                                                            play
                                                                            numbers={it.votesDown.toLocaleString()}
                                                                        />
                                                                    </Box>
                                                                </Grid>
                                                            </Button>
                                                        </Stack>
                                                    </Badge>
                                                </ListItem>
                                            </Flipped>
                                        ))}
                                    </Flipper>
                                </List>
                                <Typography
                                    textAlign="center"
                                    sx={{ mt: 4, mb: 2 }}
                                    variant="h4"
                                    component="div"
                                >
                                    Raised:{" "}
                                    <span style={{ fontWeight: "bold", color: "orange" }}>
                                        <FlipNumbers
                                            color="inherit"
                                            background="transparent"
                                            width={30}
                                            height={35}
                                            play
                                            numbers={formatCurrency({ amount: sum(items!, rankster.costUsd) })}
                                        />
                                    </span>
                                </Typography>
                            </Box>
                        </Grid>
                    </Grid>
                </Box>
            </Grid>
            {selected && <SimpleDialog selected={selected} down={down} onClose={closeDialog} />}
            <RanksterFooter />
        </>
    );
}

export const sum = (items: RankingItemModel[], cost: number) =>
    items.reduce((accumulator, object) => {
        return accumulator + (object.votesUp + object.votesDown) * cost;
    }, 0);

export interface SimpleDialogProps {
    selected: RankingItemModel;
    down: boolean;
    onClose: (paid?: boolean) => void;
    theme?: any;
}

function SimpleDialog(props: SimpleDialogProps) {
    const { onClose, selected, down } = props;
    const theme = useTheme();

    return !selected ? null : (
        <Dialog onClose={() => onClose()} open={!!selected}>
            <DialogTitle>
                Vote with Bitcoin
                {" "}
                {onClose ? (
                    <IconButton
                        aria-label="close"
                        onClick={() => onClose()}
                        sx={{
                            position: "absolute",
                            right: 8,
                            top: 8,
                            color: (theme) => theme.palette.grey[500]
                        }}
                    >
                        <CloseIcon />
                    </IconButton>
                ) : null}
            </DialogTitle>
            <DialogContent>
                <Stack direction="column" spacing={2}>
                    <Typography component="div">
                        Pay below to vote <Typography component="span" fontWeight={ 700}>{selected.name}</Typography>{" "}
                        <span
                            style={{
                                color: down ? theme.palette.error.main : theme.palette.success.main,
                                fontWeight: "bold"
                            }}
                        >
                            {down ? "Down" : "Up"}
                        </span>
                    </Typography>
                    <Invoice voteType={(down ? VoteType.Down : VoteType.Up)} rankingItemId={selected.id} sessionId={sessionId} onClose={paid => onClose(paid)} />
                </Stack>
            </DialogContent>
        </Dialog>
    );
}
export default RankingComponent;

