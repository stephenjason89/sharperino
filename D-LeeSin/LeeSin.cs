using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LeagueSharp;
using LeagueSharp.Common;
using System.Drawing;
using SharpDX;

namespace D_leeSin
{
    class LeeSin
    {
        public static Vector2 testSpellCast;
        public static Vector2 testSpellProj;
        private static int _wardJumpRange = 600;
        public static bool loai = false;
        public static Vector3 tg1, tg;
        public static float tx, tz;
        private static readonly int _wardDistance = 300;
        public static Obj_AI_Hero Player = ObjectManager.Player;
        public static bool da = false;
        public static Spellbook sBook = Player.Spellbook;
        public static float lastwardjump = 0, lastTimeJump = 0;
        public static float lasttick = 0;




        public static SpellDataInst Qdata = sBook.GetSpell(SpellSlot.Q);
        public static SpellDataInst Wdata = sBook.GetSpell(SpellSlot.W);
        public static SpellDataInst Edata = sBook.GetSpell(SpellSlot.E);
        public static SpellDataInst Rdata = sBook.GetSpell(SpellSlot.R);
        public static SpellDataInst SData = Player.Spellbook.GetSpell(Player.GetSpellSlot("summonersmite"));
        public static Spell Q = new Spell(SpellSlot.Q, 1100);
        public static Spell W = new Spell(SpellSlot.W, 700);
        public static Spell E = new Spell(SpellSlot.E, 350);
        public static Spell R = new Spell(SpellSlot.R, 375);

        private static Items.Item _Bilge = new Items.Item(3144, 475f);
        private static Items.Item _Blade = new Items.Item(3153, 425f);
        private static Items.Item _Hydra = new Items.Item(3074, 375f);
        private static Items.Item _Tiamat = new Items.Item(3077, 375f);
        private static Items.Item _Rand = new Items.Item(3143, 490f);
        private static Items.Item _lotis = new Items.Item(3190, 590f);

        public static Obj_AI_Hero LockedTarget;

        public static Vector2 harassStart;

        public static void checkLock(Obj_AI_Hero target)
        {
            //if (!target.IsValidTarget())
            //    return;
            if (!LeeSinSharp.Config.Item("ActiveHarass").GetValue<KeyBind>().Active && LockedTarget != null)//Reset all values
            {
                LockedTarget = null;
            }
            else if (LeeSinSharp.Config.Item("ActiveCombo").GetValue<KeyBind>().Active)
                LockedTarget = target;
            else if (target.IsValidTarget() && LockedTarget == null || LeeSinSharp.Config.Item("ActiveHarass").GetValue<KeyBind>().Active)
            {
                LockedTarget = target;
            }
        }

        private static void UseItemes(Obj_AI_Hero target)
        {
            var iBilge = LeeSinSharp.Config.Item("Bilge").GetValue<bool>();
            var iBilgeEnemyhp = target.Health <= (target.MaxHealth * (LeeSinSharp.Config.Item("BilgeEnemyhp").GetValue<Slider>().Value) / 100);
            var iBilgemyhp = Player.Health <= (Player.MaxHealth * (LeeSinSharp.Config.Item("Bilgemyhp").GetValue<Slider>().Value) / 100);
            var iBlade = LeeSinSharp.Config.Item("Blade").GetValue<bool>();
            var iBladeEnemyhp = target.Health <= (target.MaxHealth * (LeeSinSharp.Config.Item("BladeEnemyhp").GetValue<Slider>().Value) / 100);
            var iBlademyhp = Player.Health <= (Player.MaxHealth * (LeeSinSharp.Config.Item("Blademyhp").GetValue<Slider>().Value) / 100);
            var iOmen = LeeSinSharp.Config.Item("Omen").GetValue<bool>();
            var iOmenenemys = ObjectManager.Get<Obj_AI_Hero>().Count(hero => hero.IsValidTarget(450)) >=
                              LeeSinSharp.Config.Item("Omenenemys").GetValue<Slider>().Value;
            var iTiamat = LeeSinSharp.Config.Item("Tiamat").GetValue<bool>();
            var iHydra = LeeSinSharp.Config.Item("Hydra").GetValue<bool>();
            var ilotis = LeeSinSharp.Config.Item("lotis").GetValue<bool>();
            //var ihp = _config.Item("Hppotion").GetValue<bool>();
            // var ihpuse = _player.Health <= (_player.MaxHealth * (_config.Item("Hppotionuse").GetValue<Slider>().Value) / 100);
            //var imp = _config.Item("Mppotion").GetValue<bool>();
            //var impuse = _player.Health <= (_player.MaxHealth * (_config.Item("Mppotionuse").GetValue<Slider>().Value) / 100);

            if (Player.Distance(target) <= 450 && iBilge && (iBilgeEnemyhp || iBilgemyhp) && _Bilge.IsReady())
            {
                _Bilge.Cast(target);

            }
            if (Player.Distance(target) <= 450 && iBlade && (iBladeEnemyhp || iBlademyhp) && _Blade.IsReady())
            {
                _Blade.Cast(target);

            }
            if (Utility.CountEnemysInRange(350) >= 1 && iTiamat && _Tiamat.IsReady())
            {
                _Tiamat.Cast(target);

            }
            if (Utility.CountEnemysInRange(350) >= 1 && iHydra && _Hydra.IsReady())
            {
                _Hydra.Cast(target);

            }
            if (iOmenenemys && iOmen && _Rand.IsReady())
            {
                _Rand.Cast();

            }
            if (ilotis)
            {
                foreach (var hero in ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.IsAlly || hero.IsMe))
                {
                    if ((hero.Health / hero.MaxHealth) * 100 <= LeeSinSharp.Config.Item("lotisminhp").GetValue<Slider>().Value && hero.Distance(Player.ServerPosition) <= _lotis.Range && _lotis.IsReady())
                        _lotis.Cast();
                }
            }

        }

