using HealthEngineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace HealthEngineAPI.Utility
{
    public class MessageSender
    {
        public ResponseData SendSMS(string toPhone,int code)
        {
            try
            {
                var response = MessageResource.Create(
                                to: toPhone,
                                from: new PhoneNumber("+12563804125"),
                                body: "Your Verification Code is "+ code
                                );
                return new ResponseData { Status = true, Message = "Message has been sent!" };
            }
            catch(Exception ae)
            {
                return new ResponseData { Status = false, Message = ae.Message.ToString() };
            }
            
        }
    }
}
