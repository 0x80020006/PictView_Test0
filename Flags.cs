

namespace PictView_Test0
{
	class Flags
	{
		//フラグ管理
		public enum fileFlags
		{
			NONE = 0,
			FILE_LOAD = 1,
		}

		public enum keyFlags
		{
			NONE = 0,
			UP = 1,
			DOWN = 2,
			LEFT = 4,
			RIGHT = 8,
		}

		public enum mouseFlags
		{
			NONE = 0,
			MOUSE_MOVE = 1,
			MOUSE_DOWN = 2,
			DOUBLECLICK_EXPANSION = 4,
		}

		public static fileFlags fileFlag;
		public static keyFlags keyFlag;
		public static mouseFlags mouseFlag;

	}
}
