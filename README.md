# GenerativeDoom
Procedural generation of doom level using prefabs to build a coherent Level design

## What is it ?
> GenerativeDoom is a plugin for Doom builder that let you create a whole map with a coherent level design given an amount of specific rooms.

## How to use it ?
- Copy the source in your Doom Builder project in the "doombuilder/Sources/Plugins/" folder. 

- Change MapSet.Disposal() and Map ctor to public
- Expose General.Map.map vars (required to clear the map between two LD generation)

            //clear map
            General.Map.map.Dispose();
            General.Map.map = new MapSet();

- Expose InsertPrefabStream and PastePrefab functions in the CopyPasteManager class (to be able to copy and paste prefabs).
- paste the "prefabs/" folder in the build folder. 

- You're done !


## How does it work ?
The generation is made in a few steps:

- First we import all rooms by reading the json files extracting data about saved prefabs.
- Then we sort the rooms by RoomType


        public enum RoomType {
        NONE,      // default (should not occur)
        Key,       // contains a key (spawned by a script)
        Locked,    // doors are locked (you need a key to access this room)
        Boss,      // contains a Boss
        Basic,     // contains whatever you want
        Spawn,     // PlayerStart thing
        End        // End thing
        }

- We can now order the rooms following a simple algorithm :
Spawn the **Locked** rooms first then the **Boss** rooms and then the **Basic** ones.
We *shuffle the rooms* to randomize their order.
Afterwards we spawn the **Key** rooms to be sure that there is at least a key room before a locked room.
Then we only need to add the **Spawn** and **End** room and we are done !

- Given this order we assigned random direction to each room relative to the previous one avoiding superposition or going up (see figure below) (the rooms are generated in 3 direction only, down, left or right).

![LD generation](https://image.noelshack.com/fichiers/2017/13/1490696099-builder-2017-03-28-12-13-10.png)

- The last thing to do is to **link rooms to eachothers**. To do so we look for possible **shortcuts** (*for example at the bottom left of the figure above where there are 4 rooms, the bottom left one has been linked to the one above because none of the 2 other rooms at the left are locked so this link does'nt break the LD*).

----

## Want to contribute or re-use my work ?
Sure ! Just credit me if you do so :)
