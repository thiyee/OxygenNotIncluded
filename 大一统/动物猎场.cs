using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;
using HarmonyLib;
using STRINGS;
using System.Linq;
using System.Reflection;

namespace 大一统{
	public class KillerTrigger : KMonoBehaviour
	{
		protected override void OnSpawn()
		{
			base.OnSpawn();
			GameObject gameObject = base.gameObject;

			this.partitionerEntry1 = GameScenePartitioner.Instance.Add("Trap1", gameObject, Grid.PosToCell(gameObject), GameScenePartitioner.Instance.trapsLayer, new Action<object>(this.OnCreatureOnTrap));
			this.partitionerEntry2 = GameScenePartitioner.Instance.Add("Trap2", gameObject, Grid.CellRight(Grid.PosToCell(gameObject)), GameScenePartitioner.Instance.trapsLayer, new Action<object>(this.OnCreatureOnTrap));
			foreach (GameObject gameObject2 in this.storage.items)
			{
				this.SetStoredPosition(gameObject2);
				KBoxCollider2D component = gameObject2.GetComponent<KBoxCollider2D>();
				if (component != null)
				{
					component.enabled = true;
				}
			}
		}

		private void SetStoredPosition(GameObject go)
		{
			Vector3 position = Grid.CellToPosCBC(Grid.PosToCell(base.transform.GetPosition()), Grid.SceneLayer.BuildingBack);
			position.x += 0.5f;
			position.y += 0f;
			go.transform.SetPosition(position);
			go.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.BuildingBack);
		}

