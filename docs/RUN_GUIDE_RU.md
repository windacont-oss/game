# Как запустить текущий проект (шаг за шагом)

Важно: в репозитории сейчас **каркас мода и документация**, а не готовая полностью собранная игра/бинарник.

## 1) Что нужно установить

1. Steam
2. Source SDK Base 2013 Singleplayer (из Steam, Tools)
3. (Опционально, если хочешь кодить DLL) исходники `source-sdk-2013` и Visual Studio


## Быстрый вариант: один скрипт с меню

Теперь можно запускать через лаунчер:

```bash
bash tools/launch_mod.sh neo_city17
```

Для проверки команды без фактического старта Steam:

```bash
bash tools/launch_mod.sh neo_city17 --dry-run
```

Можно запускать без меню (удобно для проверки):

```bash
bash tools/launch_mod.sh neo_city17 --mode 1
```

Где `--mode`:
- `1` обычный
- `2` с консолью
- `3` оконный 1280x720

Что делает скрипт автоматически:

1. Проверяет/создаёт каркас мода
2. Делает симлинк в `sourcemods`
3. Открывает меню запуска (обычный / с консолью / windowed)
4. Стартует окно игры через Steam

## 2) Сгенерировать каркас мода

Из корня репозитория:

```bash
bash tools/bootstrap_source_mod.sh neo_city17
```

После этого появится папка:

- `mod/neo_city17/`

## 3) Подключить мод в Steam

Нужно скопировать/сделать ссылку на `mod/neo_city17` в папку `sourcemods`.

Типичные пути:

- Linux: `~/.steam/steam/steamapps/sourcemods/`
- Windows: `C:\Program Files (x86)\Steam\steamapps\sourcemods\`

Пример для Linux (симлинк):

```bash
mkdir -p ~/.steam/steam/steamapps/sourcemods
ln -s "$(pwd)/mod/neo_city17" ~/.steam/steam/steamapps/sourcemods/neo_city17
```

Перезапусти Steam.

## 4) Запуск

После перезапуска Steam мод должен появиться в библиотеке/списке инструментов как отдельный Source-мод.

Если не появился:

- проверь, что `gameinfo.txt` лежит в корне папки мода;
- проверь имя папки и симлинка;
- полностью закрой и снова открой Steam.

## 5) Что запускать внутри мода

Сейчас это каркас. Чтобы реально играть, нужно:

1. Собрать карту в Hammer (например `vs_intro.bsp`)
2. Положить `.bsp` в `mod/neo_city17/maps/`
3. Запустить карту через консоль:

```text
map vs_intro
```

## 6) Быстрый sanity-check каркаса

```bash
rg --files mod/neo_city17
```

Должны быть как минимум:

- `gameinfo.txt`
- `cfg/skill.cfg`
- `README.md`

---

Если хочешь, следующим шагом могу добавить **минимальный playable blockout-план** первой карты (что именно ставить в Hammer по шагам, чтобы получить 5–10 минут геймплея).
