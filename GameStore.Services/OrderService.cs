﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameStore.Data.Context;
using GameStore.Data.Models;
using GameStore.Services.Abstract;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Services
{
    public class OrderService : IOrderService
    {
        private readonly GameStoreContext storeContext;

        public OrderService(GameStoreContext storeContext)
        {
            this.storeContext = storeContext ?? throw new ArgumentNullException(nameof(storeContext));
        }

        public async Task<Order> AddToOrderAsync(string accountId, int productId)
        {
            var tempOrder = await CreateOrderAsync(accountId);

            var newOrderProducts = new OrderProducts
            {
                OrderId = tempOrder.Id,
                ProductId = productId
            };
            await storeContext.OrdersProducts.AddAsync(newOrderProducts);
            await storeContext.SaveChangesAsync();
            return tempOrder;
        }

        public async Task<Order> AddToOrderAsync(string accountId, IEnumerable<Product> products)
        {
            
            Order tempOrder = await CreateOrderAsync(accountId);
            foreach (var tempProduct in products)
            {
                tempOrder = await FindLastOrderAsync(accountId);
                if (tempOrder.OrderProducts.Any(p => p.ProductId == tempProduct.Id)) continue;

                var newOrderProducts = new OrderProducts
                {
                    OrderId = tempOrder.Id,
                    ProductId = tempProduct.Id
                };
                storeContext.OrdersProducts.Add(newOrderProducts);
            }

            await storeContext.SaveChangesAsync();
            return tempOrder;
        }

        public async Task<IEnumerable<Order>> FindOrdersAsync(string accountId)
        {
            return await storeContext.Orders.Where(o => o.AccountId == accountId).ToListAsync();
        }

        public async Task<Order> FindLastOrderAsync(string accountId)
        {
            return await storeContext.Orders.Where(o => o.AccountId == accountId).OrderBy(o => o.OrderTimestamp).FirstAsync();
        }

        /// <summary>
        ///     Create a new blank order for the given account.
        ///     For 'private' usage.
        /// </summary>
        /// <param name="account">Account (type)</param>
        /// <returns></returns>
        private async Task<Order> CreateOrderAsync(string accountId)
        {
            var newOrder = new Order
            {
                AccountId = accountId,
                OrderTimestamp = DateTime.Now
            };
            await storeContext.Orders.AddAsync(newOrder);
            await storeContext.SaveChangesAsync();

            return newOrder;
        }
    }
}