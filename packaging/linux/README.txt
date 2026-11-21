Educ8 for Linux
===============

Install for your user:   ./install.sh      (remove again with ./uninstall.sh)
Or run it in place:      ./app/educ8

Needs these system libraries (most GNOME desktops have them):
  GTK 4, libadwaita 1
  Ubuntu/Debian: sudo apt install libgtk-4-1 libadwaita-1-0
  Fedora:        sudo dnf install gtk4 libadwaita

Before the first sign-in, set your itslearning site and client ID in ~/.local/share/Educ8/config.json:
  { "BaseUrl": "https://yourschool.itslearning.com", "ClientId": "<your client id>" }

Sign-in happens in your default browser. install.sh registers Educ8 for itsl-itslearning:// links, so the browser
can hand the finished sign-in back to the app; allow it when the browser asks.

The MCP server for Claude Desktop is app/educ8-mcp (after install: ~/.local/opt/educ8/educ8-mcp).
Both share the login and settings in ~/.local/share/Educ8.