        public static void setSkillShots()
        {
            Q.SetSkillshot(0.4f, 60f, 1800f, true, SkillshotType.SkillshotLine);
            E.SetSkillshot(0.4f, 350f, 0f, false, SkillshotType.SkillshotCircle);
        }

        private static bool CheckingCollision(Obj_AI_Hero target)
        {
            foreach (var col in MinionManager.GetMinions(Player.Position, 1500, MinionTypes.All, MinionTeam.NotAlly))
            {
                var Segment = Geometry.ProjectOn(col.ServerPosition.To2D(), Player.ServerPosition.To2D(),
                    col.Position.To2D());
                if (Segment.IsOnSegment &&
                    target.ServerPosition.To2D().Distance(Segment.SegmentPoint) <= GetHitBox(col) + 70)
                {
                    if (col.IsValidTarget(SData.SData.CastRange[0]) &&
                        col.Health < Player.GetSummonerSpellDamage(col, Damage.SummonerSpell.Smite))
                    {
                        Player.Spellbook.CastSpell(SData.Slot, col);
                        return true;
                    }
                }
            }
            return false;
        }

        static float GetHitBox(Obj_AI_Base minion)
        {
            var nameMinion = minion.Name.ToLower();
            if (nameMinion.Contains("mech")) return 65;
            if (nameMinion.Contains("wizard") || nameMinion.Contains("basic")) return 48;
            if (nameMinion.Contains("wolf") || nameMinion.Contains("wraith")) return 50;
            if (nameMinion.Contains("golem") || nameMinion.Contains("lizard")) return 80;
            if (nameMinion.Contains("dragon") || nameMinion.Contains("worm")) return 100;
            return 50;
        }

        public static void doHarass()
        {
            var SReady = (SData != null && SData.Slot != SpellSlot.Unknown && SData.State == SpellState.Ready);
            if (LockedTarget == null) return;
            var jumpObj = ObjectManager.Get<Obj_AI_Base>().Where(i => !i.IsMe && !(i is Obj_AI_Turret) && i.Distance(Player.ServerPosition) <= W.Range).OrderBy(i => i.Distance(ObjectManager.Get<Obj_AI_Turret>().Where(a => a.IsAlly).OrderBy(a => a.Distance(Player.ServerPosition)).First().ServerPosition)).FirstOrDefault();
            if (Q.IsReady())
            {
                if (Qdata.Name == "BlindMonkQOne")
                {
                    if (LeeSinSharp.Config.Item("smite").GetValue<bool>() && SReady &&
                        Q.GetPrediction(LockedTarget).CollisionObjects.Count == 1)
                    {
                        CheckingCollision(LockedTarget);
                        Q.Cast(LockedTarget, Packets());
                    }
                    else if (Q.GetPrediction(LockedTarget).Hitchance >= Qchange())
                    {
                        Q.Cast(LockedTarget, Packets());
                    }

                }
                else if ((LockedTarget.HasBuff("BlindMonkQOne", true) || LockedTarget.HasBuff("blindmonkqonechaos", true)) && LockedTarget.IsValidTarget(1300) && W.IsReady() && Wdata.Name == "BlindMonkWOne" && Player.Mana >= LeeSinSharp.Config.Item("manahara").GetValue<Slider>().Value && (Player.Health * 100 / Player.MaxHealth) >= LeeSinSharp.Config.Item("harrMode").GetValue<Slider>().Value)
                {
                    if (jumpObj != null) Q.Cast();
                }
            }
            if (!Q.IsReady() && LeeSinSharp.Config.Item("useHarrE").GetValue<bool>() && E.IsReady() &&
                Edata.Name == "BlindMonkEOne" && LockedTarget.IsValidTarget(E.Range))
            {
                E.Cast();

            }
            if (!Q.IsReady() && W.IsReady() && Wdata.Name == "BlindMonkWOne" && ((LeeSinSharp.Config.Item("useHarrE").GetValue<bool>() && LockedTarget.HasBuff("BlindMonkEOne", true)) || (!LeeSinSharp.Config.Item("useHarrE").GetValue<bool>() && Utility.CountEnemysInRange(200) >= 1)))
            {
                if ((Environment.TickCount - lastTimeJump) > 200)
                {
                    W.Cast(jumpObj, Packets());
                    lastTimeJump = Environment.TickCount;
                }
            }
        }
        /* public static void doHarass()
         {
             if (LockedTarget == null)
                 return;

             if (!castQFirstSmart())
                 if (!castQSecondSmart())
                     if (!castEFirst())
                         getBackHarass();
         }*/

