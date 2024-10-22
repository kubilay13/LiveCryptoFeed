﻿using Binance.Net.Clients;
using BlockChainAI;
using Microsoft.AspNetCore.SignalR;

namespace LiveCryptoFeed.Services
{
    public class BinanceService
    {
        private readonly BinanceSocketClient _binanceSocketClient;
        private readonly IHubContext<PriceUpdateHub> _hubContext;

        public event Action<decimal> OnBtcPriceUpdated;
        public event Action<decimal> OnEthPriceUpdated;
        public event Action<decimal> OnBnbPriceUpdated;
        public event Action<decimal> OnTrxPriceUpdated;
        public event Action<decimal> OnSolPriceUpdated;
        public event Action<decimal> OnLtcPriceUpdated;
        public event Action<decimal> OnTonPriceUpdated;
        public event Action<decimal> OnArbPriceUpdated;
        public event Action<decimal> OnXrpPriceUpdated;
        public event Action<decimal> OnUsddPriceUpdated;
        public event Action<decimal> OnDogePriceUpdated;
        public event Action<decimal> OnPepePriceUpdated;
        public event Action<decimal> OnBchPriceUpdated;
        public event Action<decimal> OnXautPriceUpdated;
        public event Action<decimal> OnUsdcPriceUpdated;

        public BinanceService(BinanceSocketClient binanceSocketClient, IHubContext<PriceUpdateHub> hubContext)
        {
            _binanceSocketClient = binanceSocketClient;
            _hubContext = hubContext;
        }

        private List<string> _allCoins = new List<string>
        {
            "BTCUSDT", "ETHUSDT", "BNBUSDT", "TRXUSDT",
            "SOLUSDT", "LTCUSDT", "TONUSDT", "ARBUSDT",
            "XRPUSDT", "DOGEUSDT", "PEPEUSDT", "BCHUSDT",
            "USDCUSDT","XAUTUSDT","USDDUSDT"
        };

        public async Task SubscribeToPriceUpdatesAsync()
        {
            foreach (var coin in _allCoins)
            {
                await SubscribeToTickerUpdates(coin);
            }
        }

        public async Task SubscribeToTickerUpdates(string symbol)
        {
            var result = await _binanceSocketClient.SpotApi.ExchangeData.SubscribeToTickerUpdatesAsync(symbol, (update) =>
            {
                var lastPrice = update.Data.LastPrice;
                InvokeEvent(symbol, lastPrice);
            });

            if (result.Success)
            {
                Console.WriteLine($"Successfully subscribed to {symbol}");
            }
            else
            {
                Console.WriteLine($"Subscription failed for {symbol}: {result.Error}");
            }
        }

        private void InvokeEvent(string symbol, decimal price)
        {
            switch (symbol)
            {
                case "BTCUSDT":
                    OnBtcPriceUpdated?.Invoke(price);
                    break;
                case "ETHUSDT":
                    OnEthPriceUpdated?.Invoke(price);
                    break;
                case "BNBUSDT":
                    OnBnbPriceUpdated?.Invoke(price);
                    break;
                case "TRXUSDT":
                    OnTrxPriceUpdated?.Invoke(price);
                    break;
                case "SOLUSDT":
                    OnSolPriceUpdated?.Invoke(price);
                    break;
                case "LTCUSDT":
                    OnLtcPriceUpdated?.Invoke(price);
                    break;
                case "TONUSDT":
                    OnTonPriceUpdated?.Invoke(price);
                    break;
                case "ARBUSDT":
                    OnArbPriceUpdated?.Invoke(price);
                    break;
                case "XRPUSDT":
                    OnXrpPriceUpdated?.Invoke(price);
                    break;
                case "DOGEUSDT":
                    OnDogePriceUpdated?.Invoke(price);
                    break;
                case "PEPEUSDT":
                    OnPepePriceUpdated?.Invoke(price);
                    break;
                case "BCHUSDT":
                    OnBchPriceUpdated?.Invoke(price);
                    break;
                case "USDCUSDT":
                    OnUsdcPriceUpdated?.Invoke(price);
                    break;
                case "XAUTUSDT":
                    OnXautPriceUpdated?.Invoke(price);
                    break;
                case "USDDUSDT":
                    OnUsddPriceUpdated?.Invoke(price);
                    break;
            }
            _hubContext.Clients.All.SendAsync("ReceivePriceUpdate", symbol, price);
        }
    }
}
