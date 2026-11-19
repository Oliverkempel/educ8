Educ8 for Linux
===============

Install for your user:   ./install.sh      (remove again with ./uninstall.sh)
Or run it in place:      ./app/educ8

Needs these system libraries (most GNOME desktops have them):
  GTK 4, libadwaita 1, WebKitGTK 6.0
  Ubuntu/Debian: sudo apt install libgtk-4-1 libadwaita-1-0 libwebkitgtk-6.0-4
  Fedora:        sudo dnf install gtk4 libadwaita webkitgtk6.0

Before the first sign-in, set your itslearning client ID in ~/.local/share/Educ8/config.json:
  { "ClientId": "<your client id>" }

The MCP server for Claude Desktop is app/educ8-mcp (after install: ~/.local/opt/educ8/educ8-mcp).
Both share the login and settings in ~/.local/share/Educ8.
