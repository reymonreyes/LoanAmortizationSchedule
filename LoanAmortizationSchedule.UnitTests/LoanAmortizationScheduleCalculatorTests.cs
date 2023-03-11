using LoanAmortizationSchedule.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanAmortizationSchedule.UnitTests
{
    public class LoanAmortizationScheduleCalculatorTests
    {
        private decimal[] _interestRates = new decimal[] { 5m };
        [Fact]
        public void Calculate_AnnualInterestRate_ShouldReturnCorrectMonthlyInterestRate()
        {
            var calculator = new LoanAmortizationScheduleCalculator();
            var annualInterestRate = 5;
            calculator.Calculate(100000, annualInterestRate, 1);
            Assert.Equal(0.0042m, decimal.Round(calculator.MonthlyInterestRate, 4));
        }

        [Fact]
        public void Calculate_YearsToPay_ShouldReturnNumberOfPayments()
        {
            var calculator = new LoanAmortizationScheduleCalculator();
            var yearsToPay = 1;
            calculator.Calculate(100000, 5, yearsToPay);

            Assert.Equal(12, calculator.NumberOfPayments);
        }

        [Theory]
        [InlineData(100000, 5, 1, 8560.75)]
        [InlineData(165000, 4.5, 30, 836.03)]
        public void Calculate_LoanParameters_ShouldReturnTotalMonthlyPayment(int loanAmount, decimal interest, int yearsToPay, decimal expected)
        {
            var calculator = new LoanAmortizationScheduleCalculator();
            calculator.Calculate(loanAmount, interest, yearsToPay);

            Assert.Equal(expected, calculator.TotalMonthlyPayment);
        }

        [Fact]
        public void Calculate_Loan_ShouldReturnCorrectSchedules()
        {
            var calculator = new LoanAmortizationScheduleCalculator();
            var schedule = calculator.Calculate(1000, 6m, 1);

            var expectedSchedule = new List<AmortizationSchedule>() {
                new AmortizationSchedule {Period = 1    , Principal = 81.07m,   Interest = 5.00m,   Balance = 918.93m},
                new AmortizationSchedule {Period = 2    , Principal = 81.47m,   Interest = 4.59m,   Balance = 837.46m},
                new AmortizationSchedule {Period = 3    , Principal = 81.88m,   Interest = 4.19m,   Balance = 755.58m},
                new AmortizationSchedule {Period = 4    , Principal = 82.29m,   Interest = 3.78m,   Balance = 673.29m},
                new AmortizationSchedule {Period = 5    , Principal = 82.70m,   Interest = 3.37m,   Balance = 590.59m},
                new AmortizationSchedule {Period = 6    , Principal = 83.11m,   Interest = 2.95m,   Balance = 507.48m},
                new AmortizationSchedule {Period = 7    , Principal = 83.53m,   Interest = 2.54m,   Balance = 423.95m},
                new AmortizationSchedule {Period = 8    , Principal = 83.95m,   Interest = 2.12m,   Balance = 340.01m},
                new AmortizationSchedule {Period = 9    , Principal = 84.37m,   Interest = 1.70m,   Balance = 255.64m},
                new AmortizationSchedule {Period = 10   , Principal = 84.79m,   Interest = 1.28m,   Balance = 170.85m},
                new AmortizationSchedule {Period = 11   , Principal = 85.21m,   Interest = 0.85m,   Balance = 85.64m},
                new AmortizationSchedule {Period = 12   , Principal = 85.64m,   Interest = 0.43m,   Balance = 0.00m}
            };

            Assert.Equal(expectedSchedule, schedule, new AmortizationScheduleComparer());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Calculate_InvalidLoanAmount_ShouldThrowException(int loanAmount)
        {
            var calculator = new LoanAmortizationScheduleCalculator();
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var schedule = calculator.Calculate(loanAmount, 0, 0);
            });
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(101)]
        public void Calculate_InvalidAnnualInterestRatePercent_ShouldThrowException(int annualInterestRatePercent)
        {
            var calculator = new LoanAmortizationScheduleCalculator();
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var schedule = calculator.Calculate(10000, annualInterestRatePercent, 0);
            });           
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(31)]
        public void Calculate_InvalidYearsToPay_ShouldThrowException(int yearsToPay)
        {
            var calculator = new LoanAmortizationScheduleCalculator();
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var schedule = calculator.Calculate(10000, 6, yearsToPay);
            });
        }
    }    

    public class AmortizationScheduleComparer : IEqualityComparer<AmortizationSchedule>
    {
        public bool Equals(AmortizationSchedule? x, AmortizationSchedule? y)
        {
            return x?.Period == y?.Period && 
                decimal.Round(x?.Principal ?? 0m, 0) == decimal.Round(y?.Principal ?? 0m, 0) &&
                decimal.Round(x?.Interest ?? 0m, 0) == decimal.Round(y?.Interest ?? 0m, 0) &&
                decimal.Round(x?.Balance ?? 0m, 0) == decimal.Round(y?.Balance ?? 0m, 0);
        }

        public int GetHashCode([DisallowNull] AmortizationSchedule obj)
        {
            return this.GetHashCode();
        }
    }
}
