using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ExileCore.Shared.Attributes;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Helpers;
using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;
using ImGuiNET;
using Newtonsoft.Json;
using SharpDX;
using Vector4 = System.Numerics.Vector4;

namespace ExpeditionIcons;

public class ExpeditionIconsSettings : ISettings
{
    public const MapIconsIndex DefaultBadModsIcon = MapIconsIndex.RedFlag;
    public const MapIconsIndex DefaultEliteMonsterIcon = MapIconsIndex.HeistSpottedMiniBoss;
    public const MapIconsIndex DefaultChestIcon = MapIconsIndex.MissionTarget;

    internal IntPtr _iconsImageId;
    private IconPickerIndex? _shownIconPicker;
    private string _iconFilter = "";
    public int IconPickerSize = 20;
    public int IconsPerRow = 15;
    public Dictionary<IconPickerIndex, IconDisplaySettings> IconMapping = new();

    public ExpeditionIconsSettings()
    {
        GoodModsIconPicker = new CustomNode
        {
            DrawDelegate = () =>
            {
                foreach (var expeditionMarkerIconDescription in Icons.ExpeditionRelicIcons)
                {
                    ImGui.PushID($"IconLine{expeditionMarkerIconDescription.IconPickerIndex}");
                    PickIcon(expeditionMarkerIconDescription.IconPickerIndex, expeditionMarkerIconDescription.DefaultIcon);
                    ImGui.PopID();
                }
            }
        };
        ChestSettings = new CustomNode
        {
            DrawDelegate = () =>
            {
                foreach (var expeditionMarkerIconDescription in Icons.LogbookChestIcons)
                {
                    ImGui.PushID($"IconLine{expeditionMarkerIconDescription.IconPickerIndex}");
                    PickIcon(expeditionMarkerIconDescription.IconPickerIndex, expeditionMarkerIconDescription.DefaultIcon);
                    ImGui.PopID();
                }
            }
        };
        DrawEliteMonstersInWorld = new CustomNode
        {
            DrawDelegate = () => { PickIcon(IconPickerIndex.EliteMonstersIndicator, DefaultEliteMonsterIcon); }
        };
        DrawBadMods = new CustomNode
        {
            DrawDelegate = () => { PickIcon(IconPickerIndex.BadModsIndicator, DefaultBadModsIcon); }
        };
    }

    public ToggleNode Enable { get; set; } = new ToggleNode(false);

    [JsonIgnore]
    public CustomNode DrawEliteMonstersInWorld { get; set; }

    public RangeNode<int> WorldIconSize { get; set; } = new RangeNode<int>(50, 25, 200);
    public RangeNode<int> MapIconSize { get; set; } = new RangeNode<int>(30, 15, 200);

    [Menu("Good mods", 100, CollapsedByDefault = true)]
    [JsonIgnore]
    public EmptyNode SettingsEmptyGood { get; set; }

    [Menu(null, parentIndex = 100)]
    public ToggleNode DrawGoodModsOnMap { get; set; } = new ToggleNode(true);

    [Menu(null, parentIndex = 100)]
    public ToggleNode DrawGoodModsInWorld { get; set; } = new ToggleNode(true);

    [JsonIgnore]
    [Menu(null, parentIndex = 100)]
    public CustomNode GoodModsIconPicker { get; }

    [Menu("Bad mods", 101, CollapsedByDefault = true)]
    [JsonIgnore]
    public EmptyNode SettingsEmptyBad { get; set; }

    [Menu(null, parentIndex = 101)]
    [JsonIgnore]
    public CustomNode DrawBadMods { get; }

    [Menu("Warn for physical immune", parentIndex = 101)]
    public ToggleNode WarnPhysImmune { get; set; } = new ToggleNode(false);

    [Menu("Warn for fire immune", parentIndex = 101)]
    public ToggleNode WarnFireImmune { get; set; } = new ToggleNode(false);

