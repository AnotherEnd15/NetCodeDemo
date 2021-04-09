using ET;

namespace EventIDType
{
	public struct AppStart
	{
	}

	public struct ChangePosition
	{
		public Unit Unit;
	}

	public struct ChangeRotation
	{
		public Unit Unit;
	}

	public struct MoveStart
	{
		public Unit Unit;
	}

	public struct MoveStop
	{
		public Unit Unit;
	}
	
	public struct NumbericChange
	{
		public Entity Parent;
		public NumericType NumericType;
		public long Old;
		public long New;
	}

	public struct FrameEnd
	{
		public Unit unit;
	}
}