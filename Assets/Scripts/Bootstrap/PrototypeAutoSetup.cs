using System.Collections.Generic;
using ImbaLife.Core;
using ImbaLife.Gameplay;
using UnityEngine;

namespace ImbaLife.Bootstrap
{
    public sealed class PrototypeAutoSetup : MonoBehaviour
    {
        [SerializeField] private bool createDemoSceneOnPlay = true;

        private void Awake()
        {
            if (!createDemoSceneOnPlay) return;
            BuildIfEmpty();
        }

        private void BuildIfEmpty()
        {
            EnsureGameState();
            TaskBoard taskBoard = EnsureTaskBoard();
            EnsurePlayer(taskBoard);
            EnsureWorld();
        }

        private static void EnsureGameState()
        {
            if (FindObjectOfType<GameState>() != null) return;
            GameObject state = new GameObject("GameState");
            state.AddComponent<GameState>();
        }

        private static TaskBoard EnsureTaskBoard()
        {
            TaskBoard existing = FindObjectOfType<TaskBoard>();
            if (existing != null) return existing;

            GameObject boardObject = new GameObject("TaskBoard");
            TaskBoard board = boardObject.AddComponent<TaskBoard>();
            board.ConfigureTasks(new List<HouseTask>
            {
                HouseTask.Create("clean_room", "Убраться в комнате", "Собрать вещи и протереть пыль", 12, 120),
                HouseTask.Create("cook_food", "Приготовить еду", "Сделать ужин на вечер", 8, 90),
                HouseTask.Create("take_out_trash", "Выбросить мусор", "Вынести пакет к контейнеру", 7, 70),
                HouseTask.Create("take_shower", "Принять душ", "Сбросить усталость после дня", 5, 60)
            });

            return board;
        }

        private static void EnsurePlayer(TaskBoard board)
        {
            if (FindObjectOfType<PlayerInteractor>() != null) return;

            GameObject player = new GameObject("Player");
            CharacterController controller = player.AddComponent<CharacterController>();
            controller.height = 1.8f;
            controller.center = new Vector3(0f, 0.9f, 0f);

            player.AddComponent<BasicFirstPersonMotor>();

            GameObject cam = new GameObject("PlayerCamera");
            cam.transform.SetParent(player.transform);
            cam.transform.localPosition = new Vector3(0f, 1.6f, 0f);
            Camera cameraComponent = cam.AddComponent<Camera>();
            cameraComponent.tag = "MainCamera";

            PlayerInteractor interactor = player.AddComponent<PlayerInteractor>();
            interactor.Configure(cameraComponent, board);

            player.transform.position = new Vector3(0f, 0f, -3f);
        }

        private static void EnsureWorld()
        {
            if (GameObject.Find("DemoFloor") != null) return;

            GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
            floor.name = "DemoFloor";
            floor.transform.position = Vector3.zero;

            CreateTaskCube("TrashBin", new Vector3(-2f, 0.5f, 1f), new Color(0.1f, 0.6f, 0.1f), "take_out_trash");
            CreateTaskCube("Kitchen", new Vector3(2f, 0.5f, 2f), new Color(0.8f, 0.4f, 0.2f), "cook_food");
            CreateTaskCube("DirtyCorner", new Vector3(-2.5f, 0.5f, -2f), new Color(0.7f, 0.7f, 0.2f), "clean_room");

            CreateUtilityCube("Shower", new Vector3(2.5f, 0.5f, -2f), new Color(0.2f, 0.5f, 0.8f), UtilityType.Shower, 10, -10, "take_shower");
            CreateUtilityCube("Bed", new Vector3(0f, 0.5f, 3.5f), new Color(0.5f, 0.2f, 0.7f), UtilityType.Bed, 25, -5, string.Empty);

            Light light = FindObjectOfType<Light>();
            if (light == null)
            {
                GameObject lightObj = new GameObject("Directional Light");
                light = lightObj.AddComponent<Light>();
                light.type = LightType.Directional;
            }

            light.intensity = 0.8f;
        }

        private static void CreateTaskCube(string name, Vector3 position, Color color, string taskId)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = name;
            cube.transform.position = position;
            cube.GetComponent<Renderer>().material.color = color;

            InteractableTaskObject interactable = cube.AddComponent<InteractableTaskObject>();
            interactable.Configure(taskId, 12, "Нажми E");

        }

        private static void CreateUtilityCube(
            string name,
            Vector3 position,
            Color color,
            UtilityType type,
            int energyDelta,
            int stressDelta,
            string optionalTaskId)
        {
            TaskBoard board = FindObjectOfType<TaskBoard>();
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = name;
            cube.transform.position = position;
            cube.GetComponent<Renderer>().material.color = color;

            UtilityStation station = cube.AddComponent<UtilityStation>();
            station.Configure(type, energyDelta, stressDelta, optionalTaskId, board);
        }
    }
}
