using System.Linq;
using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace JoshFirstMod
{
    public class ShowLimitsThreading : ThreadingExtensionBase
    {
        private bool _countsProcessed = false;
        private bool _carsProcessed = false;

        private static bool IsControlPressed() =>
            Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            //ShowCounts();
            FlipParkedCars();
        }

        private void FlipParkedCars()
        {
            var parkedVehicles = VehicleManager.instance.m_parkedVehicles.m_buffer.ToList();

            parkedVehicles.ForEach(parkedVehicle =>
            {
                parkedVehicle.m_position.y += 2;
                parkedVehicle.m_rotation.x = 0;
            });

            // -----------------------------

            var isShortcutBeingInvoked = IsControlPressed() && Input.GetKey(KeyCode.J);

            if (!isShortcutBeingInvoked)
            {
                _carsProcessed = false;
                return;
            }

            if (_carsProcessed) return;

            _carsProcessed = true;

            // --------------------------------

            ExceptionPanel panel = UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel");
            panel.SetMessage("Show Limits", $"There are {parkedVehicles.Count} parked vehicles.", false);
        }

        public void ShowCounts()
        {
            var isShortcutBeingInvoked = IsControlPressed() && Input.GetKey(KeyCode.I);

            if (!isShortcutBeingInvoked)
            {
                _countsProcessed = false;
                return;
            }

            if (_countsProcessed) return;

            _countsProcessed = true;

            int treeCount = TreeManager.instance.m_treeCount;
            int maxTreeCount = TreeManager.MAX_TREE_COUNT;

            int buildingCount = BuildingManager.instance.m_buildingCount;
            int maxBuildingCount = BuildingManager.MAX_BUILDING_COUNT;

            string message = $"Trees: {treeCount} of {maxTreeCount}\nBuildings: {buildingCount} of {maxBuildingCount}";

            ExceptionPanel panel = UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel");
            panel.SetMessage("Show Limits", message, false);
        }
    }
}
