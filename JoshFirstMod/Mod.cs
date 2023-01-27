using ICities;
using System;

namespace JoshFirstMod;

public class Mod : IUserMod
{
    public string Name => "Josh's First Mod";
    public string Description => "Giving this a try! -Josh";

    private static readonly string[] OptionLabels =
    {
        "Default",
        "California Palm"
    };

    private static readonly string[] OptionValues =
    {
        "Tree2Variant",
        "mp9-CaliforniaPalm"
    };

    public void OnSettingsUI(UIHelperBase helper)
    {
        var config = Configuration<TreeReplaceConfiguration>.Load();

        // Small Roads
        helper.AddDropdown("Small Road Tree", 
            OptionLabels, GetSelectedOptionIndex(config.SmallRoadTree), sel =>
        {
            config.SmallRoadTree = OptionValues[sel];
            Configuration<TreeReplaceConfiguration>.Save();
        });

        // Medium Roads
        helper.AddDropdown("Medium Road Tree",
            OptionLabels, GetSelectedOptionIndex(config.MediumRoadTree), sel =>
        {
            // Change config value and save config
            config.MediumRoadTree = OptionValues[sel];
            Configuration<TreeReplaceConfiguration>.Save();
        });

        // Large Roads
        helper.AddDropdown("Large Road Tree",
            OptionLabels, GetSelectedOptionIndex(config.LargeRoadTree), sel =>
        {
            // Change config value and save config
            config.LargeRoadTree = OptionValues[sel];
            Configuration<TreeReplaceConfiguration>.Save();
        });
    }

    // Returns the index number of the option that is currently selected
    private int GetSelectedOptionIndex(string value)
    {
        int index = Array.IndexOf(OptionValues, value);
        if (index < 0) index = 0;

        return index;
    }
}