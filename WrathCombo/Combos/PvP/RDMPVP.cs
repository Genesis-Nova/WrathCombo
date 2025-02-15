﻿using WrathCombo.CustomComboNS;

namespace WrathCombo.Combos.PvP
{
    internal static class RDMPvP
    {
        public const byte JobID = 35;

        public const uint
            Verstone = 29683,
            EnchantedRiposte = 41488,
            Resolution = 41492,
            MagickBarrier = 29697,
            CorpsACorps = 29699,
            Displacement = 29700,
            EnchantedZwerchhau = 41489,
            EnchantedRedoublement = 41490,
            Frazzle = 29698,
            SouthernCross = 29704,
            Embolden = 41494,
            Forte = 41496,
            Jolt3 = 41486,
            ViceofThorns = 41493;

        public static class Buffs
        {
            public const ushort
                WhiteShift = 3245,
                BlackShift = 3246,
                Dualcast = 1393,
                EnchantedRiposte = 3234,
                EnchantedRedoublement = 3236,
                EnchantedZwerchhau = 3235,
                VermilionRadiance = 3233,
                MagickBarrier = 3240,
                Embolden = 2282,
                Forte = 4320,
                PrefulgenceReady = 4322,
                ThornedFlourish = 0;
        }

        public static class Debuffs
        {
            public const ushort
                Monomachy = 3242;
        }

        internal class RDMPvP_BurstMode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.RDMPvP_BurstMode;

            protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
            {
                if (actionID is Jolt3)
                {
                    if (IsOffCooldown(Forte) && CanWeave(actionID))
                        return Forte;

                    if (!PvPCommon.IsImmuneToDamage())
                    {
                        if (IsEnabled(CustomComboPreset.RDMPvP_Burst_Resolution) && !GetCooldown(Resolution).IsCooldown)
                            return OriginalHook(Resolution);

                        if (IsEnabled(CustomComboPreset.RDMPvP_Burst_CorpsACorps) && !InMeleeRange() && GetCooldown(CorpsACorps).RemainingCharges > 0 && !GetCooldown(EnchantedRiposte).IsCooldown)
                            return OriginalHook(CorpsACorps);

                        if (InMeleeRange())
                        {
                            if (IsEnabled(CustomComboPreset.RDMPvP_Burst_Embolden))
                            {
                                if (IsOffCooldown(Embolden) && (WasLastAbility(CorpsACorps) || TargetHasEffect(Debuffs.Monomachy)) || HasEffect(Buffs.PrefulgenceReady))
                                    return OriginalHook(Embolden);

                            }

                            if (IsEnabled(CustomComboPreset.RDMPvP_Burst_EnchantedRiposte))
                            {
                                if (!GetCooldown(EnchantedRiposte).IsCooldown || lastComboActionID == EnchantedRiposte || lastComboActionID == EnchantedZwerchhau || lastComboActionID == EnchantedRedoublement)
                                    return OriginalHook(EnchantedRiposte);
                            }


                            if (IsEnabled(CustomComboPreset.RDMPvP_Burst_Displacement) && lastComboActionID == EnchantedRedoublement && GetCooldown(Displacement).RemainingCharges > 0)
                                return OriginalHook(Displacement);
                        }

                        if (lastComboActionID == EnchantedRedoublement)
                            return OriginalHook(EnchantedRiposte);

                        if (HasEffect(Buffs.VermilionRadiance))
                            return OriginalHook(EnchantedRiposte);
                    }

                }

                return actionID;
            }
        }
    }
}
