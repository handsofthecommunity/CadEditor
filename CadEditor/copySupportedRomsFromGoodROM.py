from shutil import copyfile
import os

GoodNesPath = r"d:\ROM\Dendy\GoodNES3.1\best"

fnames = [
    "Adventures in the Magic Kingdom (E) [!].nes",
    "Alien 3 (E) [!].nes",
    "Batman (U) [!].nes",
    "Batman III [p1].nes"
    "Battletoads & Double Dragon - The Ultimate Team (E) [!p].nes",
    "Battletoads (U) [!].nes",
    "Banana Prince (G) [!].nes",
    "Bucky O'Hare (E).nes",
    "Captain America and The Avengers (U) [!].nes",
    "Chip 'n Dale Rescue Rangers (U) [!].nes",
    "Chip 'n Dale Rescue Rangers 2 (U) [!].nes",
    "Contra Force (U) [!].nes",
    "Darkwing Duck (U) [!].nes",
    "Duck Tales (U) [!].nes",
    "Duck Tales 2 (U) [!].nes",
    "Flintstones, The - The Rescue of Dino & Hoppy (E) [!p].nes",
    "Flintstones, The - The Surprise at Dinosaur Peak! (U) [!p].nes",
    "Goonies II, The (U) [!].nes",
    "Hook (U) [!].nes",
    "Hudson's Adventure Island II (U) [!].nes",
    "Hudson's Adventure Island III (U) [!].nes",
    "Jackal (U) [!].nes",
    "Jungle Book, The (U) [!].nes",
    "Kyatto Ninja Teyandee (J).nes",
    "Little Mermaid, The (U) [!].nes",
    "Megaman (U) [!].nes",
    "Megaman II (U) [!].nes",
    "Megaman III (U) [!].nes",
    "Megaman IV (U) (PRG0) [!].nes",
    "Megaman V (E) [!].nes",
    "Mickey Mouse 3 - Yume Fuusen (J).nes",
    "Mickey's Adventures in Numberland (U) [!p].nes",
    "Micro Machines (Unl) [!].nes",
    "Mitsume ga Tooru (J).nes",
    "Monster In My Pocket (U) [!].nes",
    "New Ghostbusters II (E) [!].nes",
    "Ninja Gaiden (U) [!].nes",
    "Power Blade (E) [!].nes",
    "Power Blade 2 (U) [!].nes",
    "Raf World (J).nes",
    "Rockin' Kats (U) [!].nes",
    "Shadow of the Ninja (U) [!].nes",
    "Shatterhand (U) [!].nes",
    "Super C (U) [!].nes",
    "Super Robin Hood [p1][!].nes",
    "Super Spy Hunter (U) [!].nes",
    "TaleSpin (E) [!].nes",
    "Teenage Mutant Ninja Turtles (U) [!].nes",
    "Teenage Mutant Ninja Turtles II - The Arcade Game (U) [!].nes",
    "Teenage Mutant Ninja Turtles III - The Manhattan Project (U) [!].nes",
    "Terminator 2 - Judgment Day (E) [!].nes",
    "Tiny Toon Adventures (U) [!].nes",
    "Tom & Jerry (and Tuffy) (J).nes",
    "Young Indiana Jones Chronicles, The (U) [!].nes"
]

for fname in fnames:
    copyfile(os.path.join(GoodNesPath, fname), fname)