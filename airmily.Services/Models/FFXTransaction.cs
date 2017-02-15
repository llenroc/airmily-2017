using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace airmily.Services.Models
{
	[JsonObject]
	public class FFXTransaction : BaseSchema
	{
		[JsonProperty("description")]
		public string Description { get; set; }
		
		[JsonProperty("currency")]
		public string Currency { get; set; }

		[JsonProperty("transactionamount")]
		public string InternalDifference { get; set; }

		[JsonProperty("transactionbill")]
		public string Amount { get; set; }

		[JsonProperty("isdebit")]
		public bool NegativeAmount { get; set; }
		
		[JsonProperty("transactiondate")]
		public string TransDate { get; set; }

		[JsonProperty("postdate")]
		public string PostDate { get; set; }

		//Operator Override
		public static bool operator ==(FFXTransaction a, FFXTransaction b)
		{
			bool desc = a.Description == b.Description;
			bool diff = a.InternalDifference == b.InternalDifference;
			bool amnt = a.Amount == b.Amount;
			bool ngtv = a.NegativeAmount == b.NegativeAmount;
			bool trns = a.TransDate == b.TransDate;
			bool post = a.PostDate == b.PostDate;

			return desc && diff && amnt && ngtv && trns && post;
		}

		public static bool operator !=(FFXTransaction a, FFXTransaction b)
		{
			return !(a == b);
		}

		public override bool Equals(object obj)
		{
			try { return this == (FFXTransaction)obj; }
			catch { /*ignored*/ }

			return false;
		}

		public override int GetHashCode()
		{
			// ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
			return base.GetHashCode();
		}
	}
}
