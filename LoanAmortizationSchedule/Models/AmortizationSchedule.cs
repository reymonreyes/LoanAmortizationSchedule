namespace LoanAmortizationSchedule.Models
{
    public class AmortizationSchedule
    {
        public int Period { get; set; }
        public decimal TotalMonthlyPayment { get; set; }
        public decimal Principal { get; set; }
        public decimal Interest { get; set; }
        public decimal Balance { get; set; }
    }
}
