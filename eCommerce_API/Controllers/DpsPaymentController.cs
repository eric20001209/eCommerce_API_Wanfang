using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using eCommerce_API.Data;
using eCommerce_API.Dto;
using Microsoft.Extensions.Logging;
using eCommerce_API.Services;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace eCommerce_API.Controllers
{
    [Route("api/dps")]   //handle dps payment
    [ApiController]
    public class DpsPaymentController : ControllerBase
    {
        rst374_cloud12Context _context = new rst374_cloud12Context();
        FreightContext _contextf = new FreightContext();
        private ILogger<OrderController> _logger;

        private string PxPayUserId = ""; // _contextf.Settings.Where(s => s.Cat == "DPS" && s.Name == "PxPayUserId").FirstOrDefault().Value;
        private string PxPayKey = ""; // _contextf.Settings.Where(s => s.Cat == "DPS" && s.Name == "PxPayKey").FirstOrDefault().Value;
        private string sServiceUrl = ""; // _contextf.Settings.Where(s => s.Cat == "DPS" && s.Name == "sServiceUrl").FirstOrDefault().Value;
        public DpsPaymentController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{orderId}")]
        public IActionResult CreateDpsUI(string orderId)
        {
            string host_url = "http://" + HttpContext.Request.Host;
            string sReturnUrlFail = "https://localhost:44338" + "/api/dps/result?t=result&ret=fail&oid=" + orderId;
            string sReturnUrlSuccess = "https://localhost:44338" + "/api/dps/result?action=paymentSuccess&order=" + orderId;

            PxPayUserId = _contextf.Settings.Where(s => s.Cat == "DPS" && s.Name == "PxPayUserId").FirstOrDefault().Value;
            PxPayKey = _contextf.Settings.Where(s => s.Cat == "DPS" && s.Name == "PxPayKey").FirstOrDefault().Value;
            sServiceUrl = _contextf.Settings.Where(s => s.Cat == "DPS" && s.Name == "sServiceUrl").FirstOrDefault().Value;
            if (PxPayUserId == null || PxPayKey == null || sServiceUrl == null)
            {
                PxPayUserId = Startup.Configuration["PxPayUserId"];
                PxPayKey = Startup.Configuration["PxPayKey"];
                sServiceUrl = Startup.Configuration["sServiceUrl"];
            }
            //get order total

            var order = _context.Orders.Where(o => o.Id == Convert.ToInt32(orderId))
                        .Join(_context.Invoice,
                                o=>o.InvoiceNumber, 
                                i=>i.InvoiceNumber,
                                (o,i) => new {o.InvoiceNumber, o.Id, Total = i.Total ?? 0}).FirstOrDefault();
            decimal orderAmount = 0;
            if (order != null)
                orderAmount = order.Total;
            
            PxPay WS = new PxPay(sServiceUrl, PxPayUserId, PxPayKey);
            RequestInput input = new RequestInput();
            input.AmountInput = Math.Round(orderAmount,2).ToString();
            input.CurrencyInput = "NZD";
            input.MerchantReference = orderId;
            input.TxnType = "Purchase";
            input.UrlFail = sReturnUrlFail; 
            input.UrlSuccess = sReturnUrlSuccess;

            Guid newOrderId = Guid.NewGuid();
            input.TxnId = newOrderId.ToString().Substring(0, 16);
            RequestOutput output = WS.GenerateRequest(input);
            if(output.valid == "1")
            {
                var result = output.Url;
                return Ok(result);
            }

            return NotFound();
        }

        [HttpGet("result")]
        public async Task<IActionResult> GetPaymentResult([FromQuery] string result, [FromQuery] string action, [FromQuery] string orderId)
        {
            if (result == null)
                return NotFound();
            if (action != "paymentSuccess")
                return BadRequest();

            PxPayUserId = _contextf.Settings.Where(s => s.Cat == "DPS" && s.Name == "PxPayUserId").FirstOrDefault().Value;
            PxPayKey = _contextf.Settings.Where(s => s.Cat == "DPS" && s.Name == "PxPayKey").FirstOrDefault().Value;
            sServiceUrl = _contextf.Settings.Where(s => s.Cat == "DPS" && s.Name == "sServiceUrl").FirstOrDefault().Value;
            if (PxPayUserId == null || PxPayKey == null || sServiceUrl == null)
            {
                PxPayUserId = Startup.Configuration["PxPayUserId"];
                PxPayKey = Startup.Configuration["PxPayKey"];
                sServiceUrl = Startup.Configuration["sServiceUrl"];
            }
            PxPay WS = new PxPay(sServiceUrl, PxPayUserId, PxPayKey);
            ResponseOutput outputQs = WS.ProcessResponse(result);
            string sSuccess = outputQs.Success;
            PropertyInfo[] properties = outputQs.GetType().GetProperties();
            foreach (PropertyInfo oPropertyInfo in properties)
            {
                if (oPropertyInfo.CanRead)
                {
                    string name = oPropertyInfo.Name;
                    string value = (string)oPropertyInfo.GetValue(outputQs, null);
                }
            }
            //         string order_number = Request.QueryString["order"];

            var connect = _context.Database.GetDbConnection();
            var connectstring = _context.Database.GetDbConnection().ConnectionString;
            connect.Open();
            System.Data.Common.DbCommand dbCommand = connect.CreateCommand();

            var order = _context.Orders.Where(o => o.Id == Convert.ToInt32(orderId))
                        .Join(_context.Invoice,
                                o => o.InvoiceNumber,
                                i => i.InvoiceNumber,
                                (o, i) => new { o.InvoiceNumber, o.Id, o.CardId, Total = i.Total ?? 0 }).FirstOrDefault();
            int cardId = 0;
            decimal orderAmount = 0;
            if (order != null)
            {
                cardId = order.CardId;
                orderAmount = order.Total;
            }
                
            int paymentmethod = 14; // paymentMethodCast(paymentInfo.payment_method);

            if (sSuccess == "1")
            {
                //input payment info
                try
                {
                    var note = dbCommand.CreateParameter();

                    note.ParameterName = "@note";
                    note.DbType = System.Data.DbType.String;
                    note.Value = orderId;

                    var shop_branch = dbCommand.CreateParameter();
                    shop_branch.ParameterName = "@shop_branch";
                    shop_branch.DbType = System.Data.DbType.Int32;
                    shop_branch.Value = 1032;

                    var Amount = dbCommand.CreateParameter();
                    Amount.ParameterName = "@Amount";
                    Amount.DbType = System.Data.DbType.String;
                    Amount.Value = order.Total;

                    var nDest = dbCommand.CreateParameter();
                    nDest.ParameterName = "@nDest";
                    nDest.DbType = System.Data.DbType.Int32;
                    nDest.Value = "1116";

                    var staff_id = dbCommand.CreateParameter();
                    staff_id.ParameterName = "@staff_id";
                    staff_id.DbType = System.Data.DbType.Int32;
                    staff_id.Value = order.CardId.ToString();

                    var card_id = dbCommand.CreateParameter();
                    card_id.ParameterName = "@card_id";
                    card_id.DbType = System.Data.DbType.Int32;
                    card_id.Value = order.CardId.ToString();

                    var payment_method = dbCommand.CreateParameter();
                    payment_method.ParameterName = "@payment_method";
                    payment_method.DbType = System.Data.DbType.Int32;
                    payment_method.Value = paymentmethod;

                    var invoice_number = dbCommand.CreateParameter();
                    invoice_number.ParameterName = "@invoice_number";
                    invoice_number.DbType = System.Data.DbType.Int32;
                    invoice_number.Value = Convert.ToInt32(order.InvoiceNumber);

                    var amountList = dbCommand.CreateParameter();
                    amountList.ParameterName = "@amountList";
                    amountList.DbType = System.Data.DbType.String;
                    amountList.Value = orderAmount;


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
                    var thisOrder = _context.Orders.Where(o => o.Id == Convert.ToInt32(orderId)).FirstOrDefault();
                    thisOrder.WebOrderStatus = 4; //update order status
                    _context.Update(thisOrder);

                    var thisInvoice = _context.Invoice.Where(i => i.InvoiceNumber == Convert.ToInt32(order.InvoiceNumber)).FirstOrDefault();
                    thisInvoice.Paid = true;  //update payment status
                    _context.Update(thisInvoice);

                    await _context.SaveChangesAsync();
                    return Ok("sent");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
                finally
                {
                    connect.Close();
                    connect.Dispose();
                }
            }
            return Ok();
        }
    }
}