using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LX_Orbwalker;
using LeagueSharp;
using LeagueSharp.Common;
using System.Drawing;
using SharpDX;

/*
 * ToDo:
 * 
 * */


namespace D_leeSin
{
    internal class LeeSinSharp
    {
        public static string[] testSpells = { "RelicSmallLantern", "RelicLantern", "SightWard", "wrigglelantern", "ItemGhostWard", "VisionWard",
                                     "BantamTrap", "JackInTheBox","CaitlynYordleTrap", "Bushwhack"};

        private const string CharName = "LeeSin";


        public static Menu Config;

        public static Map map;

        public static Obj_AI_Hero target;

        private static Int32 _lastSkin;

        public static Obj_AI_Hero Player = ObjectManager.Player;


        public LeeSinSharp()
        {
            /* CallBAcks */
            CustomEvents.Game.OnGameLoad += onLoad;

        }

        private static void onLoad(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName != CharName) return;
            map = new Map();

            Game.PrintChat("<font color='#881df2'>Quangcha LeeSin Reworked By Diabaths </font>Loaded!");

            try
            {

                Config = new Menu("LeeSin", "LeeSin", true);
                var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
                TargetSelector.AddToMenu(targetSelectorMenu);
                Config.AddSubMenu(targetSelectorMenu);

                var orbwalkerMenu = new Menu("LX-Orbwalker", "LX-Orbwalker");
                LXOrbwalker.AddToMenu(orbwalkerMenu);
                Config.AddSubMenu(orbwalkerMenu);

                Config.AddSubMenu(new Menu("Combo", "Combo"));
                Config.SubMenu("Combo").AddItem(new MenuItem("UseQCombo", "Use Q")).SetValue(true);
                Config.SubMenu("Combo").AddItem(new MenuItem("UseWCombo", "Use W")).SetValue(true);
                Config.SubMenu("Combo").AddItem(new MenuItem("passive", "Use passive").SetValue(new Slider(1, 0, 2)));
                Config.SubMenu("Combo").AddItem(new MenuItem("autowusage", "HP% to use W<").SetValue(new Slider(30, 100, 1)));
                Config.SubMenu("Combo").AddItem(new MenuItem("UseECombo", "Use E")).SetValue(true);
                Config.SubMenu("Combo").AddItem(new MenuItem("UseRCombo", "Use R")).SetValue(true);
                Config.SubMenu("Combo").AddItem(new MenuItem("ActiveCombo", "Combo!").SetValue(new KeyBind(32, KeyBindType.Press)));
                Config.SubMenu("Combo").AddItem(new MenuItem("ActiveCombo1", "Combo2!").SetValue((new KeyBind("Z".ToCharArray()[0], KeyBindType.Press, false))));

                Config.AddSubMenu(new Menu("Lane/Jungle Clear Settings", "LaneJungClear"));
                Config.SubMenu("LaneJungClear").AddItem(new MenuItem("useClearQ", "Use Q").SetValue(true));
                Config.SubMenu("LaneJungClear").AddItem(new MenuItem("useClearW", "Use W").SetValue(true));
                Config.SubMenu("LaneJungClear").AddItem(new MenuItem("useClearE", "Use E").SetValue(true));
                Config.SubMenu("LaneJungClear").AddItem(new MenuItem("checkmana", "Check Energy %").SetValue(new Slider(50, 1, 100)));
                Config.SubMenu("LaneJungClear").AddItem(new MenuItem("useClearI", "Use Tiamat/Hydra Item").SetValue(true));
                Config.SubMenu("LaneJungClear").AddItem(new MenuItem("JungleLane", "Jungle-lane!").SetValue((new KeyBind("V".ToCharArray()[0], KeyBindType.Press, false))));

                Config.AddSubMenu(new Menu("Harass", "Harass"));
                Config.SubMenu("Harass").AddItem(new MenuItem("useHarrE", "Use E").SetValue(true));
                Config.SubMenu("Harass").AddItem(new MenuItem("harrMode", "Harass if HP % >").SetValue(new Slider(20, 1)));
                Config.SubMenu("Harass").AddItem(new MenuItem("manahara", "Harass if Energy >").SetValue(new Slider(100, 1, 200)));
                Config.SubMenu("Harass").AddItem(new MenuItem("ActiveHarass", "Harass!").SetValue((new KeyBind("C".ToCharArray()[0], KeyBindType.Press, false))));

                //items
                //Items public static Int32 Tiamat = 3077, Hydra = 3074, Blade = 3153, Bilge = 3144, Rand = 3143, lotis = 3190;
                Config.AddSubMenu(new Menu("items", "items"));
                Config.SubMenu("items").AddSubMenu(new Menu("Offensive", "Offensive"));
                Config.SubMenu("items").SubMenu("Offensive").AddItem(new MenuItem("Tiamat", "Use Tiamat")).SetValue(true);
                Config.SubMenu("items").SubMenu("Offensive").AddItem(new MenuItem("Hydra", "Use Hydra")).SetValue(true);
                Config.SubMenu("items").SubMenu("Offensive").AddItem(new MenuItem("Bilge", "Use Bilge")).SetValue(true);
                Config.SubMenu("items").SubMenu("Offensive").AddItem(new MenuItem("BilgeEnemyhp", "If Enemy Hp < ").SetValue(new Slider(85, 1, 100)));
                Config.SubMenu("items").SubMenu("Offensive").AddItem(new MenuItem("Bilgemyhp", "Or your Hp < ").SetValue(new Slider(85, 1, 100)));
                Config.SubMenu("items").SubMenu("Offensive").AddItem(new MenuItem("Blade", "Use Blade")).SetValue(true);
                Config.SubMenu("items").SubMenu("Offensive").AddItem(new MenuItem("BladeEnemyhp", "If Enemy Hp < ").SetValue(new Slider(85, 1, 100)));
                Config.SubMenu("items").SubMenu("Offensive").AddItem(new MenuItem("Blademyhp", "Or Your  Hp < ").SetValue(new Slider(85, 1, 100)));
                Config.SubMenu("items").AddSubMenu(new Menu("Deffensive", "Deffensive"));
                Config.SubMenu("items").SubMenu("Deffensive").AddItem(new MenuItem("Omen", "Use Randuin Omen")).SetValue(true);
                Config.SubMenu("items").SubMenu("Deffensive").AddItem(new MenuItem("Omenenemys", "Randuin if enemys>").SetValue(new Slider(2, 1, 5)));
                Config.SubMenu("items").SubMenu("Deffensive").AddItem(new MenuItem("lotis", "Use Iron Solari")).SetValue(true);
                Config.SubMenu("items").SubMenu("Deffensive").AddItem(new MenuItem("lotisminhp", "Solari if Ally Hp<  ").SetValue(new Slider(35, 1, 100)));

                Config.AddSubMenu(new Menu("Insec", "Insec"));
                Config.SubMenu("Insec").AddItem(new MenuItem("ActiveInsec", "Insec!").SetValue((new KeyBind("T".ToCharArray()[0], KeyBindType.Press, false))));

                Config.AddSubMenu(new Menu("Misc", "Misc"));
                Config.SubMenu("Misc").AddItem(new MenuItem("smite", "Auto Smite Minion").SetValue(true));
                Config.SubMenu("Misc").AddItem(new MenuItem("UseR", "R killsteal")).SetValue(true);
                Config.SubMenu("Misc").AddItem(new MenuItem("usePackets", "Usepackes")).SetValue(true);
                Config.SubMenu("Misc").AddItem(new MenuItem("skinLee", "Use Custom Skin").SetValue(true));
                Config.SubMenu("Misc").AddItem(new MenuItem("skinleesin", "Skin Changer").SetValue(new Slider(4, 1, 7)));

                Config.AddSubMenu(new Menu("KillSteal", "KillSteal"));
                Config.SubMenu("KillSteal").AddItem(new MenuItem("UseQKs", "Q killsteal")).SetValue(true);
                Config.SubMenu("KillSteal").AddItem(new MenuItem("UseEKs", "E killsteal")).SetValue(true);
                Config.SubMenu("KillSteal").AddItem(new MenuItem("UseRKs", "R killsteal")).SetValue(true);

                Config.AddSubMenu(new Menu("HitChange", "HitChange"));
                Config.SubMenu("HitChange")
                                   .AddItem(new MenuItem("Qchange", "Q Hit").SetValue(
                                    new StringList(new[] { "Low", "Medium", "High", "Very High" })));

                Config.AddSubMenu(new Menu("WardJump", "WardJump"));
                Config.SubMenu("WardJump").AddItem(new MenuItem("ActiveWard", "WardJump!").SetValue((new KeyBind("G".ToCharArray()[0], KeyBindType.Press, false))));

                Config.AddSubMenu(new Menu("Drawings", "Drawings"));
                Config.SubMenu("Drawings").AddItem(new MenuItem("DrawQ", "Draw Q")).SetValue(true);
                Config.SubMenu("Drawings").AddItem(new MenuItem("DrawE", "Draw E")).SetValue(true);
                Config.SubMenu("Drawings").AddItem(new MenuItem("DrawW", "Draw W")).SetValue(true);
                Config.SubMenu("Drawings").AddItem(new MenuItem("DrawR", "Draw R")).SetValue(true);
                Config.SubMenu("Drawings").AddItem(new MenuItem("DrawInsec", "Draw Insec")).SetValue(true);
                Config.SubMenu("Drawings").AddItem(new MenuItem("CircleQuality", "Circles Quality").SetValue(new Slider(100, 100, 10)));
                Config.SubMenu("Drawings").AddItem(new MenuItem("CircleThickness", "Circles Thickness").SetValue(new Slider(1, 10, 1)));
                Config.AddToMainMenu();
                Drawing.OnDraw += onDraw;
                Game.OnGameUpdate += OnGameUpdate;

                GameObject.OnCreate += OnCreateObject;
                GameObject.OnDelete += OnDeleteObject;
                Obj_AI_Base.OnProcessSpellCast += OnProcessSpell;
                if (Config.Item("skinLee").GetValue<bool>() && SkinChanged())
                {
                    GenModelPacket(Player.ChampionName, Config.Item("skinleesin").GetValue<Slider>().Value);
                    _lastSkin = Config.Item("skinleesin").GetValue<Slider>().Value;
                }
                LeeSin.setSkillShots();
            }
            catch
            {
            }

        }