        public static void LaneJungClear()
        {
            var minionObj =
                MinionManager.GetMinions(Player.Position, Q.Range, MinionTypes.All, MinionTeam.NotAlly)
                    .OrderBy(i => i.Distance(Player))
                    .FirstOrDefault();
            if (minionObj == null) return;
            var mymana = Player.Mana >=
                         (Player.MaxMana * (LeeSinSharp.Config.Item("checkmana").GetValue<Slider>().Value) / 100);
            var passive = ObjectManager.Player.HasBuff("blindmonkpassive_cosmetic");
            if (LeeSinSharp.Config.Item("useClearW").GetValue<bool>() && W.IsReady() && mymana &&
                minionObj.IsValidTarget(Orbwalking.GetRealAutoAttackRange(minionObj)))
            {
                if (Wdata.Name == "BlindMonkWOne" && !passive)
                {
                    W.Cast(Player, Packets());
                }
                else if ((Environment.TickCount - W.LastCastAttemptT) > 2500 ||
                         !Player.HasBuff("blindmonkwoneshield") && !passive)
                    W.Cast();
            }
            if (LeeSinSharp.Config.Item("useClearQ").GetValue<bool>() && Q.IsReady() && mymana)
            {
                if (Qdata.Name == "BlindMonkQOne" && !passive)
                {
                    Q.Cast(minionObj, Packets());
                }
                else if ((minionObj.HasBuff("BlindMonkQOne", true) || minionObj.HasBuff("blindmonkqonechaos", true)) &&
                         (minionObj.Health < Q.GetDamage(minionObj, 1) || !passive ||
                          (Environment.TickCount - Q.LastCastAttemptT) > 2500 || Player.Distance(minionObj) > 500))

                    Q.Cast();
            }
            if (LeeSinSharp.Config.Item("useClearE").GetValue<bool>() && E.IsReady() && minionObj.IsValidTarget(E.Range) && mymana)
            {
                if (Edata.Name == "BlindMonkEOne" && !passive)
                {
                    E.Cast();
                }
                else if (minionObj.HasBuff("BlindMonkEOne", true) &&
                         ((Environment.TickCount - E.LastCastAttemptT) > 2500 || Player.Distance(minionObj) > 400) && !passive)

                    E.Cast();
            }
            if (LeeSinSharp.Config.Item("useClearI").GetValue<bool>() && Player.Distance(minionObj) <= 350 &&
                _Tiamat.IsReady())
            {
                _Tiamat.Cast();
            }
            if (LeeSinSharp.Config.Item("useClearI").GetValue<bool>() && Player.Distance(minionObj) <= 350 &&
               _Hydra.IsReady())
            {
                _Hydra.Cast();
            }
        }
        private static int GetBuffStacks()
        {
            if (Player.HasBuff("blindmonkpassive_cosmetic"))
            {
                return Player.Buffs
                    .Where(x => x.DisplayName == "blindmonkpassive_cosmetic")
                    .Select(x => x.Count)
                    .First();
            }
            else
            {
                return 0;
            }
        }

