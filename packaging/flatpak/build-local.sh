#!/bin/sh
# Builds the Flatpak locally from a fresh self-contained publish (x86_64 or aarch64), like the release workflow does.
# Usage: packaging/flatpak/build-local.sh [version] [x64|arm64]   → educ8-<version>-<arch>.flatpak in the repo root.
# Needs: dotnet, flatpak, the GNOME runtime/SDK and a builder, e.g. for your user only:
#   flatpak install --user flathub org.flatpak.Builder org.gnome.Platform//51 org.gnome.Sdk//51
set -eu
root=$(cd "$(dirname "$0")/../.." && pwd)
version="${1:-0.0.0-dev}"
cpu="${2:-x64}"
arch=$([ "$cpu" = arm64 ] && echo aarch64 || echo x86_64)
stage="$root/packaging/flatpak/stage"

rm -rf "$stage" && mkdir -p "$stage/app"
for project in Educ8.App.Gtk Educ8.Mcp.Server; do
  dotnet publish "$root/src/$project" -c Release -r "linux-$cpu" --self-contained -p:DebugType=none -p:Version="$version" -o "$stage/app"
done
cp "$root"/packaging/linux/io.github.educ8.Educ8.desktop "$root"/packaging/linux/io.github.educ8.Educ8.svg \
   "$root"/packaging/linux/io.github.educ8.Educ8.metainfo.xml "$stage/"

cd "$root/packaging/flatpak"
if command -v flatpak-builder >/dev/null 2>&1; then builder=flatpak-builder; else builder="flatpak run --no-documents-portal org.flatpak.Builder"; fi
$builder --force-clean --user --disable-rofiles-fuse --arch="$arch" --repo=repo build io.github.educ8.Educ8.yml
flatpak build-bundle --arch="$arch" repo "$root/educ8-$version-$arch.flatpak" io.github.educ8.Educ8
echo "Built $root/educ8-$version-$arch.flatpak"