        private static void OnGameUpdate(EventArgs args)
        {
            if (Config.Item("JungleLane").GetValue<KeyBind>().Active)
            {
                LeeSin.LaneJungClear();
            }
            if (Config.Item("ActiveWard").GetValue<KeyBind>().Active)
            {
                LeeSin.wardJump(Game.CursorPos.To2D());
            }
            if (Config.Item("ActiveHarass").GetValue<KeyBind>().Active)
            {
                LeeSin.doHarass();
            }

            if (Config.Item("ActiveCombo").GetValue<KeyBind>().Active)
            {
                LeeSin.combo();
            }
            if (Config.Item("ActiveCombo1").GetValue<KeyBind>().Active)
            {
                LeeSin.combo2();

            }
            if (Config.Item("ActiveInsec").GetValue<KeyBind>().Active)
            {
                LeeSin.useinsec();
            }

            if (Config.Item("skinLee").GetValue<bool>() && SkinChanged())
            {
                GenModelPacket(Player.ChampionName, Config.Item("skinleesin").GetValue<Slider>().Value);
                _lastSkin = Config.Item("skinleesin").GetValue<Slider>().Value;
            }
            LeeSin.Killsteal();
            LeeSin.loaidraw();
            target = TargetSelector.GetTarget(1500, TargetSelector.DamageType.Physical);
            LeeSin.checkLock(target);
            LXOrbwalker.SetAttack(true);
        }
        static void GenModelPacket(string champ, int skinId)
        {
            Packet.S2C.UpdateModel.Encoded(new Packet.S2C.UpdateModel.Struct(Player.NetworkId, skinId, champ)).Process();
        }

