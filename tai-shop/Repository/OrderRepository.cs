using Microsoft.EntityFrameworkCore;
using System;
using tai_shop.Data;
using tai_shop.Dtos;
using tai_shop.Dtos.Payment;
using tai_shop.Enums;
using tai_shop.Interfaces;
using tai_shop.Models;

namespace tai_shop.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IPaymentRepository _paymentRepository;

        public OrderRepository(ApplicationDbContext context, IPaymentRepository paymentRepository)
        {
            _context = context;
            _paymentRepository = paymentRepository;
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.ItemOrders)
                .ThenInclude(io => io.Item)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.ItemOrders)
                .ThenInclude(io => io.Item)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(string customerId)
        {
            return await _context.Orders
                .Include(o => o.ItemOrders)
                .ThenInclude(io => io.Item)
                .Where(o => o.UserId == customerId)
                .ToListAsync();
        }

        public async Task<Order> AddOrderAsync(Cart cart, AddressDto addressDto, ShippingMethod shippingMethod)
        {
            var shippingAddress = new Address
            {
                Street = addressDto.Street,
                City = addressDto.City,
                PostalCode = addressDto.PostalCode,
                Country = addressDto.Country
            };

            if (cart.UserId?.StartsWith("anon_") != true)
            {
                var user = await _context.Users
                    .Include(u => u.Address)
                    .FirstOrDefaultAsync(u => u.Id == cart.UserId);

                if (user.Address == null)
                {
                    shippingAddress.UserId = cart.UserId;
                    user.Address = shippingAddress;
                    _context.Update(user);
                }
                else
                {
                    shippingAddress = new Address
                    {
                        Street = user.Address.Street,
                        City = user.Address.City,
                        PostalCode = user.Address.PostalCode,
                        Country = user.Address.Country
                    };
                }
            }
            else
            {
                shippingAddress.FirstName = addressDto.FirstName;
                shippingAddress.LastName = addressDto.LastName;
            }

            if (_context.Entry(shippingAddress).State == EntityState.Detached)
            {
                _context.Addresses.Add(shippingAddress);
                await _context.SaveChangesAsync();
            }

            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                ShippingAddressId = shippingAddress.Id,
                ShippingMethod = shippingMethod,
                ItemOrders = cart.CartItems.Select(ci => new ItemOrder
                {
                    ItemId = ci.ItemId,
                    Quantity = ci.Quantity,
                    Price = ci.UnitPrice
                }).ToList()
            };

            if (cart.UserId?.StartsWith("anon_") != true)
            {
                order.UserId = cart.UserId;
            }
            else
            {
                order.AnonymousUserId = cart.UserId;
            }

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> UpdateOrderAsync(int orderId, Order order)
        {
            var existingOrder = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);

            if (existingOrder == null)
            {
                return null;
            }

            existingOrder.ItemOrders = order.ItemOrders;
            await _context.SaveChangesAsync();
            return existingOrder;
        }

        public async Task<Order?> DeleteOrderAsync(int orderId)
        {
            var order = await GetOrderByIdAsync(orderId);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<PaymentResponseDto> ProcessOrderPaymentAsync(int orderId, CreatePaymentDto createPaymentDto)
        {
            var order = await GetOrderByIdAsync(orderId);
            if (order == null)
            {
                throw new ArgumentException($"Order with ID {orderId} not found");
            }

            decimal totalAmount = order.FinalAmount;

            //if (createPaymentDto.Amount != totalAmount)
            //{
            //    throw new ArgumentException("Payment amount does not match order total");
            //}

            var paymentDto = new PaymentDto
            {
                OrderId = orderId,
                Amount = order.TotalAmount,
                PaymentMethod = createPaymentDto.PaymentMethod,
                CardNumber = createPaymentDto.CardNumber,
                ExpiryDate = createPaymentDto.ExpiryDate,
                CVV = createPaymentDto.CVV
            };

            var payment = await _paymentRepository.ProcessPayment(paymentDto);

            order.PaymentStatus = payment.Status;
            order.PaymentDate = payment.PaymentDate;

            if (payment.Status == PaymentStatus.Successful)
            {
                order.Status = OrderStatus.Processing;
            }
            else
            {
                order.Status = OrderStatus.PaymentFailed;
            }

            await _context.SaveChangesAsync();

            return new PaymentResponseDto
            {
                PaymentId = payment.Id,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                Status = payment.Status,
                PaymentDate = payment.PaymentDate,
                TransactionId = payment.TransactionId,
                FailureReason = payment.FailureReason
            };
        }

        public async Task<Payment> RefundOrderAsync(int orderId)
        {
            var order = await GetOrderByIdAsync(orderId);
            if (order == null)
            {
                throw new ArgumentException($"Order with ID {orderId} not found");
            }

            var payment = await _paymentRepository.RefundPayment(orderId);

            order.PaymentStatus = payment.Status;
            order.Status = OrderStatus.Refunded;

            await _context.SaveChangesAsync();
            return payment;
        }
    }
}