		private Vector3 GetDropSpawnLocation()
		{
			if (!base.isNull && base.transform != null)
			{
				Vector3 position = Grid.CellToPosCBC(Grid.PosToCell(base.transform.GetPosition()), Grid.SceneLayer.BuildingBack);
				if (position == null)
				{
					Debug.LogError("Position is null in GetDropSpawnLocation.");
					return Vector3.zero;
				}

				int num = Grid.PosToCell(position);
				int num2 = Grid.CellAbove(num);
				if (Grid.IsValidCell(num2) && !Grid.Solid[num2])
				{
					return position;
				}
				return position;
			}
			return Vector3.zero;
		}
		public void OnCreatureOnTrap(object data)
		{
			try
			{
				if (this.CreatureKiller)
				{
					CreatureKillerSM creatureKillerSM = this.CreatureKiller.GetComponent<CreatureKillerSM>();
					Operational operational = this.CreatureKiller.GetComponent<Operational>();
					if (!operational.IsOperational)
					{
						return;
					}
				}
				Trappable trappable = (Trappable)data;
				if (!trappable.HasTag(GameTags.Creature)) {
					Console.WriteLine("Trap non Creature " + trappable.name);
					return;
				}
				Butcherable butcherable;
				if (trappable.gameObject == null) {

					Console.WriteLine("Trap114 " + trappable.name);
					string sztag = "tag:";
					KPrefabID cm = trappable.GetComponent<KPrefabID>();
					var fieldInfo = typeof(KPrefabID).GetField("tags", BindingFlags.NonPublic | BindingFlags.Instance);
					HashSet<Tag> tags = (HashSet<Tag>)fieldInfo.GetValue(cm);
					foreach (Tag tag in tags) {
						sztag += tag;
					}
					Console.WriteLine(sztag);
					return;
				}
				if (trappable.gameObject.TryGetComponent<Butcherable>(out butcherable)) {
					foreach (string drop in butcherable.drops) {
						GameObject gameObject = Scenario.SpawnPrefab(Grid.PosToCell(GetDropSpawnLocation()), 0, 0, drop, Grid.SceneLayer.Ore);
						gameObject.SetActive(true);
						Edible component2 = gameObject.GetComponent<Edible>();
						if (component2)
						{
							ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, component2.Calories, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.BUTCHERED, "{0}", gameObject.GetProperName()), UI.ENDOFDAYREPORT.NOTES.BUTCHERED_CONTEXT);
						}
						this.storage.Store(gameObject, true, false, true, false);
						SetStoredPosition(gameObject);
					}
				}
				trappable.gameObject.DeleteObject();
			}
			catch (NullReferenceException ex) {
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);

				// 如果想要进一步分析StackTrace，可以解析它
				// 通常情况下，StackTrace的第一行会是发生异常的实际代码行
				string[] stackTraceLines = ex.StackTrace.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
				string errorLine = stackTraceLines[0]; // 第一行通常是错误行
				Console.WriteLine("错误发生在: " + errorLine);
			}

		}

		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			GameScenePartitioner.Instance.Free(ref this.partitionerEntry1);
			GameScenePartitioner.Instance.Free(ref this.partitionerEntry2);
			//GameScenePartitioner.Instance.Free(ref this.partitionerEntry3);
			//GameScenePartitioner.Instance.Free(ref this.partitionerEntry4);
		}
		public GameObject CreatureKiller;
		private HandleVector<int>.Handle partitionerEntry1;
		private HandleVector<int>.Handle partitionerEntry2;
		//private HandleVector<int>.Handle partitionerEntry3;
		//private HandleVector<int>.Handle partitionerEntry4;
		public Tag[] killableCreatures;

		public Vector2 killedOffset = Vector2.zero;

		[MyCmpReq]
		private Storage storage;
	}
	public class CreatureKillerSM : StateMachineComponent<CreatureKillerSM.StatesInstance>
	{
		public class StatesInstance : GameStateMachine<States, StatesInstance, CreatureKillerSM, object>.GameInstance
		{
			public Operational operational;

			public StatesInstance(CreatureKillerSM master) : base(master)
			{
				operational = master.GetComponent<Operational>();
			}
		}

		public class States : GameStateMachine<States, StatesInstance, CreatureKillerSM>
		{
			public State idle;
			public State active;

			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = idle;
				idle.PlayAnim("idle", KAnim.PlayMode.Loop).Transition(active, (StatesInstance smi) => smi.operational.IsOperational);
				active.PlayAnim("active", KAnim.PlayMode.Loop).Transition(idle, (StatesInstance smi) => !smi.operational.IsOperational);
			}
		}

		protected override void OnSpawn()
		{
			base.OnSpawn();
			smi.StartSM();
		}
	}
	public class CreatureKillerConfig : IBuildingConfig
	{
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("CreatureKiller", 2, 1, "creaturetrap_kanim", 10, 10f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, MATERIALS.RAW_METALS, 1600f, BuildLocationRule.OnFloor, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);

			buildingDef.AudioCategory = "Metal";
			buildingDef.Floodable = false;
			buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
			buildingDef.BuildLocationRule = BuildLocationRule.NotInTiles;
			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{

			Storage storage = go.AddOrGet<Storage>();
			storage.allowItemRemoval = true;
			storage.SetDefaultStoredItemModifiers(CreatureKillerConfig.StoredItemModifiers);
			storage.sendOnStoreOnSpawn = true;
			KillerTrigger trapTrigger = go.AddOrGet<KillerTrigger>();
			trapTrigger.CreatureKiller = go;
			// 添加 DropAllWorkable 组件
			DropAllWorkable dropAllWorkable = go.AddOrGet<DropAllWorkable>();


			go.AddOrGet<CreatureKillerSM>();
		}


		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddOrGet<LogicOperationalController>();
			go.AddOrGetDef<OperationalController.Def>();
		}

		public const string ID = "CreatureKiller";

		private static readonly List<Storage.StoredItemModifier> StoredItemModifiers = new List<Storage.StoredItemModifier>();



	}
	[AnyHarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings", ControlName: new string[] { nameof(大一统.大一统控制台UI.动物猎场)})]
	class 动物猎场
	{
		public static void Prefix()
		{
				Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.CREATUREKILLER.NAME", "动物猎场" });
				Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.CREATUREKILLER.EFFECT", "动物猎场" });
				Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.CREATUREKILLER.DESC", "当动物进入动物猎场时将会被自动处死并收集其产物" });
				ModUtil.AddBuildingToPlanScreen("Base", "CreatureKiller");
			
		}
	}
}