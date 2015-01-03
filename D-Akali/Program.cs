using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace D_Akali
{

    class Program
    {
        private const string ChampionName = "Akali";

        private static Orbwalking.Orbwalker _orbwalker;

        public static Menu TargetSelectorMenu;

        private static Spell _q, _w, _e, _r;

        private static Menu _config;

        private static Obj_AI_Hero _player;

        private static Items.Item _dfg;

        private static SpellSlot _igniteSlot;
        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            _player = ObjectManager.Player;
            if (ObjectManager.Player.BaseSkinName != ChampionName) return;

            _q = new Spell(SpellSlot.Q, 600);
            _w = new Spell(SpellSlot.W, 700);
            _e = new Spell(SpellSlot.E, 325);
            _r = new Spell(SpellSlot.R, 800);

           

            _igniteSlot = _player.GetSpellSlot("SummonerDot");
            _dfg = new Items.Item(3128, 750f);
            //D Zyra
            _config = new Menu("D-Akali", "D-Akali", true);

            //TargetSelector
            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(TargetSelectorMenu);
            _config.AddSubMenu(TargetSelectorMenu);


            //Orbwalker
            _config.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));
            _orbwalker = new Orbwalking.Orbwalker(_config.SubMenu("Orbwalking"));


            //Combo usedfg, useignite
            _config.AddSubMenu(new Menu("Combo", "Combo"));
            _config.SubMenu("Combo").AddItem(new MenuItem("usedfg", "Use DFG")).SetValue(true);
            _config.SubMenu("Combo").AddItem(new MenuItem("useignite", "Use Ignite")).SetValue(true);
            _config.SubMenu("Combo").AddItem(new MenuItem("useQC", "Use Q")).SetValue(true);
            _config.SubMenu("Combo").AddItem(new MenuItem("useWC", "Use W").SetValue(true));
            _config.SubMenu("Combo").AddItem(new MenuItem("useEC", "Use E")).SetValue(true);
            _config.SubMenu("Combo").AddItem(new MenuItem("useRC", "Use R")).SetValue(true);
            _config.SubMenu("Combo")
                .AddItem(new MenuItem("ActiveCombo", "Combo!").SetValue(new KeyBind(32, KeyBindType.Press)));

            //harass
            _config.AddSubMenu(new Menu("Harass", "Harass"));
            _config.SubMenu("Harass").AddItem(new MenuItem("useQH", "Use Q").SetValue(true));
            _config.SubMenu("Harass").AddItem(new MenuItem("useWH", "Use W").SetValue(true));
            _config.SubMenu("Harass").AddItem(new MenuItem("Energyharass", "Minimum Energy% >").SetValue(new Slider(35, 1, 100)));
            _config.SubMenu("Harass").AddItem(new MenuItem("harasstoggle", "AutoHarass (toggle)").SetValue(new KeyBind("G".ToCharArray()[0],KeyBindType.Toggle)));
            _config.SubMenu("Harass").AddItem(new MenuItem("ActiveHarass", "Harass!").SetValue(new KeyBind("C".ToCharArray()[0], KeyBindType.Press)));

            //Farm
            _config.AddSubMenu(new Menu("Farm", "Farm"));
            _config.SubMenu("Farm").AddSubMenu(new Menu("LaneClear", "LaneClear"));
            _config.SubMenu("Farm").SubMenu("LaneClear").AddItem(new MenuItem("useQL", "Use Q").SetValue(true));
            _config.SubMenu("Farm").SubMenu("LaneClear").AddItem(new MenuItem("useEL", "Use E").SetValue(true));
            _config.SubMenu("Farm").SubMenu("LaneClear").AddItem(new MenuItem("Energylane", "Minimum Energy% >").SetValue(new Slider(35, 1, 100)));
            _config.SubMenu("Farm").SubMenu("LaneClear").AddItem(new MenuItem("Activelane", "LaneClear!").SetValue(new KeyBind("V".ToCharArray()[0], KeyBindType.Press)));
            _config.SubMenu("Farm").AddSubMenu(new Menu("Jungle", "Jungle"));
            _config.SubMenu("Farm").SubMenu("Jungle").AddItem(new MenuItem("useQJ", "Use Q").SetValue(true));
            _config.SubMenu("Farm").SubMenu("Jungle").AddItem(new MenuItem("useEJ", "Use E").SetValue(true));
            _config.SubMenu("Farm").SubMenu("Jungle").AddItem(new MenuItem("useWJ", "Use W").SetValue(true));
            _config.SubMenu("Farm").SubMenu("LaneClear").AddItem(new MenuItem("Wifhp", "Use W if HP% <").SetValue(new Slider(35, 1, 100)));
            _config.SubMenu("Farm").SubMenu("Jungle").AddItem(new MenuItem("Energyjungle", "Minimum Energy% >").SetValue(new Slider(35, 1, 100)));
            _config.SubMenu("Farm").SubMenu("Jungle").AddItem(new MenuItem("ActiveJungle", "Jungle!").SetValue(new KeyBind("V".ToCharArray()[0], KeyBindType.Press)));

            //Misc
            _config.AddSubMenu(new Menu("Misc", "Misc"));
            _config.SubMenu("Misc").AddItem(new MenuItem("usePackets", "Usepackes")).SetValue(true);
            _config.SubMenu("Misc").AddItem(new MenuItem("Inter_E", "Interrupter E")).SetValue(true);
            _config.SubMenu("Misc").AddItem(new MenuItem("Gap_E", "GapClosers E")).SetValue(true);

            //Damage after combo:
            MenuItem dmgAfterComboItem = new MenuItem("DamageAfterCombo", "Draw damage after combo").SetValue(true);
            Utility.HpBarDamageIndicator.DamageToUnit = ComboDamage;
            Utility.HpBarDamageIndicator.Enabled = dmgAfterComboItem.GetValue<bool>();
            dmgAfterComboItem.ValueChanged +=
                delegate(object sender, OnValueChangeEventArgs eventArgs)
                {
                    Utility.HpBarDamageIndicator.Enabled = eventArgs.GetNewValue<bool>();
                };
            //Draw
            _config.AddSubMenu(new Menu("Drawing", "Drawing"));
            _config.SubMenu("Drawing").AddItem(new MenuItem("DrawQ", "Draw Q").SetValue(true));
            _config.SubMenu("Drawing").AddItem(new MenuItem("DrawW", "Draw W").SetValue(true));
            _config.SubMenu("Drawing").AddItem(new MenuItem("DrawE", "Draw E").SetValue(true));
            _config.SubMenu("Drawing").AddItem(new MenuItem("DrawR", "Draw R").SetValue(true));
            _config.SubMenu("Drawing").AddItem(dmgAfterComboItem);
            _config.SubMenu("Drawing").AddItem(new MenuItem("CircleLag", "Lag Free Circles").SetValue(true));
            _config.SubMenu("Drawing").AddItem(new MenuItem("CircleQuality", "Circles Quality").SetValue(new Slider(100, 100, 10)));
            _config.SubMenu("Drawing").AddItem(new MenuItem("CircleThickness", "Circles Thickness").SetValue(new Slider(1, 10, 1)));

            _config.AddToMainMenu();

            Game.PrintChat("<font color='#881df2'>D-Akali by Diabaths</font> Loaded.");
            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnGameUpdate += Game_OnGameUpdate;
           }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (_config.Item("ActiveCombo").GetValue<KeyBind>().Active)
            {
                int assassinRange = TargetSelectorMenu.Item("AssassinSearchRange").GetValue<Slider>().Value;

                IEnumerable<Obj_AI_Hero> xEnemy = ObjectManager.Get<Obj_AI_Hero>()
                    .Where(
                        enemy =>
                            enemy.Team != ObjectManager.Player.Team && !enemy.IsDead && enemy.IsVisible &&
                            TargetSelectorMenu.Item("Assassin" + enemy.ChampionName) != null &&
                            TargetSelectorMenu.Item("Assassin" + enemy.ChampionName).GetValue<bool>() &&
                            ObjectManager.Player.Distance(enemy) < assassinRange);

                Obj_AI_Hero[] objAiHeroes = xEnemy as Obj_AI_Hero[] ?? xEnemy.ToArray();

                if (objAiHeroes.Length > 2)
                {
                    Game.PrintChat(objAiHeroes[0].Distance(objAiHeroes[1]).ToString());
                }

                Obj_AI_Hero t = !objAiHeroes.Any()
                    ? TargetSelector.GetTarget(_q.Range, TargetSelector.DamageType.Magical)
                    : objAiHeroes[0];

                Combo(t);
            }
            if ((_config.Item("ActiveHarass").GetValue<KeyBind>().Active ||
                 _config.Item("harasstoggle").GetValue<KeyBind>().Active) &&
                (100 * (_player.Mana / _player.MaxMana)) > _config.Item("Energyharass").GetValue<Slider>().Value)
            {
                Harass();

            }
            if (_config.Item("Activelane").GetValue<KeyBind>().Active &&
                (100 * (_player.Mana / _player.MaxMana)) > _config.Item("Energylane").GetValue<Slider>().Value)
            {
                Laneclear();
            }
            if (_config.Item("ActiveJungle").GetValue<KeyBind>().Active &&
                (100 * (_player.Mana / _player.MaxMana)) > _config.Item("Energyjungle").GetValue<Slider>().Value)
            {
                JungleClear();
            }
            _player = ObjectManager.Player;

            _orbwalker.SetAttack(true);

        }

        private static float ComboDamage(Obj_AI_Hero hero)
        {
            var dmg = 0d;

            if (_q.IsReady())
            {
                if (_w.IsReady())
                {
                    dmg += _player.GetSpellDamage(hero, SpellSlot.Q) + (23 + 6.5 * ObjectManager.Player.Level) +
                           (1.2 * _player.FlatMagicDamageMod);
                }
                else dmg += _player.GetSpellDamage(hero, SpellSlot.Q);
            }

            if (_e.IsReady())
            {
                if (_w.IsReady())
                {
                    dmg += _player.GetSpellDamage(hero, SpellSlot.E) + (23 + 6.5 * ObjectManager.Player.Level) +
                           (1.2 * _player.FlatMagicDamageMod);
                }
                else dmg += _player.GetSpellDamage(hero, SpellSlot.E);
            }
            if (_r.IsReady())
                dmg += _player.GetSpellDamage(hero, SpellSlot.R);
            if (Items.HasItem(3128))
            {
                dmg += _player.GetItemDamage(hero, Damage.DamageItems.Dfg);
                dmg = dmg * 1.2;
            }
            if (ObjectManager.Player.GetSpellSlot("SummonerIgnite") != SpellSlot.Unknown)
            {
                dmg += _player.GetSummonerSpellDamage(hero, Damage.SummonerSpell.Ignite);
            }
            dmg += _player.GetAutoAttackDamage(hero, true);
            return (float)dmg;
        }

      
        private static void Combo(Obj_AI_Hero t)
        {
//            var target = SimpleTs.GetTarget(_q.Range, SimpleTs.DamageType.Magical);
            var target = t;
//            if (target != null)
            var usedfg = _config.Item("usedfg").GetValue<bool>();
            var useignite = _config.Item("useignite").GetValue<bool>();
            if (_player.Distance(target) <= _dfg.Range && usedfg &&
                      _dfg.IsReady() && target.Health <= ComboDamage(target))
            {
                _dfg.Cast(target);
            }
            if (useignite && _igniteSlot != SpellSlot.Unknown && _player.Distance(target) <= 600 &&
                   _player.Spellbook.CanUseSpell(_igniteSlot) == SpellState.Ready)
            {
                if (target.Health <= ComboDamage(target))
                {
                    _player.Spellbook.CastSpell(_igniteSlot, target);
                }
            }
            if (_config.Item("UseRE").GetValue<bool>() || _config.Item("use_ulti").GetValue<bool>())
                CastREnemy();
            if (_config.Item("useQ_TeamFight").GetValue<bool>())
                CastQEnemy();
            if (_config.Item("useE_TeamFight").GetValue<bool>())
                CastEEnemy();
        }

        private static void Harass()
        {
            if (_config.Item("useQ_Harass").GetValue<bool>())
                CastQEnemy();
            if (_config.Item("useE_Harass").GetValue<bool>())
                CastEEnemy();
        }

        private static void Laneclear()
        {
            if (_config.Item("useQ_LaneClear").GetValue<bool>())
                CastQMinion();
            if (_config.Item("useE_LaneClear").GetValue<bool>())
                CastEMinion();
        }

        private static void JungleClear()
        {
            if (_config.Item("useQ_Jungle").GetValue<bool>())
                CastQjungleMinion();
            if (_config.Item("useE_Jungle").GetValue<bool>())
                CastEjungleMinion();
        }

        private static bool Packets()
        {
            return _config.Item("usePackets").GetValue<bool>();
        }

        private static bool ZyraisZombie()
        {
            return ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Name ==
                   ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).Name ||
                   ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Name ==
                   ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).Name;
        }
        private static void CastEjungleMinion()
        {
            if (!_e.IsReady())
                return;
            var minions = MinionManager.GetMinions(_player.ServerPosition, _e.Range,
                MinionTypes.All,
                MinionTeam.Neutral, MinionOrderTypes.MaxHealth);
            if (minions.Count == 0)
                return;
            var castPostion =
                MinionManager.GetBestLineFarmLocation(
                    minions.Select(minion => minion.ServerPosition.To2D()).ToList(),
                    _e.Width, _e.Range);
            _e.Cast(castPostion.Position, Packets());
            if (_config.Item("useW_Passivej").GetValue<bool>() && _w.IsReady())
            {
                var pos = castPostion.Position.To3D();
                Utility.DelayAction.Add(50, () => _w.Cast(new Vector3(pos.X - 5, pos.Y - 5, pos.Z), Packets()));
                Utility.DelayAction.Add(150, () => _w.Cast(new Vector3(pos.X + 5, pos.Y + 5, pos.Z), Packets()));
            }
        }
        private static void CastEMinion()
        {
            if (!_e.IsReady())
                return;
            var minions = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, _e.Range, MinionTypes.All);
            if (minions.Count == 0)
                return;
            var castPostion =
                MinionManager.GetBestLineFarmLocation(
                    minions.Select(minion => minion.ServerPosition.To2D()).ToList(),
                    _e.Width, _e.Range);
            _e.Cast(castPostion.Position, Packets());
            if (_config.Item("useW_Passivel").GetValue<bool>() && _w.IsReady())
            {
                var pos = castPostion.Position.To3D();
                Utility.DelayAction.Add(50, () => _w.Cast(new Vector3(pos.X - 5, pos.Y - 5, pos.Z), Packets()));
                Utility.DelayAction.Add(150, () => _w.Cast(new Vector3(pos.X + 5, pos.Y + 5, pos.Z), Packets()));
            }
        }


        private static void CastQjungleMinion()
        {
            if (!_q.IsReady())
                return;
            var minions = MinionManager.GetMinions(_player.ServerPosition, _q.Range,
               MinionTypes.All,
               MinionTeam.Neutral, MinionOrderTypes.MaxHealth);
            if (minions.Count == 0)
                return;
            var castPostion =
                MinionManager.GetBestCircularFarmLocation(
                    minions.Select(minion => minion.ServerPosition.To2D()).ToList(), _q.Width, _q.Range);
            _q.Cast(castPostion.Position, Packets());
            if (_config.Item("useW_Passivej").GetValue<bool>() && _w.IsReady())
            {
                var pos = castPostion.Position.To3D();
                Utility.DelayAction.Add(50, () => _w.Cast(new Vector3(pos.X - 5, pos.Y - 5, pos.Z), Packets()));
                Utility.DelayAction.Add(150, () => _w.Cast(new Vector3(pos.X + 5, pos.Y + 5, pos.Z), Packets()));
            }
        }
        private static void CastQMinion()
        {
            if (!_q.IsReady())
                return;
            var minions = MinionManager.GetMinions(ObjectManager.Player.Position, _q.Range + (_q.Width / 2),
                MinionTypes.All, MinionTeam.NotAlly);
            if (minions.Count == 0)
                return;
            var castPostion =
                MinionManager.GetBestCircularFarmLocation(
                    minions.Select(minion => minion.ServerPosition.To2D()).ToList(), _q.Width, _q.Range);
            _q.Cast(castPostion.Position, Packets());
            if (_config.Item("useW_Passivel").GetValue<bool>() && _w.IsReady())
            {
                var pos = castPostion.Position.To3D();
                Utility.DelayAction.Add(50, () => _w.Cast(new Vector3(pos.X - 5, pos.Y - 5, pos.Z), Packets()));
                Utility.DelayAction.Add(150, () => _w.Cast(new Vector3(pos.X + 5, pos.Y + 5, pos.Z), Packets()));
            }
        }

        private static void CastREnemy()
        {
            if (!_r.IsReady())
            {
                return;
            }
            var target = TargetSelector.GetTarget(_e.Range, TargetSelector.DamageType.Magical);
            if (!target.IsValidTarget(_r.Range))
            {
                return;
            }
            if (ComboDamage(target) > target.Health && _config.Item("use_ulti").GetValue<bool>() &&
                GetNumberHitByR(target) >= 1)
            {
                _r.CastIfHitchanceEquals(target, HitChance.VeryHigh, Packets());
            }
            if (GetNumberHitByR(target) >= _config.Item("MinTargets").GetValue<Slider>().Value)
            {
                _r.CastIfHitchanceEquals(target, HitChance.High, Packets());
            }
        }

        private static int GetNumberHitByR(Obj_AI_Hero target)
        {
            int Enemys = 0;
            foreach (Obj_AI_Hero enemys in ObjectManager.Get<Obj_AI_Hero>())
            {
                var pred = _r.GetPrediction(enemys, true);
                if (pred.Hitchance >= HitChance.High && !enemys.IsMe && enemys.IsEnemy && Vector3.Distance(_player.Position, pred.UnitPosition) <= _r.Range)
                {
                    Enemys = Enemys + 1;
                }
            }
            return Enemys;
        }
        private static void CastQEnemy()
        {
            if (!_q.IsReady())
                return;
            var target = TargetSelector.GetTarget(_q.Range + (_q.Width / 2), TargetSelector.DamageType.Magical);
            if (!target.IsValidTarget(_q.Range))
                return;
            _q.CastIfHitchanceEquals(target, HitChance.High, Packets());
            if (_config.Item("useW_Passive").GetValue<bool>() && _w.IsReady())
            {
                var pos = _q.GetPrediction(target).CastPosition;
                Utility.DelayAction.Add(50, () => _w.Cast(new Vector3(pos.X - 2, pos.Y - 2, pos.Z), Packets()));
                Utility.DelayAction.Add(150, () => _w.Cast(new Vector3(pos.X + 2, pos.Y + 2, pos.Z), Packets()));
            }
        }

        private static void CastEEnemy()
        {
            if (!_e.IsReady())
                return;
            var target = TargetSelector.GetTarget(_e.Range, TargetSelector.DamageType.Magical);
            if (!target.IsValidTarget(_e.Range))
                return;
            _e.CastIfHitchanceEquals(target, HitChance.High, Packets());
            if (_config.Item("useW_Passive").GetValue<bool>() && _w.IsReady())
            {
                var pos = _e.GetPrediction(target).CastPosition;
                Utility.DelayAction.Add(50, () => _w.Cast(new Vector3(pos.X - 5, pos.Y - 5, pos.Z), Packets()));
                Utility.DelayAction.Add(150, () => _w.Cast(new Vector3(pos.X + 5, pos.Y + 5, pos.Z), Packets()));
            }
        }

      

        private static void Drawing_OnDraw(EventArgs args)
        {

            if (_config.Item("CircleLag").GetValue<bool>())
            {

                if (_config.Item("DrawQ").GetValue<bool>())
                {
                    Utility.DrawCircle(ObjectManager.Player.Position, _q.Range, System.Drawing.Color.Gray,
                        _config.Item("CircleThickness").GetValue<Slider>().Value,
                        _config.Item("CircleQuality").GetValue<Slider>().Value);
                }
                if (_config.Item("DrawW").GetValue<bool>())
                {
                    Utility.DrawCircle(ObjectManager.Player.Position, _w.Range, System.Drawing.Color.Gray,
                        _config.Item("CircleThickness").GetValue<Slider>().Value,
                        _config.Item("CircleQuality").GetValue<Slider>().Value);
                }
                if (_config.Item("DrawE").GetValue<bool>())
                {
                    Utility.DrawCircle(ObjectManager.Player.Position, _e.Range, System.Drawing.Color.Gray,
                        _config.Item("CircleThickness").GetValue<Slider>().Value,
                        _config.Item("CircleQuality").GetValue<Slider>().Value);
                }
                if (_config.Item("DrawR").GetValue<bool>())
                {
                    Utility.DrawCircle(ObjectManager.Player.Position, _r.Range, System.Drawing.Color.Gray,
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
