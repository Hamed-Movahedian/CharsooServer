using System;
using System.IO;
using System.Net;

namespace CharsooWebAPI.Services
{
    public class SmsService
    {
        private const string API_KEY = @"39535673714A7051336D6C2B554E6E564D4B4C736352526D42493555764F3757";


        public static string CallSmsService(string phoneNumber, string message)
        {
            try
            {
                var wc = new WebClient();
                wc.DownloadString(
                    $@"https://api.kavenegar.com/v1/{API_KEY}/sms/send.json?receptor={phoneNumber}&sender=10007919676213&message={message}");
            }
            catch (WebException ex)
            {
                // Get error code
                int code= (int) ((HttpWebResponse) ex.Response).StatusCode;

                // code 411 is invalid phone number
                if (code == 411)
                    return "Invalid Phone Number";

                return "No Sms Service";
            }
            catch (Exception)
            {
                return "No Sms Service";
            }

            return "Success";
        }

        public static string CallVerifyService(string phoneNumber, string template,string vcode)
        {
            try
            {
                var wc = new WebClient();
                wc.DownloadString(
                    $@"https://api.kavenegar.com/v1/{API_KEY}/verify/lookup.json?receptor={phoneNumber}&sender=10007919676213&token={vcode}&template={template}");
            }
            catch (WebException ex)
            {
                // Get error code
                int code= (int) ((HttpWebResponse) ex.Response).StatusCode;

                // code 411 is invalid phone number
                if (code == 411)
                    return "Invalid Phone Number";

                return "No Sms Service";
            }
            catch (Exception)
            {
                return "No Sms Service";
            }

            return "Success";
        }

    }
}