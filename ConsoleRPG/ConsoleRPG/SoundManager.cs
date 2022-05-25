using System;
using System.Collections.Generic;
using System.Media;

namespace ConsoleRPG
{
    internal static class SoundManager
    {
        static Dictionary<SoundType, SoundPlayer> mySounds;

        public static void LoadSounds()
        {
            if (OperatingSystem.IsWindows())
            {
                mySounds = new Dictionary<SoundType, SoundPlayer>();

                mySounds.Add(SoundType.MansionAmbience, new SoundPlayer(@"Audio/mansionAmbience.wav"));

                mySounds.Add(SoundType.VillageAmbience, new SoundPlayer(@"Audio/villageAmbience.wav"));

                mySounds.Add(SoundType.EnemyHurt, new SoundPlayer(@"Audio/batHit.wav"));
                
                mySounds.Add(SoundType.PlayerHurt, new SoundPlayer(@"Audio/playerHit.wav"));
                
                mySounds.Add(SoundType.BattleStart, new SoundPlayer(@"Audio/battleStart.wav"));
                
                mySounds.Add(SoundType.Heal, new SoundPlayer(@"Audio/healEffect.wav"));
                
                mySounds.Add(SoundType.LighningHurt, new SoundPlayer(@"Audio/batLightningHit.wav"));
                
                mySounds.Add(SoundType.BattleVictory, new SoundPlayer(@"Audio/battleVictory.wav"));

                mySounds.Add(SoundType.PortalCast, new SoundPlayer(@"Audio/portalEffect.wav"));

                mySounds.Add(SoundType.OpenChest, new SoundPlayer(@"Audio/chestOpenEffect.wav"));

                mySounds.Add(SoundType.GetKey, new SoundPlayer(@"Audio/recieveKeyEffect.wav"));

                mySounds.Add(SoundType.BattleLost, new SoundPlayer(@"Audio/battleLost.wav"));
            }
        }
        public static void PlaySound(SoundType aSoundType, bool aPlayLooping = false, bool aPlaySynced = false)
        {
            if (OperatingSystem.IsWindows() && mySounds.ContainsKey(aSoundType))
            {
                if (aPlaySynced && !aPlayLooping)
                {
                    mySounds[aSoundType].PlaySync();
                }
                else if (aPlayLooping && !aPlaySynced)
                {
                    mySounds[aSoundType].PlayLooping();
                }
                else
                {
                    mySounds[aSoundType].Play();
                }
            }
        }
    }
}
