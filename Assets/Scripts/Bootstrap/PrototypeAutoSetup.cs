using System.Collections.Generic;
using ImbaLife.Core;
using ImbaLife.Gameplay;
using ImbaLife.UI;
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
            PlayerInteractor interactor = EnsurePlayer(taskBoard);
            EnsureWorld();
            EnsureSessionController(taskBoard);
            EnsureOverlay(taskBoard, interactor);
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

        private static PlayerInteractor EnsurePlayer(TaskBoard board)
        {
            PlayerInteractor existing = FindObjectOfType<PlayerInteractor>();
            if (existing != null) return existing;

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

            player.transform.position = new Vector3(0f, 0.2f, -3f);
            return interactor;
        }

        private static void EnsureSessionController(TaskBoard board)
        {
            if (FindObjectOfType<PrototypeSessionController>() != null) return;

            GameObject controllerObject = new GameObject("SessionController");
            PrototypeSessionController session = controllerObject.AddComponent<PrototypeSessionController>();
            session.Configure(board);
        }

        private static void EnsureOverlay(TaskBoard board, PlayerInteractor interactor)
        {
            if (FindObjectOfType<PrototypeGameOverlay>() != null) return;

            GameObject overlayObj = new GameObject("PrototypeOverlay");
            PrototypeGameOverlay overlay = overlayObj.AddComponent<PrototypeGameOverlay>();
            PrototypeSessionController session = FindObjectOfType<PrototypeSessionController>();
            overlay.Configure(board, session, interactor);
        }

        private static void EnsureWorld()
        {
            if (GameObject.Find("DemoFloor") != null) return;

            GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
            floor.name = "DemoFloor";
            floor.transform.position = Vector3.zero;
            floor.transform.localScale = new Vector3(1.2f, 1f, 1.2f);
            floor.GetComponent<Renderer>().material.color = new Color(0.18f, 0.18f, 0.2f);

            CreateWall("Wall_N", new Vector3(0f, 1.5f, 6f), new Vector3(12f, 3f, 0.2f));
            CreateWall("Wall_S", new Vector3(0f, 1.5f, -6f), new Vector3(12f, 3f, 0.2f));
            CreateWall("Wall_E", new Vector3(6f, 1.5f, 0f), new Vector3(0.2f, 3f, 12f));
            CreateWall("Wall_W", new Vector3(-6f, 1.5f, 0f), new Vector3(0.2f, 3f, 12f));

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

            light.intensity = 0.75f;
            RenderSettings.ambientLight = new Color(0.28f, 0.28f, 0.33f);
        }

        private static void CreateWall(string wallName, Vector3 position, Vector3 scale)
        {
            GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.name = wallName;
            wall.transform.position = position;
            wall.transform.localScale = scale;
            wall.GetComponent<Renderer>().material.color = new Color(0.36f, 0.35f, 0.38f);
        }

        private static void CreateTaskCube(string name, Vector3 position, Color color, string taskId)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = name;
            cube.transform.position = position;
            cube.GetComponent<Renderer>().material.color = color;

            InteractableTaskObject interactable = cube.AddComponent<InteractableTaskObject>();
            interactable.Configure(taskId, 12, "Выполнить задачу");
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