    [Menu("Warn for ailment immune", parentIndex = 101)]
    public ToggleNode WarnIgniteImmune { get; set; } = new ToggleNode(false);

    [Menu("Warn for cold immune", parentIndex = 101)]
    public ToggleNode WarnColdImmune { get; set; } = new ToggleNode(false);

    [Menu("Warn for lightning immune", parentIndex = 101)]
    public ToggleNode WarnLightningImmune { get; set; } = new ToggleNode(false);

    [Menu("Warn for chaos immune", parentIndex = 101)]
    public ToggleNode WarnChaosImmune { get; set; } = new ToggleNode(false);

    [Menu("Warn for crit immune", parentIndex = 101)]
    public ToggleNode WarnCritImmune { get; set; } = new ToggleNode(false);

    [Menu("Warn for curse immune", parentIndex = 101)]
    public ToggleNode WarnCurseImmune { get; set; } = new ToggleNode(false);

    [Menu("Warn for armor pen (100% overwhelm)", parentIndex = 101)]
    public ToggleNode WarnArmorPen { get; set; } = new ToggleNode(false);

    [Menu("Warn for no flask", parentIndex = 101)]
    public ToggleNode WarnNoFlask { get; set; } = new ToggleNode(false);

    [Menu("Warn for no evade", parentIndex = 101)]
    public ToggleNode WarnNoEvade { get; set; } = new ToggleNode(false);

    [Menu("Warn for no leech", parentIndex = 101)]
    public ToggleNode WarnNoLeech { get; set; } = new ToggleNode(true);

    [Menu("Warn for petrify", parentIndex = 101)]
    public ToggleNode WarnPetrify { get; set; } = new ToggleNode(true);

    [Menu("Warn for 20% cull", parentIndex = 101)]
    public ToggleNode WarnCull { get; set; } = new ToggleNode(true);

    [Menu("Warn for monster regen", parentIndex = 101)]
    public ToggleNode WarnMonsterRegen { get; set; } = new ToggleNode(false);

    [Menu("Warn for monster block", parentIndex = 101)]
    public ToggleNode WarnMonsterBlock { get; set; } = new ToggleNode(false);

    [Menu("Warn for monster resistances", parentIndex = 101)]
    public ToggleNode WarnMonsterResist { get; set; } = new ToggleNode(false);

    [Menu("Warn for corrupted items", parentIndex = 101)]
    public ToggleNode WarnCorrupted { get; set; } = new ToggleNode(false);

    [Menu("Warn for \"always crit\"", parentIndex = 101)]
    public ToggleNode WarnAlwaysCrit { get; set; } = new ToggleNode(false);

    [Menu("Warn for reduced damage taken", parentIndex = 101)]
    public ToggleNode WarnReducedDamageTaken { get; set; } = new ToggleNode(false);

    [Menu("Warn for bleed", parentIndex = 101)]
    public ToggleNode WarnBleed { get; set; } = new ToggleNode(false);
    
    [Menu("Warn for poison", parentIndex = 101)]
    public ToggleNode WarnPoison { get; set; } = new ToggleNode(false);
    [Menu("Warn for phys as chaos", parentIndex = 101)]
    public ToggleNode WarnPhysicalAsExtraChaos { get; set; } = new ToggleNode(false);

    [Menu("Chest settings", index = 103, CollapsedByDefault = true)]
    [JsonIgnore]
    public EmptyNode ChestSettingsHeader { get; set; }

    [Menu(null, parentIndex = 103)]
    [JsonIgnore]
    public CustomNode ChestSettings { get; set; }

    public ExpeditionExplosiveSettings ExplosivesSettings { get; set; } = new ExpeditionExplosiveSettings();
    public PlannerSettings PlannerSettings { get; set; } = new PlannerSettings();


