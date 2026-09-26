using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley.GameData;
using System.Collections.Generic;

namespace DelayDazzle.AudioStreamOptimizer
{
    public class ModEntry : Mod
    {
        public override void Entry(IModHelper helper)
        {
            // 在游戏启动完成时执行音频修改
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            // 1. 加载 Data/AudioChanges 资产
            var audioChanges = Helper.GameContent.Load<Dictionary<string, AudioCueData>>("Data/AudioChanges");

            // 2. 需要改为流式加载的音频 ID 列表
            var cueIdsToStream = new List<string>
            {
                // ========== Stardew Valley Expanded (SVE) ==========
                // 模组：SVE，用途：SVE新增的原创音乐，用于特定角色事件或过场
                "FlashShifter.StardewValleyExpandedCP_Lament",
                "FlashShifter.StardewValleyExpandedCP_HaruhiViolinSolo",
                "FlashShifter.StardewValleyExpandedCP_ZCCC_Crowd_Ambience",
                "FlashShifter.StardewValleyExpandedCP_SDVMagicalShoes",
                "FlashShifter.StardewValleyExpandedCP_FirstSlashTheme",
                "FlashShifter.StardewValleyExpandedCP_StardewValleyExpandedTrailer",
                "FlashShifter.StardewValleyExpandedCP_Taxi",

                // ========== Ridgeside Village (RSV) ==========
                // 模组：RSV，用途：与音乐会、舞蹈等节日或事件相关的音乐
                "AlissaConcert",        // RSV - Alissa 音乐会事件
                "YsaDance",             // RSV - Ysabelle 舞蹈事件
                "JunePiano",            // RSV - June 钢琴演奏事件
                "HypeUpShutUp",         // RSV - 事件/过场音乐
                "EmberNight",           // RSV - 事件/过场音乐
                "AweAmbience",          // RSV - 事件/环境氛围音乐
                "Satiety",              // RSV - 事件/角色主题曲
                "BeforeAcceptance",     // RSV - 角色主题曲/事件
                "FallInRidgeside",      // RSV - 角色主题曲/事件
                "InTuneWithYou",        // RSV - 角色主题曲/事件
                "Saliency",             // RSV - 角色主题曲/事件
                "SelfWhispers",         // RSV - 角色主题曲/事件
                "SincereSteps",         // RSV - 角色主题曲/事件
                "SmilingTears",         // RSV - 角色主题曲/事件
                "SubtleGestures",       // RSV - 角色主题曲/事件
                "SweetNightingale",     // RSV - Alissa 角色主题曲
                "Tinkering",            // RSV - 角色主题曲/事件
                "TreetopDawn",          // RSV - Corine 角色主题曲
                "WinterMountain",       // RSV - 冬季事件/角色主题曲
                "Companion",            // RSV - Anton & Paula 角色主题曲
                "Compathy",             // RSV - Paula 角色主题曲
                "Compunctious",         // RSV - Anton 角色主题曲
                "CuriousHeart",         // RSV - Flor 角色主题曲
                "Daydreaming",          // RSV - 角色主题曲/事件
                "Delighting",           // RSV - 角色主题曲/事件
                "EmbraceAfterDusk",     // RSV - Daia 角色主题曲
                "Fidget",               // RSV - Sean 角色主题曲
                "GrownGuppy",           // RSV - Blair 角色主题曲
                "MailMeBackHome",       // RSV - Ian 角色主题曲
                "Playdate",             // RSV - Jeric 角色主题曲
                "Poise",                // RSV - Ysabelle 角色主题曲
                "SubtlyComfy"           // RSV - Philip 角色主题曲
            };

            // 3. 遍历并修改
            foreach (var cueId in cueIdsToStream)
            {
                if (audioChanges.TryGetValue(cueId, out AudioCueData cueData))
                {
                    // 将 StreamedVorbis 属性设置为 true，即改为流式加载
                    cueData.StreamedVorbis = true;
                }
            }

            // 4. 保存修改后的资产（刷新缓存）
            Helper.GameContent.InvalidateCache("Data/AudioChanges");
        }
    }
}
