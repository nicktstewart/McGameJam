# Wayward Bones
[![See it on itch!](https://img.itch.zone/aW1nLzg0MDMxNTUucG5n/original/FbEfaa.png)](https://akjanklin.itch.io/wayward-bones)

## TODO 💗
 * Update UI of Game Over 2 (Bossil Fight ver) menu to indicate return to Bossil Fight
### Bugs 🐍
 * Digging dirt too quickly gets the player stuck. 
   - To replicate: enter `Digging` scene and spam any direction (e.g. spam the `A` key) such that the player digs through a line of **dirt** blocks. After breaking two dirt blocks, the player becomes unresponsive to input and the game is unplayable.

## Devlog 🦴
 * Added new Game Over scene to build settings and allowed player to respawn from Bossil Fight if died in Bossil Fight
 * Added some platforms and a heal item to beginning of Boss Fight to familiarise player with mechanics and make level easier
 * Updated bones in dashboard during Boss Fight to match cutscene information
 * Set drill velocity to player velocity to prevent internal collisions
 * Added shadows to Digging scene to hide unexplored areas of map
