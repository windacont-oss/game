# Исправление ошибки "Missing NebulaStrike Modules"

Если UE показывает сообщение:

> The following modules are missing or built with a different engine version: NebulaStrike

сделай шаги строго по порядку:

1. Закрой Unreal Editor.
2. Удали папки в корне проекта:
   - `Binaries`
   - `Intermediate`
   - `.vs` (если есть)
3. Кликни правой кнопкой по `NebulaStrike.uproject` → **Generate Visual Studio project files**.
4. Открой `NebulaStrike.sln` в Visual Studio 2022.
5. Выбери конфигурацию `Development Editor` + `Win64`.
6. Собери проект (**Build Solution**).
7. Запусти `NebulaStrike.uproject` снова.

## Важно

- Нужны workloads в Visual Studio:
  - **Desktop development with C++**
  - **Game development with C++**
- Должна быть установлена именно версия Unreal Engine **5.7.3**.
- Если установлен только 5.7.0/5.7.1, проект может требовать пересборку и не запускаться.

## Если всё ещё не собирается

Открой `Output Log`/лог сборки и проверь первую ошибку компилятора (обычно C++ include/SDK/Toolset). Без первой ошибки все остальные сообщения вторичны.
