using System;
using System.Collections.Generic;
using Database;
using HarmonyLib;
using KSerialization;
using STRINGS;
using UnityEngine;





[AddComponentMenu("KMonoBehaviour/Workable/Plantable")]
public class Plantable : Workable
{
    [Serialize]
    private bool isMarkedForPlanting;

    private Chore chore;

    private static readonly EventSystem.IntraObjectHandler<Plantable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Plantable>(delegate (Plantable component, object data)
    {
        component.OnRefreshUserMenu(data);
    });

    protected override void OnPrefabInit()
    {
        base.OnPrefabInit();
        base.Subscribe<Plantable>(493375141, Plantable.OnRefreshUserMenuDelegate);
        this.workerStatusItem = Db.Get().DuplicantStatusItems.Emptying;

        base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
    }

    protected override void OnSpawn()
    {
        base.OnSpawn();
        if (this.isMarkedForPlanting)
        {
            this.StartChore();
        }
        this.overrideAnims = new KAnimFile[]
        {
            Assets.GetAnim("anim_interacts_dumpable_kanim")
        };
        this.workAnims = new HashedString[]
        {
            "working"
        };
        this.synchronizeAnims = false;
        base.SetWorkTime(0.1f);
    }
    [System.Runtime.InteropServices.DllImport("user32.dll")]
    public static extern short GetAsyncKeyState(int vKey);
    const int VK_SHIFT = 0x10;
    const int KEY_PRESSED = 0x8000;
    bool IsShiftPressed => ((int)GetAsyncKeyState(VK_SHIFT) & KEY_PRESSED) != 0;
    int PlantCount = 0;
    int MaxCount => Mathf.CeilToInt(this.GetComponent<Pickupable>().TotalAmount);
    bool IsStarted = false;
    public void TogglePlanting()
    {

        if (DebugHandler.InstantBuildMode)
        {
            this.OnCompleteWork(null);
            return;
        }


        if (IsShiftPressed)
        {
            if (PlantCount < MaxCount)
            {
                PlantCount++;
                this.isMarkedForPlanting = true;

            }
            else
            {
                PlantCount = 0;
                this.isMarkedForPlanting = false;
            }
        }
        else
        {
            if (this.isMarkedForPlanting) this.isMarkedForPlanting = false;
            else
            {
                PlantCount = 1;
                this.isMarkedForPlanting = true;
            }
        }
        if (this.isMarkedForPlanting) StartChore();
        else StopChore();
    }
    private void StopChore()
    {
        if (IsStarted)
        {
            this.isMarkedForPlanting = false;
            PlantCount = 0;
            this.chore.Cancel("Cancel Planting!");
            Prioritizable.RemoveRef(base.gameObject);
            this.chore = null;
            base.ShowProgressBar(false);
            IsStarted = false;
        }

    }
    private void StartChore()
    {

        if (!IsStarted)
        {
            Prioritizable.AddRef(base.gameObject);
            this.chore = new WorkChore<Plantable>(Db.Get().ChoreTypes.EmptyStorage, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
        }
        IsStarted = true;
    }

    protected override void OnCompleteWork(WorkerBase worker)
    {
        this.Plant();
        StopChore();
    }
    public bool SuitableAt(PlantableSeed plantableSeed, int cell)
    {
        if (!Grid.IsValidCell(cell))
        {
            return false;
        }
        int num;
        if (plantableSeed.Direction == SingleEntityReceptacle.ReceptacleDirection.Bottom)
        {
            num = Grid.CellAbove(cell);
        }
        else
        {
            num = Grid.CellBelow(cell);
        }
        if (plantableSeed.replantGroundTag.IsValid && !Grid.Element[num].HasTag(plantableSeed.replantGroundTag))
        {
            return false;
        }
        GameObject prefab = Assets.GetPrefab(plantableSeed.PlantID);
        EntombVulnerable component = prefab.GetComponent<EntombVulnerable>();
        if (component != null && !component.IsCellSafe(cell))
        {
            return false;
        }
        DrowningMonitor component2 = prefab.GetComponent<DrowningMonitor>();
        if (component2 != null && !component2.IsCellSafe(cell))
        {
            return false;
        }
        UprootedMonitor component4 = prefab.GetComponent<UprootedMonitor>();
        if (component4 != null && !component4.IsSuitableFoundation(cell))
        {
            return false;
        }
        OccupyArea component5 = prefab.GetComponent<OccupyArea>();
        return !(component5 != null) || component5.CanOccupyArea(cell, ObjectLayer.Plants);

    }
    public void Plant()
    {
        PlantableSeed seed = GetComponent<PlantableSeed>();
        if (seed != null)
        {
            seed.timeUntilSelfPlant = Util.RandomVariance(2400f, 600f);
            int cell = Grid.PosToCell(base.gameObject);
            Pickupable PlantSeeds = this.GetComponent<Pickupable>().Take(PlantCount);

            for (int c = 0; c < PlantCount; c++)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (SuitableAt(seed, cell))
                    {
                        Vector3 position = Grid.CellToPosCBC(cell, Grid.SceneLayer.BuildingFront);
                        GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(seed.PlantID), position, Grid.SceneLayer.BuildingFront, null, 0);
                        MutantPlant component = gameObject.GetComponent<MutantPlant>();
                        if (component != null)
                        {
                            base.GetComponent<MutantPlant>().CopyMutationsTo(component);
                        }
                        gameObject.SetActive(true);
                        break;
                    }
                    cell = Grid.CellAbove(cell);
                }
            }
            if (PlantSeeds != null)
            {
                gameObject.GetComponent<Crop>();
                Util.KDestroyGameObject(PlantSeeds.gameObject);
                return;
            }
        }



        PlantableSeed plantableSeed = base.GetComponent<PlantableSeed>();
        if (plantableSeed != null)
        {
            plantableSeed.timeUntilSelfPlant = Util.RandomVariance(2400f, 600f);
            int cell = Grid.PosToCell(base.gameObject);
            for (int i = 0; i < 3; i++)
            {
                if (SuitableAt(plantableSeed, cell))
                {
                    Vector3 position = Grid.CellToPosCBC(cell, Grid.SceneLayer.BuildingFront);
                    GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(plantableSeed.PlantID), position, Grid.SceneLayer.BuildingFront, null, 0);
                    MutantPlant component = gameObject.GetComponent<MutantPlant>();
                    if (component != null)
                    {
                        base.GetComponent<MutantPlant>().CopyMutationsTo(component);
                    }
                    gameObject.SetActive(true);



                    Pickupable pickupable = plantableSeed.GetComponent<Pickupable>().Take(PlantCount);
                    PlantCount = 0;
                    if (pickupable != null)
                    {
                        gameObject.GetComponent<Crop>();
                        Util.KDestroyGameObject(pickupable.gameObject);
                        return;
                    }
                    KCrashReporter.Assert(false, "Seed has fractional total amount < 1f", null);
                    break;
                }
                cell = Grid.CellAbove(cell);
            }
        }
    }

    private void OnRefreshUserMenu(object data)
    {
        if (this.HasTag(GameTags.Stored))
        {
            return;
        }
        if (IsShiftPressed)
        {
            KIconButtonMenu.ButtonInfo button = (PlantCount >= MaxCount)
? new KIconButtonMenu.ButtonInfo("action_empty_contents", "取消种植", new System.Action(this.TogglePlanting), global::Action.NumActions, null, null, null, "取消这条种植指令", true)
: new KIconButtonMenu.ButtonInfo("action_empty_contents", $"种植[{PlantCount}/{MaxCount}]", new System.Action(this.TogglePlanting), global::Action.NumActions, null, null, null, "将种子种在地上", true);
            Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
        }
        else
        {
            KIconButtonMenu.ButtonInfo button = this.isMarkedForPlanting
? new KIconButtonMenu.ButtonInfo("action_empty_contents", "取消种植", new System.Action(this.TogglePlanting), global::Action.NumActions, null, null, null, "取消这条种植指令", true)
: new KIconButtonMenu.ButtonInfo("action_empty_contents", $"种植", new System.Action(this.TogglePlanting), global::Action.NumActions, null, null, null, "将种子种在地上", true);
            Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
        }



    }

}




[AnyHarmonyPatch(typeof(EntityTemplates), "CreateAndRegisterSeedForPlant", ControlName: new string[] { nameof(大一统.大一统控制台UI.白嫖怪) })]
[AnyHarmonyPatch(null, null, ExecuteOnInit: nameof(ExecuteOnInit), ControlName: new string[] { nameof(大一统.大一统控制台UI.白嫖怪) })]
public class 白嫖怪
{
    public static void Postfix(ref GameObject __result)
    {
        __result.AddComponent<Plantable>();
    }
    public static void ExecuteOnInit()
    {
        Strings.Add(new string[] { "UI.USERMENUACTIONS.PLANT.NAME_OFF", "取消种植" });
        Strings.Add(new string[] { "UI.USERMENUACTIONS.PLANT.NAME", "种植" });
        Strings.Add(new string[] { "UI.USERMENUACTIONS.PLANT.TOOLTIP_OFF", "取消这条种植指令" });
        Strings.Add(new string[] { "UI.USERMENUACTIONS.PLANT.TOOLTIP", "将种子种在地上" });

    }

}