    private bool PickIcon(string iconName, ref MapIconsIndex icon, Vector4 tintColor)
    {
        var isOpen = true;
        ImGui.Begin($"Pick icon for {iconName}", ref isOpen, ImGuiWindowFlags.AlwaysAutoResize);
        if (!isOpen)
        {
            return true;
        }

        ImGui.InputTextWithHint("##Filter", "Filter", ref _iconFilter, 100);
        ImGui.SliderInt("Icon size (only in this menu)", ref IconPickerSize, 15, 60);
        ImGui.SliderInt("Icons per row", ref IconsPerRow, 5, 60);
        var icons = Enum.GetValues<MapIconsIndex>()
            .Where(x => string.IsNullOrEmpty(_iconFilter) || x.ToString().Contains(_iconFilter, StringComparison.InvariantCultureIgnoreCase))
            .ToArray();
        for (var i = 0; i < icons.Length; i++)
        {
            var testIcon = icons[i];
            var rect = SpriteHelper.GetUV(testIcon);
            if (icon == testIcon)
            {
                ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(1, 0, 1, 1));
            }
            else
            {
                ImGui.PushStyleColor(ImGuiCol.Button, ImGui.GetColorU32(ImGuiCol.WindowBg));
            }

            var btnClicked = ImGui.ImageButton($"btn{i}", new ImTextureRef { TexID = _iconsImageId }, System.Numerics.Vector2.One * IconPickerSize,
                rect.TopLeft.ToVector2Num(), rect.BottomRight.ToVector2Num(), Vector4.Zero, tintColor);
            ImGui.PopStyleColor();
            if (btnClicked)
            {
                icon = testIcon;
                ImGui.End();
                return true;
            }

            if ((i + 1) % IconsPerRow != 0)
            {
                ImGui.SameLine();
            }
        }

        ImGui.End();
        return false;
    }

    private void PickIcon(IconPickerIndex iconKey, MapIconsIndex defaultIcon)
    {
        var iconSettings = IconMapping.GetValueOrDefault(iconKey, new IconDisplaySettings());
        ImGui.Checkbox($"Show {iconKey} on map", ref iconSettings.ShowOnMap);
        ImGui.SameLine();
        ImGui.Text("(");
        ImGui.SameLine(0, 0);
        ImGui.Checkbox("in world", ref iconSettings.ShowInWorld);
        ImGui.SameLine(0, 0);
        ImGui.Text(")");
        ImGui.SameLine();
        var effectiveIcon = iconSettings.Icon ?? defaultIcon;
        var uv = SpriteHelper.GetUV(effectiveIcon);
        var uv0 = uv.TopLeft.ToVector2Num();
        var uv1 = uv.BottomRight.ToVector2Num();
        ImGui.PushID(iconKey.ToString());
        var tintVector = (iconSettings.Tint ?? Color.White).ToImguiVec4();
        var buttonClicked = ImGui.ImageButton("iconbtn", new ImTextureRef { TexID = _iconsImageId }, System.Numerics.Vector2.One * 15, uv0, uv1, Vector4.Zero, tintVector);
        ImGui.SameLine();
        if (ImGui.ColorEdit4("Tint", ref tintVector,
                ImGuiColorEditFlags.AlphaBar | ImGuiColorEditFlags.NoInputs |
                ImGuiColorEditFlags.AlphaPreviewHalf))
        {
            var tint = tintVector.ToSharpColor();
            if (tint != Color.White)
            {
                iconSettings.Tint = tint;
            }
        }

        if (buttonClicked)
        {
            _iconFilter = "";
        }

        if (buttonClicked || iconKey == _shownIconPicker)
        {
            _shownIconPicker = iconKey;
            if (PickIcon(iconKey.ToString(), ref effectiveIcon, tintVector))
            {
                iconSettings.Icon = effectiveIcon != defaultIcon ? effectiveIcon : null;
                _shownIconPicker = null;
            }
        }

        ImGui.PopID();
        IconMapping[iconKey] = iconSettings;
    }
}

[Submenu]
public class PlannerSettings
{
    public Dictionary<IconPickerIndex, ChestSettings> ChestSettingsMap = new()
    {
        [IconPickerIndex.LeagueChest] = new ChestSettings { Weight = 2 }
    };

