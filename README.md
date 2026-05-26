# CSoC 2026 — Game Dev · Week 1 Project: Shadow Protocol

<aside>
  
**The Pitch** — You are a master thief infiltrating a high-security corporate vault. Security drones patrol the halls on strict routes. You must stick to the shadows, dodge their flashlight beams, grab the high-value assets, and escape. If you are spotted, run — but if an enemy is too close when you try to hide, it's **Game Over**.

This project bridges all your Week 1 systems — **Meshes, Physics, NavMesh, Raycasts, Game Managers, Scriptable Objects, and UI** — into a tense stealth-action prototype.

</aside>

<aside>

**How to approach this** — Build *iteratively*. Get the player moving first. Then make one drone patrol. Then add the flashlight. Only wire up the Game Manager once your core stealth loop feels fun. Testing at every step will save you hours of debugging.

</aside>

---

## Phase 1 — The Facility & The Thief

*Meshes, ProBuilder, Rigidbody Movement*

Stealth games live and die by level design. You need corridors with chokepoints, rooms with cover, and — critically — hiding spots that the player can duck into when things go wrong.

### 1.1 Sculpt the Facility

- Open **Window → Package Manager** and install **ProBuilder** (it's free).
- Use ProBuilder (or Unity primitives) to build a **maze-like facility** with:
    - Narrow corridors that force the player into a drone's path
    - At least one open central room (the vault)
    - **Hiding Spots** — alcoves, large crates, or vents the player can slip into
- Keep geometry clean: use **Box Colliders** on walls, not Mesh Colliders (much cheaper for physics).

> 💡 ProBuilder adds colliders automatically when you create geometry. Anything you sculpt is immediately physics-ready — no extra setup needed.
> 

### 1.2 Create the Player (The Thief)

1. Create a **Capsule** GameObject → rename it `Player`.
2. Add a **Rigidbody** component. Set **Constraints → Freeze Rotation X, Y, Z** so the capsule doesn't tip over.
3. Add a **Capsule Collider** (already on the object by default).
4. Write your `PlayerMovement` script:

```csharp
// Read raw input each frame
float h = Input.GetAxisRaw("Horizontal");
float v = Input.GetAxisRaw("Vertical");

// Build a direction, then apply in FixedUpdate
moveDir = new Vector3(h, 0f, v).normalized;

// YOUR CODE: move the rb using moveDir & speed
rb.???(rb.position + ???);
```

<aside>

> **Critical Rule** — Never move a Rigidbody using `transform.position` or `Transform.Translate`. This teleports the object *outside* the physics simulation, causing tunnelling and broken collisions. Always use `MovePosition`, `AddForce`, or `velocity` inside `FixedUpdate`.

</aside>

### 1.3 Bonus Challenge — Crouch Mechanic

Add a crouch that halves speed and shrinks the collider

---

## Phase 2 — The Patrol & The Flashlight

*NavMesh, Waypoints, Spotlight, Raycasting*

### 2.1 Bake the NavMesh

1. Select your floor/walkable geometry in the scene.
2. Open **Window → AI → Navigation** (or install **AI Navigation** package for Unity 2022+).
3. Add a **NavMesh Surface** component to your floor → click **Bake**.
4. Blue overlay = walkable area. If walls are blocking the surface, use **NavMesh Modifier** (set to *Not Walkable*) on them.

> For Unity 2022 LTS, NavMesh is in the **AI Navigation** package. Install it from Window → Package Manager → Unity Registry.
> 

### 2.2 Create the Enemy Drone Prefab

1. Create a **Cube** (or Capsule) → rename it `EnemyDrone`.
2. Add a **NavMesh Agent** component. Set **Speed: 2**, **Angular Speed: 360**, **Stopping Distance: 0.1**.
3. Add an **empty child GameObject** named `SpotlightPivot`. Add a **Light** component to it:
    - Type: **Spot**
    - Range: **8** (note this number — your raycast must match it exactly)
    - Spot Angle: **45**
    - Intensity: **3**
    - Color: a harsh white or pale yellow
    - Point it **forward** along the drone's local Z axis.
4. Save as a **Prefab**.

### 2.3 Waypoint Patrol Script

The enemy has two states — `Patrol` and `Chase`  managed by a C# enum. Each frame, `Update()` checks which state is active and runs the matching behaviour. The patrol behaviour handles waypoint cycling; the chase behaviour locks onto the player's position every frame using `SetDestination()`. The transition between them is triggered by `CanSeePlayer()` — a raycast that fires every frame during patrol. Think of it as: *patrol until you see something, then chase until you lose it.*

```csharp
// Move to next waypoint when close
if (agent.remainingDistance < 0.5f)
    GoToNextWaypoint();

// Cycle the index (wrap around)
currentWP = (currentWP + ???) % waypoints.Length;

// YOUR CODE: send the agent to the right waypoint
agent.???(waypoints[currentWP].position);
```

<aside>
  
> 💡 **The key insight** — `visionRange` in code and `Range` on the Spotlight component must be the **same value**. If they drift apart, the flashlight will visually illuminate something the raycast doesn't detect, which breaks the player's sense of the rules.

</aside>

```csharp
// Fire a ray forward up to visionRange
if (Physics.Raycast(
    transform.position,
    transform.forward,
    out RaycastHit hit,
    visionRange))        // must match Spotlight Range!
{
    // YOUR CODE: check if we hit the player
    if (hit.collider.???("Player"))
        return true;
}
```

### 2.4 Set Up Waypoints in the Scene

1. Create **empty GameObjects** named `Waypoint_0`, `Waypoint_1`, etc. and position them along the patrol route.
2. Select your `EnemyDrone` prefab instance → drag each waypoint into the **Waypoints** array in the Inspector.
3. Tag your Player GameObject as **"Player"** (Edit → Project Settings → Tags & Layers).

### 2.5 Bonus Challenge — Wider Vision Cone

Upgrade `CanSeePlayer()` to use `Physics.SphereCast` or multiple rays spread across the spotlight's angle, simulating a proper cone instead of a single ray.

---

## Phase 3 — The Hiding Mechanic

*Trigger Colliders, Vector3.Distance, Coroutines*

### 3.1 Set Up Hiding Spots

1. Inside each alcove/crate/vent, add an **empty GameObject** named `HideSpot`.
2. Add a **Box Collider** → check **Is Trigger**.
3. Assign it to a new Layer called `HideSpot` (so it doesn't interfere with other collisions).

### 3.2 Player Hiding Script (add to your Player)

```csharp
// Trigger sets the flag
void OnTriggerEnter(Collider other) {
    if (other.CompareTag("HideSpot"))
        isHidden = true;  // flip it back on Exit
}

// Enemy side — measure the gap
float dist = Vector3.Distance(transform.position, ???);

// YOUR CODE: what happens if dist < 5f vs ≥ 5f?
if (dist < 5f) { ??? }
else           { StartCoroutine(???); }
```

> Tag your HideSpot trigger colliders with a new `"HideSpot"` tag.
> 

### 3.3 Detection Threshold in the Enemy Script

Extend `ChaseBehaviour()` to check whether the player successfully hid:

<aside>

> 💡 **`IEnumerator` / `StartCoroutine`** — A Coroutine lets you pause execution for a set time without blocking the rest of the game. `yield return new WaitForSeconds(2f)` suspends *only* this function for 2 seconds, then picks up where it left off.

</aside>

### 3.4 Bonus Challenge — Alert State

Add a middle state between Patrol and Chase: when the player hides successfully, the enemy goes into **Alert** mode — moving to the *last known player position* before giving up, rather than stopping immediately.

---

## Phase 4 — The Vault Assets

*Scriptable Objects*

### 4.1 Create the LootDataSO Blueprint

Create a new C# script called `LootDataSO`. Give it three public fields — a `string` for the name, an `int` for the credit value, and a `Color` for the mesh color. Add the `[CreateAssetMenu]` attribute above the class so it shows up in the right-click Project menu. Once the script compiles, right-click in your Project folder → **Create → Shadow Protocol → Loot Data** to generate your first asset instance.

### **4.2 Create the Asset Instances**

1. Right-click in **Project → Create → Shadow Protocol → Loot Data**.
2. Create **Corporate Data Drive** → Color: Blue, Value: 1000.
3. Create **Prototype Core** → Color: Gold (`#FFD700`), Value: 5000.

### 4.3 The Loot Prefab Script

```csharp
// SO holds the data — you just read it
public LootDataSO data;

void Start() {
    // YOUR CODE: apply data.meshColor to the renderer
    GetComponent<Renderer>().material.??? = data.???;
}

void OnTriggerEnter(Collider other) {
    if (other.CompareTag("Player")) {
        // YOUR CODE: tell the GameManager & destroy self
        GameManager.Instance.???(data.lootValue);
        Destroy(gameObject);
    }
}
```

1. Create a generic **Sphere** prefab named `LootPickup`. Add a **Sphere Collider → Is Trigger: true**.
2. Attach `LootPickup.cs` → drag the appropriate SO into the **Data** slot for each instance.

<aside>
💡

**Why Scriptable Objects here?** All 50 loot prefabs in a full game share *one* data asset in memory per type. Change the value or color of the Corporate Data Drive? Update the SO once — every instance in every scene reflects it instantly.

</aside>

### 4.4 Bonus Challenge — Loot UI Popup

When loot is collected, briefly show a floating `+1000 Credits` text that fades out. Use `TextMeshPro` and a `Coroutine` to animate the alpha from 1 → 0 over 1.5 seconds.

---

## Phase 5 — The Command Center

*Game Manager Singleton, Canvas UI, Scene Management*

### 5.1 The Game Manager

Create a new C# script `GameManager.cs` .

```csharp
// Singleton guard — one instance only
void Awake() {
    if (Instance != null) { Destroy(gameObject); return; }
    Instance = this;
}

// YOUR CODE: pause the game
void TriggerGameOver() {
    Time.timeScale = ???;        // freeze everything
    gameOverPanel.SetActive(???);
}

// YOUR CODE: restart
void RestartLevel() {
    Time.timeScale = 1f;
    SceneManager.???(SceneManager.GetActiveScene().???);
}
```

### 5.2 The 4 Core Menus

**Canvas Setup (all menus):**

- Add a **Canvas** → set **Canvas Scaler → Scale With Screen Size → 1920×1080, Match: 0.5**.
- Each menu is a **Panel** child of the Canvas. Enable/disable via `SetActive()`.

| **Menu** | **Contents** | **Key Code** |
| --- | --- | --- |
| Start Menu (Scene 0) | Title text, "Start Heist" button, "Quit" button | `SceneManager.LoadScene(1)` / `Application.Quit()` |
| Pause Menu | "Resume" + "Main Menu" buttons | `GameManager.Instance.TogglePause()` / `LoadMainMenu()` |
| Game Over | Red panel, "Restart" button | `GameManager.Instance.RestartLevel()` |
| Win Screen | Credits stolen text, "Play Again" button | `GameManager.Instance.RestartLevel()` |

**Getaway Van trigger** (attach to a trigger zone at the exit):

```csharp
void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Player"))
        GameManager.Instance.TriggerWin();
}
```

<aside>

> ⚠️ **Build Settings reminder** — Before testing scene transitions: go to **File → Build Settings** and drag *every* scene into the list. The Start Menu must be index 0, your game level index 1.

</aside>

---

## Bonus Challenges (Master Thief Track)

Completed all five phases? Prove you're a master thief:

- [ ]  **Audio** — Add ambient sound (looping footsteps, heartbeat when near an enemy) using `AudioSource.PlayOneShot()`. Separate the clips into a `SoundManagerSO` Scriptable Object.
- [ ]  **Peripheral Vision** — Upgrade `CanSeePlayer()` with `Vector3.Angle(transform.forward, dirToPlayer)` to check the player is within the spotlight's actual cone angle, not just straight ahead.
- [ ]  **Chase Speed Boost** — When an enemy enters Chase state, increase `agent.speed` to 4f. Reset it to 2f when it returns to Patrol.
- [ ]  **Time Attack Mode** — Display a countdown timer in the UI. Reaching zero triggers Game Over even if the player hasn't been caught.
- [ ]  **Multiple Enemy Types** — Create a second SO-driven `EnemyDataSO` with fields for patrol speed, vision range, and chase speed. Create two different drone types using different SO instances.
- [ ]  **Minimap** — Add a second Camera above the scene rendering only to a **Render Texture** displayed in a UI RawImage in the corner.
