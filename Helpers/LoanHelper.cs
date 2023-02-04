using System;
using loan_calculator_mvc.Models;

namespace loan_calculator_mvc.Helpers
{
	public class LoanHelper
	{
		public Loan GetPayments(Loan loan)
		{
			// calculate monthly payment
			loan.Payment = CalcPayment(loan.Amount, loan.Term, loan.Rate);

			// create a loop from 1 to the term
			var balance = loan.Amount;
			var totalInterest = 0.0m;
			var monthlyInterest = 0.0m;
			var monthlyPrincipal = 0.0m;
			var monthlyRate = CalcMonthlyRate(loan.Rate);


			for (int month = 1; month <= loan.Term; month++)
			{
				monthlyInterest = CalcMonthlyInterest(balance, monthlyRate);
				totalInterest += monthlyInterest;
				monthlyPrincipal = loan.Payment - monthlyInterest;
				balance -= monthlyPrincipal;

				LoanPayment loanPayment = new();

				loanPayment.Month = month;
				loanPayment.Payment = loan.Payment;
				loanPayment.MonthlyPrincipal = monthlyPrincipal;
				loanPayment.MonthlyInterest = monthlyInterest;
				loanPayment.TotalInterest = totalInterest;
				loanPayment.Balance = balance;

                // push the payment into the loan
                loan.Payments.Add(loanPayment);
            }

			loan.TotalInterest = totalInterest;
			loan.TotalCost = loan.Amount + totalInterest;

			// return the loan to the view
			return loan;
		}

		private decimal CalcPayment(decimal amount, int term, decimal rate)
		{
			var monthlyRate = CalcMonthlyRate(rate);

			var rateD = Convert.ToDouble(monthlyRate);
			var amountD = Convert.ToDouble(amount);

			var PaymentD = (amountD * rateD) / (1 - Math.Pow(1 + rateD, -term));

			return Convert.ToDecimal(PaymentD);
		}

		private decimal CalcMonthlyRate(decimal rate)
		{
			return rate / 1200;
		}

		private decimal CalcMonthlyInterest(decimal balance, decimal monthlyRate)
		{
			return balance * monthlyRate;
		}
	}
}

