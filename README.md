# Nebula Strike (Unreal Engine 5.7.3)

Я сделал **полноценную базу под “супер-игру”** на UE 5.7.3: C++ модуль, игровой режим, персонаж с Enhanced Input, оружие с трассировкой попаданий, компонент здоровья и конфиги под современный рендер (Lumen + Nanite + VSM).

## Что уже готово

- Структура UE-проекта (`.uproject`, `Source`, `Config`).
- Игровой цикл-шаблон:
  - `ANebulaStrikeCharacter` — движение, обзор, стрельба.
  - `UWeaponComponent` — hitscan-оружие и урон.
  - `UHealthComponent` — здоровье и событие смерти.
  - `ANebulaStrikeGameMode` — цель миссии по убийствам.
- Рендер-настройки под high-end визуал:
  - Lumen GI/Reflections
  - Nanite
  - Virtual Shadow Maps
  - DX12

## Как запустить в UE 5.7.3

1. Открой `NebulaStrike.uproject` через Unreal Engine 5.7.3.
2. Согласись на генерацию project files и сборку C++.
3. Создай в Editor:
   - `BP_NebulaStrikeCharacter` (на базе `ANebulaStrikeCharacter`)
   - `BP_NebulaStrikeGameMode` (на базе `ANebulaStrikeGameMode`)
4. Создай `Input Actions`:
   - `IA_Move` (Vector2D)
   - `IA_Look` (Vector2D)
   - `IA_Fire` (Bool)
5. Создай `Input Mapping Context` и свяжи клавиши/мышь/геймпад.
6. Назначь эти assets в поля персонажа (`MoveAction`, `LookAction`, `FireAction`).
7. В `World Settings` выбери `BP_NebulaStrikeGameMode`.

## Что нужно добавить для реально “AAA-ощущения”

- **Контент**: качественные персонажи, окружение, VFX, анимации, звук/музыка.
- **Gameplay-системы**: AI Behavior Trees, миссии, инвентарь, прогрессия, сохранения.
- **UI/UX**: HUD, меню, настройки, локализация.
- **Полировка**: анимационные state machines, blending, camera shake, hit reactions.
- **Оптимизация**: профилирование на целевых GPU/CPU, scalability tiers, memory budget.

## Roadmap (рекомендуемый)

- Milestone 1: Vertical Slice (1 карта, 1 миссия, 2 типа врагов).
- Milestone 2: Core Combat (оружие, способности, AI тактики).
- Milestone 3: Production Content (5-10 уровней, синематики).
- Milestone 4: QA + Polish + Release Candidate.

---

Если хочешь, следующим шагом я могу сразу добавить:
1) AI врага с преследованием/атакой через Behavior Tree,  
2) систему лута и прокачки,  
3) полноценный HUD + паузу + настройки графики.
