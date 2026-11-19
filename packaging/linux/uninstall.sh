#!/bin/sh
# Removes what install.sh installed. Your login and settings (~/.local/share/Educ8) are kept.
set -eu
prefix="${XDG_DATA_HOME:-$HOME/.local/share}"
rm -rf "$HOME/.local/opt/educ8"
rm -f "$HOME/.local/bin/educ8" "$HOME/.local/bin/educ8-mcp"
rm -f "$prefix/applications/io.github.educ8.Educ8.desktop" "$prefix/icons/hicolor/scalable/apps/io.github.educ8.Educ8.svg"
update-desktop-database "$prefix/applications" >/dev/null 2>&1 || true
echo "Educ8 removed. Your login and settings in ~/.local/share/Educ8 are kept; delete that folder to remove them too."
