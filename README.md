# PS4Mono
Library for using PS4 Controllers in Monogame

To initialize the controllers, go to your game's initialize method and add this code:

<code>PS4Mono.InputManager.Initialize(this)</code>

Then in your game's update method go ahead and add this at the end:

<code>PS4Mono.InputManager.Update()</code>

That's all it needs to be set up. To actually use the code, call one of the various GampadCheck functions.

<code>bool down = PS4Mono.InputManager.GamepadCheck(0, Buttons.A);</code>

<code>bool justPressed = PS4Mono.InputManager.GamepadCheckPressed(0, Buttons.A);</code>

There's a special version of this that doesn't work on a frame by frame basis, so if you want to only 
check the buttons being pressed at the exact moment of time of the function call use:

<code>PS4Mono.InputManager.GamepadCheckAsync(0, Buttons.A);</code>

To set the deadzone used to check if the joystick is being held in a specific direction,
you can set this property to a value between 0 and 1.

<code>PS4Mono.InputManager.GamepadAxisDeadZone = value;</code>

If you want to get raw axis or trigger values, you can call:

<code>var rawLeftXAxis = PS4Mono.InputManager.GamepadRawAxis(0, Axis.LeftX);</code>

or

<code>var rawLeftTrigger = PS4Mono.InputManager.GamepadRawTrigger(0, Buttons.LeftTrigger);</code>

The prgram only looks for new controllers every 2 seconds by default, so don't expect your game to react to new input immediately. You can change this in the initialization function by explicitly specifying the poll interval if that's an issue.

Big thanks to www.pinvoke.net and https://github.com/YHiniger/ManagedX.Input
