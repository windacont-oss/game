#!/usr/bin/env bash
set -euo pipefail

if [[ $# -lt 1 ]]; then
  echo "Usage: $0 <mod_name>"
  exit 1
fi

MOD_NAME="$1"
ROOT_DIR="$(pwd)"
TARGET="$ROOT_DIR/mod/$MOD_NAME"

mkdir -p "$TARGET"/{maps,materials,models,scripts,sound,resource,cfg}

cat > "$TARGET/gameinfo.txt" <<GI
"GameInfo"
{
	game		"$MOD_NAME"
	title		"$MOD_NAME"
	type		singleplayer_only
	FileSystem
	{
		SteamAppId	243730
		SearchPaths
		{
			game		|gameinfo_path|.
			game		hl2
		}
	}
}
GI

cat > "$TARGET/cfg/skill.cfg" <<CFG
// Baseline balance values for vertical slice
sk_player_head	2
sk_player_chest	1
sk_player_stomach	1
sk_player_arm	0.8
sk_player_leg	0.8
CFG

cat > "$TARGET/README.md" <<TXT
# $MOD_NAME

Этот каркас создан скриптом bootstrap_source_mod.sh.

## Что дальше
1. Подключи папку в sourcemods.
2. Запусти SDK и открой Hammer.
3. Сделай карту maps/vs_intro.bsp.
4. Итеративно внедряй механики по docs/MECHANICS_CHECKLIST.md.
TXT

echo "Created Source mod skeleton at: $TARGET"