        public static void combo()
        {
            if (LockedTarget == null) return;
            var passive = LeeSinSharp.Config.Item("passive").GetValue<Slider>().Value;
            if (Player.Distance(LockedTarget) > 450 && Q.IsReady() &&
                GetBuffStacks() < passive)
            {
                castQFirstSmart();
            }
            if (Player.Distance(LockedTarget) <= E.Range && E.IsReady() && GetBuffStacks() < passive)
            {
                castEFirst();
            }
            else if (Q.IsReady() && Player.Distance(LockedTarget) <= Q.Range && GetBuffStacks() < passive)
            {
                castQFirstSmart();
            }
            // If QLanded, if E Range, will try to E. If not, just Q. If far, 2ndQ too.
            if (Q.IsReady() && LockedTarget.HasBuff("BlindMonkEOne", true) && Player.Distance(LockedTarget) <= Q.Range)
            {
                if (Player.Distance(LockedTarget) <= E.Range && GetBuffStacks() < passive && E.IsReady())
                {
                    castEFirst();
                }

                else if (GetBuffStacks() < passive && Player.Distance(LockedTarget) <= E.Range)
                {
                    castQSecondSmart();
                }

                else if (Player.Distance(LockedTarget) <= Q.Range)
                {
                    castQSecondSmart();
                }
            }
            if (LeeSinSharp.Config.Item("UseECombo").GetValue<bool>() && E.IsReady() && LockedTarget.IsValidTarget(500) && LockedTarget.HasBuff("BlindMonkEOne", true))
            {
                if (Player.Distance(LockedTarget) > 400 || (Environment.TickCount - E.LastCastAttemptT) > 1800)
                    E.Cast();
            }
            if (LeeSinSharp.Config.Item("UseRCombo").GetValue<bool>() && R.IsReady() && LockedTarget.IsValidTarget(R.Range) && (!LockedTarget.HasBuff("JudicatorIntervention") || !LockedTarget.HasBuff("UndyingRage")))
            {
                if (LockedTarget.Health < R.GetDamage(LockedTarget) || (LockedTarget.Health < GetQ2Dmg(LockedTarget, R.GetDamage(LockedTarget)) && Q.IsReady() && (LockedTarget.HasBuff("BlindMonkQOne", true) || LockedTarget.HasBuff("blindmonkqonechaos", true)) && Player.Mana >= 50))
                    R.Cast(LockedTarget, Packets());
            }
            if (LeeSinSharp.Config.Item("UseWCombo").GetValue<bool>() && W.IsReady() && LockedTarget.IsValidTarget(E.Range) && (Player.Health * 100 / Player.MaxHealth) <= LeeSinSharp.Config.Item("autowusage").GetValue<Slider>().Value)
            {
                if (Wdata.Name == "BlindMonkWOne")
                {
                    W.Cast(Player, Packets());
                }
                else if (!Player.HasBuff("blindmonkwoneshield", true)) W.Cast();
            }
            UseItemes(LockedTarget);
        }

