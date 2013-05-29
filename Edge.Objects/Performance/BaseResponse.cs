namespace Edge.Objects.Performance
{
	public abstract class BaseResponse
	{
		public bool HasError { get; set; }
		public string ErrorMsg { get; set; }
		public string DisplayError { get; set; }
	}
}
