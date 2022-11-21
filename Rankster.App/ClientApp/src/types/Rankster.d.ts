export { };
declare global {
    interface RanksterModel {
        name: string;
        description: string;
        items: RankingItem[];
        costUsd: number;
    }
    interface RankingItemModel {
        id: string;
        name: string;
        description: string;
        votesUp: number;
        votesDown: number;
    }
}