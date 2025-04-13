using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static PathFinder.Path;

namespace 大一统{

	public class NavigatorAccess
	{
		public Navigator navigator;
		public FieldAccess reservedCell;
		public FieldAccess tactic;
		public FuncAccess ValidatePath;
		public FuncAccess SetReservedCell;
		public FuncAccess ClearReservedCell;
		public void SetReservedCellProxy(int cell)
		{
			this.SetReservedCell.Invoke(new object[] { cell });
		}
		public void ClearReservedCellProxy()
		{
			this.ClearReservedCell.Invoke();
		}
		public bool ValidatePathProxy(ref PathFinder.Path path, out bool atNextNode)
		{
			bool ret;
			object[] Parameter = { path, null };
			ret = (bool)this.ValidatePath.Invoke(Parameter);
			path = (PathFinder.Path)Parameter[0];
			atNextNode = (bool)Parameter[1];
			return ret;
		}

		public NavigatorAccess(Navigator navigator)
		{
			this.navigator = navigator;
			FieldAccess.Initialize(this, navigator);
			FuncAccess.Initialize(this, navigator);
		}

	}
	public class TransitionDriverAccess
	{
		public TransitionDriver transitionDriver;

		public FieldAccess navigator;
		public FieldAccess interruptOverrideStack;
		public FieldAccess transition;
		public FieldAccess isComplete;
		public FieldAccess brain;
		public FieldAccess targetPos;

		public TransitionDriverAccess(TransitionDriver transitionDriver)
		{
			this.transitionDriver = transitionDriver;
			FieldAccess.Initialize(this, transitionDriver);
			FuncAccess.Initialize(this, transitionDriver);
		}

	}



	//[AnyHarmonyPatch(typeof(Navigator), "AdvancePath", Prefix: nameof(AdvancePathPrefix))]
	[AnyHarmonyPatch(typeof(Navigator), "BeginTransition", Prefix: nameof(BeginTransitionPrefix), ControlName: new string[] { nameof(大一统.大一统控制台UI.超时空传送) })]

	public class 超时空传送{
		public static bool BeginTransitionPrefix(Navigator __instance,ref NavGrid.Transition transition)
        {
			if (!(__instance.gameObject.HasTag(GameTags.DupeBrain)))
			{
				return true;
			}
            if (__instance.path.nodes.Count > 2)
            {
				var sec= __instance.path.nodes[__instance.path.nodes.Count-2];
				var last = __instance.path.nodes.Last();
				__instance.path.nodes = new List<Node>() { sec, last };

				Navigator navigator = __instance;
				navigator.SetCurrentNavType(sec.navType);
				navigator.transform.SetPosition(Grid.CellToPosCBC(sec.cell, navigator.sceneLayer));
				transition = __instance.NavGrid.transitions[last.transitionId];
				//__instance.Stop(true, true);
				return true;
				//__instance.path.nodes = new List<Node>() { first,last };
				//transition = __instance.NavGrid.transitions[(int)last.transitionId];
			}
			return true;
        }
		public static bool AdvancePathPrefix(ref Navigator __instance, bool trigger_advance = true)
		{

			if (!(__instance.gameObject.HasTag(GameTags.DupeBrain)))
			{
				return true;
			}

			NavigatorAccess navigatorAccess = new NavigatorAccess(__instance);
			int num = Grid.PosToCell(__instance);
			if (__instance.target == null)
			{
				__instance.Trigger(-766531887, null);
				__instance.Stop(false, true);
			}
			else if (num == (int)navigatorAccess.reservedCell.Get() && __instance.CurrentNavType != NavType.Tube)
			{
				__instance.Stop(true, true);
			}
			else
			{
				bool flag2;
				bool flag = !(bool)navigatorAccess.ValidatePathProxy(ref __instance.path, out flag2);

				if (flag2)
				{
					__instance.path.nodes.RemoveAt(0);
				}
				if (flag)
				{
					int root = Grid.PosToCell(__instance.target);
					int cellPreferences = ((NavTactic)navigatorAccess.tactic.Get()).GetCellPreferences(root, __instance.targetOffsets, __instance);
					navigatorAccess.SetReservedCellProxy(cellPreferences);
					if ((int)navigatorAccess.reservedCell.Get() == NavigationReservations.InvalidReservation)
					{
						__instance.Stop(false, true);
					}
					else
					{
						PathFinder.PotentialPath potential_path = new PathFinder.PotentialPath(num, __instance.CurrentNavType, __instance.flags);
						PathFinder.UpdatePath(__instance.NavGrid, __instance.GetCurrentAbilities(), potential_path, PathFinderQueries.cellQuery.Reset((int)navigatorAccess.reservedCell.Get()), ref __instance.path);
					}
				}
				if (__instance.path.IsValid())
				{
					Navigator navigator = __instance;
					int end = __instance.path.nodes.Count - 1;
					navigator.SetCurrentNavType(__instance.path.nodes[end].navType);
					navigator.transform.SetPosition(Grid.CellToPosCBC(__instance.path.nodes[end].cell, navigator.sceneLayer));
					__instance.Stop(true, true);
				}
				else if (__instance.path.HasArrived())
				{
					__instance.Stop(true, true);
				}
				else
				{
					navigatorAccess.ClearReservedCellProxy();
					__instance.Stop(false, true);
				}
			}

			if (trigger_advance)
			{
				__instance.Trigger(1347184327, null);
			}
			return false;



		}

	}

}
