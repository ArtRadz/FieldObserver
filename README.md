# FieldObserver Utility

A lightweight C# utility for observing changes to **fields** on **arbitrary objects** at runtime. Designed for use in Unity or any C# environment that supports reflection.

This utility tracks specific fields on given objects and notifies you via a callback when the value changes. Great for building reactive systems, editor tooling, or debugging utilities without needing to refactor into property getters/setters.

---

## 🔧 Features

- Observe **private** and **public** fields using reflection.
- Supports multiple observers per field.
- Automatically stops tracking when the object is destroyed or callback is removed.
- Centralized polling system (via `Tick()`).

---

## 📦 API Overview

```csharp
FieldObserver.ObserveField(object target, string fieldName, Action<object> callback)
Begins watching the specified field on a target object. The callback is invoked when the field's value changes.

csharp
Copy
Edit
FieldObserver.UnObserveFieldGlobal(object target, string fieldName)
Completely removes all callbacks and stops tracking this field.

csharp
Copy
Edit
FieldObserver.UnobserveFieldScoped(object target, string fieldName, Action<object> callback)
Removes a specific callback from this field. If it's the last one, the field is no longer tracked.

csharp
Copy
Edit
// Must be called manually (e.g., in Unity's Update or Editor loop)
FieldObserver.Tick()
Performs a check on all tracked fields and triggers callbacks if changes are detected.

🧪 Example Usage
csharp
Copy
Edit
void Start()
{
    FieldObserver.ObserveField(myComponent, "_health", OnHealthChanged);
}

void OnHealthChanged(object newValue)
{
    Debug.Log($"Health changed to {newValue}");
}

void OnDestroy()
{
    FieldObserver.UnObserveFieldGlobal(myComponent, "_health");
}
Note: Tick must be called every frame (e.g., from a MonoBehaviour) to detect changes.

⚠️ Limitations
Only works with fields, not properties.

Does not track nested objects or collections changing internally—only top-level field references.

Requires manual Tick() call; no automatic hooks.

💡 Use Cases
Editor tools observing runtime state.

Debugging internal values without modifying the original class.

Lightweight reactive bindings.