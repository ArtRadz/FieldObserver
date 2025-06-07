using System;
using System.Collections.Generic;
using System.Reflection;

namespace ObserverUtil
{
    public static class FieldObserver
    {
        private static Dictionary<(object, string), WatchedField> WatchedFields = new();
        private class WatchedField
        {
            public object Target;
            public FieldInfo Field;
            public object LastValue;
            public List<Action<object>> Callbacks = new();
        }
        

        public static void ObserveField(object target, string fieldName, Action<object> callback)
        {
            var key = (target, fieldName);

            if (WatchedFields.TryGetValue(key, out var wf))
            {
                wf.Callbacks.Add(callback);
                return;
            }

            var type = target.GetType();
            var field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (field == null)
            {
                UnityEngine.Debug.LogWarning($"[FieldObserver] Field '{fieldName}' not found on {type.Name}");
                return;
            }

            var currentValue = field.GetValue(target);
            var newWatched = new WatchedField
            {
                Target = target,
                Field = field,
                LastValue = currentValue
            };
            newWatched.Callbacks.Add(callback);

            WatchedFields[key] = newWatched;
        }

        public static void UnObserveFieldGlobal(object target, string fieldName)
        {
            var key = (target, fieldName);
            if (!DoesContainKey(key))
            {
                return;
            }
            WatchedFields.Remove(key);
        }

        public static void UnObserveFieldScoped(object target, string fieldName, Action<object> callback)
        {
            var key = (target, fieldName);
            if (!DoesContainKey(key))
            {
                return;
            }

            WatchedFields[key].Callbacks.Remove(callback);
            if (WatchedFields[key].Callbacks.Count == 0)
            {
                WatchedFields.Remove(key);
            }
        }

        private static bool DoesContainKey((object, string) key)
        {
           return WatchedFields.ContainsKey(key);
        }

        public static void Tick()
        {
            foreach (var kvp in WatchedFields)
            {
                var wf = kvp.Value;

                if (wf.Target == null) continue; // Object destroyed?

                var current = wf.Field.GetValue(wf.Target);

                if (!Equals(current, wf.LastValue))
                {
                    wf.LastValue = current;

                    foreach (var callback in wf.Callbacks)
                    {
                        try
                        {
                            callback(current);
                        }
                        catch (Exception ex)
                        {
                            UnityEngine.Debug.LogError($"[FieldObserver] Callback error: {ex}");
                        }
                    }
                }
            }
        }
    }
}
