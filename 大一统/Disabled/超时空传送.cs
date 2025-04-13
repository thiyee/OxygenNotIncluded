using HarmonyLib;
using PeterHan.PLib.Detours;
using System;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

namespace 超时空传送
{
    //class 超时空传送
    //{
    //    private static readonly IDetouredField<TransitionDriver, Navigator.ActiveTransition> TRANSITION =PDetours.DetourField<TransitionDriver, Navigator.ActiveTransition>("transition");
    //    [HarmonyPatch(typeof(Navigator), nameof(Navigator.AdvancePath))]
    //    public static class PathFinder_UpdatePath_Patch
    //    {
    //        // Prefix 方法用于在原始方法执行之前修改其行为
    //        public static bool Prefix(Navigator __instance, ref NavTactic ___tactic, ref int ___reservedCell){
    //            // 检查角色是否有目标、是否有指定的标志位以及当前位置是否与预留的位置不同
    //            if (!PeterHan.PLib.Options.SingletonOptions<大一统.大一统控制台UI>.Instance.超时空传送) return true;
    //            if (__instance.target == null) return true;
    //            if (Grid.PosToCell(__instance) == ___reservedCell) return true;
    //            
    //            int mycell = Grid.PosToCell(__instance);
    //            int target_position_cell = Grid.PosToCell(__instance.target);
    //    
    //            // 检查当前位置和目标位置是否合法，如果不合法则停止角色的移动并返回 true
    //            if ((!Grid.IsValidCell(mycell)) || (!Grid.IsValidCell(target_position_cell))) return true; // 控制权交给原函数
    //            
    //    
    //            // 检查是否能够通过目标位置的附近的偏移位置
    //            for (int i = 0; i < __instance.targetOffsets.Length; i++){
    //                int cell = Grid.OffsetCell(target_position_cell, __instance.targetOffsets[i]);
    //                if (__instance.CanReach(cell) && mycell == cell) return true;
    //                
    //            }
    //    
    //    
    //            // 计算新的目标位置和相关的变量
    //            int cellPreferences = ___tactic.GetCellPreferences(target_position_cell, __instance.targetOffsets, __instance);
    //            NavigationReservations.Instance.RemoveOccupancy(___reservedCell);
    //            ___reservedCell = cellPreferences;
    //            NavigationReservations.Instance.AddOccupancy(cellPreferences);
    //    
    //            if (___reservedCell != NavigationReservations.InvalidReservation)
    //            {
    //                // 设置角色状态为移动，并进行位置的变换
    //                __instance.transitionDriver.EndTransition();
    //                __instance.smi.GoTo(__instance.smi.sm.normal.moving);
    //                Navigator.ActiveTransition transition = TRANSITION.Get(__instance.transitionDriver);
    //                transition = new Navigator.ActiveTransition();
    //                int reservedCell = ___reservedCell;
    //                Vector3 position = Grid.CellToPos(reservedCell, CellAlignment.Bottom, Grid.SceneLayer.Move);
    //                __instance.transform.SetPosition(position);
    //                if (Grid.HasLadder[reservedCell])__instance.CurrentNavType = NavType.Ladder;
    //                //if (Grid.HasPole[reservedCell])__instance.CurrentNavType = NavType.Pole;
    //                //if (GameNavGrids.FloorValidator.IsWalkableCell(reservedCell, Grid.CellBelow(reservedCell), true))__instance.CurrentNavType = NavType.Floor;
    //                __instance.Stop(arrived_at_destination: true, true);
    //            }
    //    
    //            return false; // 停止原始方法的执行
    //            }
    //        
    //    }
    //
    //    //[HarmonyPatch(typeof(Navigator), nameof(Navigator.RunQuery))]
    //    //public static class Navigator_RunQuery_Patch
    //    //{
    //    //
    //    //    public static bool Prefix(ref Navigator __instance,ref PathFinderQuery query){
    //    //        if (!__instance.gameObject.HasTag( MinionConfig.ID)) return false;
    //    //        Debug.Log("CC:RunQuery " + __instance.gameObject.name + "CurrentNavType:" + __instance.CurrentNavType.ToString());
    //    //        __instance.SetFlags(PathFinder.PotentialPath.Flags.HasJetPack);
    //    //        __instance.CurrentNavType = NavType.Hover;
    //    //        return true;
    //    //    }
    //    //}
    //    //[HarmonyPatch(typeof(Navigator), nameof(Navigator.SetCurrentNavType))]
    //    //public static class 关闭坠落判断 {
    //    //    public static bool Prefix(ref Navigator __instance,ref NavType nav_type) {
    //    //        if (!__instance.gameObject.HasTag(MinionConfig.ID)) return true; //调用原函数
    //    //        Debug.Log("CC:Set" + __instance.gameObject.name + "NavType:" + __instance.CurrentNavType.ToString() + nav_type.ToString());
    //    //        PrintCallerMethodName();
    //    //        /*if(nav_type==NavType.Floor)*/ nav_type = NavType.Hover;
    //    //        return true;
    //    //    }
    //    //}
    //    //[HarmonyPatch(typeof(FallMonitor.Instance), nameof(FallMonitor.Instance.UpdateFalling))]
    //    //public static class UpdateFallingPrefix
    //    //{
    //    //    public static bool Prefix(ref FallMonitor.Instance __instance) {
    //    //        if (!__instance.gameObject.HasTag(MinionConfig.ID)) return true; //调用原函数
    //    //
    //    //        
    //    //        return true;
    //    //    }
    //    //}        
    //
    //    //[HarmonyPatch(typeof(FallMonitor.Instance), nameof(FallMonitor.Instance.LandFloor))]
    //    //public static class LandFloorPatch{
    //    //    public static bool Prefix(ref FallMonitor.Instance __instance) {
    //    //        if (!__instance.gameObject.HasTag(MinionConfig.ID)) return true; //调用原函数
    //    //
    //    //
    //    //        return false;
    //    //    }
    //    //}        
    //
    //}


}



