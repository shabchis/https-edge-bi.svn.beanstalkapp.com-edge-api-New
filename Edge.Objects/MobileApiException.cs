using System;

namespace Edge.Objects
{
	public class MobileApiException : ApplicationException
	{
		public MobileApiException() {}

		public MobileApiException(string message)
			: base(message) {}

		public MobileApiException(string message, string displayMessage)
			:base(message)
		{
			DisplayMessage = displayMessage;
		}

		public string DisplayMessage { get; set; }
	}
}
