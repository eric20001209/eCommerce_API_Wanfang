using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using eCommerce_API.Data;
using eCommerce_API.Dto;
using eCommerce_API.Models;
using eCommerce_API.Services;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace eCommerce_API.Controllers
{
    [Route("api/latipay")]
    [ApiController]
    public class LatipayController : ControllerBase
    {
        rst374_cloud12Context _context = new rst374_cloud12Context();
        private ILogger<OrderController> _logger;
        public LatipayController(ILogger<OrderController> logger)
        {
            _logger = logger;
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

            string myCheckingString = merchant_reference + latipayment_method + status + currenty + amount;

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
}