using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using eCommerce_API.Data;
using eCommerce_API.Dto;
using eCommerce_API.Models;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using eCommerce_API.Services;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using NLog.Web;
using AutoMapper;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace eCommerce_API.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        rst374_cloud12Context _context = new rst374_cloud12Context();
        private ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }

        [HttpGet()]
        public IActionResult getOrders([FromQuery] int id, [FromQuery] bool? invoiced, [FromQuery] bool? paid,[FromQuery] int? status, [FromQuery] string customer,
            [FromQuery] DateTime start, [FromQuery] DateTime end, [FromQuery] string keyword)
        {
            var filter = new OrderFilterDto();
            filter.id = id;
            filter.inoviced = invoiced;
            filter.paid = paid;
            filter.status = status;
            filter.customer = customer;
            if(start != DateTime.MinValue )
                filter.start = start;
            if(end != DateTime.MinValue)
                filter.end = end;
            filter.keyword = keyword;

            var orderList = orderlist(filter);
            return Ok(orderList);
        }

        private List<OrderDto> orderlist([FromBody] OrderFilterDto filter)
        {

            _context.ChangeTracker.QueryTrackingBehavior
                = Microsoft.EntityFrameworkCore.QueryTrackingBehavior.NoTracking;

            var orderList = (from o in _context.Orders
                             join i in _context.Invoice on o.InvoiceNumber equals i.InvoiceNumber into oi
                             from i in oi.DefaultIfEmpty()

                             join si in _context.ShippingInfo on o.Id equals si.orderId into sio
                             from si in sio.DefaultIfEmpty()

                             where o.CardId == filter.id
                              //&& (filter.paid != null ? i.Paid == filter.paid : true)
                              //&& (filter.status != null ? i.Status == filter.status : true)
                              //&& (filter.start != null ? o.RecordDate >= filter.start : true) && (filter.end != null ? o.RecordDate <= filter.end : true)
                              //&& (filter.keyword != null ? i.InvoiceNumber.ToString().Contains(filter.keyword) || o.PoNumber.ToString().Contains(filter.keyword) : true)
                             select new OrderDto
                             {
                                 id= o.Id,
                                 card_id = o.CardId,
                                 branch = o.Branch,
                                 po_number = o.PoNumber,
                                 status = o.Status.ToString(),
                                 invoice_number = o.InvoiceNumber,
                                 TotalAmount_GstIncl = i.Total,
                                 TotalAmount_GSTExcl = i.Price,
                                 GstAmount = i.Tax,
                                 record_date = o.RecordDate,
                                 shipto = o.Shipto,
                                 special_shipto = o.SpecialShipto,
                                 date_shipped = o.DateShipped,
                                 freight = o.Freight,
                                 ticket = o.Ticket,
                                 shipping_method = o.ShippingMethod,
                                 payment_type = o.PaymentType,
                                 paid = o.Paid,
                                 receiver_name = si.receiver,
                                 receiver_phone = si.receiver_phone,
                                 is_web_order = true,
                                 web_order_status = o.WebOrderStatus
                             })
                             .ToList();

            foreach (var o in orderList)
            {
                try
                {
                    if (o.web_order_status == 0)
                        o.web_order_status = 1;
                    o.status = getOrderStatus(o.web_order_status);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical($"error occur when changing order status!",ex);
                    
                }
            }

            return orderList;
        }

        private string getOrderStatus(int status_id)
        {
            //if (status_id == null)
            //    return "1";
            var status = _context.Enum.Where(e => e.Id == status_id && e.Class =="web_order_status").FirstOrDefault().Name;
            return status;
        }

        [HttpPut("shipping/{order_id}")]
        public async Task<IActionResult> updateOrderShipping(int? order_id)
        {
            var orderToUpdate = _context.Orders.Where(o => o.Id == order_id).FirstOrDefault();
            if (orderToUpdate == null)
                return NotFound();
            var shippingStatus = orderToUpdate.Status;
            if (shippingStatus == 5)
                orderToUpdate.Status = 6;    //from shipping to received
            else if (shippingStatus == 6)
                orderToUpdate.Status = 5;    //from received to shipping
            try {
                _context.Update(orderToUpdate);
                await _context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                throw e;
            }
            return NoContent();
        }

        [HttpGet("{order_id}")]
        public async Task<IActionResult> orderDetail(int? order_id)
        {
            if (order_id == null)
            {
                _logger.LogInformation($"Order with id {order_id} was null.");
                return NotFound();
            }

            if (!await _context.Orders.AnyAsync(o => o.Id == order_id))
            {
                _logger.LogInformation($"Order with id {order_id} wasn't found.");
                return NotFound();
            }
            else
            {
                _logger.LogInformation($"Order with id {order_id} was found.");
            }

            try
            {
                var orderDetail = new OrderDetailDto();

                var myOrder = _context.Orders.Where(o => o.Id == order_id)
                    .Include(b => b.shippinginfo)
                    .Include(b=> b.invoiceFreight)
                    .Join(_context.Invoice.Select(i=>new { i.InvoiceNumber, i.Paid, i.PaymentType,i.Freight, i.Total,i.Tax,i.Price}),
                    (b=>b.InvoiceNumber),
                    (i=>i.InvoiceNumber),
                    (b,i)=> new {b.shippinginfo,b.invoiceFreight, b.Id, b.InvoiceNumber,b.CustomerGst, b.CardId, i.Freight, OrderTotal = i.Price, i.Total, i.Tax, b.WebOrderStatus, i.Paid, i.PaymentType  })

                    .Join(_context.Enum.Where(e=>e.Class == "payment_method"),
                    (b=>(int)b.PaymentType),
                    (e=>e.Id),
                    (b, e) => new { b.shippinginfo, b.invoiceFreight, b.Id, b.InvoiceNumber, b.CustomerGst, b.CardId, b.Freight, b.OrderTotal,b.Total, b.Tax, b.WebOrderStatus, b.Paid, PaymentType = e.Name })

                    .Join(_context.Enum.Where(e => e.Class == "web_order_status"),
                    (b => b.WebOrderStatus),
                    (e => e.Id),
                    (b, e) => new { b.shippinginfo, b.invoiceFreight, b.Id, b.InvoiceNumber, b.CustomerGst, b.CardId, b.Freight, b.OrderTotal, b.Total, b.Tax, WebOrderStatus = e.Name, b.Paid, b.PaymentType })

                    .FirstOrDefault();
                    if (myOrder == null)
                    {
                        _logger.LogInformation($"Order with id {order_id} was null.");
                        return NotFound();
                    }

                var customerGst = myOrder.CustomerGst;

                var orderItem = _context.OrderItem.Where(o => o.Id == order_id)
                    .Select(oi => new OrderItemDto
                    {
                        Kid = oi.Kid,
                        Id = oi.Id,
                        Code = oi.Code,
                        SupplierCode = oi.SupplierCode,
                        Quantity = oi.Quantity,
                        ItemName = oi.ItemName,
                        ItemNameCn = oi.ItemNameCn,
                        CommitPrice = oi.CommitPrice,
                        PriceGstInc = Math.Round(oi.CommitPrice * Convert.ToDecimal(customerGst),2),
                        Cat = oi.Cat
                    }).ToListAsync();

                var shippingInfo = myOrder.shippinginfo
                    .Select(s => new ShippingInfoDto
                    {
                        id = s.id,
                        sender = s.sender,
                        orderId = s.orderId,
                        sender_phone = s.sender_phone,
                        sender_address = s.sender_address,
                        sender_city = s.sender_city,
                        sender_country = s.receiver_country,
                        receiver = s.receiver,
                        receiver_company = s.receiver_company,
                        receiver_address1 = s.receiver_address1,
                        receiver_address2 = s.receiver_address2,
                        receiver_address3 = s.receiver_address3,
                        receiver_city = s.receiver_city,
                        receiver_country = s.receiver_country,
                        receiver_phone = s.receiver_phone,
                        receiver_contact = s.receiver_contact,
                        note = s.note
                    })
                    .FirstOrDefault();

                List<FreightInfoDto> freightInfo = myOrder.invoiceFreight
                    .Select(i => new FreightInfoDto
                    {
                        ship_name = i.ShipName,
                        ship_desc = i.ShipDesc,
                        ship_id = i.ShipId.Value,
                        ticket = i.Ticket,
                        price = i.Price
                        
                    }).ToList();

                orderDetail.invoice_number = myOrder.InvoiceNumber.Value;
                orderDetail.card_id = myOrder.CardId;
                orderDetail.freight = (double)myOrder.Freight;
                orderDetail.order_id = myOrder.Id;
                orderDetail.total = (double)myOrder.Total;
                orderDetail.sub_total = (double)myOrder.OrderTotal;
                orderDetail.tax = (double)myOrder.Tax;
                orderDetail.payment_method = (myOrder.PaymentType);
                orderDetail.status = (myOrder.WebOrderStatus);
                orderDetail.orderItems = await orderItem;
                orderDetail.shippingInfo = shippingInfo;
                orderDetail.freightInfo = freightInfo;

                return Ok(orderDetail);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"Exception while getting order detail with order_id {order_id}.");

                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> createOrder([FromBody] CartDto cart)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            if (cart == null || cart.cartItems == null) //if no item in cart, return not found; 
            {
                logger.Debug("shopping cart is empty");
                return NotFound();
            }
            //bool hasCardid = await _context.Card.AnyAsync(c => c.Id == cart.card_id);

            if (!await _context.Card.AnyAsync(c => c.Id == cart.card_id))
            {
                logger.Debug("This user doesn't exists, id : "+cart.card_id+"");
                return NotFound("This account does not exist, card_id :" + cart.card_id + " !");
            }

            foreach (var item in cart.cartItems)
            {

                if (!await _context.CodeRelations.AnyAsync(c => c.Code == Convert.ToInt32(item.code)))
                {
                    logger.Debug("This item doesn't sell any longer, item code : "+item.code+"");
                    return NotFound("This item does not sell any longer, item code :" + item.code + " !");
                }
            }
            var inoviceInfo = new object();
            var branch_id = 1;

            if (await _context.Branch.AnyAsync(b => b.Name.Trim() == "Online Shop"))
            {
                branch_id = _context.Branch.Where(b => b.Name.Trim() == "Online Shop").FirstOrDefault().Id;
                logger.Debug("Get online shop id: " + branch_id + "");
            }

            var newOrder = new Orders();
            newOrder.CardId = cart.card_id;
            newOrder.Branch = branch_id;
            newOrder.Freight = (decimal)cart.freight;
            newOrder.OrderTotal = (decimal)cart.sub_total;
            newOrder.ShippingMethod = 6;
            newOrder.CustomerGst = cart.customer_gst;
            newOrder.IsWebOrder = true;
            newOrder.WebOrderStatus = 1;
            newOrder.Status = 2;
            try
            {

                await _context.Orders.AddAsync(newOrder);
                await _context.SaveChangesAsync();
                logger.Debug("Input order item, order id: " + newOrder.Id + "");
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception on creating order.");
                throw;
            }
            var newOrderId = newOrder.Id;
            var customerGst = newOrder.CustomerGst;
            try
            {
                logger.Debug("Input order item, order id: " + newOrderId + "");
                await inputOrderItem(cart.cartItems, newOrderId, customerGst);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception on inputing order items.");
                throw;
            }

            try
            {

                logger.Debug("Create invoice, order id: " + newOrderId + "");
                inoviceInfo = await CreateInvoiceAsync(cart, newOrderId);

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception on creating invoice.");
                throw;
            }


            try
            {
                logger.Debug("shipping and clear shopping cart.");
                await ClearShoppingCart(cart.card_id);
                await inputShippingInfo(cart.shippingInfo, newOrderId);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception happen on shipping");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }


            return Ok(inoviceInfo);
        }
        private async Task<IActionResult> CreateInvoiceAsync([FromBody] CartDto cart, int orderid )
        {
            if (cart == null || cart.cartItems == null) //if no item in cart, return not found; 
            {
                return NotFound();
            }

            if (!_context.Card.Any(c => c.Id == cart.card_id))
            {
                return NotFound("This account does not exist, card_id :" + cart.card_id + " !");
            }

            if (!_context.Orders.Any(o=> o.Id == orderid))
            {
                return NotFound("This order does not exist, card_id :" + cart.card_id + " !");
            }

            var currentOrder = _context.Orders.Where(o => o.Id == orderid).FirstOrDefault();
            var branch = _context.Orders.Where(o => o.Id == orderid).FirstOrDefault().Branch;
            var shippingMethod = _context.Orders.Where(o => o.Id == orderid).FirstOrDefault().ShippingMethod;

            var newInvoice = new Invoice();
            newInvoice.Branch = branch;
            newInvoice.CardId = cart.card_id;
            newInvoice.Price = (decimal?)cart.sub_total;
            newInvoice.ShippingMethod = shippingMethod;
            newInvoice.Tax = (decimal?)cart.tax;
            newInvoice.Freight = (decimal?)cart.freight;
            newInvoice.Total = (decimal?)(cart.total);// + cart.freight);
            newInvoice.CommitDate = DateTime.Today;
            _context.Add(newInvoice);
            _context.SaveChanges();

            var invoiceNumber = newInvoice.Id;
            var customerGst = cart.customer_gst;
            currentOrder.InvoiceNumber = invoiceNumber;
            newInvoice.InvoiceNumber = invoiceNumber;
            _context.SaveChanges();

            IActionResult a = await inputSalesItem(cart.cartItems, invoiceNumber, customerGst);

            return Ok(new { orderid, invoiceNumber, newInvoice.Total });
        }
        private async Task<IActionResult> inputOrderItem(List<CartItemDto> itemsInCart, int? orderId, double? customerGst)
        {

            if (itemsInCart == null || orderId == null)
            {
                return NotFound("Nothing in shopping cart!");
            }
            foreach (var item in itemsInCart)
            {
                var newOrderItem = new OrderItem();
                newOrderItem.Id = orderId.GetValueOrDefault();
                newOrderItem.Code = Convert.ToInt32(item.code);
                newOrderItem.ItemName = item.name;
                newOrderItem.Quantity = Convert.ToDouble(item.quantity);
                newOrderItem.SupplierCode = item.supplier_code;
                newOrderItem.Supplier = "";
                newOrderItem.CommitPrice = Convert.ToDecimal(item.sales_price) / Convert.ToDecimal(1 + customerGst ?? 0.15);

                newOrderItem.Cat = _context.CodeRelations.Where(c => c.Code == Convert.ToInt32(item.code)).FirstOrDefault().Cat;
                newOrderItem.SCat = _context.CodeRelations.Where(c => c.Code == Convert.ToInt32(item.code)).FirstOrDefault().SCat;
                newOrderItem.SsCat = _context.CodeRelations.Where(c => c.Code == Convert.ToInt32(item.code)).FirstOrDefault().SsCat;
                 await _context.AddAsync(newOrderItem);
            }
             await _context.SaveChangesAsync();
            return Ok();
        }
        private async Task<IActionResult> inputSalesItem(List<CartItemDto> itemsInCart, int? inoviceNumber, double? customerGst)
        {

            if (itemsInCart == null || inoviceNumber == null)
            {
                return NotFound("Cannot find inoivce!");
            }
            foreach (var item in itemsInCart)
            {
                var newSales = new Sales();
                newSales.InvoiceNumber = inoviceNumber.GetValueOrDefault();
                newSales.Code = Convert.ToInt32(item.code);
                newSales.Name = item.name;
                newSales.Quantity = Convert.ToDouble(item.quantity);
                newSales.SupplierCode = item.supplier_code;
                newSales.Supplier = "";
                newSales.CommitPrice = Convert.ToDecimal(item.sales_price) / Convert.ToDecimal(1+ customerGst ?? 0.15);

                newSales.Cat = _context.CodeRelations.Where(c => c.Code == Convert.ToInt32(item.code)).FirstOrDefault().Cat;
                newSales.SCat = _context.CodeRelations.Where(c => c.Code == Convert.ToInt32(item.code)).FirstOrDefault().SCat;
                newSales.SsCat = _context.CodeRelations.Where(c => c.Code == Convert.ToInt32(item.code)).FirstOrDefault().SsCat;
                await _context.AddAsync(newSales);
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        private async Task<IActionResult>ClearShoppingCart(int card_id)
        {
            var recordAffected = _context.Cart.Where(c => c.CardId == card_id).ToList();
            if (recordAffected.Count == 0)
                return NotFound();
            if (recordAffected.Count > 0)
            {
                 _context.RemoveRange(recordAffected);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }
        private async Task<IActionResult> inputShippingInfo(ShippingInfoDto shippingInfo, int? orderId)
        {
            if (shippingInfo == null || orderId == null)
            {
                return NotFound("No shipping address or order_id!");
            }
            shippingInfo.orderId = orderId.GetValueOrDefault();
            var newShipping = new ShippingInfo();
            newShipping.orderId = shippingInfo.orderId;
            newShipping.sender = shippingInfo.sender;
            newShipping.sender_phone = shippingInfo.sender_phone;
            newShipping.sender_address = shippingInfo.sender_address;
            newShipping.sender_city = shippingInfo.sender_city;
            newShipping.sender_country = shippingInfo.sender_country;

            newShipping.receiver = shippingInfo.receiver;
            newShipping.receiver_phone = shippingInfo.receiver_phone;
            newShipping.receiver_address1 = shippingInfo.receiver_address1;
            newShipping.receiver_address2 = shippingInfo.receiver_address2;
            newShipping.receiver_address3 = shippingInfo.receiver_address3;
            newShipping.receiver_city = shippingInfo.receiver_city;
            newShipping.receiver_country = shippingInfo.receiver_country;
            newShipping.receiver_company = shippingInfo.receiver_company;
            newShipping.receiver_contact = shippingInfo.receiver_contact;
            newShipping.note = shippingInfo.note;

            await _context.ShippingInfo.AddAsync(newShipping);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("del/{id}")]
        public async Task<IActionResult> deleteOrder(int? id)
        {
            if (id == null)
                return NotFound();
            var orderToDel = _context.Orders.Where(o => o.Id == id).FirstOrDefault();
            orderToDel.OrderDeleted = 1;
            orderToDel.WebOrderStatus = 2;
            _context.Update(orderToDel);
            //var orderItemToDel = _context.OrderItem.Where(oi => oi.Id == id).ToList();
            //var invoiceToDel = _context.Invoice.Where(i => i.InvoiceNumber == orderToDel.InvoiceNumber).FirstOrDefault();
            //var salesTodel = _context.Sales.Where(s => s.InvoiceNumber == orderToDel.InvoiceNumber).ToList();
            //_context.Orders.Remove(orderToDel);
            //_context.OrderItem.RemoveRange(orderItemToDel);
            //_context.Invoice.Remove(invoiceToDel);
            //_context.Sales.RemoveRange(salesTodel);
            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpPost("test")]
 //       [Consumes("application/x-www-form-urlencoded")]
        public IActionResult test(string url, string postData)
        {
            string result = "";
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";

                req.ContentType = "application/x-www-form-urlencoded";

                req.Timeout = 800;//请求超时时间

                byte[] data = Encoding.UTF8.GetBytes(postData);

                req.ContentLength = data.Length;
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);

                    reqStream.Close();
                }

                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                Stream stream = resp.GetResponseStream();

                //获取响应内容
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
            }
            catch
            {

            }
          return Ok(result);
        }

        [HttpPost("pay")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> updatePayment([FromForm] LatipayPaymentDto paymentInfo)
        {
            //         data = "merchant_reference=10110&order_id=2017232323345678&amount=12.50&currency=NZD&payment_method=alipay&pay_time=2017-07-07%2010%3A53%3A50&status=paid&signature=840151e0dc39496e22b410b83058b4ddd633b786936c505ae978fae029a1e0f1";
            if (paymentInfo == null)
                return BadRequest("model is null");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //string ObjInStr = "{ \r\n";
            //string[] newstr = data.Split("&");
            //foreach (string ns in newstr)
            //{
            //    var index = newstr.ToList().IndexOf(ns);
            //    if (index < newstr.Length - 1)
            //    {
            //        string[] key = ns.Split("=");
            //        ObjInStr += "\"";
            //        ObjInStr += key[0] + "\" : ";
            //        ObjInStr += "\"";
            //        ObjInStr += key[1] + "\", \r\n";
            //    }
            //    else
            //    {
            //        string[] key = ns.Split("=");
            //        ObjInStr += "\"";
            //        ObjInStr += key[0] + "\" : ";
            //        ObjInStr += "\"";
            //        ObjInStr += key[1] + "\" \r\n";
            //    }
            //}
            //ObjInStr += "}";

            //LatipayPaymentDto paymentInfo = JsonConvert.DeserializeObject<LatipayPaymentDto>(ObjInStr);

            var merchant_reference = paymentInfo.merchant_reference;

            var order = _context.Orders.Where(o => o.InvoiceNumber == Convert.ToInt32(merchant_reference)).FirstOrDefault();
            bool isTran_invoiced = await _context.TranInvoice.AnyAsync(i => i.InvoiceNumber == Convert.ToInt32(merchant_reference));
            if (order == null)
            {
                return BadRequest("Can not find this order!");
            }
            var paid = order.WebOrderStatus;
            if (paid > 1 && isTran_invoiced)
                return Ok("This order is paid!");
            //int latiinvoice_number = Convert.ToInt32(merchant_reference);

            var latipayment_method = paymentInfo.payment_method;
            var status = paymentInfo.status;
            var currenty = paymentInfo.currency;
            var amount = paymentInfo.amount;
            var signature = paymentInfo.signature;
            var order_id = paymentInfo.order_id;

            string myCheckingString =  merchant_reference + latipayment_method + status + currenty + amount;

            var apikey = Startup.Configuration["Latipay_apiKey"];
            byte[] secret = Encoding.UTF8.GetBytes(apikey);
            byte[] msg = Encoding.UTF8.GetBytes(myCheckingString);
            MyHMACSHA256 hmacsha256 = new MyHMACSHA256();
            byte[] SHA256HMACSignature = hmacsha256.HashHMAC(secret, msg);
            string mysignature = BitConverter.ToString(SHA256HMACSignature).Replace("-", "").ToLower();

           // return Ok(paymentInfo.signature + "////" + mysignature);

            if (signature != mysignature)
            {
                _logger.LogCritical($"error occur when update payment!");
                return BadRequest("error occur when update payment!");

            }

            if (paymentInfo == null)
                return NotFound();
            var connect = _context.Database.GetDbConnection();
            var connectstring = _context.Database.GetDbConnection().ConnectionString;
            connect.Open();
            System.Data.Common.DbCommand dbCommand = connect.CreateCommand();

            var cardid = _context.Invoice.Where(i => i.InvoiceNumber.ToString() == paymentInfo.merchant_reference).FirstOrDefault().CardId;
            int paymentmethod = paymentMethodCast(paymentInfo.payment_method);

            try
            {
                var note = dbCommand.CreateParameter();

                note.ParameterName = "@note";
                note.DbType = System.Data.DbType.String;
                note.Value = order_id;

                var shop_branch = dbCommand.CreateParameter();
                shop_branch.ParameterName = "@shop_branch";
                shop_branch.DbType = System.Data.DbType.Int32;
                shop_branch.Value = 1032;

                var Amount = dbCommand.CreateParameter();
                Amount.ParameterName = "@Amount";
                Amount.DbType = System.Data.DbType.String;
                Amount.Value = paymentInfo.amount;

                var nDest = dbCommand.CreateParameter();
                nDest.ParameterName = "@nDest";
                nDest.DbType = System.Data.DbType.Int32;
                nDest.Value = "1116";

                var staff_id = dbCommand.CreateParameter();
                staff_id.ParameterName = "@staff_id";
                staff_id.DbType = System.Data.DbType.Int32;
                staff_id.Value = cardid.ToString();

                var card_id = dbCommand.CreateParameter();
                card_id.ParameterName = "@card_id";
                card_id.DbType = System.Data.DbType.Int32;
                card_id.Value = cardid.ToString();

                var payment_method = dbCommand.CreateParameter();
                payment_method.ParameterName = "@payment_method";
                payment_method.DbType = System.Data.DbType.Int32;
                payment_method.Value = paymentmethod;

                var invoice_number = dbCommand.CreateParameter();
                invoice_number.ParameterName = "@invoice_number";
                invoice_number.DbType = System.Data.DbType.Int32;
                invoice_number.Value = Convert.ToInt32(merchant_reference);

                var amountList = dbCommand.CreateParameter();
                amountList.ParameterName = "@amountList";
                amountList.DbType = System.Data.DbType.String;
                amountList.Value = paymentInfo.amount;


                var return_tran_id = dbCommand.CreateParameter();
                return_tran_id.ParameterName = "@return_tran_id";
                return_tran_id.Direction = System.Data.ParameterDirection.Output;
                return_tran_id.DbType = System.Data.DbType.Int32;

                dbCommand.Parameters.Add(note);
                dbCommand.Parameters.Add(shop_branch);
                dbCommand.Parameters.Add(Amount);
                dbCommand.Parameters.Add(staff_id);
                dbCommand.Parameters.Add(card_id);
                dbCommand.Parameters.Add(payment_method);
                dbCommand.Parameters.Add(invoice_number);
                dbCommand.Parameters.Add(amountList);
                dbCommand.Parameters.Add(return_tran_id);
                dbCommand.CommandText = "eznz_payment";
                dbCommand.CommandType = System.Data.CommandType.StoredProcedure;

                var obj = await dbCommand.ExecuteNonQueryAsync();
                //       return Ok(return_tran_id.Value);

                order.WebOrderStatus = 4;
                _context.Update(order);
                await _context.SaveChangesAsync();
                return Ok("sent");
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
            finally
            {
                connect.Close();
                connect.Dispose();
            }
        }

        private string PaymentType(byte? pm)
        {
            var mytype = _context.Enum.Where(e => e.Class == "payment_method" && e.Id == pm).FirstOrDefault().Name;
            return mytype;

        }
        private string status(int status)
        {
            var mystatus = _context.Enum.Where(e => e.Class == "web_order_status" && e.Id == status).FirstOrDefault().Name;
            return mystatus;

        }
        private int paymentMethodCast(string payment_method)
        {
            if (payment_method == "wechat")
                return 14;
            else if (payment_method == "alipay")
                return 15;
            else if (payment_method == "onlineBank")
                return 16;
            else if (payment_method == "unionpay")
                return 17;
            return 1;
        }
    }




    public class DbHelper
    {
        public static string GenerateCommandText(string storedProcedure, SqlParameter[] parameters)
        {
            string CommandText = "EXEC {0} {1}";
            string[] ParameterNames = new string[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                ParameterNames[i] = parameters[i].ParameterName;
            }

            return string.Format(CommandText, storedProcedure, string.Join(",", ParameterNames));
        }
    }
}