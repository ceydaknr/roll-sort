using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace RollSort.Runtime.EventHandler
{
    public enum GameEvent
    {
        OnLevelStarted,
        OnLevelCompleted,
        OnLevelFailed
    }

    public static class EventManager
    {
        private static readonly Dictionary<GameEvent, Action> _events = new();
        private static readonly Dictionary<GameEvent, Delegate> _genericEvents = new();

        public static void AddListener(GameEvent gameEvent, Action action)
        {
            if (!_events.TryAdd(gameEvent, action))
                _events[gameEvent] += action;
        }

        public static void RemoveListener(GameEvent gameEvent, Action action)
        {
            if (!_events.ContainsKey(gameEvent)) return;

            _events[gameEvent] -= action;
            if (_events[gameEvent] == null)
                _events.Remove(gameEvent);
        }

        public static void AddListener<T>(GameEvent gameEvent, Action<T> action)
        {
            if (!_genericEvents.TryAdd(gameEvent, action))
                _genericEvents[gameEvent] = Delegate.Combine(_genericEvents[gameEvent], action);
        }

        public static void RemoveListener<T>(GameEvent gameEvent, Action<T> action)
        {
            if (!_genericEvents.ContainsKey(gameEvent)) return;

            _genericEvents[gameEvent] = Delegate.Remove(_genericEvents[gameEvent], action);
            if (_genericEvents[gameEvent] == null)
                _genericEvents.Remove(gameEvent);
        }

        public static async void Broadcast(GameEvent gameEvent, float delay = 0)
        {
            if (delay > 0) await UniTask.Delay(TimeSpan.FromSeconds(delay));

            if (_events.TryGetValue(gameEvent, out Action @event) && @event != null)
                @event.Invoke();
        }

        public static async void Broadcast<T>(GameEvent gameEvent, T param, float delay = 0)
        {
            if (delay > 0) await UniTask.Delay(TimeSpan.FromSeconds(delay));

            if (_genericEvents.TryGetValue(gameEvent, out Delegate genericEvent) && genericEvent is Action<T> action)
                action.Invoke(param);

            Broadcast(gameEvent);
        }
    }
}