# Configuration and AppSettings

## Configuration

1. Port - on what port application should start
1. Environment - current environment, files based on this field are loaded
1. ReturnFullErrorMessage - if true then will return the whole error stack trace

## Default server configuration

Port = "7000"
Environment = "Development"
ReturnFullErrorMessage = false

## How to override the default configuration

If you want to override default values you have to do this in `AppSettings/ServerSetting.json` for example

```json
{
  "Port": "7001",
  "Environment": "Prod",
  "ReturnFullErrorMessage": true
}
```

## AppSettings

You can write your appsettings, by just adding it in AppSettings.json or AppSettings.{Env}.json file. Where AppSettings.{Env}.json will always override value.
For example
`AppSettings.json`

```json
{
  "settings": {
    "howManyConnection": 1
  }
}
```

`AppSettings.Prod.json`

```json
{
  "settings": {
    "howManyConnection": 2
  }
}
```

that means the default value of howManyConnection is 1 but on the `Prod` env will be overridden to 2

## How to use AppSettings

If you want to use your settings in a controller or other service at the beginning you have to register your settings.

To do it you have to use `Builder.AddConfig<ConfigModel>();` in `Programs.cs`.
There are two overloaded methods.

1. `AddConfig<T>();` - this will automatically load the object from `AppSettings` where the key is the name of the class.
2. `AddConfig<T>(string key);` - this will load the object from `AppSettings` where the key is passing by argument. If an object is nested just use `:`
