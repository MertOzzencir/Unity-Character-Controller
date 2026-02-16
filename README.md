# Physics-Based Character Controller

A fully physics-driven third-person character controller built with **spring forces** and **torque-based rotation**. No CharacterController component — pure Rigidbody physics for realistic, responsive movement.

![Movement Demo](screenshots/movement.gif)
*Smooth, physics-based movement with spring suspension and dynamic rotation*

## 🎯 Project Goals

After working with Unity's CharacterController and basic Rigidbody movement, I wanted to build a **truly physics-based** controller that feels both responsive and realistic. This implementation uses springs, dampers, and torque to create natural-feeling movement.

**Key Learning Objectives:**
- Understand **spring-damper systems** for suspension
- Implement **quaternion-based rotation** correction
- Use **animation curves** for dynamic acceleration
- Build **camera-relative movement** without transform.Translate

## 🔧 Core Systems

### 1. Spring-Based Suspension

The character "hovers" above the ground using a spring-damper system — similar to car suspension.

**Spring Force Formula:**
```
force = (targetHeight - currentHeight) × springStrength - velocity × damping
```

**How It Works:**
- Raycast downward to measure distance from ground
- Calculate compression/extension from target ride height
- Apply spring force to maintain height
- Damper prevents bouncing

This creates smooth movement over uneven terrain without rigid ground detection.

### 2. Acceleration with Animation Curves

Instead of constant acceleration, the controller uses curves to modify acceleration based on movement direction.

**Velocity Dot Product:**
```csharp
float velDot = Vector3.Dot(movementDirection, currentVelocity)

// velDot = 1.0  → Moving in same direction (maintain speed)
// velDot = 0.0  → Perpendicular turn (moderate acceleration)
// velDot = -1.0 → Reversing direction (maximum acceleration)
```

**Why This Matters:**
- Turning feels different from straight movement
- Reversing direction requires more force
- Creates more natural, arcade-style feel

### 3. Quaternion-Based Rotation

Rotation uses **torque** instead of directly setting transform.rotation.

**Upright Correction:**
```csharp
Quaternion currentRotation = transform.rotation;
Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
Quaternion correction = targetRotation × Inverse(currentRotation);

// Convert to angle-axis
correction.ToAngleAxis(out angle, out axis);

// Apply as torque
rigidbody.AddTorque(axis × angle × strength - angularVelocity × damping);
```

**Benefits:**
- Smooth, physics-based rotation
- No snapping or teleporting
- Responds naturally to collisions
- Damping prevents overshooting

### 4. Camera-Relative Movement

Movement input is transformed to world space based on camera direction.

```csharp
Vector3 forward = camera.forward;
forward.y = 0; // Flatten to ground plane

Vector3 right = camera.right;
right.y = 0;

Vector3 moveDirection = right × input.x + forward × input.y;
```

This allows intuitive "press forward to move where camera looks" control.

## 💡 What I Learned

### Spring-Damper Systems
- Springs alone cause infinite bouncing
- Damping absorbs energy and stabilizes
- Tuning spring strength vs damping is an art
- Higher spring = stiffer suspension
- Higher damping = less bouncy response

### Quaternion Math
- Quaternions represent rotations without gimbal lock
- `Inverse(q)` represents opposite rotation
- `q1 × Inverse(q2)` gives rotation between q1 and q2
- `w < 0` check ensures shortest rotation path
- ToAngleAxis converts to torque-friendly format

### Animation Curves for Game Feel
- Linear acceleration feels robotic
- Curves based on dot product create natural transitions
- Different curves for acceleration vs deceleration
- Easy to tune in editor without code changes

### Physics-Based vs Kinematic
**Kinematic Controllers:**
- Predictable and precise
- No physics interactions
- Manual collision handling

**Physics-Based Controllers:**
- Natural interactions with environment
- Responds to forces and collisions
- Can feel "floaty" without tuning
- More complex to implement

## 🎓 Technical Highlights

**Spring Suspension:**
- Raycast-based ground detection
- Considers moving platforms (hitBody.velocity)
- Separate spring strength and damping parameters

**Movement Force:**
- Goal velocity system with MoveTowards
- Force clamping prevents infinite acceleration
- Per-axis force scaling (forceScale)

**Rotation System:**
- Quaternion slerp would be simpler but less physics-accurate
- Torque-based allows collision response
- Separate strength and damping for fine control

## 🛠️ Technical Stack

- **Unity 6000.0.2**
- **Pure Rigidbody physics** (no CharacterController)
- **New Input System**
- **Animation Curves** for tuning

## 📂 Code Structure

```
Assets/
├── PlayerController.cs    # Main physics controller
└── InputManager.cs         # Input handling
```

## 🔍 Challenges & Solutions

**Unstable Suspension**
- Problem: Character bouncing endlessly
- Solution: Increased damping value, tuned spring strength

**Slow Turning Response**
- Problem: Rotation felt sluggish
- Solution: Higher upright correction strength, lower damping

**Ice Skating Feel**
- Problem: Too much sliding after releasing input
- Solution: Adjusted acceleration curves, added force scaling

**Camera Conflicts**
- Problem: Movement direction jumpy during camera rotation
- Solution: Flatten camera vectors to Y=0 plane

## 📚 Learning Resource

This implementation was built following concepts from:
- **Toyful Games** YouTube tutorial on physics-based character controllers
- Unity documentation on Rigidbody physics
- Animation curve-based acceleration techniques

## 🎯 Future Improvements

- [ ] Add jump with spring compression
- [ ] Wall detection and climbing
- [ ] Speed-based animation blending
- [ ] Ground material response (ice, mud, etc.)
- [ ] Air control when not grounded

## ⚠️ Note

This is a **learning implementation** focused on understanding physics-based movement. It demonstrates core spring-damper concepts and quaternion rotation, though production controllers would need additional features like jump, crouch, and complex collision handling.

---

**Developer**: Mert Özzencir  
**GitHub**: [MertOzzencir](https://github.com/MertOzzencir)  
**Unity Version**: 6000.0.2  
**Learning Focus**: Spring-damper systems, quaternion math, physics-based gameplay  
**Credits**: Inspired by **Toyful Games** physics controller tutorials