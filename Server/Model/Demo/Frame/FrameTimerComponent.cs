using System;
using System.Collections.Generic;

namespace ET
{
    // 以帧为时间单位的时间组件
    // 所有的都基于Frame了
    public class FrameTimerComponent: Entity
    {
        
        /// <summary>
        /// key: time, value: timer id
        /// </summary>
        private readonly MultiMap<long, long> TimeId = new MultiMap<long, long>();

        private readonly Queue<long> timeOutTime = new Queue<long>();

        private readonly Queue<long> timeOutTimerIds = new Queue<long>();

        // 记录最小时间，不用每次都去MultiMap取第一个值
        private long minTime;

        private long CurrFrame;
        
        public void Update(long CurrFrame)
        {
            if (this.TimeId.Count == 0)
            {
                return;
            }

            this.CurrFrame = CurrFrame;
            long timeNow = CurrFrame;

            if (timeNow < this.minTime)
            {
                return;
            }

            foreach (KeyValuePair<long, List<long>> kv in this.TimeId)
            {
                long k = kv.Key;
                if (k > timeNow)
                {
                    minTime = k;
                    break;
                }

                this.timeOutTime.Enqueue(k);
            }

            while (this.timeOutTime.Count > 0)
            {
                long time = this.timeOutTime.Dequeue();
                foreach (long timerId in this.TimeId[time])
                {
                    this.timeOutTimerIds.Enqueue(timerId);
                }

                this.TimeId.Remove(time);
            }

            while (this.timeOutTimerIds.Count > 0)
            {
                long timerId = this.timeOutTimerIds.Dequeue();

                TimerAction timerAction = this.GetChild<TimerAction>(timerId);
                if (timerAction == null)
                {
                    continue;
                }
                Run(timerAction);
            }
        }

        private void Run(TimerAction timerAction)
        {
            switch (timerAction.TimerClass)
            {
                case TimerClass.OnceWaitTimer:
                {
                    ETTaskCompletionSource<bool> tcs = timerAction.Callback as ETTaskCompletionSource<bool>;
                    this.Remove(timerAction.Id);
                    tcs.SetResult(true);
                    break;
                }
                case TimerClass.OnceTimer:
                {
                    Action action = timerAction.Callback as Action;
                    this.Remove(timerAction.Id);
                    action?.Invoke();
                    break;
                }
                case TimerClass.RepeatedTimer:
                {
                    Action action = timerAction.Callback as Action;
                    long tillTime = this.CurrFrame + timerAction.Time;
                    this.AddTimer(tillTime, timerAction);
                    action?.Invoke();
                    break;
                }
            }
        }
        
        private void AddTimer(long tillTime, TimerAction timer)
        {
            this.TimeId.Add(tillTime, timer.Id);
            if (tillTime < this.minTime)
            {
                this.minTime = tillTime;
            }
        }
        
        /// <summary>
        /// 创建一个RepeatedTimer
        /// </summary>
        private long NewRepeatedTimerInner(long time, Action action)
        {
            long tillTime = this.CurrFrame + time;
            TimerAction timer = EntityFactory.CreateWithParent<TimerAction, TimerClass, long, object>(this, TimerClass.RepeatedTimer, time, action, true);
            this.AddTimer(tillTime, timer);
            return timer.Id;
        }

        public long NewRepeatedFrameTimer(long frameInterval, Action action)
        {
            return NewRepeatedTimerInner(frameInterval, action);
        }

        public void Remove(ref long id)
        {
            this.Remove(id);
            id = 0;
        }

        public bool Remove(long id)
        {
            if (id == 0)
            {
                return false;
            }

            TimerAction timerAction = this.GetChild<TimerAction>(id);
            if (timerAction == null)
            {
                return false;
            }
            timerAction.Dispose();
            return true;
        }

        public long NewOnceFrameTimer(long frame, Action action)
        {
            if (frame < this.CurrFrame)
            {
                Log.Error($"new once time too small: {frame}");
            }
            TimerAction timer = EntityFactory.CreateWithParent<TimerAction, TimerClass, long, object>(this, TimerClass.OnceTimer, 0, action, true);
            this.AddTimer(frame, timer);
            return timer.Id;
        }
    }
}