using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Entities.Products;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Service.Contract;
using Talabat.Core.Spicifications.Order_Spec;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        ///private readonly IGenaricRepositoriy<Product> _productrepo;
        ///private readonly IGenaricRepositoriy<DeliveryMethod> _deliveryrepo;
        ///private readonly IGenaricRepositoriy<Order> _ordersrepo;

        public OrderService(
            IBasketRepository basketRepository,
            IUnitOfWork unitOfWork)

        ///IGenaricRepositoriy<Product> productrepo,
        ///IGenaricRepositoriy<DeliveryMethod> deliveryrepo,
        ///IGenaricRepositoriy<Order> ordersrepo
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;

            ///_productrepo = productrepo;
            ///_deliveryrepo = deliveryrepo;
            ///_ordersrepo = ordersrepo;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            // 1. Get Basket From Basket Repo.
            var basket = await _basketRepository.GetBasketAsync(basketId);

            // 2. Get Selected Items at Basket From Products Repo.

            var orderItems = new List<OrderItem>();

            if(basket?.Items?.Count > 0)
            {
                var productRepository = _unitOfWork.Repositoriy<Product>();
                foreach (var item in basket.Items)
                {
                    var product = await productRepository.GetByIdAsync(item.Id);
                    if (product == null)
                        throw new Exception($"Product with Id {item.Id} not found");

                    var productItemOrdered = new ProductItemOrdered(item.Id, product.Name, product.PictureUrl);
                    
                    var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);

                    orderItems.Add(orderItem);
                }
            }

            // 3. Calac SubTotal.

            var subTotal = orderItems.Sum(orderItems => orderItems.Price * orderItems.Quantity);

            // 4. Get Delivery Method From DeliveryMethod Repo.

            var deliveryMethod = await _unitOfWork.Repositoriy<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            if (deliveryMethod == null)
                throw new Exception("Invalid delivery method");


            // 5. Create Order.

            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subTotal);

            await _unitOfWork.Repositoriy<Order>().AddAsync(order);

            // 6. Save To Database [TODO]

            var result =  await _unitOfWork.CompleteAsync();  

            if(result <= 0) return null;

            return order;
        }

      
        public async Task<IReadOnlyList<Order>> GetOrdersForUsersAsync(string buyerEmail)
        {
            var ordersRepo = _unitOfWork.Repositoriy<Order>();

            var spec = new OrderSpecification(buyerEmail);

            var orders = await ordersRepo.GetAllWithSpecAsync(spec);

            return orders;
        }

        public Task<Order?> GetOrderByIdAsync(int orderId, string buyerEmail)
        {
            var ordersRepo = _unitOfWork.Repositoriy<Order>();

            var orderSpec = new OrderSpecification(orderId, buyerEmail);

            var order = ordersRepo.GetByIdWithSpecAsync(orderSpec);

            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDiliveryMethodsAsync()
          => await _unitOfWork.Repositoriy<DeliveryMethod>().GetAllAsync();
    }
}
