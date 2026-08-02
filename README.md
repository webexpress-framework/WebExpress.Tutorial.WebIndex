![WebExpress-Framework](https://raw.githubusercontent.com/webexpress-framework/.github/main/docs/assets/img/banner.png)

# WebExpress
`WebExpress` is a lightweight web server optimized for use in low-performance environments (e.g. Raspberry Pi). By providing 
a powerful plugin system and a comprehensive API, web applications can be easily and quickly integrated into a .net 
language (e.g. C#). Some advantages of `WebExpress` are:

- It is easy to use.
- It offers a variety of features and tools that can help you build and manage your website.
- It is fast and efficient and can help you save time and money.
- It is flexible and can be customized to meet your specific requirements.

The `WebExpress` family includes the following projects:

- [WebExpress](https://github.com/webexpress-framework/WebExpress#readme) - The web server for `WebExpress` applications and the documentation.
- [WebExpress.WebCore](https://github.com/webexpress-framework/WebExpress.WebCore#readme) - The core for `WebExpress` applications.
- [WebExpress.WebUI](https://github.com/webexpress-framework/WebExpress.WebUI#readme) - Common templates and controls for `WebExpress` applications.
- [WebExpress.WebIndex](https://github.com/webexpress-framework/WebExpress.WebIndex#readme) - Reverse index for `WebExpress` applications.
- [WebExpress.WebApp](https://github.com/webexpress-framework/WebExpress.WebApp#readme) - Business application template for `WebExpress` applications.

`WebExpress` is part of the `WebExpress` family. The project provides a web server for `WebExpress` applications.

To get started with `WebExpress`, use the following links.

- [installation guide](https://github.com/webexpress-framework/WebExpress/blob/main/docs/installation_guide.md) 
- [development guide](https://github.com/webexpress-framework/WebExpress/blob/main/docs/development_guide.md)

# Tutorial
A tutorial to demonstrate the `WebIndex`. The tutorial discusses and explains how to use the `WebIndex` and `WebExpress` jobs.

## Prerequisites
- Create a `WebExpress` application after the [WebApp](https://github.com/webexpress-framework/WebExpress.Tutorial.WebApp#readme) tutorial but name it `WebExpress.Tutorial.WebIndex`.

## Compile and register in WebExpress
- Compile the solution as a release. To do this, open the command line or terminal in the solution directory and run the following command:
  ```bash
  dotnet build --configuration Release
  ```
  This command compiles the solution in release mode. You can find the compiled files in the `bin/Release` directory of your project.

- Run the solution by starting the `WebIndex.App` project.
  ```bash
  cd WebIndex.App\bin\Release\net9.0
  dotnet run --project ../../../WebIndex.App.csproj
  ```

- After compiling, there should be a file with the `.wxp` extension in the `pkg/Release` directory. This file do you need in `WebExpress`.

## Try the application
- Check the result by calling up the following URL in the browser: http://localhost/webindex

Good luck!

## AI transparency notice

Parts of this software, its documentation, and its assets were created with the assistance of AI-based tools, including large language models. AI-assisted contributions are reviewed by the project maintainer before they are included.
    
# Tags
#WebExpress #WebServer #WebCore #WebUI #Tutorial #DotNet
