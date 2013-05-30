using System.Collections.Generic;
using Edge.Objects;

namespace Edge.Api.Mobile.Settings
{
	/// <summary>
	/// Manager for retrievals of System Settings (Measures, Themes, Countries, Accounts, etc.)
	/// </summary>
	public interface ISettingsManager
	{
		// get segments by segment type and account
		SettingsResponse GetSegmentInfo(int accountId, SegmentType segmentType);

		SettingsResponse GetAccounts(int userId);
	}
}
