namespace LoanAmortizationSchedule.Models
{
    public class Loan
    {
        public Loan()
        {
            AmortizationSchedule = new List<AmortizationSchedule>();
        }
        public decimal Amount { get; set; }
        public int AnnualInterestRate { get; set; }
        public int NumberOfYears { get; set; }
        public int NumberOfPayments { get; set; }
        public List<AmortizationSchedule> AmortizationSchedule { get; set; }
    }
}
