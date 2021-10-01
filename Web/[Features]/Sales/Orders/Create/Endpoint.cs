﻿using FastEndpoints;
using Web.PipelineBehaviors.PostProcessors;
using Web.PipelineBehaviors.PreProcessors;
using Web.SystemEvents;

namespace Sales.Orders.Create
{
    public class Endpoint : Endpoint<Request, Response>
    {
        public Endpoint()
        {
            Verbs(Http.POST);
            Routes("/sales/orders/create");
            PreProcessors(
                new MyRequestLogger<Request>());
            PostProcessors(
                new MyResponseLogger<Request, Response>());
        }

        protected override async Task HandleAsync(Request r, CancellationToken t)
        {
            var saleNotification = new NewOrderCreated
            {
                CustomerName = "new customer",
                OrderID = Random.Shared.Next(0, 10000),
                OrderTotal = 12345.67m
            };

            await Event<NewOrderCreated>.PublishAsync(saleNotification, Mode.WaitForNone);

            await SendAsync(new Response
            {
                Message = "order created!",
                OrderID = 54321
            });
        }
    }
}