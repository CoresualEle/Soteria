
# Anwendungsprojekt Soteria

## Projektstruktur

Das Projekt ist im Ordner "src" zu finden. Unter `src/resources` werden
Texturen, Sound-Dateien und ähnliches abgelegt.

Jede Szene kommt in einen eigenen Ordner.

## Bearbeiten in Godot

Zum Bearbeiten in Godot gibt es zwei Möglichkeiten:

- Importieren der "project.godot"-Datei aus dem "src"-Ordner in Godot mit der
  Hilfe vom "Importieren"-Button im Hauptmenu.
- Öffnen der "project.godot"-Datei aus dem "src"-Ordner mit Godot.

## Debuggen mit Godot

To be documented

## Programmieren mit Visual Studio

Das Projekt kann in Visual Studio geöffnet werden, wenn man die Datei
"Soteria.sln" aus dem "src"-Ordner mit Visual Studio öffnet.

Hierbei ist jedoch zu beachten, dass nur der Quellcode bearbeitet werden und das
Projekt nicht gestartet werden kann. Das Projekt kann jedoch in beiden
Anwendungen parallel bearbeitet werden.

## Debuggen mit Visual Studio

To be documented

## Using Godot with git

Die Änderungen können aus Visual Studio oder auch über externe Tools
synchronisiert werden.

## Resharper laufen lassen

Resharper ist ein Code-Checker, der die Syntax und die Logik von Code-Blöcken
überprüft. Um es zu benutzen, müssen folgende Befehle ausgeführt werden:

```shell
dotnet tool restore

dotnet jb cleanupcode src/Soteria.sln \
  -s="src/Soteria.sln.DotSettings" \
  -s="src/Soteria.sln.DotSettings.user" \
  -s="src/rules/StyleCop.dotSettings" \
  -s="src/rules/Team.DotSettings"

dotnet jb inspectcode src/Soteria.sln \
  --profile="src/Soteria.sln.DotSettings" \
  --profile="src/Soteria.sln.DotSettings.user" \
  --profile="src/rules/StyleCop.dotSettings" \
  --profile="src/rules/Team.DotSettings" \
  -o=result.txt --format=Text -s=WARNING
```

Alternativ kann auch [pre-commit](https://pre-commit.com/) installiert werden.
Bevor man `pre-commit install` laufen lassen kann, muss aber auch ein `dotnet
tool restore` ausgeführt werden.