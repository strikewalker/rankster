export { };
declare global {
    interface LightningInvoiceModel {
        voteId: string;
        lnInvoiceId: string;
        expirationInSeconds: number;
        costUsd: number;
    }
}