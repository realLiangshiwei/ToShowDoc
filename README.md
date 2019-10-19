# ToShowDoc

**Automatically generate documents to showdoc !**

<img src="https://raw.githubusercontent.com/liangshiw/ToShowDoc/master/ToShowDoc/src/logo.png" width="200" height="200" /> 

[![NuGet](https://img.shields.io/nuget/v/ToShowDoc.svg)](https://www.nuget.org/packages/ToShowDoc/)
[![NuGet](https://img.shields.io/nuget/dt/ToShowDoc.svg)](https://www.nuget.org/packages/ToShowDoc/)

## install

`dotnet tool install -g toshowdoc`

## commands

```shell
  add        add project
  del        delete project
  list       show all project
  sync       sync doc
  update     update project
```

### add

```shell
add project

Usage: ToShowDoc add [options]

Options:
  -h|--help             Show help information
  -n|--name             Project Name
  -ak|--ApiKey          ShowDoc Project ApiKey
  -at|--ApiToken        ShowDoc Project ApiToken
  -su|--SwaggerJsonUrl  Swagger Json Url
  -sdu|--ShowDocUrl     ShowDocUrl Name
```

### update

```shell
update project

Usage: ToShowDoc update [options]

Options:
  -h|--help             Show help information
  -n|--name             Project Name
  -ak|--ApiKey          ShowDoc Project ApiKey
  -at|--ApiToken        ShowDoc Project ApiToken
  -su|--SwaggerJsonUrl  Swagger Json Url
  -sdu|--ShowDocUrl     ShowDocUrl Name
```

### list

```shell
show all project

Usage: ToShowDoc list [options]

Options:
  -h|--help  Show help information
```

### del

```shell
delete project

Usage: ToShowDoc del [options]

Options:
  -h|--help  Show help information
  -n|--name  Project Name
```

### sync

```shell
sync doc

Usage: ToShowDoc sync [options]

Options:
  -h|--help  Show help information
  -n|--name  Project Name
```