        static bool SkinChanged()
        {
            return (Config.Item("skinleesin").GetValue<Slider>().Value != _lastSkin);
        }

        private static void onDraw(EventArgs args)
        {
            if (Config.Item("DrawQ").GetValue<bool>())
            {
                Utility.DrawCircle(ObjectManager.Player.Position, 1000, System.Drawing.Color.Gray,
                    Config.Item("CircleThickness").GetValue<Slider>().Value,
                    Config.Item("CircleQuality").GetValue<Slider>().Value);
            }
            if (Config.Item("DrawW").GetValue<bool>())
            {
                Utility.DrawCircle(ObjectManager.Player.Position, 700, System.Drawing.Color.Gray,
                    Config.Item("CircleThickness").GetValue<Slider>().Value,
                    Config.Item("CircleQuality").GetValue<Slider>().Value);
            }
            if (Config.Item("DrawE").GetValue<bool>())
            {
                Utility.DrawCircle(ObjectManager.Player.Position, 350, System.Drawing.Color.Gray,
                    Config.Item("CircleThickness").GetValue<Slider>().Value,
                    Config.Item("CircleQuality").GetValue<Slider>().Value);
            }
            if (Config.Item("DrawR").GetValue<bool>())
            {
                Utility.DrawCircle(ObjectManager.Player.Position, 375, System.Drawing.Color.Gray,
                    Config.Item("CircleThickness").GetValue<Slider>().Value,
                    Config.Item("CircleQuality").GetValue<Slider>().Value);
            }
            if (Config.Item("DrawInsec").GetValue<bool>() && LeeSin.R.IsReady())
            {
                if (!LeeSin.loaidraw())
                {
                    Vector2 heroPos = Drawing.WorldToScreen(LeeSin.LockedTarget.Position);
                    Vector2 diempos = Drawing.WorldToScreen(LeeSin.getward1(LeeSin.LockedTarget));
                    Drawing.DrawLine(heroPos[0], heroPos[1], diempos[0], diempos[1], 1, System.Drawing.Color.White);
                }
                else
                {
                    Vector2 heroPos = Drawing.WorldToScreen(LeeSin.LockedTarget.Position);
                    Vector2 diempos = Drawing.WorldToScreen(LeeSin.getward3(LeeSin.LockedTarget));
                    Drawing.DrawLine(heroPos[0], heroPos[1], diempos[0], diempos[1], 1, System.Drawing.Color.White);
                }
            }
        }

        private static void OnCreateObject(GameObject sender, EventArgs args)
        {
            if (sender.Name.Contains("Missile") || sender.Name.Contains("Minion"))
                return;
        }

        private static void OnDeleteObject(GameObject sender, EventArgs args)
        {

        }

        public static void OnProcessSpell(LeagueSharp.Obj_AI_Base obj, LeagueSharp.GameObjectProcessSpellCastEventArgs arg)
        {
            if (testSpells.ToList().Contains(arg.SData.Name))
            {
                LeeSin.testSpellCast = arg.End.To2D();
                Polygon pol;
                if ((pol = map.getInWhichPolygon(arg.End.To2D())) != null)
                {
                    LeeSin.testSpellProj = pol.getProjOnPolygon(arg.End.To2D());
                }
            }
        }




    }
}
