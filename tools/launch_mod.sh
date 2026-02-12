#!/usr/bin/env bash
set -euo pipefail

APP_ID=243730 # Source SDK Base 2013 Singleplayer
MOD_NAME="neo_city17"
DRY_RUN=0
AUTO_CHOICE=""

usage() {
  cat <<USAGE
Usage:
  bash tools/launch_mod.sh [mod_name] [options]

Options:
  --dry-run         Показать команды запуска, но не запускать Steam
  --mode <1|2|3>    Запуск без интерактивного меню
                    1 = обычный
                    2 = с консолью
                    3 = оконный 1280x720
  -h, --help        Показать справку
USAGE
}

while [[ $# -gt 0 ]]; do
  case "$1" in
    --dry-run)
      DRY_RUN=1
      shift
      ;;
    --mode)
      AUTO_CHOICE="${2:-}"
      shift 2
      ;;
    -h|--help)
      usage
      exit 0
      ;;
    --*)
      echo "[ERROR] Неизвестный флаг: $1"
      usage
      exit 1
      ;;
    *)
      MOD_NAME="$1"
      shift
      ;;
  esac
done

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
MOD_DIR="$ROOT_DIR/mod/$MOD_NAME"

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

steam_cmd_exists() {
  command -v steam >/dev/null 2>&1
}

xdg_open_exists() {
  command -v xdg-open >/dev/null 2>&1
}

run_cmd() {
  if [[ "$DRY_RUN" -eq 1 ]]; then
    echo "[DRY-RUN] $*"
    return 0
  fi
  "$@"
}

urlencode() {
  python -c 'import sys, urllib.parse; print(urllib.parse.quote(sys.argv[1], safe=""))' "$1"
}

launch_with_applaunch() {
  local extra_args="$1"
  if [[ "$DRY_RUN" -eq 1 ]]; then
    run_cmd steam -applaunch "$APP_ID" -game "$MOD_DIR" $extra_args
    return 0
  fi

  if steam_cmd_exists; then
    run_cmd steam -applaunch "$APP_ID" -game "$MOD_DIR" $extra_args
    return 0
  fi
  return 1
}

launch_with_url() {
  local extra_args="$1"
  local game_arg="-game $MOD_DIR $extra_args"
  local encoded
  encoded="$(urlencode "$game_arg")"
  local url="steam://run/$APP_ID//${encoded}"

  # URL launch как fallback (без гарантии для всех окружений)
  if steam_cmd_exists; then
    run_cmd steam "$url"
    return 0
  fi

  if xdg_open_exists; then
    run_cmd xdg-open "$url"
    return 0
  fi

  return 1
}

launch_game() {
  local extra_args="$1"
  echo "[INFO] Запускаю окно игры ($MOD_NAME)..."
  echo "[INFO] Если мод не появился после первого запуска — полностью перезапусти Steam."

  if launch_with_applaunch "$extra_args"; then
    return 0
  fi

  if launch_with_url "$extra_args"; then
    return 0
  fi

  echo "[ERROR] Не удалось найти способ запуска Steam (нет steam и xdg-open)."
  echo "[HINT] Установи Steam и/или xdg-open и попробуй снова."
  exit 1
}

resolve_extra_args() {
  case "$1" in
    1) echo "" ;;
    2) echo "-console" ;;
    3) echo "-windowed -w 1280 -h 720" ;;
    *)
      echo "[ERROR] Неверный режим: $1 (ожидалось 1/2/3)" >&2
      exit 1
      ;;
  esac
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
    1|2|3)
      launch_game "$(resolve_extra_args "$choice")"
      ;;
    4)
      echo "Выход."
      exit 0
      ;;
    *)
      echo "Неверный выбор"
      exit 1
      ;;
  esac
}

if [[ -n "$AUTO_CHOICE" ]]; then
  launch_game "$(resolve_extra_args "$AUTO_CHOICE")"
else
  show_menu
fi