    public Dictionary<IconPickerIndex, RelicSettings> RelicSettingsMap = new()
    {
        [IconPickerIndex.Logbooks] = new RelicSettings { Multiplier = 1.5f, },
        [IconPickerIndex.LogbooksExcavatedChest] = new RelicSettings { Multiplier = 1.5f, },
        [IconPickerIndex.Fractured] = new RelicSettings { Multiplier = 1.3f, },
        [IconPickerIndex.FracturedExcavatedChest] = new RelicSettings { Multiplier = 1.3f, },
        [IconPickerIndex.PackSize] = new RelicSettings { Multiplier = 1.25f, },
        [IconPickerIndex.Scarabs] = new RelicSettings { Increase = 0.25f, },
        [IconPickerIndex.ScarabsExcavatedChest] = new RelicSettings { Increase = 0.25f, },
        [IconPickerIndex.Artifacts] = new RelicSettings { Increase = 0.4f, },
        [IconPickerIndex.ArtifactsExcavatedChest] = new RelicSettings { Increase = 0.4f, },
        [IconPickerIndex.Quantity] = new RelicSettings { Increase = 0.4f, },
        [IconPickerIndex.QuantityExcavatedChest] = new RelicSettings { Increase = 0.4f, },
        [IconPickerIndex.Currency] = new RelicSettings { Increase = 0.4f, },
        [IconPickerIndex.CurrencyExcavatedChest] = new RelicSettings { Increase = 0.4f, },
        [IconPickerIndex.StackedDecks] = new RelicSettings { Increase = 0.4f, },
        [IconPickerIndex.StackedDecksExcavatedChest] = new RelicSettings { Increase = 0.4f, },
        [IconPickerIndex.Rerolls] = new RelicSettings { Increase = 0.25f, },
    };

    public RelicSettings DefaultRelicSettings = RelicSettings.Default;

    public PlannerSettings()
    {
        ChestWeightSettings = new CustomNode
        {
            DrawDelegate = () =>
            {
                foreach (var expeditionMarkerIconDescription in Icons.LogbookChestIcons)
                {
                    ImGui.PushID($"IconLine{expeditionMarkerIconDescription.IconPickerIndex}");
                    var chestSettings = ChestSettingsMap.GetValueOrDefault(
                        expeditionMarkerIconDescription.IconPickerIndex, new ChestSettings());
                    if (ImGui.SliderFloat($"{expeditionMarkerIconDescription.IconPickerIndex} weight", ref chestSettings.Weight, 0, 5))
                    {
                        ChestSettingsMap[expeditionMarkerIconDescription.IconPickerIndex] = chestSettings;
                    }

                    ImGui.PopID();
                }
            }
        };
        RelicWeightSettings = new CustomNode
        {
            DrawDelegate = () =>
            {
                if (ImGui.BeginTable("Relic Weight", 4, ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.Borders))
                {
                    ImGui.TableSetupColumn("Name");
                    ImGui.TableSetupColumn("Multiplier", ImGuiTableColumnFlags.WidthFixed, 300);
                    ImGui.TableSetupColumn("Increase", ImGuiTableColumnFlags.WidthFixed, 300);
                    ImGui.TableHeadersRow();
                    foreach (var expeditionMarkerIconDescription in Icons.ExpeditionRelicIcons)
                    {
                        ImGui.PushID($"Icon{expeditionMarkerIconDescription.IconPickerIndex}");
                        ImGui.TableNextRow(ImGuiTableRowFlags.None);
                        ImGui.TableNextColumn();
                        ImGui.Text($"{expeditionMarkerIconDescription.IconPickerIndex}");
                        var relicSettings = RelicSettingsMap.GetValueOrDefault(
                            expeditionMarkerIconDescription.IconPickerIndex, RelicSettings.Default);

                        ImGui.TableNextColumn();
                        ImGui.SetNextItemWidth(300);
                        if (ImGui.SliderFloat("##multiplier", ref relicSettings.Multiplier, 0, 5))
                        {
                            RelicSettingsMap[expeditionMarkerIconDescription.IconPickerIndex] = relicSettings;
                        }

                        ImGui.TableNextColumn();
                        ImGui.SetNextItemWidth(300);
                        if (ImGui.SliderFloat("##increase", ref relicSettings.Increase, 0, 5))
                        {
                            RelicSettingsMap[expeditionMarkerIconDescription.IconPickerIndex] = relicSettings;
                        }
                        ImGui.PopID();
                    }

                    {
                        ImGui.PushID("OtherRelics");
                        ImGui.TableNextRow(ImGuiTableRowFlags.None);
                        ImGui.TableNextColumn();
                        ImGui.Text("Other relics");
                        var relicSettings = DefaultRelicSettings;

                        ImGui.TableNextColumn();
                        ImGui.SetNextItemWidth(300);
                        ImGui.SliderFloat("##multiplier", ref relicSettings.Multiplier, 0, 5);

                        ImGui.TableNextColumn();
                        ImGui.SetNextItemWidth(300);
                        ImGui.SliderFloat("##increase", ref relicSettings.Increase, 0, 5);
                        ImGui.PopID();
                    }

                    ImGui.EndTable();
                }
            }
        };
    }