        /*
        public static void combo()
        {
            var SReady = (SData != null && SData.Slot != SpellSlot.Unknown && SData.State == SpellState.Ready);
            if (LockedTarget == null) return;
            if (LeeSinSharp.Config.Item("UseECombo").GetValue<bool>() && E.IsReady() && Edata.Name == "BlindMonkEOne" && LockedTarget.IsValidTarget(E.Range))
                E.Cast();
            if (LeeSinSharp.Config.Item("UseQCombo").GetValue<bool>() && Q.IsReady() && Qdata.Name == "BlindMonkQOne")
            {
                if (SReady && LeeSinSharp.Config.Item("smite").GetValue<bool>() &&
                    Q.GetPrediction(LockedTarget).CollisionObjects.Count == 1)
                {
                    CheckingCollision(LockedTarget);
                    Q.Cast(LockedTarget, Packets());
                }
                else if (Q.GetPrediction(LockedTarget).Hitchance >= Qchange())
                {
                    Q.Cast(LockedTarget, Packets());
                }
            }

            if (LeeSinSharp.Config.Item("UseQCombo").GetValue<bool>() && Q.IsReady() && LockedTarget.IsValidTarget(1300) && (LockedTarget.HasBuff("BlindMonkQOne", true) || LockedTarget.HasBuff("blindmonkqonechaos", true)))
            {
                if (Player.Distance(LockedTarget) < Q.Range || LockedTarget.Health < Q.GetDamage(LockedTarget, 1) || (LockedTarget.HasBuff("BlindMonkEOne", true) && LockedTarget.IsValidTarget(E.Range)) || (Environment.TickCount - Q.LastCastAttemptT) > 1800)
                    Q.Cast();
            }
            if (LeeSinSharp.Config.Item("UseECombo").GetValue<bool>() && E.IsReady() && LockedTarget.IsValidTarget(500) && LockedTarget.HasBuff("BlindMonkEOne", true))
            {
                if (Player.Distance(LockedTarget) > 400 || (Environment.TickCount - E.LastCastAttemptT) > 1800)
                    E.Cast();
            }
            if (LeeSinSharp.Config.Item("UseRCombo").GetValue<bool>() && R.IsReady() && LockedTarget.IsValidTarget(R.Range) && (!LockedTarget.HasBuff("JudicatorIntervention") || !LockedTarget.HasBuff("UndyingRage")))
            {
                if (LockedTarget.Health < R.GetDamage(LockedTarget) || (LockedTarget.Health < GetQ2Dmg(LockedTarget, R.GetDamage(LockedTarget)) && Q.IsReady() && (LockedTarget.HasBuff("BlindMonkQOne", true) || LockedTarget.HasBuff("blindmonkqonechaos", true)) && Player.Mana >= 50))
                    R.Cast(LockedTarget, Packets());
            }
            if (LeeSinSharp.Config.Item("UseWCombo").GetValue<bool>() && W.IsReady() && LockedTarget.IsValidTarget(E.Range) && (Player.Health * 100 / Player.MaxHealth) <= LeeSinSharp.Config.Item("autowusage").GetValue<Slider>().Value)
            {
                if (Wdata.Name == "BlindMonkWOne")
                {
                    W.Cast(Player, Packets());
                }
                else if (!Player.HasBuff("blindmonkwoneshield", true)) W.Cast();
            }
            UseItemes(LockedTarget);
        }*/

            private static
            HitChance Qchange()
        {
            switch (LeeSinSharp.Config.Item("Qchange").GetValue<StringList>().SelectedIndex)
            {
                case 0:
                    return HitChance.Low;
                case 1:
                    return HitChance.Medium;
                case 2:
                    return HitChance.High;
                case 3:
                    return HitChance.VeryHigh;
                default:
                    return HitChance.Medium;
            }
        }
        private static bool Packets()
        {
            return LeeSinSharp.Config.Item("usePackets").GetValue<bool>();
        }
        static double GetQ2Dmg(Obj_AI_Base target, double dmgPlus)
        {
            Int32[] dmgQ = { 50, 80, 110, 140, 170 };
            return Player.CalcDamage(target, Damage.DamageType.Physical, dmgQ[Q.Level - 1] + 0.9 * Player.FlatPhysicalDamageMod + 0.08 * (target.MaxHealth - (target.Health - dmgPlus)));
        }

        /* public static void combo2()
         {
             if (inDistance(LockedTarget.Position.To2D(), Player.ServerPosition.To2D(), 375))
             {
                 castQFirstSmart();
                 castEFirst();
                 if (targetHasQ(LockedTarget))
                     R.Cast(LockedTarget);
                 if (!R.IsReady())
                     castQSecondSmart();
                 castE2();
             }
             UseItemes(LockedTarget);
         }*/

