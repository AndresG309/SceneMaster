# SceneMaster

SceneMaster is a Unity scene transition tool with animated visual effects and a fluent builder API.

It provides a single persistent `SceneMaster` object that can load scenes by name or build index, play transition effects, run callbacks, and optionally use asynchronous loading screens.

## Requirements

- Unity `2022.3.62f3` is the version currently tested with this package.
- Other Unity versions have not been verified yet.
- Destination scenes must be included in **File > Build Settings**.

## Installation

Install SceneMaster through Unity Package Manager using the Git URL:

1. Open **Window > Package Manager**.
2. Select the **+** button.
3. Select **Add package from git URL...**.
4. Enter:

   ```text
   https://github.com/AndresG309/SceneMaster.git
   ```

5. Select **Add**.

## Setup

After installing the package, create the manager in the active scene:

1. In the Hierarchy, right-click or open the **GameObject** menu.
2. Select **SceneMaster > Create SceneMaster**.
3. Keep one `SceneMaster` object in the project. It persists between scene loads and destroys duplicate instances automatically.

The created object includes the default transition effect. You can assign a different `TransitionEffect` to `defaultTransition` in the Inspector. A transition effect must be a child of the `SceneMaster` object when it is configured as the default effect.

## Quick Start

Call `TransitionTo` at the point in your game where the scene should change, then call `Execute`:

```csharp
using AndresG09.SceneMaster;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
	public void OpenGameplay()
	{
		SceneMaster.Instance
			.TransitionTo("Gameplay")
			.Execute();
	}
}
```

The name must match the destination scene's file name, without the `.unity` extension. The scene must also be present in Build Settings.

You can also load by build index:

```csharp
SceneMaster.Instance
	.TransitionTo(1)
	.Execute();
```

## Transition Options

The API uses a fluent builder. Configure the request before calling `Execute`.

### Custom transition effect

Pass a `TransitionEffect` component to use it for the request:

```csharp
SceneMaster.Instance
	.TransitionTo("Gameplay")
	.WithTransitionEffect(myEffect)
	.Execute();
```

By default, a custom effect is registered and used only for the current transition. To also make it the default effect for later transitions, pass `true` as the second argument:

```csharp
SceneMaster.Instance
	.TransitionTo("Gameplay")
	.WithTransitionEffect(myEffect, true)
	.Execute();
```

Transitions without a custom effect use the current default effect.

### Asynchronous loading

Use `LoadAsync` to load the destination scene asynchronously:

```csharp
SceneMaster.Instance
	.TransitionTo("Gameplay")
	.LoadAsync()
	.Execute();
```

### Callback after loading

Callbacks use an `IEnumerator`. The callback runs after the destination scene has loaded and before the transition ends:

```csharp
public void OpenGameplay()
{
	SceneMaster.Instance
		.TransitionTo("Gameplay")
		.WithCallback(OnGameplayLoaded())
		.Execute();
}

IEnumerator OnGameplayLoaded()
{
	yield return null;
}
```

### Loading screen

Configure `LoadingScreenSceneName` on the `SceneMaster` object with the name of a loading screen scene, then request it with `WithLoadingScreen`:

```csharp
SceneMaster.Instance
	.TransitionTo("Gameplay")
	.WithLoadingScreen()
	.Execute();
```

`WithLoadingScreen` automatically enables asynchronous loading. The loading screen scene must be included in Build Settings and contain a `LoadingScreen` implementation.

## Public API Summary

| API | Purpose |
| --- | --- |
| `SceneMaster.Instance` | Access the persistent manager singleton. |
| `TransitionTo(string sceneName)` | Create a transition request using a scene file name. |
| `TransitionTo(int sceneIndex)` | Create a transition request using a Build Settings index. |
| `WithTransitionEffect(TransitionEffect effect, bool setAsDefault)` | Use a specific effect for the request and optionally make it the default effect. |
| `WithCallback(IEnumerator callback)` | Run a coroutine after the destination scene loads. |
| `LoadAsync()` | Load the destination scene asynchronously. |
| `WithLoadingScreen()` | Load asynchronously through the configured loading screen scene. |
| `Execute()` | Start the configured transition. |

## Reporting Bugs and Sharing Suggestions

Feedback is welcome through GitHub:

- [Report a bug or request an improvement](https://github.com/AndresG309/SceneMaster/issues/new/choose) through **Issues**.
- Use [GitHub Discussions](https://github.com/AndresG309/SceneMaster/discussions) for questions, recommendations, general feedback and any other thing that might not need opening an issue.

When reporting a problem, include your Unity version, SceneMaster version or commit, reproduction steps, relevant console errors, or any other thing that would help understanding the situation.

## License

SceneMaster is released under the [MIT License](LICENSE.md).
