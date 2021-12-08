Requirements:
- Tested on Unity 2020.3.11f1

Installation:
- Unpack gdrsdk to empty project
- Open Window->Package Manager, select packages type to "Unity Registry", find "Universal RP" (URP) install it (any version. ps Tested on v.10.5.1)
- Go to Edit->Project Settings->Graphics->Scriptable Render Pipeline Settings select MyURP, also go to Quality->Rendering select MyURP

Things to adjust:
- Look at MyURP shadow distance
- Window->Rendering->Lighting set skybox material if you need it, also recommend you to set environment reflections cubemap
- Other setting like in Main scene

Scene to use:
- Current version of scene where you should start play around is "Main"
- Old version of scene stored as "OldScene" and contains all "test" objects just in case

Scene content:
- QLog (you cant rename it in hierarchy, sorry about that). It's main debug tool to test your scripts and dont mess with someone's debug strings (remember through PP saving instead inspector so you will always see your logs), just tick which scripts should use debug. You can add debug code in your scripts like that: 
if (isDebug) DLog.D($"InitializeGame"); 
but also you should create such fields:
public static bool isDebug;
public static Color debugColor;
Then add your script to QLog.DrawData method
- Main Camera contains CameraMover.cs you can use all public methods, they have proper naming and inspector tweaks
- Default Camera View - move it around the scene, item for CameraMover
- Directional Light (almost default settings)
- GeneralScripts - contain all sdk base scripts
- GameManager - main cs logic binder, should contain all script-to-script relations if they arent created as one logic
- MainCanvas - logic to UI, singleton, can use freely to access all UI elements, also has script that generates UIManager.cs related on Canvas organization. Good choice to use in all static UI inits for all scripts, button binding etc.
- MainData - local database of global in-game variables, such as level, money, etc
- LevelManager - main component for level instantiation, should create prefabs of levels and iterate them
- EditorDatabase - Addon which fill data from folders (used to be extendable for your needs) (note that this script load feature works only in editor, it just prepares some prefabs for you instead manual work)
- Devs - Collector Script - contain all links to game objects in scene, even hidden and inactive ones, used in singleton initialization (if singleton was hidden or diactivated). Coroutine Actions - must have and use tool for coroutine templates such as ExecuteAfterDelay, WaitForConditionAndDoAction, Change any value in selected timings. All coroutines are runned on singleton game object, so you can dont care about game object activity for coroutine working
- SetValueAnimCoins - actually PP reading and animated value increace/decreace for selected Text component, pretty handy, can place on object itself, ideally upgrade to value referer (ref float/int) in inspector (gl with achieving that, System.Reflection should help)
- Event system - default event system for UI and any other type of event
- SDK - should contain all sdk scripts
- Scripts - will contain in-game scripts with some default ones
- SwipeSystem + SwipeManager contain Swipe detection (which side), also can get swipeVelocity, zoom pinching with it's delta
- TimeManager - store TimeSpan, DateTime into PP. Has google online time sync to prevent cheats (if have internet). Can use for counting time to some event like "end of sell" or "next day free gift"
- CameraShake - will shake your camera if you want to add such effect while blowing things
- Independed Move System - multitask tool, can rotate, move, move by path, repeat move, local move, local rotate your objects DOTween/HOTween not used, but pretty much the same. Use all public methods, also watch how it behaves if you will set next method while previous one is still active
- Level Location Manager - some old script for env changing while it's not a part of level prefab
- CanvasBG - easy world space custom gradient material plane for drawing some gradients you want. Only screen space working

Tips:
- To clear game player prefs use MainData.ClearGamePP method (available through menu "Edit/Clear Game PP", also you can add there necessary PP data.
