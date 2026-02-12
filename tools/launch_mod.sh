#!/usr/bin/env bash
set -euo pipefail

MOD_NAME="${1:-neo_city17}"
DRY_RUN="${2:-}"
ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
MOD_DIR="$ROOT_DIR/mod/$MOD_NAME"

APP_ID=243730 # Source SDK Base 2013 Singleplayer

if [[ ! -d "$MOD_DIR" ]]; then
  echo "[INFO] Мод '$MOD_NAME' не найден, создаю каркас..."
  bash "$ROOT_DIR/tools/bootstrap_source_mod.sh" "$MOD_NAME"
fi

if [[ ! -f "$MOD_DIR/gameinfo.txt" ]]; then
  echo "[ERROR] Не найден $MOD_DIR/gameinfo.txt"
  exit 1
fi

find_sourcemods_dir() {
  local candidates=(
    "$HOME/.steam/steam/steamapps/sourcemods"
    "$HOME/.local/share/Steam/steamapps/sourcemods"
    "$HOME/.var/app/com.valvesoftware.Steam/.steam/steam/steamapps/sourcemods"
  )

  for p in "${candidates[@]}"; do
    if [[ -d "$p" ]]; then
      echo "$p"
      return 0
    fi
  done

  echo "${candidates[0]}"
}

SOURCEMODS_DIR="$(find_sourcemods_dir)"
mkdir -p "$SOURCEMODS_DIR"
TARGET_LINK="$SOURCEMODS_DIR/$MOD_NAME"

if [[ -L "$TARGET_LINK" || -e "$TARGET_LINK" ]]; then
  if [[ "$(readlink "$TARGET_LINK" 2>/dev/null || true)" != "$MOD_DIR" ]]; then
    echo "[WARN] $TARGET_LINK уже существует и указывает не на текущий мод."
    echo "[WARN] Удали/переименуй его вручную и запусти снова."
    exit 1
  fi
else
  ln -s "$MOD_DIR" "$TARGET_LINK"
  echo "[INFO] Создан симлинк: $TARGET_LINK -> $MOD_DIR"
fi

if [[ "$DRY_RUN" != "--dry-run" ]] && ! command -v steam >/dev/null 2>&1; then
  echo "[ERROR] Команда steam не найдена. Установи Steam и повтори запуск."
  exit 1
fi

urlencode() {
  python - <<'PY' "$1"
import sys, urllib.parse
print(urllib.parse.quote(sys.argv[1], safe=''))
PY
}

launch_game() {
  local extra_args="$1"
  echo "[INFO] Запускаю окно игры ($MOD_NAME)..."
  echo "[INFO] Если мод не появился после первого запуска — полностью перезапусти Steam."

  # Вариант через URL чаще стабильно прокидывает параметры запуска.
  local game_arg
  game_arg="-game $MOD_DIR $extra_args"
  local encoded
  encoded="$(urlencode "$game_arg")"

  if [[ "$DRY_RUN" == "--dry-run" ]]; then
    echo "[DRY-RUN] steam steam://run/$APP_ID//${encoded}"
    return 0
  fi

  steam "steam://run/$APP_ID//${encoded}"
}

show_menu() {
  echo
  echo "==== Launcher: $MOD_NAME ===="
  echo "1) Играть (обычный запуск)"
  echo "2) Играть с консолью (-console)"
  echo "3) Играть в оконном режиме 1280x720"
  echo "4) Выход"
  echo

  read -r -p "Выбери пункт [1-4]: " choice

  case "$choice" in
    1) launch_game "" ;;
    2) launch_game "-console" ;;
    3) launch_game "-windowed -w 1280 -h 720" ;;
    4) echo "Выход."; exit 0 ;;
    *) echo "Неверный выбор"; exit 1 ;;
  esac
}

show_menu
