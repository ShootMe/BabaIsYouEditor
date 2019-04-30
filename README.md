# Baba Is You Level Editor
Simple Level Editor for the game Baba Is You

## Installing
  - Download from the [Releases](https://github.com/ShootMe/BabaIsYouEditor/releases)
  - Run and enjoy

## Change Log
  - 1.1.5
    - Features
      - Added 'Level Sort by File' to World menu to sort the level list by file name instead of level name
	  - Added Shift + E when mouse is over a 'Level Object' to 'go' into that level directly
	  - Added Shift + R to then return to the level that you came from
	  - Arrow keys on keybaord will also allow you to change the direction of the current cell in the map
	  - Changed Sprite List to then move with Ctrl + Arrow Keys
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