import * as React from 'react'
const normalizeCurrency = (currency: string) => {
    switch (currency) {
        case "USDT":
            return "USD";
        default:
            return currency;
    }
};

export const formatCurrency = ({ amount, currency = "USD", locales = "en" }: { amount: number, currency?: string, locales?: string }) => {
    return new Intl.NumberFormat(locales, {
        style: "currency",
        currency: normalizeCurrency(currency),
    })
        .format(amount);
};


export function randomIntFromInterval(min:number, max:number) {
    // min and max included
    return Math.floor(Math.random() * (max - min + 1) + min);
}

export function useInterval(callback:Function, delay?: number) {
    const intervalRef = React.useRef<number>();
    const savedCallback = React.useRef(callback);
    React.useEffect(() => {
        savedCallback.current = callback;
    }, [callback]);
    React.useEffect(() => {
        const tick = () => savedCallback.current();
        if (typeof delay === "number") {
            intervalRef.current = window.setInterval(tick, delay);
            return () => window.clearInterval(intervalRef.current);
        }
    }, [delay]);
    return intervalRef;
}
