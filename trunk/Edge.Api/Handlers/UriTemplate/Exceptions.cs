using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;

namespace Edge.Api.Handlers.Template
{
	[Serializable]
	public class UriTemplateException : HttpStatusException
	{
		public UriTemplateException(string message, string path, HttpStatusCode httpStatusCode) :
			base(message + "\nPath: " + path, httpStatusCode)
		{
		}
	}
}