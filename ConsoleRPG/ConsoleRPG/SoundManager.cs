using System;
using System.Collections.Generic;
using System.Media;

namespace ConsoleRPG
{
    internal static class SoundManager
    {
        static Dictionary<SoundType, SoundPlayer> mySounds;

        static SoundPlayer myBatHitPlayer;
        static SoundPlayer myPlayerHitPlayer;
        static SoundPlayer myBattleStartPlayer;
        static SoundPlayer myHealEffectPlayer;
        static SoundPlayer myBatLightningHitPlayer;
        static SoundPlayer myBattleVictoryPlayer;
        static SoundPlayer myMansionAmbiencePlayer;
        static SoundPlayer myVillageAmbiencePlayer;
        static SoundPlayer myPortalCastPlayer;
        static SoundPlayer myOpenChestPlayer;
        static SoundPlayer myGetKeyPlayer;
        static SoundPlayer myBattleLostPlayer;

        public static void LoadSounds()
        {
            if (OperatingSystem.IsWindows())
            {
                mySounds = new Dictionary<SoundType, SoundPlayer>();

                myMansionAmbiencePlayer = new SoundPlayer(@"Audio\mansionAmbience.wav");
                mySounds.Add(SoundType.MansionAmbience, myMansionAmbiencePlayer);

                myVillageAmbiencePlayer = new SoundPlayer(@"Audio\villageAmbience.wav");
                mySounds.Add(SoundType.VillageAmbience, myVillageAmbiencePlayer);

                myBatHitPlayer = new SoundPlayer(@"Audio\batHit.wav");
                mySounds.Add(SoundType.EnemyHurt, myBatHitPlayer);
                
                myPlayerHitPlayer = new SoundPlayer(@"Audio\playerHit.wav");
                mySounds.Add(SoundType.PlayerHurt, myPlayerHitPlayer);
                
                myBattleStartPlayer = new SoundPlayer(@"Audio\battleStart.wav");
                mySounds.Add(SoundType.BattleStart, myBattleStartPlayer);
                
                myHealEffectPlayer = new SoundPlayer(@"Audio\healEffect.wav");
                mySounds.Add(SoundType.Heal, myHealEffectPlayer);
                
                myBatLightningHitPlayer = new SoundPlayer(@"Audio\batLightningHit.wav");
                mySounds.Add(SoundType.LighningHurt, myBatLightningHitPlayer);
                
                myBattleVictoryPlayer = new SoundPlayer(@"Audio\battleVictory.wav");
                mySounds.Add(SoundType.BattleVictory, myBattleVictoryPlayer);

                myPortalCastPlayer = new SoundPlayer(@"Audio\portalEffect.wav");
                mySounds.Add(SoundType.PortalCast, myPortalCastPlayer);

                myOpenChestPlayer = new SoundPlayer(@"Audio\chestOpenEffect.wav");
                mySounds.Add(SoundType.OpenChest, myOpenChestPlayer);

                myGetKeyPlayer = new SoundPlayer(@"Audio\recieveKeyEffect.wav");
                mySounds.Add(SoundType.GetKey, myGetKeyPlayer);

                myBattleLostPlayer = new SoundPlayer(@"Audio\battleLost.wav");
                mySounds.Add(SoundType.BattleLost, myBattleLostPlayer);
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
