# JumpCase – Welfish Studio

Developer: BERKAY KUŞ

Engine: Unity 2023.2.20f1

--------------------------------------------------
🎮 GAME MECHANICS & CONTROLS
--------------------------------------------------

- The game is a side-scrolling 3D platformer where the player controls a slime character.

- The main mechanic is a "jump":
    • Hold SPACE to charge jump power
    • Release SPACE to jump (force scales with charge)

- Movement is controlled using:
    • A-D keys on keyboard
    • Or left/right on gamepad joystick

--------------------------------------------------
⚙️ CORE SYSTEMS IMPLEMENTED
--------------------------------------------------

✅ Input System:
    • Input actions set up for Move (float axis) and Jump (button)
    • Supports both keyboard and gamepad

✅ Charging Jump System:
    • Squash effect during charge
    • More powerful jump via charging.

✅ Coyote Time:
    • Player can still jump briefly (~0.25s) after leaving a platform

✅ Dynamic Gravity:
    • Gravity scale increases when falling to give better feel

✅ Platform Generation:
    • Procedurally spawns platforms as player moves forward
    • Platform Y positions and lenghts are randomized within limits

--------------------------------------------------
🎨 VISUALS & FEEDBACK
--------------------------------------------------

✅ Animations:
    • Idle, Walk -> BlendTree that uses MoveSpeed
    • JumpCharging, Jumping, Landing -> with Charge, Jump and IsGrounded parameters.

✅ Shader / Skybox:
    • Toon shader and stylized skybox.
    • Face Expression System -> Material swap on SkinnedMeshRenderer via Animation Events.

✅ VFX & SFX:
    • Jump, land and walk effects are triggered via Animation Events.

--------------------------------------------------
🤖 AI Usage Declaration
--------------------------------------------------

I used AI to:
- Plan and structure gameplay systems
- Prepare this README :)

All code was written and fully understood by me.

--------------------------------------------------

Thank you for reviewing this prototype!


