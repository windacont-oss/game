# Unity: как запустить проект

## 1) Требования
- Unity Hub
- Unity Editor 2022.3 LTS
- (Опционально) VS Code / Rider

## 2) Открыть проект
1. Открой Unity Hub.
2. Нажми **Add project from disk**.
3. Выбери папку репозитория `/workspace/game`.
4. Открой проект в Unity 2022.3 LTS.

## 3) Поднять первую игровую сцену
1. Создай сцену: `Assets/Scenes/VS_Intro.unity`.
2. Создай `Player` (CharacterController + скрипты `PlayerMovement`, `PlayerLook`, `PlayerHealth`).
3. Создай `Main Camera` как дочерний объект `PlayerCameraRoot`.
4. Создай плейн/геометрию уровня.
5. Создай 1-2 врага с `EnemyAI`.
6. Создай объект-оружие на камере со скриптом `HitscanWeapon`.

## 4) Play
- Нажми **Play**.
- Управление по умолчанию:
  - WASD — ходьба
  - Shift — бег
  - Space — прыжок
  - Мышь — обзор
  - ЛКМ — стрельба
  - E — интеракция

## 5) Типичные проблемы
- Если не работает ходьба: проверь, что на Player есть `CharacterController`.
- Если не работает обзор: проверь, что у `PlayerLook` назначен `cameraPivot`.
- Если не наносится урон врагу: проверь слой/коллайдер и что у цели есть `Health`.
