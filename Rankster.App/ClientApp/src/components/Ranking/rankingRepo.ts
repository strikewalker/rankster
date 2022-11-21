export async function createLnInvoice(rankingItemId: string, sessionId:string) {
    const response = await fetch(`/api/invoice/lnInvoice?rankingItemId=${rankingItemId}&sessionId=${sessionId}`, { method: "post" });
    return response.json() as Promise<LightningInvoiceModel>;
}
export async function getRankster(code: string) {
    const response = await fetch(`/api/rankster/${code}`);
    return response.json() as Promise<RanksterModel>;
}