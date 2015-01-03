using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace D_Zac
{
    public static class Program
    {
        private const string ChampionName = "Zac";

        private static Orbwalking.Orbwalker _orbwalker;

        private static Spell _q, _w, _e, _r;

        private static Menu _config;

        private static Obj_AI_Hero _player;

        private static int _champSkin;
        private static int _timeTick;

        private static bool _initialSkin = true;

        private static SpellSlot _igniteSlot;

        private static SpellDataInst _smiteSlot;

        private static Items.Item _tiamat, _hydra, _blade, _bilge, _rand, _lotis, _zhonya, _dfg;

        private static readonly List<string> Skins = new List<string>();

        private static bool AttacksEnabled
        {
            get
            {
                if (_e.IsCharging)
                    return false;
                return true;
            }
        }

        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            _player = ObjectManager.Player;
            if (ObjectManager.Player.BaseSkinName != ChampionName) return;

            _q = new Spell(SpellSlot.Q, 550f);
            _w = new Spell(SpellSlot.W, 350f);
            _e = new Spell(SpellSlot.E, 1550f);
            _r = new Spell(SpellSlot.R, float.MaxValue);
         
            _q.SetSkillshot(0.5f, 120f, 1800, false, SkillshotType.SkillshotLine);
            _e.SetSkillshot(0.5f, (float)(80 * Math.PI / 180), 1500, true, SkillshotType.SkillshotCone);
            _e.SetCharged("ZacE", "ZacE", 0, 1150, 1.5f);
            

            _bilge = new Items.Item(3144, 475f);
            _blade = new Items.Item(3153, 425f);
            _hydra = new Items.Item(3074, 250f);
            _tiamat = new Items.Item(3077, 250f);
            _rand = new Items.Item(3143, 490f);
            _lotis = new Items.Item(3190, 590f);
            _dfg = new Items.Item(3128, 750f);
            _zhonya = new Items.Item(3157, 10);

            _igniteSlot = _player.GetSpellSlot("SummonerDot");
            _smiteSlot = _player.Spellbook.GetSpell(_player.GetSpellSlot("summonersmite"));

            //D-Zac
            _config = new Menu("D-Zac", "D-Zac", true);

            //TargetSelector
            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(targetSelectorMenu);
            _config.AddSubMenu(targetSelectorMenu);

            //Orbwalker
            _config.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));
            _orbwalker = new Orbwalking.Orbwalker(_config.SubMenu("Orbwalking"));

            //Combo
            _config.AddSubMenu(new Menu("Combo", "Combo"));
            _config.SubMenu("Combo").AddItem(new MenuItem("UseQC", "Use Q")).SetValue(true);
            _config.SubMenu("Combo").AddItem(new MenuItem("UseWC", "Use W")).SetValue(true);
            _config.SubMenu("Combo").AddItem(new MenuItem("UseEC", "Use E")).SetValue(true);
            _config.SubMenu("Combo").AddItem(new MenuItem("UseRC", "Use R")).SetValue(true);
            _config.SubMenu("Combo")
                .AddItem(new MenuItem("ActiveCombo", "Combo!").SetValue(new KeyBind(32, KeyBindType.Press)));

            _config.AddSubMenu(new Menu("Items", "Items"));
            _config.SubMenu("Items").AddItem(new MenuItem("Youmuu", "Use Youmuu's")).SetValue(true);
            _config.SubMenu("Items").AddItem(new MenuItem("Bilge", "Use Bilge")).SetValue(true);
            _config.SubMenu("Items")
                .AddItem(new MenuItem("BilgeEnemyhp", "If Enemy Hp <").SetValue(new Slider(85, 1, 100)));
            _config.SubMenu("Items")
                .AddItem(new MenuItem("Bilgemyhp", "Or your Hp < ").SetValue(new Slider(85, 1, 100)));
            _config.SubMenu("Items").AddItem(new MenuItem("Blade", "Use Blade")).SetValue(true);
            _config.SubMenu("Items")
                .AddItem(new MenuItem("BladeEnemyhp", "If Enemy Hp <").SetValue(new Slider(85, 1, 100)));
            _config.SubMenu("Items")
                .AddItem(new MenuItem("Blademyhp", "Or Your  Hp <").SetValue(new Slider(85, 1, 100)));


            //Harass
            _config.AddSubMenu(new Menu("Harass", "Harass"));
            _config.SubMenu("Harass").AddItem(new MenuItem("UseQH", "Use Q")).SetValue(true);
            _config.SubMenu("Harass").AddItem(new MenuItem("UseWH", "Use W")).SetValue(true);
            _config.SubMenu("Harass").AddItem(new MenuItem("UseEH", "Use E")).SetValue(true);
            _config.SubMenu("Harass").AddItem(new MenuItem("harasstoggle", "AutoHarass (toggle)").SetValue(new KeyBind("G".ToCharArray()[0],KeyBindType.Toggle)));
            _config.SubMenu("Harass").AddItem(new MenuItem("ActiveHarass", "Harass!").SetValue(new KeyBind("C".ToCharArray()[0], KeyBindType.Press)));

            //Farm
            _config.AddSubMenu(new Menu("Farm", "Farm"));
            _config.SubMenu("Farm").AddSubMenu(new Menu("Lasthit", "Lasthit"));
            _config.SubMenu("Farm").SubMenu("Lasthit").AddItem(new MenuItem("UseQLH", "Q LastHit")).SetValue(true);
            _config.SubMenu("Farm").SubMenu("Lasthit").AddItem(new MenuItem("ActiveLast", "LastHit").SetValue(new KeyBind("X".ToCharArray()[0], KeyBindType.Press)));

            _config.SubMenu("Farm").AddSubMenu(new Menu("Laneclear", "Laneclear"));
            _config.SubMenu("Farm").SubMenu("Laneclear").AddItem(new MenuItem("UseQL", "Q LaneClear")).SetValue(true);
            _config.SubMenu("Farm").SubMenu("Laneclear").AddItem(new MenuItem("UseWL", "W LaneClear")).SetValue(true);
            _config.SubMenu("Farm").SubMenu("Laneclear").AddItem(new MenuItem("UseEL", "E LaneClear")).SetValue(true);
            _config.SubMenu("Farm").SubMenu("Laneclear").AddItem(new MenuItem("ActiveLane", "Lane Clear").SetValue(new KeyBind("V".ToCharArray()[0],KeyBindType.Press)));
            _config.SubMenu("Farm").AddSubMenu(new Menu("Jungleclear", "Jungleclear"));
            _config.SubMenu("Farm").SubMenu("Jungleclear").AddItem(new MenuItem("UseQJ", "Q Jungle")).SetValue(true);
            _config.SubMenu("Farm").SubMenu("Jungleclear").AddItem(new MenuItem("UseWJ", "W Jungle")).SetValue(true);
            _config.SubMenu("Farm").SubMenu("Jungleclear").AddItem(new MenuItem("UseEJ", "E Jungle")).SetValue(true);
           _config.SubMenu("Farm").SubMenu("Jungleclear").AddItem(new MenuItem("Activejungle", "Jungle Clear").SetValue(new KeyBind("V".ToCharArray()[0],KeyBindType.Press)));

            //misc
            _config.AddSubMenu(new Menu("Misc", "Misc"));
            _config.SubMenu("Misc").AddItem(new MenuItem("usePackets", "Use Packets")).SetValue(true);
            
            _config.AddToMainMenu();
            Game.OnGameUpdate += Game_OnGameUpdate;
            Orbwalking.BeforeAttack += OrbwalkingOnBeforeAttack;
            Game.OnGameSendPacket += GameOnOnGameSendPacket;
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            _e.Range = 1150 +(_player.Spellbook.GetSpell(SpellSlot.E).Level - 1)*100;
            _e.SetCharged("ZacE", "ZacE", 0, 1150 + (_player.Spellbook.GetSpell(SpellSlot.E).Level - 1) * 100, 1.5f);
            if (_config.Item("ActiveCombo").GetValue<KeyBind>().Active)
            {
               //Combo();
            }
            if ((_config.Item("ActiveHarass").GetValue<KeyBind>().Active || _config.Item("harasstoggle").GetValue<KeyBind>().Active) )
            {
                Harass();

            }
            if (_config.Item("ActiveLane").GetValue<KeyBind>().Active)
            {
                Laneclear();
            }
            if (_config.Item("Activejungle").GetValue<KeyBind>().Active)
            {
                JungleClear();
            }
            if (_config.Item("ActiveLast").GetValue<KeyBind>().Active )
            {
               // LastHit();
            }
            _player = ObjectManager.Player;

            _orbwalker.SetAttack(true);

            //KillSteal();
        }

        private static void OrbwalkingOnBeforeAttack(Orbwalking.BeforeAttackEventArgs args)
        {
            args.Process = AttacksEnabled;
        }
       
        private static void GameOnOnGameSendPacket(GamePacketEventArgs args)
        {
            if (args.PacketData[0] == Packet.C2S.Move.Header && _player.HasBuff("ZacE"))
            {
                args.Process = false;
            }
        }
        private static  Vector3 jumpE(Obj_AI_Hero enemy)
        {
            if (ObjectManager.Player.Position.Distance(enemy.Position) <= 1500)
                return enemy.Position;
            var newpos = enemy.Position - ObjectManager.Player.Position;
            newpos.Normalize();
            return ObjectManager.Player.Position + (newpos * _e.Range);
        }
       private static bool Packets()
        {
            return _config.Item("usePackets").GetValue<bool>();
        }

        private static void Harass()
        {
            var eTarget = TargetSelector.GetTarget(_e.Range, TargetSelector.DamageType.Physical);
            var useQ = _config.Item("UseQH").GetValue<bool>();
            var useW = _config.Item("UseWH").GetValue<bool>();
            var useE = _config.Item("UseEH").GetValue<bool>();
            if (eTarget != null && _e.IsReady() && useE && eTarget.Distance(_player.Position) < 1500 )
            {
                if (_e.IsCharging)
                {
                    Game.PrintChat("charge");
                    _e.StartCharging(eTarget.Position);
                }
                else
                {
                    Game.PrintChat("cast");
                    _e.Cast(eTarget.Position); //added aoe, i removed the .position
                   
                }

            }
            
            if (useW && _w.IsReady() && eTarget.Distance(_player.Position) < _w.Range)
            {
                _w.Cast();
            }
            if (useQ && _q.IsReady() && eTarget.Distance(_player.Position) < _q.Range)
            {
                var t = TargetSelector.GetTarget(_q.Range, TargetSelector.DamageType.Magical);
                var prediction = _q.GetPrediction(t);
                if (t != null && _player.Distance(t) < _q.Range && prediction.Hitchance >= HitChance.Medium)
                {
                    _q.Cast(prediction.CastPosition, Packets());
                }
            }
        }

        private static void Laneclear()
        {
            var allMinionsQ = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, _q.Range, MinionTypes.All);
            var useQl = _config.Item("UseQL").GetValue<bool>();
            var useWl = _config.Item("UseWL").GetValue<bool>();
            var useEl = _config.Item("UseEL").GetValue<bool>();
            if (_q.IsReady() && useQl)
            {
                var fl2 = _q.GetLineFarmLocation(allMinionsQ, _q.Width);
                if (fl2.MinionsHit >= 3)
                {
                    _q.Cast(fl2.Position);
                }
                else
                    foreach (var minion in allMinionsQ)
                        if (!Orbwalking.InAutoAttackRange(minion) &&
                            minion.Health < 0.75 * _player.GetSpellDamage(minion, SpellSlot.Q))
                            _q.Cast(minion);
            }
            if (_w.IsReady() && useWl)
            {
                if (allMinionsQ.Count > 2)
                {
                    _w.Cast();
                }
                else
                    foreach (var minion in allMinionsQ)
                        if (!Orbwalking.InAutoAttackRange(minion) &&
                            minion.Health < 0.75 * _player.GetSpellDamage(minion, SpellSlot.W))
                            _w.Cast();
            }
            if (_e.IsReady() && useEl)
            {
                var fl2 = _e.GetLineFarmLocation(allMinionsQ, _e.Width);
                
                if (_e.IsCharging)
                {
                    Game.PrintChat("cast");
                    _e.Cast(fl2.Position);
                }
                else
                {
                    Game.PrintChat("charge");
                    _e.StartCharging((Vector3) fl2.Position);
                }
            }
        }
        private static void JungleClear()
        {
            var mobs = MinionManager.GetMinions(_player.ServerPosition, _q.Range,
                MinionTypes.All,
                MinionTeam.Neutral, MinionOrderTypes.MaxHealth);
            var useQ = _config.Item("UseQJ").GetValue<bool>();
            var useW = _config.Item("UseWJ").GetValue<bool>();
            var useE = _config.Item("UseEJ").GetValue<bool>();
            if (mobs.Count > 0)
            {
                var mob = mobs[0];
                if (_e.IsReady() && useE && _player.Distance(mob) < _e.Range)
                {
                    if (_e.IsCharging)
                    {
                        Game.PrintChat("cast");
                        _e.Cast(mob);
                    }
                    else if (mobs.Count > 0)
                        Game.PrintChat("charge");
                        _e.StartCharging();
                }
                if (_w.IsReady() && useW && _player.Distance(mob) < _w.Range)
                {
                    _w.Cast();
                }
                if (useQ && _q.IsReady() && _player.Distance(mob) < _q.Range)
                {
                    _q.Cast(mob);
                }
            }
        }
    }
}
