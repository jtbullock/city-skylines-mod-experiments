using System.Linq;
using ColossalFramework.UI;
using ICities;
using JoshFirstMod.Extensions;
using UnityEngine;

namespace JoshFirstMod;

public class Loading : LoadingExtensionBase
{
    private const string PREFAB_NAME_PALM_TREE = "mp9-CaliforniaPalm";
    private const string PREFAB_NAME_MED_ROAD_WITH_TREES = "Medium Road Decoration Trees";

    public override void OnLevelLoaded(LoadMode mode)
    {
        HideBillboards();

        //ReplaceRoadTrees(PREFAB_NAME_MED_ROAD_WITH_TREES, PREFAB_NAME_PALM_TREE);
        var config = Configuration<TreeReplaceConfiguration>.Load();

        ReplaceRoadTrees("Basic Road Decoration Trees", config.SmallRoadTree);
        ReplaceRoadTrees("Oneway Road Decoration Trees", config.SmallRoadTree);

        ReplaceRoadTrees("Medium Road Decoration Trees", config.MediumRoadTree);
        ReplaceRoadTrees("Avenue Large With Grass", config.MediumRoadTree);
        ReplaceRoadTrees("Avenue Large With Buslanes Grass", config.MediumRoadTree);

        ReplaceRoadTrees("Large Road Decoration Trees", config.LargeRoadTree);
        ReplaceRoadTrees("Large Oneway Decoration Trees", config.LargeRoadTree);
    }

    public static void ReplaceRoadTrees(string roadName, string treeName)
    {
        var roadPrefab = PrefabCollection<NetInfo>.FindLoaded(roadName);
        var treePrefab = PrefabCollection<TreeInfo>.FindLoaded(treeName);

        if (roadPrefab == null)
        {
            Debug.LogError("RTR: network could not be found.");
            return;
        }

        if (treePrefab == null)
        {
            Debug.LogError("RTR: tree could not be found.");
            return;
        }

        if (roadPrefab.m_lanes == null)
        {
            Debug.LogError("RTR: Road does not have lanes.");
            return;
        }

        roadPrefab.m_lanes
            .Where(lane => lane?.m_laneProps?.m_props != null)
            .SelectMany(lane => 
                lane.m_laneProps.m_props.Where(prop => prop != null && 
                                                       prop.m_tree != null &&
                                                       prop.m_finalTree != null))
            .ForEach(prop =>
            {
                prop.m_tree = treePrefab;
                prop.m_finalTree = treePrefab;
            });
    }

    public static void HideBillboards()
    {
        var toHide = Enumerable.Range(0, PrefabCollection<BuildingInfo>.LoadedCount())
            .Select(i => PrefabCollection<BuildingInfo>.GetLoaded((uint) i))
            .Where(prefab => prefab != null && prefab.m_props != null)
            .SelectMany(prefab =>
                prefab.m_props
                    .Where(prop => prop != null && prop.m_prop != null)
                    .Where(prop => prop.m_prop.name.Contains("Billboard_big") ||
                                   prop.m_prop.name.Contains("neon-")))
            .ToList();
            
        toHide.ForEach(prop => prop.m_probability = 0);

        ShowExceptionMessage($"Found {toHide.Count} billboards to hide."); 
    }

    public static void ShowPrefabNames()
    {
        var message = Enumerable.Range(0, PrefabCollection<BuildingInfo>.LoadedCount())
            .Select(i => PrefabCollection<BuildingInfo>.GetLoaded((uint)i).name)
            .Pipe(t => string.Join(", ", t.ToArray()));

        ShowExceptionMessage(message);
    }

    public static void ShowPrefabCount()
    {
        var buildingPrefabCount = PrefabCollection<BuildingInfo>.LoadedCount();
        var message = "Number of building assets: " + buildingPrefabCount;
        ShowExceptionMessage(message);
    }

    public static void ShowExceptionMessage(string message)
    {
        ExceptionPanel panel = UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel");
        panel.SetMessage("City Beautifier", message, false);
    }
}