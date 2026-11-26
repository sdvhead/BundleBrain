#nullable enable
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewUI.Framework;
using StardewValley;
using GenericModConfigMenu;
using Microsoft.Xna.Framework;

namespace BundleBrain
{

    internal sealed class ModEntry : Mod
    {
        #nullable disable

        private const string SaveDataKey = "sdvhead_bundlebrain_data";
        private ModData Data = new();
        private IViewEngine viewEngine;
        #nullable enable
        internal ModConfig Config = null!;

        private IViewDrawable? overlay;
        private bool overlayIsDragging;

        private Vector2 overlayPosition;
        private Vector2 lastOverlayDragCursor;

        public override void Entry(IModHelper helper)
        {
            this.Config = helper.ReadConfig<ModConfig>();

            if (this.Config.CfgEnableBundleBrain)
            {
                helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
                helper.Events.GameLoop.GameLaunched += OnGameLaunched;
                helper.Events.GameLoop.DayStarted += OnDayStarted;
                helper.Events.Player.InventoryChanged += OnInventoryChanged;
                helper.Events.Input.ButtonPressed += OnButtonPressed;
                helper.Events.Display.RenderedHud += OnHudRendered;
                helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
                helper.Events.Input.ButtonReleased += OnButtonReleased;
                helper.Events.GameLoop.Saving += OnSaving;
            }

        }

        private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            SetupModConfigMenu();

            if (this.Config.CfgEnableBundleBrain)
            {
                overlayPosition = new Vector2(
                Config.CfgButtonOffsetX,
                Config.CfgButtonOffsetY
                );

#nullable disable
                viewEngine = Helper.ModRegistry.GetApi<IViewEngine>("focustense.StardewUI");
                viewEngine.RegisterViews("Mods/sdvhead.BundleBrain/Views", "assets/views");
                viewEngine.RegisterSprites($"Mods/sdvhead.BundleBrain/Sprites", "assets/sprites");
#if DEBUG
                viewEngine.EnableHotReloading();
#endif
                overlay = viewEngine.CreateDrawableFromAsset("Mods/sdvhead.BundleBrain/Views/BBUI");

#nullable enable
            }

        }

        private void SaveData()
        {
            this.Helper.Data.WriteSaveData(SaveDataKey, this.Data);
        }
        private void OnSaving(object? sender, SavingEventArgs e)
        {
            SaveData();
        }

        private void OnHudRendered(object? sender, RenderedHudEventArgs e)
        {
            overlay?.Draw(e.SpriteBatch, overlayPosition);
        }

        private void SetupModConfigMenu()
        {
            // get Generic Mod Config Menu's API (if it's installed)
            var configMenu = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu is null)
                return;

            // register mod
            configMenu.Register(
                mod: this.ModManifest,
                reset: () => this.Config = new ModConfig(),
                save: () => this.Helper.WriteConfig(this.Config)
            );

            //add config options
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => this.Helper.Translation.Get("cfgEnableBundleBrain.Name"),
                tooltip: () => this.Helper.Translation.Get("cfgEnableBundleBrain.Description"),
                getValue: () => this.Config.CfgEnableBundleBrain,
                setValue: value => this.Config.CfgEnableBundleBrain = value
                );

            configMenu.AddNumberOption(
                mod: this.ModManifest,
                name: () => this.Helper.Translation.Get("cfgButtonOffsetX.Name"),
                tooltip: () => this.Helper.Translation.Get("cfgButtonOffsetX.Description"),
                getValue: () => this.Config.CfgButtonOffsetX,
                setValue: value => this.Config.CfgButtonOffsetX = value,
                min: 0
            );

            configMenu.AddNumberOption(
                mod: this.ModManifest,
                name: () => this.Helper.Translation.Get("cfgButtonOffsetY.Name"),
                tooltip: () => this.Helper.Translation.Get("cfgButtonOffsetY.Description"),
                getValue: () => this.Config.CfgButtonOffsetY,
                setValue: value => this.Config.CfgButtonOffsetY = value,
                min: 0
            );

            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => this.Helper.Translation.Get("cfgButtonDraggable.Name"),
                tooltip: () => this.Helper.Translation.Get("cfgButtonDraggable.Description"),
                getValue: () => this.Config.CfgButtonDraggable,
                setValue: value => this.Config.CfgButtonDraggable = value
                );
        }

        private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
        {

            if (this.Config.CfgButtonDraggable && e.Button==SButton.MouseLeft)
            {
                Vector2 cursor = e.Cursor.GetScaledScreenPixels();
                if (DrawableContains(cursor))
                {
                    overlayIsDragging = true;
                    lastOverlayDragCursor = cursor;
                }
            }
        }

        private void OnUpdateTicked(object? sender, UpdateTickedEventArgs e)
        {
            if (this.Config.CfgButtonDraggable && overlayIsDragging)
            {
                Vector2 cursor = Helper.Input.GetCursorPosition().GetScaledScreenPixels();
                overlayPosition += cursor - lastOverlayDragCursor;
                lastOverlayDragCursor = cursor;

                Config.CfgButtonOffsetX = (int)overlayPosition.X;
                Config.CfgButtonOffsetY = (int)overlayPosition.Y;
            }
           
        }

        private void OnButtonReleased(object? sender, ButtonReleasedEventArgs e)
        {
            this.Monitor.Log($"released: {e.Button}", LogLevel.Info);

            if (this.Config.CfgButtonDraggable && overlayIsDragging && e.Button == SButton.MouseLeft)
            {
                overlayIsDragging = false;
                SaveData();
            }
        }


        private bool DrawableContains(Vector2 cursor)
        {
            if (overlay==null) return false;

            Vector2 size = overlay.ActualSize;
            this.Monitor.Log($"cursor: {cursor}, overlayPosition: {overlayPosition}, size: {size}", LogLevel.Info);


            return cursor.X >= overlayPosition.X
                && cursor.Y >= overlayPosition.Y
                && cursor.X < overlayPosition.X + size.X
                && cursor.Y < overlayPosition.Y + size.Y;
        }

        private void OnSaveLoaded(object? sender, SaveLoadedEventArgs e)
        {
            this.Data = this.Helper.Data.ReadSaveData<ModData>(SaveDataKey)
                ?? new ModData();

            ImportCommunityCenterData();
        }
    
        private void OnDayStarted(object? sender, DayStartedEventArgs e)
        {

        }

        private void OnInventoryChanged(object? sender, InventoryChangedEventArgs e)
        {
            if (!e.IsLocalPlayer || !Context.IsWorldReady)
                return;
        }

        private void ImportCommunityCenterData()
        {

        }
    } 
}
