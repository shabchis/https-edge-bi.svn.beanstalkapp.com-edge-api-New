using System;
using System.Collections.Generic;
using Edge.Objects;

namespace Edge.Api.Mobile.Settings
{
	public class MockSettingsManager : ISettingsManager
	{
		public List<SegmentInfo> GetSegmentInfo(int accountId, SegmentType segmentType)
		{
			var list = new List<SegmentInfo>();
			for (var i = 0; i < 20; i++)
				list.Add(new SegmentInfo { Id = i, Name = String.Format("{0} #{1}", segmentType.ToString(), i) });

			return list;
		}


		public List<SegmentInfo> GetAccounts(int userId)
		{
			var list = new List<SegmentInfo>();
			for (var i = 0; i < 20; i++)
				list.Add(new SegmentInfo { Id = i, Name = String.Format("Account #{0}", i) });

			return list;
		}
	}
}
