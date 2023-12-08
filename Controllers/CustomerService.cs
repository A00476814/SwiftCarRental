using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Mail;

namespace SwiftCarRental.Controllers
{
    public class CustomerService : Controller
    {   
        [HttpPost]
        public ActionResult Submit(string email, string question)
        // public ActionResult Submit()
        {
            System.Diagnostics.Debug.WriteLine("MyModel.MyProperty = " + email);
            System.Diagnostics.Debug.WriteLine("#############################");
            SmtpClient smtpclient = new SmtpClient();
            smtpclient.Host = "smtp.gmail.com";//smtp.163.com smtp.qq.com
            smtpclient.Port = 587;//gmail服务器端口

            //确定发件地址与收件地址
            string senderGmail = "guolililian@gmail.com";
            string receiverGmail = "guolililian@gmail.com";
            MailAddress sendAddress = new MailAddress(senderGmail);
            MailAddress receiveAddress = new MailAddress(receiverGmail);

            //构造一个Email的Message对象 内容信息
            MailMessage mailMessage = new MailMessage();
            mailMessage.Subject = "Swift Car Rental Customer Service"; //邮件标题
            mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
            //mailMessage.Body = email + question;

            mailMessage.Body = "Email: guoli_dlmu@163.com" + email + "\n"+"\nQuestion: test" + question;

            System.Diagnostics.Debug.WriteLine("Email from form: " + email);
            System.Diagnostics.Debug.WriteLine("Question from form: " + question);
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            mailMessage.IsBodyHtml = true;

            mailMessage.From = sendAddress;
            mailMessage.To.Add(receiveAddress);

            //邮件发送方式  通过网络发送到smtp服务器
            smtpclient.DeliveryMethod = SmtpDeliveryMethod.Network;

            //如果服务器支持安全连接，则将安全连接设为true
            smtpclient.EnableSsl = true;

            //是否使用默认凭据，若为false，则使用自定义的证书，就是下面的networkCredential实例对象
            smtpclient.UseDefaultCredentials = false;

            //指定邮箱账号和密码,需要注意的是，这个密码是你在QQ邮箱设置里开启服务的时候给你的那个授权码
            NetworkCredential networkCredential = new NetworkCredential(senderGmail, "kcex bwna exct xxyo");
            smtpclient.Credentials = networkCredential;

            //发送邮件
            smtpclient.Send(mailMessage);
            Console.WriteLine("发送邮件成功");

            return View();
        }
    }
}
