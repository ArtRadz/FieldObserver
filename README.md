# FieldObserver Utility

A lightweight C# utility for observing changes to **fields on arbitrary objects** at runtime.  
Designed for use in Unity or any C# environment that supports reflection.

This utility tracks specific fields on given objects and notifies you via a callback when the value changes.  
Great for building reactive systems, editor tooling, or debugging utilities without needing to refactor into property getters/setters.

---

## 🔧 Features

- Observe **private** and **public** fields using reflection.
- Supports **multiple observers** per field.
- Automatically **stops tracking** when the object is destroyed or the callback is removed.
- Manual **centralized polling** system (via `Tick()`).
- Non-invasive — **no code changes needed** in the observed object.

---

## 📦 Unity Package Installation

To install **ObserverUtil** via Unity Package Manager:

1. Open your Unity project's `Packages/manifest.json`
2. Add this line to the `"dependencies"` section:

   ```json
   "com.artradz.observerutil": "https://github.com/ArtRadz/FieldObserver.git"
Save the file. Unity will auto-fetch and import the package.

🎯 Requires Unity version 2021.2 or higher

🧰 API Overview
FieldObserver.ObserveField(object target, string fieldName, Action<object> callback)
Begins watching the specified field on a target object. The callback is invoked when the field’s value changes.

FieldObserver.UnObserveFieldGlobal(object target, string fieldName)
Completely removes all callbacks and stops tracking this field.

FieldObserver.UnObserveFieldScoped(object target, string fieldName, Action<object> callback)
Removes a specific callback from this field. If it's the last one, the field is no longer tracked.

FieldObserver.Tick()
Must be called manually (e.g., from Unity's Update() loop).
Performs a check on all tracked fields and invokes callbacks if changes are detected.

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

void Update()
{
    FieldObserver.Tick(); // Required each frame
}

void OnDestroy()
{
    FieldObserver.UnObserveFieldGlobal(myComponent, "_health");
}
⚠️ Limitations
Only works with fields, not properties.

Does not track nested objects or internal collection changes — only top-level field references.

Requires manual Tick() call; there are no automatic hooks or Unity integration (by design).

💡 Use Cases
🛠️ Editor tools observing runtime state

🧪 Debugging internal/private fields without modifying original code

⚡ Lightweight reactive bindings for prototypes and simulations

🙌 Maintainer
Built by @ArtRadz
Feel free to contribute, fork, or reach out with suggestions!