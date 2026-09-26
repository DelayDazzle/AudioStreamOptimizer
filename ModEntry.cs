using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley.GameData;
using System;
using System.Collections.Generic;

namespace DelayDazzle.AudioStreamOptimizer
{
    public class ModEntry : Mod
    {
        private ModConfig Config;

        public override void Entry(IModHelper helper)
        {
            // 读取配置文件，如果不存在则创建默认配置
            Config = Helper.ReadConfig<ModConfig>();
            
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            // 1. 注册 GMCM 菜单（使用 i18n 翻译）
            var gmcm = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (gmcm != null)
            {
                gmcm.Register(ModManifest, () => Config = new ModConfig(), () => Helper.WriteConfig(Config));
                
                // 使用 Helper.Translation.Get() 读取翻译
                gmcm.AddSectionTitle(ModManifest, () => Helper.Translation.Get("gmcm.section.title"));
                gmcm.AddBoolOption(
                    ModManifest,
                    () => Config.EnableStreaming,
                    (value) => Config.EnableStreaming = value,
                    () => Helper.Translation.Get("gmcm.enable-streaming.name"),
                    () => Helper.Translation.Get("gmcm.enable-streaming.tooltip")
                );
            }

            // 2. 如果玩家关闭了流式加载，则直接跳过
            if (!Config.EnableStreaming)
            {
                Monitor.Log("流式加载已被玩家禁用，使用游戏默认音频设置。", LogLevel.Info);
                return;
            }

            // 3. 加载 Data/AudioChanges 资产
            var audioChanges = Helper.GameContent.Load<Dictionary<string, AudioCueData>>("Data/AudioChanges");

            // 4. 需要改为流式加载的音频 ID 列表
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
            
            // 5. 遍历并修改
            foreach (var cueId in cueIdsToStream)
            {
                if (audioChanges.TryGetValue(cueId, out AudioCueData cueData))
                {
                    cueData.StreamedVorbis = true;
                }
            }

            // 6. 刷新缓存
            Helper.GameContent.InvalidateCache("Data/AudioChanges");
        }
    }
}