        public static void combo2()
        {
            var SReady = (SData != null && SData.Slot != SpellSlot.Unknown && SData.State == SpellState.Ready);
            if (LockedTarget == null) return;
            if (LeeSinSharp.Config.Item("UseECombo").GetValue<bool>() && E.IsReady() &&
                Edata.Name == "BlindMonkEOne" && LockedTarget.IsValidTarget(E.Range))
            {
                E.Cast();

            }
            if (LeeSinSharp.Config.Item("UseQCombo").GetValue<bool>() && Q.IsReady() && Qdata.Name == "BlindMonkQOne")
            {
                if (SReady && LeeSinSharp.Config.Item("smite").GetValue<bool>() &&
                    Q.GetPrediction(LockedTarget).CollisionObjects.Count == 1)
                {
                    CheckingCollision(LockedTarget);
                    Q.Cast(LockedTarget, Packets());
                }
                else if (Q.GetPrediction(LockedTarget).Hitchance >= Qchange())
                {
                    Q.Cast(LockedTarget, Packets());
                }
            }

            if (R.IsReady() && targetHasQ(LockedTarget))
            {
                if (Player.Distance(LockedTarget) < E.Range)
                {
                    if (LeeSinSharp.Config.Item("UseECombo").GetValue<bool>() && E.IsReady() &&
                        Edata.Name == "BlindMonkEOne" && LockedTarget.IsValidTarget(E.Range))
                    {
                        E.Cast();

                    }
                    if (LeeSinSharp.Config.Item("UseRCombo").GetValue<bool>())
                    {
                        R.Cast(LockedTarget);

                    }
                }
                else if (Player.Distance(LockedTarget) > E.Range)
                {
                   wardJump(LockedTarget.Position.To2D());
                    if (LeeSinSharp.Config.Item("UseECombo").GetValue<bool>() && E.IsReady() &&
                        Edata.Name == "BlindMonkEOne" && LockedTarget.IsValidTarget(E.Range))
                    {
                        E.Cast();

                    }
                    if (LeeSinSharp.Config.Item("UseRCombo").GetValue<bool>())
                    {
                        R.Cast(LockedTarget);

                    }
                }
            }

            if (!R.IsReady())
            {
                if (LeeSinSharp.Config.Item("UseQCombo").GetValue<bool>() && Q.IsReady() &&
                    LockedTarget.IsValidTarget(1300) &&
                    (LockedTarget.HasBuff("BlindMonkQOne", true) || LockedTarget.HasBuff("blindmonkqonechaos", true)))
                {
                    if (Player.Distance(LockedTarget) < Q.Range || LockedTarget.Health < Q.GetDamage(LockedTarget, 1) ||
                        (LockedTarget.HasBuff("BlindMonkEOne", true) && LockedTarget.IsValidTarget(E.Range)) ||
                        (Environment.TickCount - Q.LastCastAttemptT) > 1800)
                        Q.Cast();
                }
                if (LeeSinSharp.Config.Item("UseECombo").GetValue<bool>() && E.IsReady() &&
                    LockedTarget.IsValidTarget(500) && LockedTarget.HasBuff("BlindMonkEOne", true))
                {
                    if (Player.Distance(LockedTarget) > 400 || (Environment.TickCount - E.LastCastAttemptT) > 1800)
                        E.Cast();
                }
            }
            UseItemes(LockedTarget);
        }

        public static void useinsec()
        {
            foreach (Obj_AI_Hero hero in ObjectManager.Get<Obj_AI_Hero>())
            {
                if (hero.IsAlly && !hero.IsMe && hero != null && hero.Distance(Player) < 1500)
                {
                    insec1();
                }
                else
                {
                    insec();
                }
            }

        }
        public static bool loaidraw()
        {
            foreach (Obj_AI_Hero hero1 in ObjectManager.Get<Obj_AI_Hero>())
            {
                if (hero1.IsAlly && !hero1.IsMe && hero1 != null && hero1.Distance(Player) < 1500)
                    return true;
            }
            return false;
        }

        public static void insec()
        {
            if (!R.IsReady())
            {
                da = false;
                return;
            }
            try
            {
                if (da && !W.IsReady())
                {
                    R.Cast(LockedTarget);
                }
                if (Player.Distance(getward(LockedTarget)) > 600 && W.IsReady())
                {
                    castQFirstSmart();
                    castQSecondSmart();
                }
                if (Player.Distance(getward(LockedTarget)) <= 600 && W.IsReady())
                {
                    wardJump(getward(LockedTarget).To2D());
                    da = true;
                }
            }
            catch
            {

            }
        }
        public static void insec1()
        {
            if (!R.IsReady())
            {
                da = false;
                return;
            }
            try
            {
                if (da && !W.IsReady())
                {
                    R.Cast(LockedTarget);
                }
                if (Player.Distance(getward2(LockedTarget)) > 600 && W.IsReady())
                {
                    castQFirstSmart();
                    castQSecondSmart();
                }
                if (Player.Distance(getward2(LockedTarget)) < 600 && W.IsReady())
                {
                    wardJump(getward2(LockedTarget).To2D());
                    da = true;

                }
            }
            catch
            {

            }
        }

        public static bool getBackHarass()
        {
            Obj_AI_Turret closest_tower = ObjectManager.Get<Obj_AI_Turret>().Where(tur => tur.IsAlly).OrderBy(tur => tur.Distance(Player.ServerPosition)).First();
            Obj_AI_Base jumpOn = ObjectManager.Get<Obj_AI_Base>().Where(ally => ally.IsAlly && !(ally is Obj_AI_Turret) && !ally.IsMe && ally.Distance(LeeSin.Player.ServerPosition) < 700).OrderBy(tur => tur.Distance(closest_tower.ServerPosition)).First();
            W.Cast(jumpOn);
            // wardJump(closest_tower.Position.To2D());
            return false;
        }

