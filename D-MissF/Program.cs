﻿using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

namespace D_MissF
{
    class Program
    {
        private const string ChampionName = "MissFortune";

        private static Orbwalking.Orbwalker _orbwalker;

        private static Spell _q, _w, _e, _r;

        private static Menu _config;

        private static int _timeTick;

        private static Obj_AI_Hero _player;

        private static Int32 _lastSkin = 0;
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        static void Game_OnGameLoad(EventArgs args)
        {
            _player = ObjectManager.Player;
            if (ObjectManager.Player.BaseSkinName != ChampionName) return;

            _q = new Spell(SpellSlot.Q, 650F);
            _w = new Spell(SpellSlot.W);
            _e = new Spell(SpellSlot.E, 800f);
            _r = new Spell(SpellSlot.R, 1400f);

            _q.SetTargetted(0.29f, 1400f);
            _e.SetSkillshot(0.65f, 300f, 500, false, SkillshotType.SkillshotCircle);
            _r.SetSkillshot(0.333f, 200, float.MaxValue, false, SkillshotType.SkillshotLine);

            //D MissFortune
            _config = new Menu("D-MissFortune", "D-MissFortune", true);

            //TargetSelector
            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            SimpleTs.AddToMenu(targetSelectorMenu);
            _config.AddSubMenu(targetSelectorMenu);

            //Orbwalker
            _config.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));
            _orbwalker = new Orbwalking.Orbwalker(_config.SubMenu("Orbwalking"));

            //Combo
            _config.AddSubMenu(new Menu("Combo", "Combo"));
            _config.SubMenu("Combo").AddItem(new MenuItem("UseQC", "Use Q")).SetValue(true);
            _config.SubMenu("Combo").AddItem(new MenuItem("UseWC", "Use W")).SetValue(true);
            _config.SubMenu("Combo").AddItem(new MenuItem("UseEC", "Use E")).SetValue(true);
            _config.SubMenu("Combo").AddItem(new MenuItem("UseRC", "Use R(target kilable)")).SetValue(true);
            _config.SubMenu("Combo").AddItem(new MenuItem("UseRE", "AutoR Min Targ")).SetValue(true);
            _config.SubMenu("Combo").AddItem(new MenuItem("MinTargets", "Ult when>=min enemy(COMBO)").SetValue(new Slider(2, 1, 5)));
            _config.SubMenu("Combo").AddItem(new MenuItem("ActiveCombo", "Combo!").SetValue(new KeyBind(32, KeyBindType.Press)));

            //Harass
            _config.AddSubMenu(new Menu("Harass", "Harass"));
            _config.SubMenu("Harass").AddItem(new MenuItem("UseQH", "Use Q")).SetValue(true);
            _config.SubMenu("Harass").AddItem(new MenuItem("UseEH", "Use E")).SetValue(true);
            _config.SubMenu("Harass").AddItem(new MenuItem("harasstoggle", "AutoHarass (toggle)").SetValue(new KeyBind("G".ToCharArray()[0], KeyBindType.Toggle)));
            _config.SubMenu("Harass").AddItem(new MenuItem("Harrasmana", "Minimum Mana").SetValue(new Slider(60, 1, 100)));
            _config.SubMenu("Harass").AddItem(new MenuItem("ActiveHarass", "Harass!").SetValue(new KeyBind("C".ToCharArray()[0], KeyBindType.Press)));

