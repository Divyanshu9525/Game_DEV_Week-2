## Resources

### Phase 1 — Meshes & Movement

| **Resource** | **Why you need it** |
| --- | --- |
| [ProBuilder Documentation](https://docs.unity3d.com/Packages/com.unity.probuilder@6.0/manual/index.html) | Official manual — shape creation, UV editing, ProGrids snapping. Start here for level sculpting. |
| [Rigidbody API Reference](https://docs.unity3d.com/ScriptReference/Rigidbody.html) | Full list of Rigidbody methods: AddForce, MovePosition, velocity, interpolation. Essential for Phase 1. |
| [CharacterController vs Rigidbody (Unity Docs)](https://docs.unity3d.com/Manual/class-CharacterController.html) | Explains when to use each approach — important context for your movement design choice. |
| [Input.GetAxisRaw vs GetAxis](https://docs.unity3d.com/ScriptReference/Input.GetAxisRaw.html) | Understand the difference: `GetAxisRaw` returns -1/0/1 with no smoothing — what you want for responsive stealth movement. |

### Phase 2 — NavMesh & Raycasting

| **Resource** | **Why you need it** |
| --- | --- |
| [AI Navigation Package (Unity Docs)](https://docs.unity3d.com/Packages/com.unity.ai.navigation@2.0/manual/index.html) | The official manual for the modern NavMesh system used in Unity 2022+. NavMesh Surface, Modifier, Agent — all explained. |
| [NavMesh Agent API](https://docs.unity3d.com/ScriptReference/AI.NavMeshAgent.html) | Scripting reference for `SetDestination`, `remainingDistance`, `pathPending`, `ResetPath`. You'll use all of these. |
| [Physics.Raycast API Reference](https://docs.unity3d.com/ScriptReference/Physics.Raycast.html) | All overloads of Raycast explained — including how to use `RaycastHit`, `LayerMask`, and `maxDistance`. |
| [Light Component Reference (Spot Light)](https://docs.unity3d.com/Manual/class-Light.html) | Documents Range, Spot Angle, and other Spotlight parameters — so you can match them to your raycast precisely. |
| [Visualising Raycasts with Debug.DrawRay](https://docs.unity3d.com/ScriptReference/Debug.DrawRay.html) | Use `Debug.DrawRay(origin, direction * range, Color.red)` to see your raycasts in Scene view during play — invaluable for debugging vision. |

### Phase 3 — Triggers & Coroutines

| **Resource** | **Why you need it** |
| --- | --- |
| [OnTriggerEnter / Collider Events (Unity Docs)](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnTriggerEnter.html) | Official reference for all trigger callbacks: Enter, Stay, Exit. Includes the requirement that one object must have a non-kinematic Rigidbody. |
| [Vector3.Distance API](https://docs.unity3d.com/ScriptReference/Vector3.Distance.html) | How Unity calculates straight-line distance between two points. The backbone of your detection threshold. |
| [Coroutines in Unity (Unity Manual)](https://docs.unity3d.com/Manual/Coroutines.html) | Full explanation of `IEnumerator`, `yield return`, `WaitForSeconds`, and `WaitForSecondsRealtime` (needed when `timeScale = 0`). |
| [Finite State Machines in Unity (Game Dev Beginner)](https://gamedevbeginner.com/state-machines-in-unity-how-and-when-to-use-them/) | A clear guide on using C# enums for enemy states (Patrol / Chase / Alert). Great pattern for extending your AI. |

### Phase 4 — Scriptable Objects

| **Resource** | **Why you need it** |
| --- | --- |
| [ScriptableObject API Reference](https://docs.unity3d.com/ScriptReference/ScriptableObject.html) | Official API docs — CreateAssetMenu attribute syntax, CreateInstance, OnEnable lifecycle. |
| [ScriptableObject Manual Page](https://docs.unity3d.com/Manual/class-ScriptableObject.html) | Explains *when* to use SOs vs MonoBehaviours, memory sharing behaviour, and serialization. |
| [Renderer.material vs sharedMaterial](https://docs.unity3d.com/ScriptReference/Renderer-material.html) | Important: `renderer.material` creates a unique copy (good for runtime color changes). `renderer.sharedMaterial` edits the source asset. Know the difference. |

### Phase 5 — UI & Scene Management

| **Resource** | **Why you need it** |
| --- | --- |
| [SceneManager.LoadScene API](https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.LoadScene.html) | Load by name or index. Also see `LoadSceneAsync` for a loading screen. |
| [Time.timeScale (Unity Docs)](https://docs.unity3d.com/ScriptReference/Time-timeScale.html) | How to pause (set to 0) and resume (set to 1). Important note: use `WaitForSecondsRealtime` in coroutines that need to run while paused. |
| [TextMeshPro Getting Started](https://docs.unity3d.com/Packages/com.unity.textmeshpro@3.0/manual/index.html) | TMP is the standard text component — better rendering, more formatting options. Required for the credits display. |
| [Canvas Scaler Reference](https://docs.unity3d.com/Packages/com.unity.ugui@2.0/manual/script-CanvasScaler.html) | Set Scale With Screen Size for a UI that looks right on all resolutions. Match value 0.5 balances width/height scaling. |
| [Singleton Pattern — Game Architecture (Unity Blog)](https://unity.com/blog/engine-platform/level-up-your-code-with-game-programming-patterns) | Unity's own explanation of common game patterns including Singleton. Understand *why* the pattern exists before using it. |