        public static bool targetHasQ(Obj_AI_Hero target)
        {
            foreach (BuffInstance buf in target.Buffs)
            {
                if (buf.Name == "BlindMonkQOne" || buf.Name == "blindmonkqonechaos")
                    return true;
            }
            return false;
            /*if(target.HasBuff("blindmonkpassive_cosmetic") 
                || (target.HasBuff("BlindMonkQOne") && (target.Buffs.ToList().Find(buf => buf.Name == "BlindMonkQOne").EndTime-Game.Time)>=0.3))
                return true;
            return false;*/
        }
        public static bool targetHasUlti(Obj_AI_Hero target)
        {
            foreach (BuffInstance buf in target.Buffs)
            {
                if (buf.Name == "JudicatorIntervention" || buf.Name == "UndyingRage")
                    return false;
            }
            return true;
            /*if(target.HasBuff("blindmonkpassive_cosmetic") 
                || (target.HasBuff("BlindMonkQOne") && (target.Buffs.ToList().Find(buf => buf.Name == "BlindMonkQOne").EndTime-Game.Time)>=0.3))
                return true;
            return false;*/
        }

        public static bool castQFirstSmart()
        {
            var SReady = (SData != null && SData.Slot != SpellSlot.Unknown && SData.State == SpellState.Ready);
            if (!Q.IsReady() || Qdata.Name != "BlindMonkQOne" || LockedTarget == null) return false;
            if (LeeSinSharp.Config.Item("UseQCombo").GetValue<bool>() && Q.IsReady())
            {
                if (LeeSinSharp.Config.Item("smite").GetValue<bool>() && SReady &&
                    Q.GetPrediction(LockedTarget).CollisionObjects.Count == 1)
                {
                    CheckingCollision(LockedTarget);
                    Q.Cast(LockedTarget, Packets());
                }

                else if (Q.GetPrediction(LockedTarget).Hitchance >= Qchange())
                    Q.Cast(LockedTarget, Packets());
            }
            return true;
        }

        public static bool castQSecondSmart()
        {
            if (Qdata.Name != "blindmonkqtwo" || LockedTarget == null)
                return false;
            if (targetHasQ(LockedTarget) && inDistance(LockedTarget.Position.To2D(), Player.ServerPosition.To2D(), 1200))
            {
                Q.Cast();
                return true;
            }
            return true;
        }

        public static bool castEFirst()
        {
            if (!E.IsReady() || LockedTarget == null || Edata.Name != "BlindMonkEOne")
                return false;
            if (inDistance(LockedTarget.Position.To2D(), Player.ServerPosition.To2D(), E.Range))
            {
                E.Cast();
                return true;
            }
            return true;
        }
        public static bool castE2()
        {
            if (LockedTarget == null) return false;
            if (inDistance(LockedTarget.Position.To2D(), Player.ServerPosition.To2D(), 350))
            {
                E.Cast();
                return true;
            }
            return true;
        }

