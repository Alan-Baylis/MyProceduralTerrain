# MyProceduralTerrain
This is a project for infite procedural terrain, path, and location creation in Unity3D.

Things I wish to do:
*Implement terrain generation using OpenSimplex noise
*Refine terrain generation using a Quadtree LOD system
..*This needs to be as efficient as possible. Get it working first, but probably will have a lot of optimization here.
*Generate locations such as towns for the terrain, using an infinite and deterministic process
..*It needs to be deterministic so that paths can also be generated on the fly.
*Generate paths between these locations using A* algorithm.
..*Paths need to be generated on the fly, and updated as more locations are generated.
....*This means that locations have to be determined much further in advance than when players get near them, else new paths may spring up around the player.

Overall, the goal is to make an infinite procedural world.

Later tasks
*Location population.
..*Locations need to be populated with structures and inhabitants.
....*This will likely delve deeper into generation, such as generating buildings, then the rooms inside them, then the contents of those rooms.
*Encounter System
..*Generate random encounters, likely following similar logic to D&D encounter generation.
*Generate stuff to do!
..*This is broad. But, there's no point in an infinite world generation system if there is nothing to do but walk around in it.