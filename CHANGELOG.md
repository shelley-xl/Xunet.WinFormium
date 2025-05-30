## [1.0.22](https://github.com/shelley-xl/Xunet.WinFormium/commit/d405a958bd9adc1a0a20f260620a75679d5f774f) (2025-05-26)

### Features
- 包升级
- 修改包描述

## [1.0.21](https://github.com/shelley-xl/Xunet.WinFormium/commit/12b32e418cb3e8816f7161b98b0e3962ee635e18) (2025-05-08)

### Features

- 包升级
- 修复已知问题
- 新增窗体状态属性

## [1.0.20](https://github.com/shelley-xl/Xunet.WinFormium/commit/1a1e137c3cb919a1146412e71b9ab20ed0a9ca72) (2025-04-27)

### Features

- 引入System.Text.Json，替换Newtonsoft.Json
- 新增健康检查：/health/check
- 代码优化

## [1.0.19](https://github.com/shelley-xl/Xunet.WinFormium/commit/4487bdbd6ede5efe8a9201d5d0f7640390511974) (2025-04-27)

### Features

- Nuget包引用升级
- 新增.NET 9.0 支持
- 代码优化

## [1.0.18](https://github.com/shelley-xl/Xunet.WinFormium/commit/b501e9770fb7170f59beea4a5442b993f58a39aa) (2025-01-18)

### Bug Fixes

- 修复已知问题

## [1.0.17](https://github.com/shelley-xl/Xunet.WinFormium/commit/00912a490ff9fd03446e7178bc7558d7ac254619) (2024-11-12)

### Features

- 新增默认菜单功能，支持任务取消、执行
- 新增数据表格展示，
- 新增Tab标签页

### Bug Fixes

- 修复已知问题

## [1.0.16](https://github.com/shelley-xl/Xunet.WinFormium/commit/67c12933c958cab27e963a5ce94859625860902b) (2024-11-11)

### Bug Fixes

- 修复WebApi接口文档读取问题

## [1.0.15](https://github.com/shelley-xl/Xunet.WinFormium/commit/b687af80c62d59b233d4a5314d8a0f5f2ad24a3e) (2024-11-11)

### Bug Fixes

- 修复WebApi接口文档读取问题

## [1.0.14](https://github.com/shelley-xl/Xunet.WinFormium/commit/95af5da7d408ac772181648880d69f18f86e5538) (2024-10-16)

### Features

- 设置Resources为Public

## [1.0.13](https://github.com/shelley-xl/Xunet.WinFormium/commit/87b15ac1412e3161197fd46bb23e2db5a50f1407) (2024-10-15)

### Bug Fixes

- 二维码输出bug修复

## [1.0.12](https://github.com/shelley-xl/Xunet.WinFormium/commit/bf9145421da53f996c1a983f30e080218d5dc8fb) (2024-10-15)

### Features

- SuperDriver升级4.0.8
- 二维码输出优化

## [1.0.11](https://github.com/shelley-xl/Xunet.WinFormium/commit/92d6d8df7f56a918e5c348ffa93e7ec31003adf1) (2024-08-16)

### Features

- 优化Sqlite设置
- 引入SuperDriver和SuperSpider

## [1.0.10](https://github.com/shelley-xl/Xunet.WinFormium/commit/5280b2a8d28251b84de2b0ff9f13abe646b110e3) (2024-08-14)

### Features

- 新增异常处理

## [1.0.9](https://github.com/shelley-xl/Xunet.WinFormium/commit/e25b41f52b9a2ee858eba201c4c2514e15830918) (2024-08-14)

### Features

- 新增WebApi支持
- 新增属性管理器

### Bug Fixes

- 修复已知问题

## [1.0.8](https://github.com/shelley-xl/Xunet.WinFormium/commit/c15c8c2638815403ce7907f9974f26e8b76e5bcc) (2024-08-12)

### Features

- 支持使用单例模式启动（互斥锁）
- 周期作业支持停止/恢复

## [1.0.7](https://github.com/shelley-xl/Xunet.WinFormium/commit/dd0b910a649eedc332f231f3731cd58aef5a11be) (2024-08-09)

### Features

- 定时作业支持Cron表达式
- 支持依赖注入
- 优雅的启动方式

### Bug Fixes

- 修复已知问题

## [1.0.6](https://github.com/shelley-xl/Xunet.WinFormium/commit/433ca04ff36f09c2db3e41b5f0fdce531e6ed297) (2024-08-07)

### Features

- 新增去首尾空格换行制表符

### Bug Fixes

- 周期作业，解决并发执行带来的问题
- 修复已知问题

## [1.0.5](https://github.com/shelley-xl/Xunet.WinFormium/commit/769621558215a8a90057842f7120aaad038a3d4c) (2024-08-07)

### Bug Fixes

- 修复已知问题

## [1.0.4](https://github.com/shelley-xl/Xunet.WinFormium/commit/6104c8b0cb8efff01ea6767955cdacd884b5fe20) (2024-08-07)

### Bug Fixes

- 优化

## [1.0.3](https://github.com/shelley-xl/Xunet.WinFormium/commit/fbf6c7f5b0f717185b7a3727f0660ad9bf8151c4) (2024-08-07)

### Features

- 新增HtmlAgilityPack辅助方法

### Bug Fixes

- 修复已知问题

## [1.0.2](https://github.com/shelley-xl/Xunet.WinFormium/commit/6162e076449172ff5d73cb697ecefe2d6d1b18e9) (2024-08-06)

### Bug Fixes

- 修复已知问题

## 1.0.1 (2024-08-05)

### Features

- 为爬虫而生
- 支持IHttpClientFacory，便于使用HttpClient发送网络请求
- 支持HTML页面解析，默认使用XPath
- 支持数据持久化，默认使用Sqlite数据库
- 支持周期作业，通过简单配置即可实现重复性作业
- 支持分布式雪花ID
- 无需设计界面，没有Designer.cs文件
- 无需新建Windows窗体，只需新建class类，继承BaseForm即可
- 支持重写属性和方法，便于扩展
- 内置界面日志输出，开箱即用
- 内置序列化和反序列化封装方法，便于调用
