# Neo-City17 (Unity) — перезапуск проекта

Проект полностью пересоздан под **Unity** (вместо Source).

## Что сейчас есть
- Новый Unity-oriented каркас проекта (`Assets/`, `ProjectSettings/`, `docs/`, `tools/`)
- Набор базовых gameplay-скриптов:
  - FPS-перемещение + камера
  - стрельба hitscan
  - здоровье игрока
  - простое AI (патруль/агр/атака)
  - интеракции с объектами
- Production-документация по сборке vertical slice

## Быстрый старт
1. Установи Unity Hub + Unity Editor **2022.3 LTS**.
2. В Unity Hub: **Add project from disk** → выбери эту папку.
3. Создай сцену `Assets/Scenes/VS_Intro.unity`.
4. Следуй `docs/UNITY_RUN_GUIDE_RU.md`.

## Запуск из CLI (опционально)
```bash
bash tools/open_unity_project.sh
```

## Важно
Это не готовая AAA-игра «как HL2», а **честный рабочий фундамент на Unity**, который можно расширять до полноценного vertical slice.
