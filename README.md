# Baba Is You Level Editor
Simple Level Editor for the game Baba Is You

## Installing
  - Download from the [Releases](https://github.com/ShootMe/BabaIsYouEditor/releases)
  - Run and enjoy

## Change Log
  - 1.3.1
    - Changes
	  - Minor performance improvements

  - 1.3.0
    - Features
	  - Added ability to manually specify object text if it doesnt exist as an image to object editor

  - 1.2.9
    - Fixes
	  - Fixed rule parser with the facing word

  - 1.2.8
    - Changes
	  - Added argtype to object editor for use in mods
	  - Changed how TextType loads for types that dont exist in normal gmae

  - 1.2.7
    - Features
	  - Added a simple rule parser that will hopefully show the words proper active/inactive colors

  - 1.2.6
    - Fixes
	  - Fixed object editor window size when choosing a new object/image

  - 1.2.5
    - Features
	  - Added prompt on exiting or loading a new world that tells you if there are unsaved changes
    - Fixes
	  - Mouse wheel was always moving counter clockwise for changing direction of object
	  - Fixed an error when choosing a tiling for a sprite that doesnt exist for that sprite

  - 1.2.4
    - Features
	  - Added 'Show Animations' option to the level menu
    - Fixes
	  - Fixed image selector window being too big in some cases

  - 1.2.3
    - Fixes
	  - Fixed level picker window being too big

  - 1.2.2
    - Features
	  - Level list and sprite list now resize according to size of program
    - Changes
	  - Ignore bad layer data if the map and layer information mismatch
	  - Improved performance of loading worlds

  - 1.2.1
    - Fixes
	  - Fixed some issues with setting object/image settings in the object editor

  - 1.2.0
    - Changes
	  - Added ability to set operator type and arg extra for sprite objects

  - 1.1.9
    - Fixes
	  - Fixed Ctrl + Right Mouse on Level/Path/Special objects to actually pick them up
    - Changes
	  - Added ability to read custom world palettes
	  - Rendered level colors a bit more accurately

  - 1.1.8
    - Features
	  - On Shift + R with multiple presses it will now search for any 'parent' level that opens your current level
    - Changes
	  - Improved performance when placing/removing sprites in a map
	  
  - 1.1.7
    - Features
	  - Added new option on the 'Add Level' screen to make a copy of the current level you are on
	  - Added new option on the 'New Theme' screen to set if you want to only save the sprite changes and not include the palette
	- Changes
	  - Level lists in popups will now also sort based on the main level sort option (Name/File)
    - Fixes
	  - Fixed issue where when changing direction of currently selected sprite it wasnt updating the display right away

  - 1.1.6
    - Changes
	  - Changed the default program font to be a bit more readable and make things more consistent
	  - Updated the Read Me screen to have the updated controls
	  - Made the level name filter show results in a more user firendly manner
	- Fixes
	  - Fixed a scroll wheel issue introduced in 1.1.5
	  - Fixed how background images were rendered

  - 1.1.5
    - Features
      - Added 'Sort Levels by File' to World menu to sort the level list by file name instead of level name
	  - Added Shift + E when mouse is over a 'Level Object' to go into that level directly
	  - Added Shift + R to then return to the level that you came from
	  - Arrow keys on keyboard will allow you to change the direction of the current cell in the map
	  - Changed Sprite List selection to move with Ctrl + Arrow Keys instead
	  - Added ability to move whole map with Shift + (W/A/S/D)
	  - Added ability on Ctrl + Right Mouse click to 'Pick Up' the currently selected item under your mouse if one exists otherwise it will do it to the top object
	- Changes
	  - Changed new levels to be created with the format XXXlevel to allow for better sorting
    - Fixes
	  - Fixed mouse wheel support for users running Windows 8.1
	  - Fixed Specials rendering ontop of the direction arrows when enabled

  - 1.1.4
    - Features
	  - Added ability to render, add, and modify 'Special Objects' in levels
	  - Right mouse click will now fully copy the object under the mouse where before it would only create a default one
	- Changes
	  - Code cleanup
	- Fixes
	  - Fixed image cache used when choosing levels, was not getting updated all the time

  - 1.1.3
    - Fixes
	  - Fixed sprite loading, was incorrectly determining a variable causing issues

  - 1.1.2
    - Features
	  - Added ability to hit Enter on keyboard on any of the selection popups to select the current item
    - Fixes
	  - Fixed rendering colors of level objects
	  - Fixed ability to type space on keybaord and have it register as a character when searching
	  - Fixed how path objects were edited, as they could become no longer paths in error

  - 1.1.1
    - Features
	  - Added ability to modify path objects in levels
	- Changes
	  - Code cleanup

  - 1.1.0
    - Features
	  - Added ability to add and modify level objects in levels
	  - Added ability to add path objects in levels