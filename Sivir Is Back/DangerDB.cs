using System;
using System.Collections.Generic;

using System;
using LeagueSharp;
using LeagueSharp.Common;
using System.Collections.Generic;

namespace Sivir_Is_Back
{
    public enum Skilltype
    {
        Unknown = 0,
        Line = 1,
        Circle = 2,
        Cone = 3
    }

    public class DangerDB
    {
        public string HeroName { get; set; }
        public string SpellMenuName { get; set; }
        public SpellSlot Slot { get; set; }
        public Skilltype Type { get; set; }
        public float Radius { get; set; }
        public string SDataName { get; set; }
        public int DangerLevel { get; set; }

        public static List<DangerDB> GDList = new List<DangerDB>(); // Generic Dangerous List
        public static List<DangerDB> CCList = new List<DangerDB>(); // Crowd Control List
        public static List<DangerDB> SList = new List<DangerDB>();  // Silence List

        static DangerDB()
        {
            #region CCList
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Aatorx",
                    SpellMenuName = "Dark Flight",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Circle,
                    SDataName = "AatroxQ",
                    DangerLevel = 5
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Aatorx",
                    SpellMenuName = "Blades of Torment",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Cone,
                    SDataName = "AatroxE",
                    DangerLevel = 3
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Ahri",
                    SpellMenuName = "Charm",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Line,
                    SDataName = "AhriSeduce",
                    DangerLevel = 5
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Alistar",
                    SpellMenuName = "Pulverize",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Circle,
                    SDataName = "Pulverize",
                    DangerLevel = 5
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Alistar",
                    SpellMenuName = "Headbutt",
                    Slot = SpellSlot.W,
                    SDataName = "Headbutt",
                    DangerLevel = 3
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Amumu",
                    SpellMenuName = "Bandage Toss",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    SDataName = "BandageToss",
                    DangerLevel = 5
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Amumu",
                    SpellMenuName = "Curse of the Sad Mummy",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Circle,
                    SDataName = "CurseoftheSadMummy",
                    DangerLevel = 5
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Anivia",
                    SpellMenuName = "Flash Frost",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    SDataName = "FlashFrost",
                    DangerLevel = 5
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Anivia",
                    SpellMenuName = "Glacial Storm",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Circle,
                    SDataName = "GlacialStorm",
                    DangerLevel = 3
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Annie",
                    SpellMenuName = "Tibbers",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Circle,
                    SDataName = "InfernalGuardian",
                    DangerLevel = 5
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Ashe",
                    SpellMenuName = "Crystal Arrow",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Line,
                    SDataName = "EnchantedCrystalArrow",
                    DangerLevel = 5
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Ashe",
                    SpellMenuName = "Volley",
                    Slot = SpellSlot.W,
                    Type = Skilltype.Cone,
                    SDataName = "Volley",
                    DangerLevel = 3
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Azir",
                    SpellMenuName = "ShiftingSands",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Circle,
                    SDataName = "AzirE",
                    DangerLevel = 3
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Azir",
                    SpellMenuName = "Emperor's Divide",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Circle,
                    SDataName = "AzirR",
                    DangerLevel = 5
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Blitzcrank",
                    SpellMenuName = "Rocket Grab",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    SDataName = "RocketGrab",
                    DangerLevel = 5
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Blitzcrank",
                    SpellMenuName = "Power Fist",
                    Slot = SpellSlot.E,
                    SDataName = "PowerFist",
                    DangerLevel = 3
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Brand",
                    SpellMenuName = "Sear",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    SDataName = "BrandBlazeMissile",
                    DangerLevel = 3
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Bruam",
                    SpellMenuName = "Winter's Bite",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    SDataName = "BraumQ",
                    DangerLevel = 3
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Bruam",
                    SpellMenuName = "Glacial Fissure",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Line,
                    SDataName = "BraumR",
                    DangerLevel = 5
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Caitlyn",
                    SpellMenuName = "90 Caliber Net",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    SDataName = "CaitlynEntrapment",
                    DangerLevel = 3
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Cassiopeia",
                    SpellMenuName = "Petrifying Gaze",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Cone,
                    SDataName = "CassiopeiaPetrifyingGaze",
                    DangerLevel = 5
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Cho'gath",
                    SpellMenuName = "Rupture",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Circle,
                    SDataName = "Rupture",
                    DangerLevel = 5
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Darius",
                    SpellMenuName = "Aprehend",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Cone,
                    SDataName = "DariusAxeGrabCone",
                    DangerLevel = 3
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Diana",
                    SpellMenuName = "Moonfall",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Circle,
                    SDataName = "DianaVortex",
                    DangerLevel = 3
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "DrMundo",
                    SpellMenuName = "Cleaver",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    SDataName = "InfectedCleaverMissileCast",
                    DangerLevel = 3
                });

