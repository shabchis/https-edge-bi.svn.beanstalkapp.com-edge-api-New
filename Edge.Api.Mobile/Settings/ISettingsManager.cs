using System.Collections.Generic;
using Edge.Objects;

namespace Edge.Api.Mobile.Settings
{
	/// <summary>
	/// Manager for retrievals of System Settings (Measures, Themes, Countries, etc.)
	/// </summary>
	public interface ISettingsManager
	{
		List<SegmentInfo> GetSegmentInfo(int accountId, SegmentType segmentType);
	}
}