            _config.AddSubMenu(new Menu("Farm", "Farm"));
            _config.SubMenu("Farm").AddItem(new MenuItem("UseQL", "Q LaneClear")).SetValue(true);
            _config.SubMenu("Farm").AddItem(new MenuItem("UseEL", "E LaneClear")).SetValue(true);
            _config.SubMenu("Farm").AddItem(new MenuItem("UseQLH", "Q LastHit")).SetValue(true);
            _config.SubMenu("Farm").AddItem(new MenuItem("UseELH", "E LastHit")).SetValue(true);
            _config.SubMenu("Farm").AddItem(new MenuItem("UseQJ", "Q Jungle")).SetValue(true);
            _config.SubMenu("Farm").AddItem(new MenuItem("UseEJ", "E Jungle")).SetValue(true);
            _config.SubMenu("Farm").AddItem(new MenuItem("Lanemana", "Minimum Mana").SetValue(new Slider(60, 1, 100)));
            _config.SubMenu("Farm").AddItem(new MenuItem("ActiveLast", "LastHit!").SetValue(new KeyBind("X".ToCharArray()[0], KeyBindType.Press)));
            _config.SubMenu("Farm").AddItem(new MenuItem("ActiveLane", "LaneClear!").SetValue(new KeyBind("V".ToCharArray()[0], KeyBindType.Press)));

            //Misc
            _config.AddSubMenu(new Menu("Misc", "Misc"));
            _config.SubMenu("Misc").AddItem(new MenuItem("UseQM", "Use Q KillSteal")).SetValue(true);
            _config.SubMenu("Misc").AddItem(new MenuItem("UseEM", "Use E KillSteal")).SetValue(true);
            _config.SubMenu("Misc").AddItem(new MenuItem("UseRM", "Use R KillSteal")).SetValue(true);
            _config.SubMenu("Misc").AddItem(new MenuItem("Gap_E", "GapClosers E")).SetValue(true);
            _config.SubMenu("Misc").AddItem(new MenuItem("skinMF", "Use Custom Skin").SetValue(true));
            _config.SubMenu("Misc").AddItem(new MenuItem("skinMiss", "Skin Changer").SetValue(new Slider(4, 1, 7)));
            _config.SubMenu("Misc").AddItem(new MenuItem("usePackets", "Usepackes")).SetValue(true);

            //Drawings
            _config.AddSubMenu(new Menu("Drawings", "Drawings"));
            _config.SubMenu("Drawings").AddItem(new MenuItem("DrawQ", "Draw Q")).SetValue(true);
            _config.SubMenu("Drawings").AddItem(new MenuItem("DrawW", "Draw W")).SetValue(true);
            _config.SubMenu("Drawings").AddItem(new MenuItem("DrawE", "Draw E")).SetValue(true);
            _config.SubMenu("Drawings").AddItem(new MenuItem("DrawR", "Draw R")).SetValue(true);
            _config.SubMenu("Drawings").AddItem(new MenuItem("CircleLag", "Lag Free Circles").SetValue(true));
            _config.SubMenu("Drawings").AddItem(new MenuItem("CircleQuality", "Circles Quality").SetValue(new Slider(100, 100, 10)));
            _config.SubMenu("Drawings").AddItem(new MenuItem("CircleThickness", "Circles Thickness").SetValue(new Slider(1, 10, 1)));

