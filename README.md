# UnityExplorerAddons

This adds some features to the very useful unity game mod [UnityExplorer by sinai-dev](https://github.com/sinai-dev/UnityExplorer).

Right now, this is only one additional feature added: Watches

# What are Watches?

Watches are, simply, a way to watch a value update in real time. In this mod, they are implemented via a Popup box on the righthand side of your screen which can be toggled via a new button on the main UnityExplorer Nav bar. After a watch has been added to the list, it can be removed by pressing the X button next to it when the UnityExplorer UI is open.

## Simple watches

When you're inspecting a component or class, you will see a new, blue "Watch" button next to the Copy and Paste buttons for each field. When pressed, it will add a "Simple Watch" to the watch box. This will always follow the format of: 
`Class.FieldName: value.ToString()`. If it's a component attached to a GameObject, the GameObject's name will be added to the end. The watch will ensure that the object hasn't been destroyed, and if it has, will remove the watch from the list.

If you click watch on a method, it will attempt to evaluate that method every frame and show the return value from it in the same format as above. Be careful of this since if you watch the wrong method, you can cause something to happen a lot very quickly.

If the method has parameters, then until you open the Evaluator box in the inspector for that method, it will fail to be added to the watch list. Once you open it, every parameter is given a default value, and it can then be watched. However, if it errors, for example with a `NullReferenceException`, it will be removed from the watch list and the error printed. The watch shown will always run with the parameters provided in the Evaluator box at the time you click the watch button.

## Expression watches

Expression watches are a way to run custom code inside your watches. This can be useful if you want to show information from a struct or class reference that changes every frame. You provide an expression watch with a function to evaluate, and it shows the string your function returns every frame under `Expression Watches` in the window.

You can add an expression watch in a couple different ways and formats. First, you can reference `UnityExplorerAddons.API.WatchAPI` in a plugin and call the `Watch()` method(s) from there, or you can call `Watch()` directly from the Unity Explorer C# console.

The different signatures for `Watch()` are as follows:

`Watch(Func<string>)` - This is for a "static" watch. It will run the provided function and output the string it returns.

`Watch<Type>(Type, Func<Type, string>)` - Here, you define the type of the passed object, the object instance you want to be input into your function, and the function itself. It will run the function with the provided as the input.

`Watch<Type>(object, Func<Type, string>)` - More or less just an easier way to access to similar signature above. This just allows you to pass a generic object, so you don't need to manually cast it yourself while creating the function.

<sup><sup>Technically the funcs are predefined delegates, but those are the func signatures you would be matching with them</sup></sup>

# Why not fork??

Cause I'm lazy and this felt easier.

# How to build
(these instructions are gonna be centered around GTFO since that's the game I've been using while making this mod of a mod)

* Run GTFO with BepInEx once to generate interop folder
* Download/clone this repository somewhere
* Change the path in `GameFolder.props` to the folder your BepInEx folder resides in, such that the path `$(GameFolder)\BepInEx` points to your actual BepInEx folder
* Install UnityExplorer, and modify the path to it in `Properties.props` at line 42 to point to your UnityExplorer/UniverseLib dlls
* Build

# NOTE: WILL LIKELY NOT WORK OUT OF THE BOX WITH MONO GAMES

Since GTFO is compiled with Il2Cpp, the plugin itself is built around Il2Cpp BepInEx environments. To my knowledge the changes you'd have to make to build it against a mono game would be not that difficult, so if you're up for it yourself, make those (presumably) simple changes and build it for mono :)
