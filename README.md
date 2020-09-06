# ServerAnnouncements
This is a plugin that allows you to configure recurring announcements on the server

## Usage
After first running the plugin, it will generate a `data.yml` file in your plugins folder with some default example announcements.  
New announcements can be simply added as new elements in the yml document.  
Below is an example of how multiple announcements can be written.
```yml
Broadcasts:
-
  Interval: 20
  Duration: 6
  InitialDelay: 30
  Message: You are playing on this server\nPlease enjoy your stay!

Hints:
-
  Interval: 40
  Duration: 4
  InitialDelay: 20
  Message: Have a <color=#10ff10>nice</color> day!
-
  Interval: 40
  Duration: 6
  InitialDelay: 40
  Message: Enjoy your stay!

```

## Commands
`sa` shows the available commands. (Alias: `serverannouncements`)  
`sa reload` reloads the announcements from the data file. (Aliases: `sa r`)

## Permissions
```yml
sa # Allows to see available commands.
sa.reload # Allows reloading the announcements.
```