    public HotkeyNode StartSearchHotkey { get; set; } = new HotkeyNode(Keys.F13);
    public HotkeyNode StopSearchHotkey { get; set; } = new HotkeyNode(Keys.F13);
    public HotkeyNode ClearSearchHotkey { get; set; } = new HotkeyNode(Keys.F13);
    public HotkeyNode ConfirmEditorPlacementHotkey { get; set; } = new HotkeyNode(Keys.Enter);

    [JsonIgnore]
    [ConditionalDisplay(nameof(IsSearchRunning), false)]
    public ButtonNode StartSearch { get; set; } = new ButtonNode();

    [JsonIgnore]
    [ConditionalDisplay(nameof(IsSearchRunning))]
    public ButtonNode StopSearch { get; set; } = new ButtonNode();

    [JsonIgnore]
    [ConditionalDisplay(nameof(HasSearchResult))]
    public ButtonNode ClearSearch { get; set; } = new ButtonNode();
    public ToggleNode PlaySoundOnFinish { get; set; } = new ToggleNode(false);

    [Menu("Color for suggested explosive radius")]
    public ColorNode ExplosiveColor { get; set; } = new ColorNode(Color.Purple);

    public ColorNode MapLineColor { get; set; } = new ColorNode(Color.Red);
    public ColorNode WorldLineColor { get; set; } = new ColorNode(Color.Orange);

    [Menu("Color for captured entities in world")]
    public ColorNode CapturedEntityWorldFrameColor { get; set; } = new ColorNode(Color.Purple);

    [Menu("Color for captured entities on map")]
    public ColorNode CapturedEntityMapFrameColor { get; set; } = new ColorNode(Color.Purple);

    public RangeNode<float> TextMarkerScale { get; set; } = new RangeNode<float>(1, 0, 5);

    public RangeNode<float> MaximumGenerationTimeSeconds { get; set; } = new RangeNode<float>(5, 0, 60);
    public RangeNode<int> SearchThreads { get; set; } = new RangeNode<int>(5, 1, 10);
    public RangeNode<float> NewRandomPathInjectionRate { get; set; } = new RangeNode<float>(1f, 0, 2);
    public RangeNode<float> PathMutateChance { get; set; } = new RangeNode<float>(0.5f, 0, 1);
    public RangeNode<int> PathGenerationSize { get; set; } = new RangeNode<int>(100, 1, 1000);
    public RangeNode<int> ValidatedIntermediatePoints { get; set; } = new RangeNode<int>(1, 0, 5);
    public RangeNode<float> RunicMonsterWeight { get; set; } = new RangeNode<float>(3, 0, 5);
    public RangeNode<float> RunicMonsterLogbookWeight { get; set; } = new RangeNode<float>(3, 0, 5);
    public RangeNode<float> NormalMonsterWeight { get; set; } = new RangeNode<float>(0.2f, 0, 5);

