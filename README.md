# Grashka Bloodtusk <!-- omit in toc -->

A 2D top-down action RPG dungeon crawler built with Unity. Battle through hellish dungeons, defeat enemies, and face off against powerful bosses.

![Unity](https://img.shields.io/badge/Unity-2022.3-black?logo=unity)
![C#](https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=white)


## Table of Contents <!-- omit in toc -->
- [Gameplay](#gameplay)
- [Controls](#controls)
- [Getting Started](#getting-started)
	- [Requirements](#requirements)
	- [Running the Game](#running-the-game)
- [Project Structure](#project-structure)
- [Technical Highlights](#technical-highlights)
- [License](#license)


## Gameplay

- **Combat:** Dual weapon system with a directional bow and a returning throwing axe
- **Enemies:** Wandering foes, charging minotaurs, and a boss that summons minions
- **Progression:** Multiple dungeon levels with increasing difficulty
- **Items:** Collect coins and health pickups to survive

## Controls

| Input | Action |
|-------|--------|
| WASD / Arrow Keys | Move |
| Mouse | Aim |
| Left Click | Attack |
| Enter | Restart (game over) |
| Escape | Quit |

## Getting Started

### Requirements

- Unity 2022.3.42f1 (LTS)

### Running the Game

1. Clone the repository
2. Open the project in Unity Hub
3. Load the `Intro` scene from `Assets/Scenes/`
4. Press Play

## Project Structure

```
Assets/
├── Animations/     # Animation clips and controllers
├── Fonts/          # Text rendering assets
├── Materials/      # Visual materials
├── Prefabs/        # Reusable game objects
├── Scenes/         # Game levels (Intro, Hell_1, Labyrinth_2, BossLevel_3, etc.)
├── Scripts/        # C# game logic
├── Sprites/        # Characters, items, tiles, UI
└── TilePalettes/   # Tilemap palettes
```

## Technical Highlights

- **Object Pooling:** Efficient projectile recycling for performance
- **Quadrant-Based Aiming:** Clean 4-directional attack system using slope-line math
- **Coroutine-Driven AI:** Enemy behavior, attack cycles, and damage over time
- **Cinemachine Integration:** Smooth camera follow system
- **Multi-Scene Architecture:** Proper level transitions and progression

## License

All rights reserved.
