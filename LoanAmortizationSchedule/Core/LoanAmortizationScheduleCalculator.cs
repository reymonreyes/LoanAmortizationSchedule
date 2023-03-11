namespace LoanAmortizationSchedule.Core
{
    public class LoanAmortizationScheduleCalculator
    {
        private decimal _monthlyInterestRate;
        private int _numberOfPayments;
        private decimal _totalMonthlyPayment;
        private int _loanAmount;

        public List<AmortizationSchedule> Calculate(int loanAmount, decimal annualInterestRatePercent, int yearsToPay)
        {
            if (loanAmount <= 0)
                throw new ArgumentException("Loan amount must be greater than 0.");

            if (!(annualInterestRatePercent >= 1 && annualInterestRatePercent <= 100))
                throw new ArgumentOutOfRangeException("Annual Interest Rate must be between 1-100%");

            if (!(yearsToPay >= 1 && yearsToPay <= 30))
                throw new ArgumentOutOfRangeException("Years to Pay must be between 1-30 years");

            _loanAmount = loanAmount;
            _monthlyInterestRate = annualInterestRatePercent / 100.0m / 12;
            _numberOfPayments = yearsToPay * 12;
            _totalMonthlyPayment = GetTotalMonthlyPayment();

            return GetSchedule();
        }

        public decimal MonthlyInterestRate
        {
            get { return _monthlyInterestRate; }
        }

        public int NumberOfPayments
        {
            get { return _numberOfPayments; }
        }

        public decimal TotalMonthlyPayment
        {
            get { return _totalMonthlyPayment; }
        }

        private decimal GetTotalMonthlyPayment()
        {
            var left = MonthlyInterestRate * (decimal)Math.Pow(1 + (double)MonthlyInterestRate, NumberOfPayments);
            var right = (decimal)Math.Pow(1 + (double)MonthlyInterestRate, NumberOfPayments) -1;
            decimal result = decimal.Round(_loanAmount * (left / right), 2);

            return result;
        }

        private List<AmortizationSchedule> GetSchedule()
        {
            var result = new List<AmortizationSchedule>();
            decimal outstandingBalance = _loanAmount;
            for (int i = 0; i < NumberOfPayments; i++)
            {
                var schedule = new AmortizationSchedule();
                schedule.Period = i + 1;
                schedule.Principal = TotalMonthlyPayment - (outstandingBalance * MonthlyInterestRate);
                schedule.Interest = outstandingBalance * MonthlyInterestRate;
                schedule.Balance = outstandingBalance - schedule.Principal;
                outstandingBalance = schedule.Balance;
                result.Add(schedule);
            }

            return result;
        }
    }
    
    public class AmortizationSchedule
    {
        public int Period { get; set; }
        public decimal Principal { get; set; }
        public decimal Interest { get; set; }
        public decimal Balance { get; set; }
    }
}
