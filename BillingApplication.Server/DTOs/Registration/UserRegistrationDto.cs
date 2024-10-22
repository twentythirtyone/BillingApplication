namespace BillingApplication.DTOs.Registration
{
    public class UserRegistrationDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Number { get; set; }
        public PassportDto Passport { get; set; }
        public TariffDto Tariff { get; set; }
    }

    public class PassportDto
    {
        public string PassportNumber { get; set; }
        public string IssuedBy { get; set; }
        public string Registration { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; } 
    }

    public class TariffDto
    {
        public int id { get; set; }
    }
}
