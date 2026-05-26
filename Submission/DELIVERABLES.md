## Deliverables

## Mission Debrief — Grading Checklist

Before submitting, verify every item below:

| **Feature** | **Requirement** | Status |
| --- | --- | --- |
| Physics Movement | Player moves smoothly using Rigidbody inside `FixedUpdate`. No `Transform.Translate`.| [ ] |
| Level Design | Facility has corridors, a vault room, and at least 3 usable hiding spots.| [ ] |
| AI Patrols | Enemies navigate strictly between set waypoints using NavMesh Agent.| [ ] |
| Raycast Vision | Enemy detects player using a forward Raycast. Range in code matches Spotlight Range.| [ ] |
| Hiding Threshold | Entering a hide spot checks distance: < 5u → Game Over, ≥ 5u → AI resets to patrol after 2s. | [ ] |
| Scriptable Loot | At least two distinct LootDataSO assets drive collectible colour and credit value. | [ ] |
| Complete UI Flow | Start Menu, Pause (with time freeze), Game Over, and Win screen all fully functional. | [ ] |
| Win Condition | Win screen only triggers if player collected at least 1 piece of loot before reaching the exit. | [ ] |

---

Submit all of the following:

1. **Your Unity project folder** containing the assets ans scripts (Exclude the *Library* and *Logs* folders to keep repo size small!).
2. **A standalone build** in a folder named **Build** (exported via File → Build Settings)
3. A brief **README** inside the project root with:
   - Keyboard controls for the Rubik's Cube
   - Any known issues
4. The demo.md will contain a small clip of your task.
5. Add your details in User.sh

---

## Submission Workflow

To submit your tasks, follow these exact steps:

1. **Fork** this repository to your own GitHub account.
2. Clone your forked repository locally:
   ```bash
   git clone <your-fork-url>
   ```
3. Create a new branch using the format `<roll>-<Branch code>`  
   (e.g., `24084001-MIN`) and switch to it:
   ```bash
   git checkout -b <roll>-<Branch code>
   ```
4. Build your Unity project inside Submission directory.
   > **Tip:** Ensure your project folder includes a standard Unity `.gitignore` file so you do not accidentally commit heavy temporary files like the `Library/` directory.
5. Commit your progress to your branch
   (frequent commits are encouraged but optional):
   ```bash
   git add .
   git commit -m "Implement smooth coroutine rotation for cube faces"
   ```
6. Push your local branch to your GitHub fork:
   ```bash
   git push origin <roll>-<Branch code>
   ```
7. Head over to the original repository on GitHub and open a **Pull Request (PR)** from your fork's branch into the `main` branch.
