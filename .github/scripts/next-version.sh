#!/bin/sh
# Prints the next date-based release version (UTC): YYYY.M.D for the first release of the day, then YYYY.M.D.1, .2, …
# Looks at the existing v<version> tags, so fetch tags first (the release workflow checks out with fetch-depth: 0).
# Usage: next-version.sh [YYYY.M.D]   (the date is for testing; default: today)
set -eu
day="${1:-$(date -u +%Y.%-m.%-d)}"
exists() { git rev-parse -q --verify "refs/tags/v$1" >/dev/null; }
if ! exists "$day"; then echo "$day"; exit 0; fi
n=1
while exists "$day.$n"; do n=$((n + 1)); done
echo "$day.$n"
