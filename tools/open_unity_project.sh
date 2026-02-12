#!/usr/bin/env bash
set -euo pipefail

PROJECT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"

if command -v unityhub >/dev/null 2>&1; then
  echo "[INFO] Opening project via Unity Hub CLI..."
  unityhub -- --headless open -p "$PROJECT_DIR" || unityhub -- --headless editors -a "$PROJECT_DIR"
  exit 0
fi

if command -v Unity >/dev/null 2>&1; then
  echo "[INFO] Opening project via Unity binary..."
  Unity -projectPath "$PROJECT_DIR"
  exit 0
fi

if command -v xdg-open >/dev/null 2>&1; then
  echo "[WARN] Unity CLI not found. Opening project folder..."
  xdg-open "$PROJECT_DIR"
  exit 0
fi

echo "[ERROR] Не найден Unity Hub/Unity CLI. Открой проект вручную в Unity Hub: $PROJECT_DIR"
exit 1
