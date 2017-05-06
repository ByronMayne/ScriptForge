### Quick Note
This is quite an old project but I am still adding to it. Currently the Resources and Animation Widget are not implemented but I am working on that. 
Have any other types of widgets you think would be helpful? Feel free to request it or make your own and I can merge it in. 

# ScriptForge

Scriptforge is a tool used to help you keep your constants up to date. Instead of having to keep track of them by hand they can be generated for you automatically. All you have to do is add the code to your Unity project and go to `Edit/Project Settings/Script FOrge`. These constants can be generated for a range of things. Some example are the Scenes in the build settings, the files in select resource folders, the tags/sorting layers/layers defined in the project. The choice is left to you to choose what you would like to do. 

Can't find a forge creating what you want? Extend the `Widget.cs` class and make your own. Everything is linked with reflection so you don't have to update any other existing code. 


## Widgets
Below is an example of the output of some of the widgets that are in Script Forge.

#### Layers 
![](./docs/imgs/Layers.png)

#### Sorting Layers 
![](./docs/imgs/SortingLayers.png)

#### Scenes 
![](./docs/imgs/Scenes.png)

#### Tags 
![](./docs/imgs/Tags.png)

## Adding a Widget

By default there is always two widgets in view. The first one being the basic settings and the second being about. These are not much use to you. What we will need to do is add a new Widget. To do this click on the `Add Widget` button. 
![](./docs/gifs/AddWidgetMenu.gif)

# How It Works

Script Forge uses the power of T4 [Text Transformation Template Toolkit](https://msdn.microsoft.com/en-us/library/bb126445.aspx) to generate code behind the scenes. If you are not familiar to template take a look at my [Gamasutra blog](http://www.gamasutra.com/blogs/ByronMayne/20160121/258356/Code_Generation_in_Unity.php). TLDR: They are just a string builder with some custom syntax to make it easy.

## Project Layout
In the root of the repository you will find two folders, the first one is 'proj.unity'. This is the normal Unity project folder. The other one is 'proj.cs'. This project is where our templates are stored and then automatically exported to the Unity project with [MSBuild](https://msdn.microsoft.com/en-us/library/0k6kkbsd.aspx). This might seem a little weird but do to the fact that Unity regenerates it's `.csproj` every time scripts change it breaks the links in our templates making them useless. In this folder we also have links to every script in our Unity project just to make sure everything stays in sync. All these things are hooked up with MSBuild but you don't really need to care about that (unless you want to). 

## Error Handling

##### Input Error
Scriptforge does it's best to let you know what is happening. If you happen to make a mistake (like not setting a file path) the widget will flash red and show an error message at the top. You will also notice our status Icon changes to an `!` to make it clear even if the widget is folded.


![](./docs/gifs/Errors.gif)

##### Automatic Name Cleanup

Many of the fields you see in the inspector are used to generate code directly. This means if you set your class name as something invalid (example `!@#$%^&*`) in it's name this would cause a compile error when the code is generated. Instead of letting you do that all inputs are clean up when you hit enter or click off the fields. As a side note these are custom filed types defined in `EditorGUILayoutEx.cs`.


![](./docs/gifs/NameCleanups.gif)

## Want to help out?
If you have a feature you want to add I would be more then happy to add it to the project.


## Meta

Handcrafted by Byron Mayne [[twitter](https://twitter.com/byMayne) &bull; [github](https://github.com/ByronMayne)]

Released under the [MIT License](http://www.opensource.org/licenses/mit-license.php).

If you have any feedback or suggestions for UnityIO feel free to contact me. 