            _config.AddToMainMenu();
            Game.PrintChat("<font color='#881df2'>D-MissFortune by Diabaths</font> Loaded.");
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Hero_OnProcessSpellCast;
            AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;
            Game.OnGameSendPacket += GameOnOnGameSendPacket;
            Game.OnGameUpdate += Game_OnGameUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
            if (_config.Item("skinMF").GetValue<bool>())
            {
                GenModelPacket(_player.ChampionName, _config.Item("skinMiss").GetValue<Slider>().Value);
                _lastSkin = _config.Item("skinMiss").GetValue<Slider>().Value;
            }

        }
        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (ObjectManager.Player.HasBuff("missfortunebulletsound"))
                return;
            if (_config.Item("skinMF").GetValue<bool>() && SkinChanged())
            {
                GenModelPacket(_player.ChampionName, _config.Item("skinMiss").GetValue<Slider>().Value);
                _lastSkin = _config.Item("skinMiss").GetValue<Slider>().Value;
            }
            _orbwalker.SetAttack(true);

            _orbwalker.SetMovement(true);

            if (_config.Item("ActiveCombo").GetValue<KeyBind>().Active)
            {
                Combo();
            }
            if ((_config.Item("ActiveHarass").GetValue<KeyBind>().Active || _config.Item("harasstoggle").GetValue<KeyBind>().Active) && (100 * (_player.Mana / _player.MaxMana)) > _config.Item("Harrasmana").GetValue<Slider>().Value)
            {
                Harass();

            }
            if (_config.Item("ActiveLane").GetValue<KeyBind>().Active && (100 * (_player.Mana / _player.MaxMana)) > _config.Item("Lanemana").GetValue<Slider>().Value)
            {
                Laneclear();
                JungleClear();
            }
            if (_config.Item("ActiveLast").GetValue<KeyBind>().Active && (100 * (_player.Mana / _player.MaxMana)) > _config.Item("Lanemana").GetValue<Slider>().Value)
            {
                LastHit();
            }

            _player = ObjectManager.Player;

            KillSteal();
        }
        private static void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (_e.IsReady() && gapcloser.Sender.IsValidTarget(_e.Range) && _config.Item("Gap_E").GetValue<bool>())
            {
                _e.Cast(gapcloser.Sender, Packets());
            }
        }
        static void GenModelPacket(string champ, int skinId)
        {
            Packet.S2C.UpdateModel.Encoded(new Packet.S2C.UpdateModel.Struct(_player.NetworkId, skinId, champ)).Process();
        }
        static bool SkinChanged()
        {
            return (_config.Item("skinMiss").GetValue<Slider>().Value != _lastSkin);
        }
        private static void GameOnOnGameSendPacket(GamePacketEventArgs args)
        {
            if (args.PacketData[0] == Packet.C2S.Move.Header && _player.HasBuff("missfortunebulletsound"))
            {
                args.Process = false;
            }
        }

        private static void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && args.SData.Name == "MissFortuneBulletTime")
            {
                _orbwalker.SetAttack(false);
                _orbwalker.SetMovement(false);
                _timeTick = Environment.TickCount;
            }
        }

        private static bool Packets()
        {
            return _config.Item("usePackets").GetValue<bool>();
        }

        // Credits to princer007
        // Dont Work perfect but work :p
        private static void CastQEnemy()
        {
            var target = SimpleTs.GetTarget(_q.Range + 450, SimpleTs.DamageType.Physical);
            if (target.IsValidTarget(_q.Range))
            {
                _q.CastOnUnit(target, Packets());
                return;
            }
            foreach (Obj_AI_Base minion in ObjectManager.Get<Obj_AI_Base>())
                if (minion.IsValidTarget(_q.Range, true) &&
                    minion.Distance(target) < 450)
                    _q.CastOnUnit(minion, Packets());
        }

        private static void Combo()
        {
            var rtarget = SimpleTs.GetTarget(_r.Range - 200, SimpleTs.DamageType.Physical);
            var useQ = _config.Item("UseQC").GetValue<bool>();
            var useW = _config.Item("UseWC").GetValue<bool>();
            var useE = _config.Item("UseEC").GetValue<bool>();
            var useR = _config.Item("UseRC").GetValue<bool>();
            var autoR = _config.Item("UseRE").GetValue<bool>();
            if (useQ && _q.IsReady())
                CastQEnemy();

            if (useW && _w.IsReady())
            {
                var t = SimpleTs.GetTarget(_q.Range, SimpleTs.DamageType.Magical);
                if (t != null)
                    _w.Cast();
            }
            if (useE && _e.IsReady())
            {
                var t = SimpleTs.GetTarget(_e.Range, SimpleTs.DamageType.Magical);
                if (t != null && t.Distance(_player.Position) < _e.Range)
                    _e.CastIfHitchanceEquals(t, t.IsMoving ? HitChance.High : HitChance.Medium, Packets());
            }
            if (useR && _r.IsReady())
            {
                CastR();
            }
            if (_r.IsReady() && autoR && ObjectManager.Get<Obj_AI_Hero>().Count(hero => hero.IsValidTarget(_r.Range - 200)) >= _config.Item("MinTargets").GetValue<Slider>().Value)
            {
                _r.CastIfHitchanceEquals(rtarget, rtarget.IsMoving ? HitChance.High : HitChance.Medium, Packets());
                _orbwalker.SetAttack(false);
                _orbwalker.SetMovement(false);
                _timeTick = Environment.TickCount;
            }
        }

        private static void Harass()
        {
            var eTarget = SimpleTs.GetTarget(_q.Range, SimpleTs.DamageType.Physical);
            var useQ = _config.Item("UseQH").GetValue<bool>();
            var useE = _config.Item("UseEH").GetValue<bool>();

            if (useQ && _q.IsReady())
                CastQEnemy();

            if (useE && _e.IsReady())
            {
                var t = SimpleTs.GetTarget(_e.Range, SimpleTs.DamageType.Magical);
                if (t != null && t.Distance(_player.Position) < _e.Range)
                    _e.CastIfHitchanceEquals(t, t.IsMoving ? HitChance.High : HitChance.Medium, Packets());
                return;
            }
        }
        private static void Laneclear()
        {
            var allMinions = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, _q.Range, MinionTypes.All);
            var useQ = _config.Item("UseQL").GetValue<bool>();
            var useE = _config.Item("UseEL").GetValue<bool>();

            if (allMinions.Count < 2) return;

            foreach (var minion in allMinions)
            {
                if (useQ && _q.IsReady())
                {
                    Cast_Basic_Farm(_q, false);
                }
                if (_e.IsReady() && useE)
                {
                    Cast_Basic_Farm(_e, true);
                }
            }
        }
        private static void LastHit()
        {
            var allMinions = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, _q.Range, MinionTypes.All);
            var useQ = _config.Item("UseQLH").GetValue<bool>();
            var useE = _config.Item("UseELH").GetValue<bool>();
            if (allMinions.Count < 2) return;

            foreach (var minion in allMinions)
            {
                if (useQ && _q.IsReady())
                {
                    Cast_Basic_Farm(_q, false);
                }
                if (_e.IsReady() && useE)
                {
                    Cast_Basic_Farm(_e, true);
                }
            }
        }
        private static void JungleClear()
        {
            var mobs = MinionManager.GetMinions(_player.ServerPosition, _q.Range,
            MinionTypes.All,
            MinionTeam.Neutral, MinionOrderTypes.MaxHealth);
            var useQ = _config.Item("UseQJ").GetValue<bool>();
            var useE = _config.Item("UseEJ").GetValue<bool>();
            if (mobs.Count > 0)
            {
                var mob = mobs[0];
                if (useQ && _q.IsReady())
                {
                    _q.Cast(mob, Packets(), false);
                }
                if (_e.IsReady() && useE)
                {
                    _e.Cast(mob, Packets(), true);
                }
            }
        }

        //By Lexxes
        private static void Cast_Basic_Farm(Spell spell, bool skillshot = false)
        {
            if (!spell.IsReady())
                return;
            var allMinions = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, spell.Range, MinionTypes.All, MinionTeam.NotAlly, MinionOrderTypes.MaxHealth);
            foreach (var minion in allMinions)
            {
                if (!minion.IsValidTarget())
                    continue;
                var minionInRangeAa = Orbwalking.InAutoAttackRange(minion);
                var minionInRangeSpell = minion.Distance(ObjectManager.Player) <= spell.Range;
                var minionKillableAa = _player.GetAutoAttackDamage(minion, true) >= minion.Health;
                var minionKillableSpell = _player.GetSpellDamage(minion, SpellSlot.Q) >= minion.Health;
                var lastHit = _orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LastHit;
                var laneClear = _orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear;
                if ((lastHit && minionInRangeSpell && minionKillableSpell) && ((minionInRangeAa && !minionKillableAa) || !minionInRangeAa))
                    if (skillshot)
                        spell.Cast(minion.Position, Packets());
                    else
                        spell.Cast(minion, Packets());
                else if ((laneClear && minionInRangeSpell && !minionKillableSpell) && ((minionInRangeAa && !minionKillableAa) || !minionInRangeAa))
                    if (skillshot)
                        spell.Cast(minion.Position, Packets());
                    else
                        spell.Cast(minion, Packets());
            }
        }

        private static void CastR()
        {
            if (!_r.IsReady()) return;

            Obj_AI_Hero target = SimpleTs.GetTarget(_r.Range - 200, SimpleTs.DamageType.Magical);
            if (target == null) return;
            if (_r.GetDamage(target) * 8 < target.Health) return;
            if (target.HasBuff("JudicatorIntervention") && target.HasBuff("Undying Rage")) return;
            if (_r.GetPrediction(target).Hitchance >= HitChance.Medium)
            {
                _r.CastIfHitchanceEquals(target, target.IsMoving ? HitChance.High : HitChance.Medium, Packets());
                Program._orbwalker.SetAttack(false);
                Program._orbwalker.SetMovement(false);
                _timeTick = Environment.TickCount;
            }
        }
        private static void KillSteal()
        {
            var target = SimpleTs.GetTarget(_w.Range, SimpleTs.DamageType.Magical);
            var qDmg = _player.GetSpellDamage(target, SpellSlot.Q);
            var eDmg = _player.GetSpellDamage(target, SpellSlot.E);
            if (_q.IsReady() && _player.Distance(target) <= _q.Range && target != null && _config.Item("UseQM").GetValue<bool>())
            {
                if (target.Health <= qDmg)
                {
                    _q.Cast(target, Packets());
                }
            }
            if (_e.IsReady() && _player.Distance(target) <= _e.Range && target != null && _config.Item("UseEM").GetValue<bool>())
            {
                if (target.Health <= eDmg)
                {
                    _e.CastIfHitchanceEquals(target, target.IsMoving ? HitChance.High : HitChance.Medium, Packets());
                }
            }
            if (_r.IsReady() && _config.Item("UseRM").GetValue<bool>())
            {
                CastR();
            }
        }
        private static void Drawing_OnDraw(EventArgs args)
        {
            if (_config.Item("CircleLag").GetValue<bool>())
            {
                if (_config.Item("DrawQ").GetValue<bool>())
                {
                    Utility.DrawCircle(ObjectManager.Player.Position, _q.Range, System.Drawing.Color.Orange,
                        _config.Item("CircleThickness").GetValue<Slider>().Value,
                        _config.Item("CircleQuality").GetValue<Slider>().Value);
                }
                if (_config.Item("DrawW").GetValue<bool>())
                {
                    Utility.DrawCircle(ObjectManager.Player.Position, _w.Range, System.Drawing.Color.Orange,
                        _config.Item("CircleThickness").GetValue<Slider>().Value,
                        _config.Item("CircleQuality").GetValue<Slider>().Value);
                }
                if (_config.Item("DrawE").GetValue<bool>())
                {
                    Utility.DrawCircle(ObjectManager.Player.Position, _e.Range, System.Drawing.Color.Orange,
                        _config.Item("CircleThickness").GetValue<Slider>().Value,
                        _config.Item("CircleQuality").GetValue<Slider>().Value);
                }
                if (_config.Item("DrawR").GetValue<bool>())
                {
                    Utility.DrawCircle(ObjectManager.Player.Position, _r.Range, System.Drawing.Color.Orange,
                        _config.Item("CircleThickness").GetValue<Slider>().Value,
                        _config.Item("CircleQuality").GetValue<Slider>().Value);
                }
            }
            else
            {
                if (_config.Item("DrawQ").GetValue<bool>())
                {
                    Drawing.DrawCircle(ObjectManager.Player.Position, _q.Range, System.Drawing.Color.White);
                }
                if (_config.Item("DrawW").GetValue<bool>())
                {
                    Drawing.DrawCircle(ObjectManager.Player.Position, _w.Range, System.Drawing.Color.White);
                }
                if (_config.Item("DrawE").GetValue<bool>())
                {
                    Drawing.DrawCircle(ObjectManager.Player.Position, _e.Range, System.Drawing.Color.White);
                }

                if (_config.Item("DrawR").GetValue<bool>())
                {
                    Drawing.DrawCircle(ObjectManager.Player.Position, _r.Range, System.Drawing.Color.White);
                }

            }
        }
    }
}
