using System.Globalization;

namespace JollyAdminClient.Helper
{
	public class FormatString
	{
		public static string FormatVND(decimal? amount)
		{
			// Chuyển số decimal thành chuỗi có dạng tiền tệ Việt Nam
			string formattedAmount = string.Format("{0:N0}", (int)amount);

			// Thay thế dấu phân tách thập phân thành dấu chấm
			formattedAmount = formattedAmount.Replace(".", ",");

			return formattedAmount;
		}
		public static string FormatVNDFromDouble(double? amount)
		{
			string formattedAmount = string.Format("{0:N0}", (int)amount);

			formattedAmount = formattedAmount.Replace(".", ",");

			return formattedAmount;
		}
		public static string FormatMilion(double amount)
		{
			double milion = amount / 1000000;
			if (milion > 0)
			{
				return ((int)milion).ToString() ;
			}
			else
			{
				return Math.Round(amount, 2).ToString();
			}
		}
	}
}
