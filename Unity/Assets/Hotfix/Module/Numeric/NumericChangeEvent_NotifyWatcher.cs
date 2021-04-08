namespace ET
{
	// 分发数值监听
	public class NumericChangeEvent_NotifyWatcher: AEvent<EventIDType.NumbericChange>
	{
		protected override async ETTask Run(EventIDType.NumbericChange args)
		{
			NumericWatcherComponent.Instance.Run(args.NumericType, args.Parent.Id, args.New);
			await ETTask.CompletedTask;
		}
	}
}
