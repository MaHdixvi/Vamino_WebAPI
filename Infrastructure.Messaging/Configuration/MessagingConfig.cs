namespace Infrastructure.Messaging.Configuration
{
    public class MessagingConfig
    {
        public string SmsApiKey { get; set; } = "Your_Kavenegar_API_Key";
        public string SenderNumber { get; set; } = "100000000000";
        public string EmailSmtpServer { get; set; } = "smtp.gmail.com";
        public int EmailPort { get; set; } = 587;
        public string EmailUsername { get; set; }
        public string EmailPassword { get; set; }
    }
}