        public static void Killsteal()
        {
            var target = TargetSelector.GetTarget(Q.Range + 200, TargetSelector.DamageType.Magical);
            var qhDmg = Player.GetSpellDamage(target, SpellSlot.Q);
            var ehDmg = Player.GetSpellDamage(target, SpellSlot.E);

            if (Q.IsReady() && Player.Distance(target) <= Q.Range && target != null &&
                LeeSinSharp.Config.Item("UseQKs").GetValue<bool>())
            {
                if (target.Health <= qhDmg && Q.GetPrediction(LockedTarget).Hitchance >= Qchange() &&
                    (Qdata.Name == "BlindMonkQOne"))
                {
                    Q.Cast(LockedTarget, Packets());

                }
                else if (target.Health <= qhDmg && (LockedTarget.HasBuff("BlindMonkQOne", true) || LockedTarget.HasBuff("blindmonkqonechaos", true)))
                {
                    Q.Cast();
                }
            }

            if (E.IsReady() && Player.Distance(target) <= E.Range && target != null &&
                LeeSinSharp.Config.Item("UseEKs").GetValue<bool>())
            {
                if (target.Health <= ehDmg && Edata.Name == "BlindMonkEOne")
                {
                    E.Cast();
                }
                else if (LockedTarget.HasBuff("BlindMonkEOne", true) && target.Health <= ehDmg)
                {
                    E.Cast();
                }
            }

            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.IsValidTarget(R.Range) && R.GetDamage(hero) >= hero.Health))
            {
                if (targetHasUlti(LockedTarget) && LeeSinSharp.Config.Item("UseRKs").GetValue<bool>())
                    R.Cast(enemy, Packets());
                return;
            }
        }

        public static int getJumpWardId()
        {
            int[] wardIds = { 3340, 3350, 3205, 3207, 2049, 2045, 2044, 3361, 3154, 3362, 3160, 2043 };
            foreach (int id in wardIds)
            {
                if (Items.HasItem(id) && Items.CanUseItem(id))
                    return id;
            }
            return -1;
        }

        public static void moveTo(Vector2 Pos)
        {
            Player.IssueOrder(GameObjectOrder.MoveTo, Pos.To3D());
        }

        public static void wardJump(Vector2 pos)
        {
            Vector2 posStart = pos;
            if (!W.IsReady())
                return;
            bool wardIs = false;
            if (!inDistance(pos, Player.ServerPosition.To2D(), W.Range + 15))
            {
                pos = Player.ServerPosition.To2D() + Vector2.Normalize(pos - Player.ServerPosition.To2D()) * 600;
            }

            if (!W.IsReady() && W.ChargedSpellName == "")
                return;
            foreach (Obj_AI_Base ally in ObjectManager.Get<Obj_AI_Base>().Where(ally => ally.IsAlly
                && !(ally is Obj_AI_Turret) && inDistance(pos, ally.ServerPosition.To2D(), 200)))
            {
                wardIs = true;
                moveTo(pos);
                if (inDistance(Player.ServerPosition.To2D(), ally.ServerPosition.To2D(), W.Range + ally.BoundingRadius))
                {
                    W.Cast(ally);

                }
                return;
            }
            Polygon pol;
            if ((pol = LeeSinSharp.map.getInWhichPolygon(pos)) != null)
            {
                if (inDistance(pol.getProjOnPolygon(pos), Player.ServerPosition.To2D(), W.Range + 15) && !wardIs && inDistance(pol.getProjOnPolygon(pos), pos, 200))
                {
                    if (lastwardjump < Environment.TickCount)
                    {
                        putWard(pos);
                        lastwardjump = Environment.TickCount + 1000;
                    }
                }
            }
            else if (!wardIs)
            {
                if (lastwardjump < Environment.TickCount)
                {
                    putWard(pos);
                    lastwardjump = Environment.TickCount + 1000;
                }
            }

        }

        public static bool putWard(Vector2 pos)
        {
            int wardItem;
            if ((wardItem = getJumpWardId()) != -1)
            {
                foreach (var slot in Player.InventoryItems.Where(slot => slot.Id == (ItemId)wardItem))
                {
                    ObjectManager.Player.Spellbook.CastSpell(slot.SpellSlot, pos.To3D());
                   // slot.UseItem(pos.To3D());
                    return true;
                }
            }
            return false;
        }


        public static bool inDistance(Vector2 pos1, Vector2 pos2, float distance)
        {
            float dist2 = Vector2.DistanceSquared(pos1, pos2);
            return (dist2 <= distance * distance) ? true : false;
        }
        public static Vector3 getward(Obj_AI_Hero target)
        {
            Obj_AI_Turret turret = ObjectManager.Get<Obj_AI_Turret>().Where(tur => tur.IsAlly && tur.Health > 0 && !tur.IsMe).OrderBy(tur => tur.Distance(Player.ServerPosition)).First();
            return target.ServerPosition + Vector3.Normalize(turret.ServerPosition - target.ServerPosition) * (-300);
        }
        public static Vector3 getward2(Obj_AI_Hero target)
        {
            Obj_AI_Hero hero = ObjectManager.Get<Obj_AI_Hero>().Where(tur => tur.IsAlly && tur.Health > 0 && !tur.IsMe).OrderBy(tur => tur.Distance(Player.ServerPosition)).First();
            return target.ServerPosition + Vector3.Normalize(hero.ServerPosition - target.ServerPosition) * (-300);
        }
        public static Vector3 getward1(Obj_AI_Hero target)
        {
            Obj_AI_Turret turret = ObjectManager.Get<Obj_AI_Turret>().Where(tur => tur.IsAlly && tur.Health > 0 && !tur.IsMe).OrderBy(tur => tur.Distance(Player.ServerPosition)).First();
            return target.Position + Vector3.Normalize(turret.Position - target.Position) * (600);
        }
        public static Vector3 getward3(Obj_AI_Hero target)
        {
            Obj_AI_Hero hero = ObjectManager.Get<Obj_AI_Hero>().Where(tur => tur.IsAlly && tur.Health > 0 && !tur.IsMe).OrderBy(tur => tur.Distance(Player.ServerPosition)).First();
            return target.Position + Vector3.Normalize(hero.Position - target.Position) * (600);
        }

    }
}
