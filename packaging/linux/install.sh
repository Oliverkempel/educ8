#!/bin/sh
# Installs Educ8 for the current user: the app and educ8-mcp in ~/.local/opt/educ8, commands in ~/.local/bin,
# and a desktop entry so Educ8 shows up in the app grid. Run it from the unpacked folder. Undo with ./uninstall.sh.
set -eu
here=$(cd "$(dirname "$0")" && pwd)
prefix="${XDG_DATA_HOME:-$HOME/.local/share}"
opt="$HOME/.local/opt/educ8"
bin="$HOME/.local/bin"

mkdir -p "$opt" "$bin" "$prefix/applications" "$prefix/icons/hicolor/scalable/apps"
rm -rf "$opt"/*
cp -R "$here/app/." "$opt/"
chmod +x "$opt/educ8" "$opt/educ8-mcp"
ln -sf "$opt/educ8" "$bin/educ8"
ln -sf "$opt/educ8-mcp" "$bin/educ8-mcp"
sed "s|^Exec=educ8|Exec=$opt/educ8|" "$here/io.github.educ8.Educ8.desktop" > "$prefix/applications/io.github.educ8.Educ8.desktop"
cp "$here/io.github.educ8.Educ8.svg" "$prefix/icons/hicolor/scalable/apps/"
update-desktop-database "$prefix/applications" >/dev/null 2>&1 || true
# Sign-in happens in the browser, which hands the itsl-itslearning:// redirect back to the app.
xdg-mime default io.github.educ8.Educ8.desktop x-scheme-handler/itsl-itslearning >/dev/null 2>&1 || true
gtk-update-icon-cache -f -t "$prefix/icons/hicolor" >/dev/null 2>&1 || true

echo "Installed Educ8 $(cat "$here/VERSION") to $opt"
echo "  App:        educ8 (or 'Educ8' in your app grid)"
echo "  MCP server: $opt/educ8-mcp  (use this path in Claude Desktop's config)"
case ":$PATH:" in *":$bin:"*) ;; *) echo "Note: $bin is not on your PATH." ;; esac