    [Menu("Chest weight", 888, CollapsedByDefault = true)]
    [JsonIgnore]
    public EmptyNode ChestWeightStub { get; set; } 

    [JsonIgnore]
    [Menu(null, parentIndex = 888)]
    public CustomNode ChestWeightSettings { get; set; }

    [Menu("Relic weight modifiers", 999, CollapsedByDefault = true)]
    [JsonIgnore]
    public EmptyNode RelicWeightStub { get; set; }

    [JsonIgnore]
    [Menu(null, parentIndex = 999)]
    public CustomNode RelicWeightSettings { get; set; }

    public RangeNode<int> LogbookCaveRunicMonsterMultiplier { get; set; } = new RangeNode<int>(3, 0, 10);
    public RangeNode<int> LogbookCaveArtifactChestMultiplier { get; set; } = new RangeNode<int>(3, 0, 10);
    public RangeNode<int> LogbookBossRunicMonsterMultiplier { get; set; } = new RangeNode<int>(10, 0, 20);

    public ToggleNode ShowScoreHistory { get; set; } = new ToggleNode(false);
    public ToggleNode ShowScoreHistoryAfterSearchEnds { get; set; } = new ToggleNode(false);

    internal bool HasSearchResult => SearchState != SearchState.Empty;
    internal bool IsSearchRunning => SearchState == SearchState.Searching;

    internal SearchState SearchState = SearchState.Empty;
}

[Submenu(CollapsedByDefault = true)]
public class ExpeditionExplosiveSettings
{
    [Menu("Show explosive radius")]
    public ToggleNode ShowExplosives { get; set; } = new ToggleNode(true);

    [Menu("Color for explosive radius")]
    public ColorNode ExplosiveColor { get; set; } = new ColorNode(Color.Red);

    [Menu("Explosive radius")]
    public RangeNode<int> ExplosiveRadius { get; set; } = new RangeNode<int>(326, 10, 600);

    [Menu("Automatically calculate Radius from map mods")]
    public ToggleNode CalculateRadiusAutomatically { get; set; } = new ToggleNode(true);

    [Menu("Merge explosive radii")]
    public ToggleNode EnableExplosiveRadiusMerging { get; set; } = new ToggleNode(true);

    [Menu("Mark entities captured by explosives in world")]
    public ToggleNode MarkCapturedEntitiesInWorld { get; set; } = new ToggleNode(true);

    [Menu("Mark entities captured by explosives on map")]
    public ToggleNode MarkCapturedEntitiesOnMap { get; set; } = new ToggleNode(true);

    [Menu("Hide icons of entities captured by explosives in world")]
    public ToggleNode HideCapturedEntitiesInWorld { get; set; } = new ToggleNode(false);

    [Menu("Hide icons of entities captured by explosives on map")]
    public ToggleNode HideCapturedEntitiesOnMap { get; set; } = new ToggleNode(false);

    [Menu("Color for captured entities in world")]
    public ColorNode CapturedEntityWorldFrameColor { get; set; } = new ColorNode(Color.Green);

    [Menu("Color for captured entities on map")]
    public ColorNode CapturedEntityMapFrameColor { get; set; } = new ColorNode(Color.Green);

    [Menu("Rectangle Thickness for captured entities in world")]
    public RangeNode<int> CapturedEntityWorldFrameThickness { get; set; } = new RangeNode<int>(2, 1, 20);

    [Menu("Rectangle Thickness for captured entities on map")]
    public RangeNode<int> CapturedEntityMapFrameThickness { get; set; } = new RangeNode<int>(2, 1, 20);
}

public enum SearchState
{
    Empty,
    Searching,
    Stopped,
}