            CCList.Add(
                new DangerDB
                {
                    HeroName = "Draven",
                    SpellMenuName = "Stand Aside",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Line,
                    SDataName = "DravenDoubleShot",
                    DangerLevel = 3
                });

            CCList.Add(
               new DangerDB
               {
                   HeroName = "Elise",
                   SpellMenuName = "Cocoon",
                   Slot = SpellSlot.E,
                   Type = Skilltype.Line,
                   SDataName = "DravenDoubleShot",
                   DangerLevel = 3
               });

            CCList.Add(
                new DangerDB
                {
                    HeroName = "Evelynn",
                    SpellMenuName = "Agony's Embrace",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Circle,
                    DangerLevel = 5,
                    SDataName = "EvelynnR",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Fizz",
                    SpellMenuName = "Chum the Waters",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Line,
                    DangerLevel = 5,
                    SDataName = "FizzMarinerDoomMissile",
                });
            CCList.Add(
                 new DangerDB
                 {
                     HeroName = "Fizz",
                     SpellMenuName = "Playful Trickster",
                     Slot = SpellSlot.E,
                     Type = Skilltype.Line,
                     DangerLevel = 3,
                     SDataName = "FizzJump",
                 });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Galio",
                    SpellMenuName = "Resolute Smite",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Circle,
                    DangerLevel = 2,
                    SDataName = "GalioResoluteSmite",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Galio",
                    SpellMenuName = "Idol Of Durand",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Circle,
                    DangerLevel = 5,
                    SDataName = "GalioIdolOfDurand",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Gnar",
                    SpellMenuName = "Boomerang Throw",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 2,
                    SDataName = "GnarQ",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Gnar",
                    SpellMenuName = "Bouldar Toss",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 2,
                    SDataName = "GnarBigQ",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Gnar",
                    SpellMenuName = "Wallop",
                    Slot = SpellSlot.W,
                    Type = Skilltype.Line,
                    DangerLevel = 2,
                    SDataName = "GnarBigW",
                });

            CCList.Add(
                new DangerDB
                {
                    HeroName = "Gnar",
                    SpellMenuName = "GNAR!",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Circle,
                    DangerLevel = 5,
                    SDataName = "GnarR",
                });

            CCList.Add(
                new DangerDB
                {
                    HeroName = "Gragas",
                    SpellMenuName = "Barrel Roll",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Circle,
                    DangerLevel = 2,
                    SDataName = "GragasQ",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Gragas",
                    SpellMenuName = "Body Slam",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Line,
                    DangerLevel = 2,
                    SDataName = "GragasE",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Gragas",
                    SpellMenuName = "Explosive Cask",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Circle,
                    DangerLevel = 5,
                    SDataName = "GragasR",
                });

            CCList.Add(
                new DangerDB
                {
                    HeroName = "Heimerdinger",
                    SpellMenuName = "Electron Storm Grenade",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Circle,
                    DangerLevel = 3,
                    SDataName = "HeimerdingerE",
                });
            CCList.Add(
                 new DangerDB
                 {
                     HeroName = "Hecarim",
                     SpellMenuName = "Onslaught of Shadows",
                     Slot = SpellSlot.R,
                     Type = Skilltype.Circle,
                     DangerLevel = 5,
                     SDataName = "HecarimUlt",
                 });
            CCList.Add(
                  new DangerDB
                  {
                      HeroName = "Hecarim",
                      SpellMenuName = "Devestating Charge",
                      Slot = SpellSlot.E,
                      Type = Skilltype.Circle,
                      DangerLevel = 3,
                      SDataName = "HecarimRamp",
                  });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Janna",
                    SpellMenuName = "Howling Gale",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "HowlingGale",
                });
            CCList.Add(
                 new DangerDB
                 {
                     HeroName = "Janna",
                     SpellMenuName = "Zephyr",
                     Slot = SpellSlot.W,
                     DangerLevel = 3,
                     SDataName = "ReapTheWhirlwind",
                 });
            CCList.Add(
                 new DangerDB
                 {
                     HeroName = "Jax",
                     SpellMenuName = "Counter Strike",
                     Slot = SpellSlot.E,
                     Type = Skilltype.Line,
                     DangerLevel = 5,
                     SDataName = "JaxCounterStrike",
                 });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "JarvanIV",
                    SpellMenuName = "Dragon Strike",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "JarvanIVDragonStrike",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Jayce",
                    SpellMenuName = "Thundering Blow",
                    Slot = SpellSlot.E,
                    DangerLevel = 3,
                    SDataName = "JayceThunderingBlow",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Jinx",
                    SpellMenuName = "Zap!",
                    Slot = SpellSlot.W,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "JinxW",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Jinx",
                    SpellMenuName = "Chompers!",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Line,
                    DangerLevel = 4,
                    SDataName = "JinxE",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Karma",
                    SpellMenuName = "Inner Flame (Mantra)",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Circle,
                    DangerLevel = 2,
                    SDataName = "KarmaQMantra",
                });
            CCList.Add(
                 new DangerDB
                 {
                     HeroName = "Karma",
                     SpellMenuName = "Sprit Bond",
                     Slot = SpellSlot.W,
                     DangerLevel = 3,
                     SDataName = "KarmaQMantra",
                 });

            CCList.Add(
                new DangerDB
                {
                    HeroName = "Kassadin",
                    SpellMenuName = "Force Pulse",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Cone,
                    DangerLevel = 3,
                    SDataName = "ForcePulse",
                });

            CCList.Add(
                new DangerDB
                {
                    HeroName = "Khazix",
                    SpellMenuName = "Void Spikes",
                    Slot = SpellSlot.W,
                    Type = Skilltype.Line,
                    DangerLevel = 2,
                    SDataName = "KhazixW",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Kayle",
                    SpellMenuName = "Reckoning",
                    Slot = SpellSlot.Q,
                    DangerLevel = 3,
                    SDataName = "JudicatorReckoning",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "KogMaw",
                    SpellMenuName = "Void Ooze",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Line,
                    DangerLevel = 2,
                    SDataName = "KogMawVoidOoze",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Leblanc",
                    SpellMenuName = "Soul Shackle",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "LeblancSoulShackle",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Leblanc",
                    SpellMenuName = "Soul Shackle (Mimic)",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "LeblancSoulShackleM",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "LeeSin",
                    SpellMenuName = "Dragon's Rage",
                    Slot = SpellSlot.R,
                    DangerLevel = 5,
                    SDataName = "BlindMonkRKick",
                });
            CCList.Add(
new DangerDB
{
    HeroName = "Leona",
    SpellMenuName = "Zenith Blade",
    Slot = SpellSlot.E,
    Type = Skilltype.Line,
    DangerLevel = 3,
    SDataName = "LeonaZenithBlade",
});
            CCList.Add(
                 new DangerDB
                 {
                     HeroName = "Leona",
                     SpellMenuName = "Shield of Daybreak",
                     Slot = SpellSlot.Q,
                     DangerLevel = 3,
                     SDataName = "LeonaShieldOfDaybreak",
                 });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Leona",
                    SpellMenuName = "Solar Flare",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Circle,
                    DangerLevel = 5,
                    SDataName = "LeonaSolarFlare",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Lissandra",
                    SpellMenuName = "Ice Shard",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 2,
                    SDataName = "LissandraQ",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Lissandra",
                    SpellMenuName = "Ring of Frost",
                    Slot = SpellSlot.W,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "LissandraW",
                });

            CCList.Add(
                new DangerDB
                {
                    HeroName = "Lulu",
                    SpellMenuName = "Glitterlance",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 2,
                    SDataName = "LuluQ"
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Lulu",
                    SpellMenuName = "Glitterlance: Extended",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "LuluQMissileTwo"
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Lux",
                    SpellMenuName = "Light Binding",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 5,
                    SDataName = "LuxLightBinding",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Lux",
                    SpellMenuName = "Lucent Singularity",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Circle,
                    DangerLevel = 3,
                    SDataName = "LuxLightStrikeKugel",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Lux",
                    SpellMenuName = "Final Spark",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Line,
                    DangerLevel = 5,
                    SDataName = "LuxMaliceCannon",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Malphite",
                    SpellMenuName = "Unstoppable Force",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Circle,
                    DangerLevel = 5,
                    SDataName = "UFSlash",
                });
            CCList.Add(
                 new DangerDB
                 {
                     HeroName = "Malphite",
                     SpellMenuName = "Sismic Shard",
                     Slot = SpellSlot.Q,
                     Type = Skilltype.Circle,
                     DangerLevel = 3,
                     SDataName = "SismicShard",
                 });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Malzahar",
                    SpellMenuName = "Nether Grasp",
                    Slot = SpellSlot.R,
                    DangerLevel = 5,
                    SDataName = "AlZaharNetherGrasp",
                });
            CCList.Add(
                 new DangerDB
                 {
                     HeroName = "Maokai",
                     SpellMenuName = "Twisted Advance",
                     Slot = SpellSlot.W,
                     DangerLevel = 3,
                     SDataName = "MaokaiUnstableGrowth",
                 });
            CCList.Add(
                 new DangerDB
                 {
                     HeroName = "Maokai",
                     SpellMenuName = "Arcane Smash",
                     Slot = SpellSlot.Q,
                     DangerLevel = 3,
                     SDataName = "MaokaiTrunkLine",
                 });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Morgana",
                    SpellMenuName = "Dark Binding",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 5,
                    SDataName = "DarkBindingMissile",
                });
            CCList.Add(
                 new DangerDB
                 {
                     HeroName = "Mordekaiser",
                     SpellMenuName = "Children of the Grave",
                     Slot = SpellSlot.Q,
                     DangerLevel = 5,
                     SDataName = "MordekaiserChildrenOfTheGrave",
                 });
            CCList.Add(
                 new DangerDB
                 {
                     HeroName = "Wukong",
                     SpellMenuName = "Cyclone",
                     Slot = SpellSlot.R,
                     Type = Skilltype.Circle,
                     DangerLevel = 5,
                     SDataName = "MonkeyKingSpinToWin",
                 });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Nami",
                    SpellMenuName = "Aqua Prision",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Circle,
                    DangerLevel = 3,
                    SDataName = "NamiQ",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Nasus",
                    SpellMenuName = "Wither",
                    Slot = SpellSlot.Q,
                    DangerLevel = 3,
                    SDataName = "NasusW",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Karthus",
                    SpellMenuName = "Wall of Pain",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Circle,
                    DangerLevel = 3,
                    SDataName = "KarthusWallOfPain",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Nami",
                    SpellMenuName = "Tidal Wave",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Line,
                    DangerLevel = 2,
                    SDataName = "NamiR",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Nautilus",
                    SpellMenuName = "Dredge Line",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "NautilusAnchorDragMissile",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Nautilus",
                    SpellMenuName = "Riptide",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Circle,
                    DangerLevel = 2,
                    SDataName = "NautilusSplashZone",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Nautilus",
                    SpellMenuName = "Depth Charge",
                    Slot = SpellSlot.R,
                    DangerLevel = 5,
                    SDataName = "NautilusGrandLine",
                });

            CCList.Add(
                new DangerDB
                {
                    HeroName = "Nidalee",
                    SpellMenuName = "Javelin Toss",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "JavelinToss",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Olaf",
                    SpellMenuName = "Undertow",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 2,
                    SDataName = "OlafAxeThrowCast",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Orianna",
                    SpellMenuName = "Command: Dissonance ",
                    Slot = SpellSlot.W,
                    Type = Skilltype.Circle,
                    DangerLevel = 3,
                    SDataName = "OrianaDissonanceCommand",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Orianna",
                    SpellMenuName = "OrianaDetonateCommand",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Circle,
                    DangerLevel = 5,
                    SDataName = "OrianaDetonateCommand",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Quinn",
                    SpellMenuName = "Blinding Assault",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 2,
                    SDataName = "QuinnQ",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Rammus",
                    SpellMenuName = "Puncturing Taunt",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 2,
                    SDataName = "PuncturingTaunt",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Rengar",
                    SpellMenuName = "Bola Strike (Emp)",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "RengarEFinal",
                });

            CCList.Add(
                 new DangerDB
                 {
                     HeroName = "Fiddlesticks",
                     SpellMenuName = "Terrify",
                     Slot = SpellSlot.Q,
                     DangerLevel = 3,
                     SDataName = "Terrify",
                 });
            CCList.Add(
                 new DangerDB
                 {
                     HeroName = "Renekton",
                     SpellMenuName = "Ruthless Predator",
                     Slot = SpellSlot.W,
                     DangerLevel = 3,
                     SDataName = "RenektonPreExecute",
                 });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Riven",
                    SpellMenuName = "Ki Burst",
                    Slot = SpellSlot.W,
                    Type = Skilltype.Line,
                    DangerLevel = 5,
                    SDataName = "RivenMartyr"
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Rumble",
                    SpellMenuName = "RumbleGrenade",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Line,
                    DangerLevel = 2,
                    SDataName = "RumbleGrenade",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Rumble",
                    SpellMenuName = "RumbleCarpetBombM",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Line,
                    DangerLevel = 4,
                    SDataName = "RumbleCarpetBombMissile",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Ryze",
                    SpellMenuName = "Rune Prision",
                    Slot = SpellSlot.W,
                    DangerLevel = 3,
                    SDataName = "RunePrison",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Sejuani",
                    SpellMenuName = "Arctic Assault",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "SejuaniArcticAssault",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Sejuani",
                    SpellMenuName = "Glacial Prision",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Line,
                    DangerLevel = 5,
                    SDataName = "SejuaniGlacialPrisonStart",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Singed",
                    SpellMenuName = "Mega Adhesive",
                    Slot = SpellSlot.W,
                    Type = Skilltype.Circle,
                    DangerLevel = 2,
                    SDataName = "MegaAdhesive",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Singed",
                    SpellMenuName = "Fling",
                    Slot = SpellSlot.E,
                    DangerLevel = 2,
                    SDataName = "Fling",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Nocturne",
                    SpellMenuName = "Unspeakable Horror",
                    Slot = SpellSlot.E,
                    DangerLevel = 3,
                    SDataName = "NocturneUnspeakableHorror",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Shen",
                    SpellMenuName = "ShenShadowDash",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "ShenShadowDash",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Shyvana",
                    SpellMenuName = "ShyvanaTransformCast",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "ShyvanaTransformCast",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Skarner",
                    SpellMenuName = "Fracture",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "SkarnerFractureMissile",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Skarner",
                    SpellMenuName = "Impale",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Line,
                    DangerLevel = 5,
                    SDataName = "SkarnerFractureMissile",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Pantheon",
                    SpellMenuName = "Aegis of Zeonia",
                    Slot = SpellSlot.W,
                    DangerLevel = 3,
                    SDataName = "PantheonW",
                });
            CCList.Add(
                 new DangerDB
                 {
                     HeroName = "Pantheon",
                     SpellMenuName = "Heroic Charge",
                     Slot = SpellSlot.W,
                     DangerLevel = 3,
                     SDataName = "PoppyHeroicCharge",
                 });
            CCList.Add(
                 new DangerDB
                 {
                     HeroName = "Nunu",
                     SpellMenuName = "Ice Blast",
                     Slot = SpellSlot.E,
                     DangerLevel = 3,
                     SDataName = "Ice Blast",
                 });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Sona",
                    SpellMenuName = "Crescendo",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Line,
                    DangerLevel = 5,
                    SDataName = "SonaCrescendo",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Swain",
                    SpellMenuName = "Nevermove",
                    Slot = SpellSlot.W,
                    Type = Skilltype.Circle,
                    DangerLevel = 3,
                    SDataName = "SwainShadowGrasp",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Syndra",
                    SpellMenuName = "Scatter the Weak",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Cone,
                    DangerLevel = 5,
                    SDataName = "SyndraE",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Thresh",
                    SpellMenuName = "Death Sentence",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "ThreshQ",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Thresh",
                    SpellMenuName = "Flay",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "ThreshEFlay",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Tristana",
                    SpellMenuName = "Buster Shot",
                    Slot = SpellSlot.R,
                    DangerLevel = 5,
                    SDataName = "BusterShot",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Trundle",
                    SpellMenuName = "Pillar of Ice",
                    Slot = SpellSlot.E,
                    DangerLevel = 3,
                    SDataName = "TrundleCircle",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Trundle",
                    SpellMenuName = "Subjugate",
                    Slot = SpellSlot.R,
                    DangerLevel = 5,
                    SDataName = "TrundlePain",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Tryndamere",
                    SpellMenuName = "Mocking Shout",
                    Slot = SpellSlot.W,
                    DangerLevel = 3,
                    SDataName = "MockingShout",
                });

            CCList.Add(
                new DangerDB
                {
                    HeroName = "Twitch",
                    SpellMenuName = "Venom Cask",
                    Slot = SpellSlot.W,
                    Type = Skilltype.Circle,
                    DangerLevel = 2,
                    SDataName = "TwitchVenomCaskMissile",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Urgot",
                    SpellMenuName = "Corrosive Charge",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Circle,
                    DangerLevel = 3,
                    SDataName = "UrgotPlasmaGrenadeBoom",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Varus",
                    SpellMenuName = "Hail of Arrowws",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Circle,
                    DangerLevel = 2,
                    SDataName = "VarusE",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Varus",
                    SpellMenuName = "Chain of Corruption",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Line,
                    DangerLevel = 5,
                    SDataName = "VarusR",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Veigar",
                    SpellMenuName = "Event Horizon",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Circle,
                    DangerLevel = 5,
                    SDataName = "VeigarEventHorizon",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Velkoz",
                    SpellMenuName = "VelkozQ",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "VelkozQ",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Velkoz",
                    SpellMenuName = "Plasma Fission",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "VelkozQSplit",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Velkoz",
                    SpellMenuName = "Tectonic Disruption",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Circle,
                    DangerLevel = 3,
                    SDataName = "VelkozE",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Vi",
                    SpellMenuName = "Vault Breaker",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "ViQ",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Vi",
                    SpellMenuName = "Assault and Battery",
                    Slot = SpellSlot.R,
                    DangerLevel = 5,
                    SDataName = "ViR",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Viktor",
                    SpellMenuName = "Gravity Field",
                    Slot = SpellSlot.W,
                    Type = Skilltype.Circle,
                    DangerLevel = 5,
                    SDataName = "ViktorGravitonField",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Vayne",
                    SpellMenuName = "Condemn",
                    Slot = SpellSlot.E,
                    DangerLevel = 3,
                    SDataName = "Vayne Condemn",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Warwick",
                    SpellMenuName = "Infinite Duress",
                    Slot = SpellSlot.R,
                    DangerLevel = 5,
                    SDataName = "InfiniteDuress",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Xerath",
                    SpellMenuName = "Eye of Destruction",
                    Slot = SpellSlot.W,
                    Type = Skilltype.Circle,
                    DangerLevel = 2,
                    SDataName = "XerathArcaneBarrage2",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Xerath",
                    SpellMenuName = "Shocking Orb",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Line,
                    DangerLevel = 2,
                    SDataName = "XerathMageSpearMissile",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "XinZhao",
                    SpellMenuName = "Three Talon Strike",
                    Slot = SpellSlot.Q,
                    DangerLevel = 3,
                    SDataName = "XenZhaoComboTarget",
                });
            CCList.Add(
                 new DangerDB
                 {
                     HeroName = "XinZhao",
                     SpellMenuName = "Audacious Charge",
                     Slot = SpellSlot.E,
                     DangerLevel = 4,
                     SDataName = "XenZhaoSweep",
                 });
            CCList.Add(
                  new DangerDB
                  {
                      HeroName = "XinZhao",
                      SpellMenuName = "Crescent Sweep",
                      Slot = SpellSlot.R,
                      Type = Skilltype.Circle,
                      DangerLevel = 5,
                      SDataName = "XenZhaoParry",
                  });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Yasuo",
                    SpellMenuName = "yasuoq2",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 2,
                    SDataName = "yasuoq2",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Yasuo",
                    SpellMenuName = "yasuoq3w",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "yasuoq3w",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Yasuo",
                    SpellMenuName = "yasuoq",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 2,
                    SDataName = "yasuoq",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Zac",
                    SpellMenuName = "Stretching Strike",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 2,
                    SDataName = "ZacQ",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Zac",
                    SpellMenuName = "Lets Bounce!",
                    Slot = SpellSlot.R,
                    DangerLevel = 5,
                    SDataName = "ZacR",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Zed",
                    SpellMenuName = "Death Mark",
                    Slot = SpellSlot.R,
                    DangerLevel = 5,
                    SDataName = "ZedUlt",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Ziggs",
                    SpellMenuName = "Satchel Charge",
                    Slot = SpellSlot.W,
                    Type = Skilltype.Circle,
                    DangerLevel = 2,
                    SDataName = "ZiggsW",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Zyra",
                    SpellMenuName = "Grasping Roots",
                    Slot = SpellSlot.E,
                    Type = Skilltype.Line,
                    DangerLevel = 5,
                    SDataName = "ZyraGraspingRoots",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Zyra",
                    SpellMenuName = "Stranglethorns",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "ZyraBrambleZone",
                });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Taric",
                    SpellMenuName = "Dazzle",
                    Slot = SpellSlot.E,
                    SDataName = "Dazzle",
                    DangerLevel = 5
                });
            CCList.Add(
                 new DangerDB
                 {
                     HeroName = "Yoric",
                     SpellMenuName = "Omen of Pestilence",
                     Slot = SpellSlot.W,
                     DangerLevel = 3,
                     SDataName = "YorickDecayed",
                 });
            CCList.Add(
                new DangerDB
                {
                    HeroName = "Yasuo",
                    SpellMenuName = "Steel Tempest (3)",
                    Slot = SpellSlot.W,
                    DangerLevel = 3,
                    SDataName = "YasuoQ3",
                });
            #endregion

            #region SList
            SList.Add(
                new DangerDB
                {
                    HeroName = "Fiddlesticks",
                    SpellMenuName = "Dark Wind",
                    Slot = SpellSlot.E,
                    DangerLevel = 3,
                    SDataName = "FiddlesticksDarkWind",
                });
            SList.Add(
                new DangerDB
                {
                    HeroName = "Blitzcrank",
                    SpellMenuName = "Static Field",
                    Slot = SpellSlot.R,
                    Type = Skilltype.Circle,
                    DangerLevel = 3,
                    SDataName = "StaticField",

                });
            SList.Add(
                new DangerDB
                {
                    HeroName = "Chogath",
                    SpellMenuName = "Feral Scream",
                    Slot = SpellSlot.W,
                    Type = Skilltype.Cone,
                    DangerLevel = 3,
                    SDataName = "FeralScream",

                });
            SList.Add(
                new DangerDB
                {
                    HeroName = "Malzahar",
                    SpellMenuName = "Call of the Void",
                    Slot = SpellSlot.Q,
                    Type = Skilltype.Line,
                    DangerLevel = 3,
                    SDataName = "AlZaharCalloftheVoid",
                });
            SList.Add(
                new DangerDB
                {
                    HeroName = "Talon",
                    SpellMenuName = "Cutthroat",
                    Type = Skilltype.Unknown,
                    Slot = SpellSlot.E,
                    DangerLevel = 3,
                    SDataName = "TalonCutthroat",
                });
            SList.Add(
                 new DangerDB
                 {
                     HeroName = "Garen",
                     SpellMenuName = "Decisive Strike",
                     Slot = SpellSlot.Q,
                     DangerLevel = 3,
                     SDataName = "GarenQ",
                 });
            SList.Add(
                  new DangerDB
                  {
                      HeroName = "Viktor",
                      SpellMenuName = "Chaos Storm",
                      Type = Skilltype.Circle,
                      Slot = SpellSlot.R,
                      DangerLevel = 3,
                      SDataName = "ViktorChaosStorm",
                  });
            SList.Add(
                   new DangerDB
                   {
                       HeroName = "Soraka",
                       SpellMenuName = "Equinox",
                       Slot = SpellSlot.E,
                       Type = Skilltype.Circle,
                       DangerLevel = 2,
                       SDataName = "SorakaE",
                   });
            #endregion

            #region GDList

            #endregion
        }
    }
}
