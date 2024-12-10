export const newProjectConstants = {
    cryptocurrencies: ["Bitcoin (BTC)", "Ethereum (ETH)", "Binance Coin (BNB)", "Ripple (XRP)", "Cardano (ADA)", "Solana (SOL)", "USD Coin (USDC)", "Dogecoin (DOGE)", "Polkadot (DOT)", "Litecoin (LTC)"],
    investmentType: ["Short-term (Day Trading)", "Medium-term (Swing Trading)", "Long-term (HODLing)"],
    statuses: {
        empty: "Empty form",
        created: "Created",
        closed: "Project closed",
        rejected: "Project rejected"
    },
    requiredFields: ['projectName', 'cryptocurrency', 'startDate', 'endDate', 'investmentAmount', 'investmentType','investmentStrategy']
}