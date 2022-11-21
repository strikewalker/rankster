import * as signalR from "@microsoft/signalr";
import { VoteType } from '../enums/Rankster'
const URL = "/hub"; //or whatever your backend port is

class Connector {
    private connection: signalR.HubConnection;
    static instance: Connector;
    constructor() {
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(URL)
            .configureLogging(signalR.LogLevel.Debug)
            .withAutomaticReconnect()
            .build();
    }

    start = async () => {
        await this.connection.start();
    }

    subscribeToRankster = async (code: string, callback: (items: RankingItemModel[]) => any) => {
        if (this.connection.state !== signalR.HubConnectionState.Connected)
            throw Error("Not connected. Refresh and try again later")
        const result: RanksterModel = await this.connection.invoke('SubscribeToRankster', code);

        if (!result) {
            return null;
        }

        this.connection.on('ItemsUpdated', callback);
        return {
            unload: () => {
                this.connection.invoke('UnsubscribeFromRankster', code);
                this.connection.off('ItemsUpdated', callback);
            },
            rankster: result
        }
    }

    subscribeToVote = async (rankItemId: string, sessionKey: string, voteType: VoteType, callback: () => any) => {
        if (this.connection.state !== signalR.HubConnectionState.Connected)
            throw Error("Not connected. Refresh and try again later")
        const result: LightningInvoiceModel = await this.connection.invoke('SubscribeToVote', rankItemId, sessionKey, voteType);

        if (!result) {
            return null;
        }

        const votePaid = (voteId: string) => {
            if (voteId !== result.voteId)
                return;
            callback();
        }

        this.connection.on('VotePaid', votePaid);

        return {
            unload: () => {
                this.connection.invoke('UnsubscribeFromVote', result.voteId);
                this.connection.off('VotePaid', votePaid);
            },
            lnInvoice: result
        }
    }

    public static async getInstance() {
        if (!Connector.instance) {
            var instance = new Connector();
            await instance.start();
            Connector.instance = instance;
        }
        return Connector.instance;
    }
}
export interface IConnector extends Connector {
}
export default Connector.getInstance;